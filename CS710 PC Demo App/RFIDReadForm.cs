using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace CS710_PC_Client
{
    public partial class RFIDReadForm : Form
    {
        byte[] m_read_buffer = new byte[65536]; // read ring buffer
        ushort m_read_buffer_size = 0;
        ushort m_read_buffer_head = 0;
        ushort m_read_buffer_tail = 0;

        byte[] m_rfid_buffer = new byte[65536]; // rfid ring buffer
        ushort m_rfid_buffer_size = 0;
        ushort m_rfid_buffer_head = 0;
        ushort m_rfid_buffer_tail = 0;

        bool m_stopRfidDecode = false;
        bool m_stopReceive = false;

        byte[] m_write_data;
        uint m_current_offset, m_total_count, m_current_count, m_word_written;
        uint m_word_read;

        readonly object locker = new object();

        public RFIDReadForm(string epc)
        {
            InitializeComponent();

            this.CenterToScreen();

            tb_epc.Text = epc;

            Thread thread1 = new Thread(new ThreadStart(RecvThread));
            thread1.Start();

            m_stopRfidDecode = false;
            Thread thread2 = new Thread(new ThreadStart(DecodeRfidCommands));
            thread2.Start();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_stopRfidDecode = true;
            m_stopReceive = true;
        }

        private void RecvThread()
        {
            while (!m_stopReceive)
            {
                // Make sure that we are connected to a device
                if (HID.IsOpened())
                {
                    int bufferSize = HID.SIZE_MAX_READ;
                    byte[] buffer = new byte[bufferSize];
                    int bytesRead = 0;

                    if (USBSocket.ReceiveData(ref buffer, buffer.Length, ref bytesRead, 1000))
                    {
                        if (bytesRead > 0)
                        {
                            for (int i = 0; i < bytesRead; i++)
                            {
                                m_read_buffer[m_read_buffer_head++] = buffer[i];
                            }
                            m_read_buffer_size += (ushort)bytesRead;
                        }
                        while (m_read_buffer_size >= 8)
                        {
                            if (m_read_buffer[m_read_buffer_tail] == Constants.PREFIX)
                            {
                                if (!DecodeCommands())
                                {
                                    break;
                                }
                            }
                            else
                            {
                                //tb_status.Text = "Wrong prefix is received";
                                MessageBox.Show("Wrong prefix is received");
                                ClearReadBuffer();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Device is not connected.");
                    this.Close();
                    return;
                }
            }
        }

        private void ClearReadBuffer()
        {
            m_read_buffer_size = 0;
            m_read_buffer_head = 0;
            m_read_buffer_tail = 0;
        }

        private void ClearRfidBuffer()
        {
            m_rfid_buffer_size = 0;
            m_rfid_buffer_head = 0;
            m_rfid_buffer_tail = 0;
        }

        private ushort GetRfidBufferSize()
        {
            lock (this.locker)
            {
                return this.m_rfid_buffer_size;
            }
        }

        private bool DecodeCommands()
        {
            ushort index = m_read_buffer_tail;
            uint prefix = m_read_buffer[index++];
            uint connection = m_read_buffer[index++];
            uint payload_len = m_read_buffer[index++];
            uint total_len = payload_len + 8;
            uint source = m_read_buffer[index++];
            uint reserve = m_read_buffer[index++];
            uint direction = m_read_buffer[index++];

            uint crc = ((uint)m_read_buffer[index++] << 8) | (uint)m_read_buffer[index++];

            if (m_read_buffer_size < total_len) return false; //not enough data

            if (crc != 0 && !CRC.CheckCRC(m_read_buffer, m_read_buffer_tail, total_len, crc))
                UpdateInfo("Wrong CRC received.");

            uint event_code = ((uint)m_read_buffer[index++] << 8) | (uint)m_read_buffer[index++];
            if (source == Constants.TYPE_RFID)
            {
                switch (event_code)
                {
                    case 0x8000:
                        byte status = m_read_buffer[index++];
                        switch (status)
                        {
                            case 0x00:
                                UpdateInfo("Power on successed.");
                                break;
                            case 0xFF:
                                UpdateInfo("Power on failed with unknown reason.");
                                break;
                            default:
                                UpdateInfo("Unknown status for rfid.");
                                break;
                        }
                        break;
                    case 0x8001:
                        status = m_read_buffer[index++];
                        switch (status)
                        {
                            case 0x00:
                                UpdateInfo("Power off successed.");
                                ClearRfidBuffer();
                                Console.WriteLine("Power off successed");
                                break;
                            case 0xFF:
                                UpdateInfo("Power off failed with unknown reason.");
                                break;
                            default:
                                UpdateInfo("Unknown status for rfid.");
                                break;
                        }
                        break;
                    case 0x8002:
                        status = m_read_buffer[index++];
                        switch (status)
                        {
                            case 0x00:
                                //UpdateInfo("Send data successed.");
                                break;
                            case 0xFF:
                                UpdateInfo("Send data failed with unknown reason.");
                                break;
                            default:
                                UpdateInfo("Unknown status for rfid.");
                                break;
                        }
                        break;
                    case 0x8100:
                        ushort len = (ushort)(payload_len - 2);
                        index = m_read_buffer_tail;
                        index += 10;
                        lock (this.locker)
                        {
                            for (int i = 0; i < len; i++)
                            {
                                m_rfid_buffer[m_rfid_buffer_head++] = m_read_buffer[index++];
                            }
                            m_rfid_buffer_size += len;
                        }
                        break;
                    default:
                        UpdateInfo("Unknown event for rfid.");
                        break;
                }
            }
            else if (source == Constants.TYPE_NOTIFY)
            {
                switch (event_code)
                {
                    case 0xA000:
                        // battery percentage
                        break;
                    case 0xA100:
                        // battery fail
                        UpdateInfo("Battery fail.");
                        break; 
                    case 0xA101:
                        // error code
                        uint err_code = ((uint)m_read_buffer[index++] << 8) | (uint)m_read_buffer[index++];
                        UpdateInfo("Error code : " + err_code.ToString());
                        break;
                    default:
                        UpdateInfo("Unknown event for notification.");
                        break;
                }
            }
            else
            {
                UpdateInfo("Wrong source for rfid.");
            }

            m_read_buffer_size -= (byte)total_len;
            m_read_buffer_tail += (byte)total_len;
            //Console.WriteLine("Read Size:" + m_read_buffer_size);

            return true;
        }

        private void DecodeRfidCommands()
        {
            ushort index = 0;
            int prefix = 0;
            int command = 0;
            byte seq_no = 0;
            int payload_length = 0;
            int datalen = 0;
            Stopwatch stopwatch1 = new Stopwatch();
            Stopwatch stopwatch2 = new Stopwatch();
            byte[] PC = new byte[2];
            byte[] EPC = new byte[64];
            byte[] TID = new byte[32];
            float rssi;
            float phasebegin;
            float phaseend;
            int epclen, tidlen;
            int timestamp;
            int tagindex;

            stopwatch1.Start();
            stopwatch2.Start();
            while (!m_stopRfidDecode)
            {
                lock (this.locker)
                {
                    if (m_rfid_buffer_size >= 7)
                    {
                        stopwatch2.Reset();
                        stopwatch2.Start();
                        //get packet header
                        index = m_rfid_buffer_tail;
                        prefix = ((int)(m_rfid_buffer[index++]) << 8) + (int)(m_rfid_buffer[index++]);
                        command = ((int)(m_rfid_buffer[index++]) << 8) + (int)(m_rfid_buffer[index++]);
                        seq_no = m_rfid_buffer[index++];
                        payload_length = ((int)(m_rfid_buffer[index++]) << 8) + (int)(m_rfid_buffer[index++]);
                        m_rfid_buffer_size -= 7;
                        m_rfid_buffer_tail += 7;
                        datalen = payload_length; //payload
                        if (prefix != Constants.COMMAND_REPLY_PREFIX &&
                            prefix != Constants.UPLINK_PREFIX)
                        {
                            UpdateInfo("Unrecognized RFID packet prefix: " + prefix.ToString("X4"));
                            ClearReadBuffer();
                            ClearRfidBuffer();
                            continue;
                        }
                    }
                    else
                    {
                        Thread.Sleep(1); // save CPU usage
                        continue;
                    }
                }

                //wait until the full packet data has come in
                bool inCompletePacket = false;
                stopwatch1.Reset();
                stopwatch1.Start();
                while (GetRfidBufferSize() < datalen)
                {
                    if (stopwatch1.ElapsedMilliseconds >= 3000)
                    {
                        UpdateInfo("Incomplete packet returned.");
                        inCompletePacket = true;
                        break;
                    }
                    Thread.Sleep(1);
                }
                if (inCompletePacket)
                {
                    ClearRfidBuffer();
                    continue;
                }

                lock (this.locker)
                {
                    //finish reading
                    index = m_rfid_buffer_tail;
                    m_rfid_buffer_size -= (ushort)datalen;
                    m_rfid_buffer_tail += (ushort)datalen;
                    //Console.WriteLine("+Rfid Size:" + m_rfid_buffer_size);
                    if (prefix == Constants.COMMAND_REPLY_PREFIX)
                    {
                        if (command == Constants.RFID_ReadReg)
                        {
                            UpdateInfo("Read Register Reply Packet.");
                            continue;
                        }
                        else if (command == Constants.RFID_WriteReg)
                        {
                            int status = m_rfid_buffer[index++];
                            UpdateInfo("Write Register Reply Packet. Status = " + status.ToString("X4"));
                            continue;
                        }
                        else if (command == Constants.SCSLRFIDStartSimpleInventory)
                        {
                            UpdateInfo("SCSLRFIDStartSimpleInventory Reply Packet.");
                            continue;
                        }
                        else if (command == Constants.SCSLRFIDStartSelectInventory)
                        {
                            UpdateInfo("SCSLRFIDStartSelectInventory Reply Packet.");
                            continue;
                        }
                        else if (command == Constants.SCSLRFIDStartMBInventory)
                        {
                            UpdateInfo("SCSLRFIDStartMBInventory Reply Packet.");
                            continue;
                        }
                        else if (command == Constants.SCSLRFIDStartSelectMBInventory)
                        {
                            UpdateInfo("SCSLRFIDStartSelectMBInventory Reply Packet.");
                            continue;
                        }
                        else if (command == Constants.SCSLRFIDStartCompactInventory)
                        {
                            UpdateInfo("SCSLRFIDStartCompactInventory Reply Packet.");
                            continue;
                        }
                        else if (command == Constants.SCSLRFIDStartSelectCompactInventory)
                        {
                            UpdateInfo("SCSLRFIDStartSelectCompactInventory Reply Packet.");
                            continue;
                        }
                        else if (command == Constants.SCSLRFIDReadMB)
                        {
                            UpdateInfo("SCSLRFIDReadMB Reply Packet.");
                            continue;
                        }
                        else if (command == Constants.SCSLRFIDWriteMB)
                        {
                            UpdateInfo("SCSLRFIDWriteMB Reply Packet.");
                            continue;
                        }
                        else if (command == Constants.SCSLRFIDBlockWriteMB)
                        {
                            UpdateInfo("SCSLRFIDBlockWriteMB Reply Packet.");
                            continue;
                        }
                        else if (command == Constants.SCSLRFIDLock)
                        {
                            UpdateInfo("SCSLRFIDLock Reply Packet.");
                            continue;
                        }
                        else if (command == Constants.SCSLRFIDKill)
                        {
                            UpdateInfo("SCSLRFIDKill Reply Packet.");
                            continue;
                        }
                        else if (command == Constants.SCSLRFIDStopOperation)
                        {
                            UpdateInfo("SCSLRFIDStopOperation Reply Packet.");
                            continue;
                        }
                        else if (command == Constants.CSLSyncTime)
                        {
                            UpdateInfo("CSLSyncTime Reply Packet.");
                            continue;
                        }
                        else if (command == Constants.SCSLRFIDAuthenticate)
                        {
                            UpdateInfo("SCSLRFIDAuthenticate Reply Packet.");
                            continue;
                        }
                        else
                        {
                            UpdateInfo("Reply Packet: " + command.ToString("X4"));
                            continue;
                        }
                    }
                    else if (prefix == Constants.UPLINK_PREFIX)
                    {
                        byte[] data = new byte[datalen];
                        for (int cnt = 0; cnt < datalen; cnt++)
                        {
                            data[cnt] = m_rfid_buffer[index++];
                        }
                        if (command == Constants.csl_tag_read_epc_only_new)
                        {
                            PC[0] = data[15];
                            PC[1] = data[16];
                            epclen = ((PC[0] >> 3) & 0x1f) * 2;
                            tidlen = payload_length - 17 - epclen;
                            for (int cnt = 0; cnt < epclen; cnt++)
                            {
                                EPC[cnt] = data[17 + cnt];
                            }
                            if (tidlen > 0)
                            {
                                for (int cnt = 0; cnt < tidlen; cnt++)
                                {
                                    TID[cnt] = data[17 + epclen + cnt];
                                }
                            }
                            timestamp = (data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3];
                            rssi = (data[4] << 8) | data[5];
                            phasebegin = (data[6] << 8) | data[7];
                            phaseend = (data[8] << 8) | data[9];
                            tagindex = (data[13] << 8) | data[14];
                            UpdateResult("EPC: " + ByteArrayToHexString(EPC, epclen) + "\r\n");
                            continue;
                        }
                        else if (command == Constants.csl_tag_read_epc_only_recurrent)
                        {
                            continue;
                        }
                        else if (command == Constants.csl_tag_read_multibank_new)
                        {
                            continue;
                        }
                        else if (command == Constants.csl_tag_read_multibank_recurrent_index_only)
                        {
                            continue;
                        }
                        else if (command == Constants.csl_miscellaneous_event)
                        {
                            int event_code = ((int)(data[4]) << 8) + (int)(data[5]);
                            UpdateInfo("Miscellaneous event: " + event_code.ToString("X4"));
                            continue;
                        }
                        else if (command == Constants.csl_operation_complete)
                        {
                            int status = ((int)(data[6]) << 8) + (int)(data[7]);
                            UpdateInfo("Operation complete with status: " + status.ToString("X4"));
                            continue;
                        }
                        else if (command == Constants.csl_tag_read_compact)
                        {
                            continue;
                        }
                        else if (command == Constants.csl_access_complete)
                        {
                            int access_command = (data[4] << 8) | data[5];
                            int tag_error_code = data[6];
                            int mac_error_code = data[7];
                            int write_word_count = (data[8] << 8) | data[9];
                            int length = datalen - 12;
                            if (tag_error_code != 0x10)
                            {
                                UpdateResult(String.Format("Tag error code: {0:X2}\r\n", tag_error_code));
                                EnableCtrls(true);
                            }
                            else if (mac_error_code != 0)
                            {
                                UpdateResult(String.Format("MAC error code: {0:X4}\r\n", mac_error_code));
                                EnableCtrls(true);
                            }
                            else
                            {
                                if (access_command == 0xC2) // read
                                {
                                    UpdateResult(String.Format("Data: "));
                                    for (int cnt = 0; cnt < m_current_count * 2; cnt++)
                                        UpdateResult(String.Format("{0:X2}", data[12 + cnt]));
                                    m_word_read += m_current_count;
                                    m_current_offset += m_current_count;
                                    if (m_word_read < m_total_count)
                                    {
                                        calculateCurrentReadCount(m_total_count - m_word_read);
                                        readBlock(m_current_offset, m_current_count);
                                    }
                                    else
                                        EnableCtrls(true);
                                }
                                else if (access_command == 0xC3) // write
                                {
                                    UpdateResult(String.Format("Write success. Offset: {0}; Count: {1}; Written: {2}\r\n", m_current_offset, m_current_count, write_word_count));
                                    m_word_written += m_current_count;
                                    m_current_offset += m_current_count;
                                    if (m_word_written < m_total_count)
                                    {
                                        calculateCurrentWriteCount(m_total_count - m_word_written);
                                        writeBlock(m_current_offset, m_current_count, m_write_data, m_word_written * 2, 0);
                                    }
                                    else
                                        EnableCtrls(true);
                                }
                                else if (access_command == 0xC6) // Access
                                {
                                    UpdateResult(String.Format("Access success.\r\n"));
                                }
                                else if (access_command == 0xC7) // blockwrite
                                {
                                    UpdateResult(String.Format("BlockWrite success. Offset: {0}; Count: {1}; Written: {2}\r\n", m_current_offset, m_current_count, write_word_count));
                                    m_word_written += m_current_count;
                                    m_current_offset += m_current_count;
                                    if (m_word_written < m_total_count)
                                    {
                                        calculateCurrentWriteCount(m_total_count - m_word_written);
                                        writeBlock(m_current_offset, m_current_count, m_write_data, m_word_written * 2, 1);
                                    }
                                    else
                                        EnableCtrls(true);
                                }
                                else if (access_command == 0xC5) // Lock
                                {
                                    UpdateResult(String.Format("Lock success.\r\n"));
                                    EnableCtrls(true);
                                }
                                else if (access_command == 0xC4) // Kill
                                {
                                    UpdateResult(String.Format("Kill success.\r\n"));
                                    EnableCtrls(true);
                                }
                                else if (access_command == 0xD5) // Authenticate
                                {
                                    UpdateResult(String.Format("Data: "));
                                    for (int cnt = 0; cnt < m_current_count * 2; cnt++)
                                        UpdateResult(String.Format("{0:X2}", data[12 + cnt]));
                                    EnableCtrls(true);
                                }
                            }
                            continue;
                        }
                        else
                        {
                            UpdateInfo("Unsupported Uplink Packet: " + command.ToString("X4"));
                            continue;
                        }
                    }
                }
                Thread.Sleep(1); // save CPU usage
            }
            stopwatch1.Stop();
            stopwatch2.Stop();
        }

        private void btn_clear_info_Click(object sender, EventArgs e)
        {
            tb_info.Clear();
        }
        
        private byte[] HexStringToByteArray(String HexString)
        {
            if (HexString.Length % 2 == 1)
            {
                throw new Exception("Lenght of hex string cannot be odd.");
            }
            int NumberChars = HexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
            }

            return bytes;
        }

        private string ByteArrayToHexString(byte[] bytes, int length)
        {
            return BitConverter.ToString(bytes, 0, length).Replace("-", string.Empty);
        }

        private delegate void EnableCtrlsDeleg(bool enable);
        private void EnableCtrls(bool enable)
        {
            if (this.InvokeRequired)
            {
                Invoke(new EnableCtrlsDeleg(EnableCtrls), new object[] { enable });
                return;
            }
            btn_read.Enabled = enable;
            btn_write.Enabled = enable;
            btn_blockwrite.Enabled = enable;
            btn_lock.Enabled = enable;
            btn_kill.Enabled = enable;
        }

        private delegate void UpdateInfoDeleg(String info);
        private void UpdateInfo(String info)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateInfoDeleg(UpdateInfo), new object[] { info });
                return;
            }
            tb_info.AppendText(info);
            tb_info.AppendText("\r\n");
        }

        private delegate void UpdateResultDeleg(String result);
        private void UpdateResult(String result)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateResultDeleg(UpdateResult), new object[] { result });
                return;
            }
            tb_result.AppendText(result);
        }

        private void SetSelect()
        {
            ushort[] address = new ushort[1];
            byte[] length = new byte[1];
            byte[,] data = new byte[1, 42];
            uint enable, bank, pointer, masklength, target, action, delay;
            byte[] mask = new byte[32];

            enable = 1;
            bank = 1;
            pointer = 32;
            masklength = (uint)tb_epc.Text.Length * 4;
            target = 4;
            action = 0;
            delay = 0;
            mask = Tools.HexStringToByteArray(tb_epc.Text);

            address[0] = Constants.REG_SelectConfiguration;
            length[0] = 42;
            data[0, 0] = (byte)enable;
            data[0, 1] = (byte)bank;
            data[0, 2] = (byte)(pointer >> 24);
            data[0, 3] = (byte)(pointer >> 16);
            data[0, 4] = (byte)(pointer >> 8);
            data[0, 5] = (byte)(pointer & 0xFF);
            data[0, 6] = (byte)masklength;
            for (int i = 0; i < mask.Length; i++)
            {
                data[0, 7 + i] = mask[i];
            }
            data[0, 39] = (byte)target;
            data[0, 40] = (byte)action;
            data[0, 41] = (byte)delay;
            RFIDCommands.WriteRegisters(1, address, length, data);
            Thread.Sleep(5);
        }

        private void SetAccessPassword()
        {
            ushort[] address = new ushort[1];
            byte[] length = new byte[1];
            byte[,] data = new byte[1, 4];
            byte[] accesspwd = new byte[4];

            address[0] = Constants.REG_AccessPassword;
            length[0] = 4;
            accesspwd = Tools.HexStringToByteArray(tb_acc_pwd.Text);
            data[0, 0] = accesspwd[0];
            data[0, 1] = accesspwd[1];
            data[0, 2] = accesspwd[2];
            data[0, 3] = accesspwd[3];
            RFIDCommands.WriteRegisters(1, address, length, data);
            Thread.Sleep(5);
        }

        private void btn_read_Click(object sender, EventArgs e)
        {
            EnableCtrls(false);
            tb_result.Clear();

            SetSelect();
            SetAccessPassword();

            m_current_offset = Convert.ToUInt32(tb_ptr.Text);
            m_total_count = Convert.ToUInt32(tb_count.Text);
            m_word_read = 0;

            calculateCurrentReadCount(m_total_count);
            readBlock(m_current_offset, m_current_count);
        }

        private void calculateCurrentReadCount(uint remaining_count)
        {
            if (remaining_count > 253)
                m_current_count = 253;
            else
                m_current_count = remaining_count;
        }

        private void readBlock(uint offset, uint count)
        {
            ushort[] address = new ushort[1];
            byte[] length = new byte[1];
            byte[,] data = new byte[1, 14];
            uint enable1, bank1, pointer1, readlength1;

            enable1 = 1;
            bank1 = Convert.ToUInt32(tb_bank.Text);
            pointer1 = offset;
            readlength1 = count;

            address[0] = Constants.REG_MultibankReadConfig;
            length[0] = 7;
            data[0, 0] = (byte)enable1;
            data[0, 1] = (byte)bank1;
            data[0, 2] = (byte)(pointer1 >> 24);
            data[0, 3] = (byte)(pointer1 >> 16);
            data[0, 4] = (byte)(pointer1 >> 8);
            data[0, 5] = (byte)(pointer1 & 0xFF);
            data[0, 6] = (byte)readlength1;
            RFIDCommands.WriteRegisters(1, address, length, data);
            Thread.Sleep(10);

            RFIDCommands.SendSCommand(Constants.SCSLRFIDReadMB);
        }

        private void tb_data_TextChanged(object sender, EventArgs e)
        {
            lb_bits.Text = (tb_data.Text.Length*4).ToString();
        }

        private void btn_write_Click(object sender, EventArgs e)
        {
            EnableCtrls(false);
            tb_result.Clear();

            SetSelect();
            SetAccessPassword();

            m_current_offset = Convert.ToUInt32(tb_ptr.Text);
            m_total_count = Convert.ToUInt32(tb_count.Text);
            m_write_data = HexStringToByteArray(tb_data.Text.PadRight(2052, '0'));
            m_word_written = 0;

            calculateCurrentWriteCount(m_total_count);
            writeBlock(m_current_offset, m_current_count, m_write_data, m_word_written * 2, 0);
        }

        private void btn_lock_Click(object sender, EventArgs e)
        {
            ushort[] address = new ushort[2];
            byte[] length = new byte[2];
            byte[,] regdata = new byte[2, 2];
            byte[] lockmask = new byte[2];
            byte[] lockaction = new byte[2];

            EnableCtrls(false);
            tb_result.Clear();

            SetSelect();
            SetAccessPassword();

            address[0] = Constants.REG_LockMask;
            length[0] = 2;
            lockmask = Tools.HexStringToByteArray(tb_lockmask.Text);
            regdata[0, 0] = lockmask[0];
            regdata[0, 1] = lockmask[1];
            address[1] = Constants.REG_LockAction;
            length[1] = 2;
            lockaction = Tools.HexStringToByteArray(tb_lockaction.Text);
            regdata[1, 0] = lockaction[0];
            regdata[1, 1] = lockaction[1];
            RFIDCommands.WriteRegisters(2, address, length, regdata);
            Thread.Sleep(5);

            RFIDCommands.SendSCommand(Constants.SCSLRFIDLock);
        }

        private void btn_kill_Click(object sender, EventArgs e)
        {
            ushort[] address = new ushort[1];
            byte[] length = new byte[1];
            byte[,] regdata = new byte[1, 4];
            byte[] killpwd = new byte[4];

            EnableCtrls(false);
            tb_result.Clear();

            SetSelect();
            SetAccessPassword();

            address[0] = Constants.REG_KillPassword;
            length[0] = 4;
            killpwd = Tools.HexStringToByteArray(tb_kill_pwd.Text);
            regdata[0, 0] = killpwd[0];
            regdata[0, 1] = killpwd[1];
            regdata[0, 2] = killpwd[2];
            regdata[0, 3] = killpwd[3];
            RFIDCommands.WriteRegisters(1, address, length, regdata);
            Thread.Sleep(5);

            RFIDCommands.SendSCommand(Constants.SCSLRFIDKill);
        }

        private void btn_test_Click(object sender, EventArgs e)
        {
            EnableCtrls(false);
            tb_result.Clear();

            SetSelect();
            SetAccessPassword();

            ushort[] address = new ushort[1];
            byte[] length = new byte[1];
            byte[,] data = new byte[1, 32];
            uint cfg, rsplen;
            byte[] message = new byte[12];

            //cfg = 0xC007;
            cfg = 0x018003;
            rsplen = 128;

            /*message[0] = 0x00;
            message[1] = 0x22;
            message[2] = 0x33;
            message[3] = 0x44;
            message[4] = 0x55;
            message[5] = 0x66;*/
            message[0] = 0;
            message[1] = 0;
            message[2] = 253;
            message[3] = 93;
            message[4] = 128;
            message[5] = 72;
            message[6] = 244;
            message[7] = 141;
            message[8] = 208;
            message[9] = 154;
            message[10] = 173;
            message[11] = 34;

            /*message[0] &= (byte)0x3u;
            message[0] |= (byte)0x4u;*/

            address[0] = Constants.REG_AuthenticateConfig;
            length[0] = 3;
            data[0, 0] = (byte)(cfg >> 16);
            data[0, 1] = (byte)(cfg >> 8);
            data[0, 2] = (byte)(cfg & 0xFF);
            RFIDCommands.WriteRegisters(1, address, length, data);
            Thread.Sleep(10);

            address[0] = Constants.REG_AuthenticateRepLen;
            length[0] = 2;
            data[0, 0] = (byte)(rsplen >> 8);
            data[0, 1] = (byte)(rsplen & 0xFF);
            RFIDCommands.WriteRegisters(1, address, length, data);
            Thread.Sleep(10);

            address[0] = Constants.REG_AuthenticateMessage;
            /*length[0] = 6;
            data[0, 0] = message[0];
            data[0, 1] = message[1];
            data[0, 2] = message[2];
            data[0, 3] = message[3];
            data[0, 4] = message[4];
            data[0, 5] = message[5];*/
            length[0] = 12;
            data[0, 0] = message[0];
            data[0, 1] = message[1];
            data[0, 2] = message[2];
            data[0, 3] = message[3];
            data[0, 4] = message[4];
            data[0, 5] = message[5];
            data[0, 6] = message[6];
            data[0, 7] = message[7];
            data[0, 8] = message[8];
            data[0, 9] = message[9];
            data[0, 10] = message[10];
            data[0, 11] = message[11];
            RFIDCommands.WriteRegisters(1, address, length, data);
            Thread.Sleep(10);

            m_current_count = 8;

            RFIDCommands.SendSCommand(Constants.SCSLRFIDAuthenticate);
        }

        private void btn_blockwrite_Click(object sender, EventArgs e)
        {
            EnableCtrls(false);
            tb_result.Clear();

            SetSelect();
            SetAccessPassword();

            m_current_offset = Convert.ToUInt32(tb_ptr.Text);
            m_total_count = Convert.ToUInt32(tb_count.Text);
            m_write_data = HexStringToByteArray(tb_data.Text.PadRight(2052, '0'));
            m_word_written = 0;

            calculateCurrentWriteCount(m_total_count);
            writeBlock(m_current_offset, m_current_count, m_write_data, m_word_written * 2, 1);
        }

        private void calculateCurrentWriteCount(uint remaining_count)
        {
            if (m_current_offset < 256) // first 4096 bits
            {
                if (m_current_offset == 0 && remaining_count > 255)
                    m_current_count = 255;
                else if (m_current_offset + remaining_count > 256)
                    m_current_count = 256 - m_current_offset;
                else
                    m_current_count = remaining_count;
            }
            else // second 4096 bits
            {
                if (m_current_offset == 256 && remaining_count == 256)
                    m_current_count = 255;
                else
                    m_current_count = m_total_count - m_word_written;
            }
        }

        private void writeBlock(uint offset, uint count, byte[] data, uint data_offset, uint method)
        {
            ushort[] address = new ushort[3];
            byte[] length = new byte[3];
            byte[,] regdata = new byte[3, 255];
            uint i, enable1, bank1, pointer1, writelength1;
            uint writeSize, bytesWritten = 0, byte_length = count * 2;
            ushort reg_index;

            enable1 = 1;
            bank1 = Convert.ToUInt32(tb_bank.Text);
            pointer1 = offset;
            writelength1 = count;
            
            address[0] = Constants.REG_MultibankWriteConfig;
            regdata[0, 0] = (byte)enable1;
            regdata[0, 1] = (byte)bank1;
            regdata[0, 2] = (byte)(pointer1 >> 24);
            regdata[0, 3] = (byte)(pointer1 >> 16);
            regdata[0, 4] = (byte)(pointer1 >> 8);
            regdata[0, 5] = (byte)(pointer1 & 0xFF);
            regdata[0, 6] = (byte)writelength1;
            bytesWritten = 0;
            writeSize = ((byte_length - bytesWritten) < (255-7)) ? (byte_length - bytesWritten) : (255-7);
            for (i = 0; i < writeSize; i++)
            {
                regdata[0, 7+i] = data[data_offset++];
            }
            length[0] = (byte)(7 + writeSize);
            bytesWritten = writeSize;
            reg_index = 0;
            while (bytesWritten < byte_length)
            {
                ++reg_index;
                writeSize = ((byte_length - bytesWritten) < 255) ? (byte_length - bytesWritten) : 255;
                length[reg_index] = (byte)(writeSize);
                address[reg_index] = (ushort)(address[reg_index - 1] + 255);
                for (i = 0; i < writeSize; i++)
                {
                    regdata[reg_index, 7 + i] = data[data_offset++];
                }
                bytesWritten += writeSize;
            }

            RFIDCommands.WriteRegisters((ushort)(reg_index+1), address, length, regdata);
            Thread.Sleep(10);

            if (method == 0)
                RFIDCommands.SendSCommand(Constants.SCSLRFIDWriteMB);
            else if (method == 1)
                RFIDCommands.SendSCommand(Constants.SCSLRFIDBlockWriteMB);
        }
    }
}
