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
using System.Globalization;
using System.IO;

namespace CS710_PC_Client
{
    public partial class BtHidConfigForm : Form
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

        byte[] m_oem_data = new byte[112];

        readonly object locker = new object();

        List<string> key_items1 = new List<string> { "(empty)",
                                                     "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
                                                     "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                                                     "1", "2", "3", "4", "5", "6", "7", "8", "9", "0",
                                                     "Enter", "Space",
                                                     "-", "=", "[", "]", "\\", ";", "'", ",", ".", "/"};
        List<string> key_items2 = new List<string> { "(empty)",
                                                     "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                                                     "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                                                     "!", "@", "#", "$", "%", "^", "&", "*", "(", ")",
                                                     "Enter", "Space",
                                                     "_", "+", "{", "}", "|", ":", "\"", "<", ">", "?"};

        public BtHidConfigForm()
        {
            InitializeComponent();

            this.CenterToScreen();


            Thread thread1 = new Thread(new ThreadStart(RecvThread));
            thread1.Start();

            Thread thread2 = new Thread(new ThreadStart(DecodeRfidCommands));
            thread2.Start();

            comboBox_prefix1.Items.Clear();
            FillComboBox(comboBox_prefix1, key_items1);
            comboBox_prefix1.SelectedIndex = 0;
            comboBox_prefix2.Items.Clear();
            FillComboBox(comboBox_prefix2, key_items1);
            comboBox_prefix2.SelectedIndex = 0;
            comboBox_suffix1.Items.Clear();
            FillComboBox(comboBox_suffix1, key_items1);
            comboBox_suffix1.SelectedIndex = 0;
            comboBox_suffix2.Items.Clear();
            FillComboBox(comboBox_suffix2, key_items1);
            comboBox_suffix2.SelectedIndex = 0;

            get_config();
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
            else if (source == Constants.TYPE_ATMEL)
            {
                byte status;
                switch (event_code)
                {
                    case 0xB004:
                        byte[] buffer = new byte[payload_len - 2];

                        index = (ushort)(m_read_buffer_tail + (ushort)10);
                        for (int i = 0; i < payload_len - 2; i++)
                        {
                            buffer[i] = m_read_buffer[index++];
                        }

                        string ascii = System.Text.Encoding.UTF8.GetString(buffer);
                        break;
                    case 0xB005:
                        status = m_read_buffer[index++];
                        switch (status)
                        {
                            case 0x00:
                                UpdateInfo("Set SN successed.");
                                break;
                            case 0x01:
                                UpdateInfo("Set SN failed.");
                                break;
                            default:
                                UpdateInfo("Unknown status.");
                                break;
                        }
                        break;
                    case 0xB006:
                        buffer = new byte[payload_len - 2];

                        index = (ushort)(m_read_buffer_tail + (ushort)10);
                        for (int i = 0; i < payload_len - 2; i++)
                        {
                            buffer[i] = m_read_buffer[index++];
                        }

                        ascii = System.Text.Encoding.UTF8.GetString(buffer);
                        break;
                    case 0xB007:
                        status = m_read_buffer[index++];
                        switch (status)
                        {
                            case 0x00:
                                UpdateInfo("Set Model successed.");
                                break;
                            case 0x01:
                                UpdateInfo("Set Model failed.");
                                break;
                            default:
                                UpdateInfo("Unknown status.");
                                break;
                        }
                        break;
                    default:
                        UpdateInfo("Unknown event.");
                        break;
                }
            }
            else if (source == Constants.TYPE_BT)
            {
                byte status;
                switch (event_code)
                {
                    case 0xC006:
                        status = m_read_buffer[index++];
                        switch (status)
                        {
                            case 0x00:
                                UpdateInfo("Set BT mode successed.");
                                break;
                            case 0x01:
                                UpdateInfo("Set BT mode failed.");
                                break;
                            default:
                                UpdateInfo("Unknown status.");
                                break;
                        }
                        break;
                    default:
                        UpdateInfo("Unknown event.");
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
                            byte[] data = new byte[256];
                            for (int cnt = 0; cnt < RFIDCommands.RFIDRegisterTable.count; cnt++)
                            {
                                for (int i = 0; i < RFIDCommands.RFIDRegisterTable.length[cnt]; i++)
                                {
                                    data[i] = m_rfid_buffer[index++];
                                }
                                if (RFIDCommands.RFIDRegisterTable.address[cnt] == Constants.REG_CurrentMode)
                                {
                                    UpdateMode(data[0]);
                                }
                                else if (RFIDCommands.RFIDRegisterTable.address[cnt] == Constants.OEM_HidFormat)
                                {
                                    UpdateFormat(data[0]);
                                }
                                else if (RFIDCommands.RFIDRegisterTable.address[cnt] == Constants.OEM_HidPrefix1)
                                {
                                    UpdatePrefix1(data[1], data[0]);
                                }
                                else if (RFIDCommands.RFIDRegisterTable.address[cnt] == Constants.OEM_HidPrefix2)
                                {
                                    UpdatePrefix2(data[1], data[0]);
                                }
                                else if (RFIDCommands.RFIDRegisterTable.address[cnt] == Constants.OEM_HidSuffix1)
                                {
                                    UpdateSuffix1(data[1], data[0]);
                                }
                                else if (RFIDCommands.RFIDRegisterTable.address[cnt] == Constants.OEM_HidSuffix2)
                                {
                                    UpdateSuffix2(data[1], data[0]);
                                }
                                else if (RFIDCommands.RFIDRegisterTable.address[cnt] == Constants.OEM_HidIntervalDelay)
                                {
                                    UpdateIntervalDelay(((int)(data[0]) << 8) + (int)(data[1]));
                                }
                                else if (RFIDCommands.RFIDRegisterTable.address[cnt] == Constants.OEM_HidReportPerInterval)
                                {
                                    UpdateReportPerInterval(((int)(data[0]) << 8) + (int)(data[1]));
                                }
                                else if (RFIDCommands.RFIDRegisterTable.address[cnt] == Constants.OEM_HidBarcodeSupport)
                                {
                                    UpdateBarcodeSupport(data[0]);
                                }
                            }
                            UpdateInfo("Read Register Reply Packet.");
                            continue;
                        }
                        else if (command == Constants.RFID_WriteReg)
                        {
                            int status = m_rfid_buffer[index++];
                            UpdateInfo("Write Register Reply Packet. Status = " + status.ToString("X4"));
                            continue;
                        }
                        else
                        {
                            UpdateInfo("Reply Packet: " + command.ToString("X4"));
                            continue;
                        }
                    }
                }
                Thread.Sleep(1); // save CPU usage
            }
            stopwatch1.Stop();
            stopwatch2.Stop();
        }

        private delegate void UpdateInfoDeleg(String info);
        private void UpdateInfo(String info)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateInfoDeleg(UpdateInfo), new object[] { info });
                return;
            }
            tb_status.AppendText(info);
            tb_status.AppendText("\r\n");
        }

        private void btn_set_Click(object sender, EventArgs e)
        {
            byte[] command;
            uint delay, count;

            ushort[] address = new ushort[8];
            byte[] length = new byte[8];
            byte[,] data = new byte[8, 2];

            address[0] = Constants.OEM_HidFormat;
            length[0] = 1;
            data[0, 0] = rb_tlv.Checked ? (byte)0 : (byte)1;

            address[1] = Constants.OEM_HidPrefix1;
            length[1] = 2;
            data[1, 0] = cb_prefix1_enable_shift.Checked ? (byte)1 : (byte)0;
            data[1, 1] = (byte)Keyboard.GetKeyCode(comboBox_prefix1.SelectedItem.ToString());

            address[2] = Constants.OEM_HidPrefix2;
            length[2] = 2;
            data[2, 0] = cb_prefix2_enable_shift.Checked ? (byte)1 : (byte)0;
            data[2, 1] = (byte)Keyboard.GetKeyCode(comboBox_prefix2.SelectedItem.ToString());

            address[3] = Constants.OEM_HidSuffix1;
            length[3] = 2;
            data[3, 0] = cb_suffix1_enable_shift.Checked ? (byte)1 : (byte)0;
            data[3, 1] = (byte)Keyboard.GetKeyCode(comboBox_suffix1.SelectedItem.ToString());

            address[4] = Constants.OEM_HidSuffix2;
            length[4] = 2;
            data[4, 0] = cb_suffix2_enable_shift.Checked ? (byte)1 : (byte)0;
            data[4, 1] = (byte)Keyboard.GetKeyCode(comboBox_suffix2.SelectedItem.ToString());

            if (rb_fast_mode.Checked)
            {
                delay = 30;
                count = 5;
            }
            else
            {
                delay = 30;
                count = 1;
            }

            address[5] = Constants.OEM_HidIntervalDelay;
            length[5] = 2;
            data[5, 0] = (byte)(delay >> 8);
            data[5, 1] = (byte)(delay & 0xFF);

            address[6] = Constants.OEM_HidReportPerInterval;
            length[6] = 2;
            data[6, 0] = (byte)(count >> 8);
            data[6, 1] = (byte)(count & 0xFF);

            address[7] = Constants.OEM_HidBarcodeSupport;
            length[7] = 1;
            data[7, 0] = cb_barcode_support.Checked ? (byte)1 : (byte)0;

            RFIDCommands.WriteRegisters(8, address, length, data);

            if (cb_bt_mode.Checked)
                command = BluetoothCommands.SendBtMode(1);
            else
                command = BluetoothCommands.SendBtMode(0);

            if (!USBSocket.TransmitData(command, command.Length))
            {
                MessageBox.Show("Device failed to transmit data.");
                return;
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            rb_tlv.Checked = true;
            rb_basic_mode.Checked = true;
            cb_prefix1_enable_shift.Checked = false;
            cb_prefix2_enable_shift.Checked = false;
            cb_suffix1_enable_shift.Checked = false;
            cb_suffix2_enable_shift.Checked = false;
            comboBox_prefix1.SelectedIndex = 0;
            comboBox_prefix2.SelectedIndex = 0;
            comboBox_suffix1.SelectedIndex = 0;
            comboBox_suffix2.SelectedIndex = 0;
            cb_barcode_support.Checked = false;
            btn_set_Click(null, null);
        }

        private string ByteArrayToHexString(byte[] bytes, int length)
        {
            return BitConverter.ToString(bytes, 0, length).Replace("-", string.Empty);
        }

        private string ByteArrayToHexString(byte[] bytes, int offset, int length)
        {
            return BitConverter.ToString(bytes, offset, length).Replace("-", string.Empty);
        }

        private byte[] HexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] data = new byte[hexString.Length / 2];
            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }

        private void cb_prefix1_enable_shift_CheckedChanged(object sender, EventArgs e)
        {
            int selected = comboBox_prefix1.SelectedIndex;
            comboBox_prefix1.Items.Clear();
            if (cb_prefix1_enable_shift.Checked)
                FillComboBox(comboBox_prefix1, key_items2);
            else
                FillComboBox(comboBox_prefix1, key_items1);
            comboBox_prefix1.SelectedIndex = selected;
        }

        private void cb_prefix2_enable_shift_CheckedChanged(object sender, EventArgs e)
        {
            int selected = comboBox_prefix2.SelectedIndex;
            comboBox_prefix2.Items.Clear();
            if (cb_prefix2_enable_shift.Checked)
                FillComboBox(comboBox_prefix2, key_items2);
            else
                FillComboBox(comboBox_prefix2, key_items1);
            comboBox_prefix2.SelectedIndex = selected;
        }

        private void cb_suffix1_enable_shift_CheckedChanged(object sender, EventArgs e)
        {
            int selected = comboBox_suffix1.SelectedIndex;
            comboBox_suffix1.Items.Clear();
            if (cb_suffix1_enable_shift.Checked)
                FillComboBox(comboBox_suffix1, key_items2);
            else
                FillComboBox(comboBox_suffix1, key_items1);
            comboBox_suffix1.SelectedIndex = selected;
        }

        private void cb_suffix2_enable_shift_CheckedChanged(object sender, EventArgs e)
        {
            int selected = comboBox_suffix2.SelectedIndex;
            comboBox_suffix2.Items.Clear();
            if (cb_suffix2_enable_shift.Checked)
                FillComboBox(comboBox_suffix2, key_items2);
            else
                FillComboBox(comboBox_suffix2, key_items1);
            comboBox_suffix2.SelectedIndex = selected;
        }

        private void FillComboBox(dynamic control, IEnumerable<string> items)
        {
            foreach (var i in items)
            {
                control.Items.Add(i);
            }
        }

        private void get_config()
        {
            ushort[] address = new ushort[9];
            byte[] length = new byte[9];

            address[0] = Constants.REG_CurrentMode;
            length[0] = 1;

            address[1] = Constants.OEM_HidFormat;
            length[1] = 1;

            address[2] = Constants.OEM_HidPrefix1;
            length[2] = 2;

            address[3] = Constants.OEM_HidPrefix2;
            length[3] = 2;

            address[4] = Constants.OEM_HidSuffix1;
            length[4] = 2;

            address[5] = Constants.OEM_HidSuffix2;
            length[5] = 2;

            address[6] = Constants.OEM_HidIntervalDelay;
            length[6] = 2;

            address[7] = Constants.OEM_HidReportPerInterval;
            length[7] = 2;

            address[8] = Constants.OEM_HidBarcodeSupport;
            length[8] = 1;

            RFIDCommands.RFIDRegisterTable.ClearRegisterTable();
            for (int i = 0; i < 9; i++)
            {
                RFIDCommands.RFIDRegisterTable.AddRegisterTable(address[i], length[i], null);
            }
            RFIDCommands.ReadRegisters(9, address, length);
        }

        private delegate void UpdateModeDeleg(int mode);
        private void UpdateMode(int mode)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateModeDeleg(UpdateMode), new object[] { mode });
                return;
            }
            cb_bt_mode.Checked = (mode == 0) ? false : true;
        }

        private delegate void UpdateBarcodeSupportDeleg(int support);
        private void UpdateBarcodeSupport(int support)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateBarcodeSupportDeleg(UpdateBarcodeSupport), new object[] { support });
                return;
            }
            cb_barcode_support.Checked = (support == 0) ? false : true;
        }

        private delegate void UpdateFormatDeleg(int format);
        private void UpdateFormat(int format)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateFormatDeleg(UpdateFormat), new object[] { format });
                return;
            }
            rb_tlv.Checked = (format == 0) ? true : false;
            rb_raw.Checked = (format == 1) ? true : false;
        }

        private delegate void UpdatePrefix1Deleg(int code, int shift);
        private void UpdatePrefix1(int code, int shift)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdatePrefix1Deleg(UpdatePrefix1), new object[] { code, shift });
                return;
            }
            cb_prefix1_enable_shift.Checked = (shift == 0) ? false : true;
            comboBox_prefix1.SelectedItem = Keyboard.GetKeyString(code, (shift == 1));
        }

        private delegate void UpdatePrefix2Deleg(int code, int shift);
        private void UpdatePrefix2(int code, int shift)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdatePrefix2Deleg(UpdatePrefix2), new object[] { code, shift });
                return;
            }
            cb_prefix2_enable_shift.Checked = (shift == 0) ? false : true;
            comboBox_prefix2.SelectedItem = Keyboard.GetKeyString(code, (shift == 1));
        }

        private delegate void UpdateSuffix1Deleg(int code, int shift);
        private void UpdateSuffix1(int code, int shift)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateSuffix1Deleg(UpdateSuffix1), new object[] { code, shift });
                return;
            }
            cb_suffix1_enable_shift.Checked = (shift == 0) ? false : true;
            comboBox_suffix1.SelectedItem = Keyboard.GetKeyString(code, (shift == 1));
        }

        private delegate void UpdateSuffix2Deleg(int code, int shift);
        private void UpdateSuffix2(int code, int shift)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateSuffix2Deleg(UpdateSuffix2), new object[] { code, shift });
                return;
            }
            cb_suffix2_enable_shift.Checked = (shift == 0) ? false : true;
            comboBox_suffix2.SelectedItem = Keyboard.GetKeyString(code, (shift == 1));
        }

        private delegate void UpdateIntervalDelayDeleg(int value);
        private void UpdateIntervalDelay(int value)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateIntervalDelayDeleg(UpdateIntervalDelay), new object[] { value });
                return;
            }
            tb_interval_delay.Text = value.ToString();
        }

        private delegate void UpdateReportPerIntervalDeleg(int value);
        private void UpdateReportPerInterval(int value)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateReportPerIntervalDeleg(UpdateReportPerInterval), new object[] { value });
                return;
            }
            tb_report_per_interval.Text = value.ToString();
            if (value == 1)
            {
                rb_basic_mode.Checked = true;
            }
            else
            {
                rb_fast_mode.Checked = true;
            }
        }

        private void btn_inv_config_Click(object sender, EventArgs e)
        {
            m_stopRfidDecode = true;
            m_stopReceive = true;
            this.Hide();
            using (RFIDForm rfidForm = new RFIDForm())
            {
                rfidForm.ShowDialog();
            }
            this.Show();
            Thread thread1 = new Thread(new ThreadStart(RecvThread));
            thread1.Start();
            Thread thread2 = new Thread(new ThreadStart(DecodeRfidCommands));
            thread2.Start();
            m_stopRfidDecode = false;
            m_stopReceive = false;
        }
    }
}
