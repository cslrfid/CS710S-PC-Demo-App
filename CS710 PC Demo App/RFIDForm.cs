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
using System.IO;

namespace CS710_PC_Client
{
    public partial class RFIDForm : Form
    {
        private UpdateProgressForm progressform = null;

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
        bool m_startInventory = false;

        int m_totaltag = 0;
        int m_tagRate = 0;
        int m_elapsed = 0;
        uint m_mb_length = 0;

        byte[] m_oem_data = new byte[2049]; // OEM data
        int m_oem_data_index = 0;
        bool m_start_read_oem = false;

        readonly object locker = new object();

        const string DATETIMEFORMAT = "yyyy-MM-dd HH:mm:ss";

        public RFIDForm()
        {
            InitializeComponent();

            this.CenterToScreen();

            progressform = new UpdateProgressForm();

            EnableCtrls(false);
            btn_rfid_clear.Enabled = false;

            Thread thread1 = new Thread(new ThreadStart(RecvThread));
            thread1.Start();

            m_stopRfidDecode = false;
            Thread thread2 = new Thread(new ThreadStart(DecodeRfidCommands));
            thread2.Start();

            comboBox_rfmode.SelectedIndex = 3;
            comboBox_session.SelectedIndex = 0;
            comboBox_target.SelectedIndex = 0;
            comboBox_algorithm.SelectedIndex = 1;
            comboBox_region.SelectedIndex = 0;
            comboBox_sel.SelectedIndex = 0;
            comboBox_bank.SelectedIndex = 1;
            comboBox_selTarget.SelectedIndex = 4;
            comboBox_action.SelectedIndex = 0;
            comboBox_bank1.SelectedIndex = 2;
            comboBox_bank2.SelectedIndex = 3;

            comboBox_rx_atten.SelectedIndex = 0;
            comboBox_rx_pga1.SelectedIndex = 3;
            comboBox_rx_pga2.SelectedIndex = 0;
            comboBox_rx_pga3.SelectedIndex = 3;
            comboBox_rx_mixer.SelectedIndex = 1;

            m_mb_length = 0;

            get_invset();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_startInventory = false;
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

                    if (USBSocket.ReceiveData(ref buffer, buffer.Length, ref bytesRead, 500))
                    {
                        if (bytesRead > 0)
                        {
                            ResetTimer();
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
                                UpdateInfo("Wrong prefix is received");
                                //MessageBox.Show("Wrong prefix is received");
                                ClearReadBuffer();
                            }
                        }
                    }
                }
                else
                {
                    if (cb_reconnect.Checked)
                    {
                        UpdateInfo("Device is not connected. Reconnecting...");
                        Reconnect();
                    }
                    else
                    {
                        MessageBox.Show("Device is not connected.");
                        this.Close();
                        return;
                    }
                }
            }
        }

        private void Reconnect()
        {
            UpdateStatus("USB reconnecting at " + DateTime.Now.ToString(DATETIMEFORMAT));
            while (!HID.IsOpened())
            {
                Thread.Sleep(1);
            }
            UpdateStatus("USB reconnected at " + DateTime.Now.ToString(DATETIMEFORMAT));
            Restart();
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
                                EnableCtrls(true);
                                RFIDCommands.SyncTime();
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
                                EnableCtrls(false);
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
                                UpdateInfo("Send data successed.");
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
                        UpdateInfo("Battery level: " + (m_read_buffer[index++] << 8 | m_read_buffer[index++]).ToString());
                        break;
                    case 0xA002:
                        UpdateInfo("Start battery report.");
                        break;
                    case 0xA003:
                        UpdateInfo("Stop battery report.");
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
                    case 0xA102:
                        UpdateInfo("Trigger is pushed");
                        break;
                    case 0xA103:
                        UpdateInfo("Trigger is released");
                        break;
                    default:
                        UpdateInfo("Unknown event for notification." + event_code.ToString());
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
                    else {
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
                    StopInventory();
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
                            string version = "";
                            int build = 0, value = 0;
                            for (int cnt = 0; cnt < RFIDCommands.RFIDRegisterTable.count; cnt++)
                            {
                                for (int i = 0; i < RFIDCommands.RFIDRegisterTable.length[cnt]; i++)
                                {
                                    data[i] = m_rfid_buffer[index++];
                                }
                                if (RFIDCommands.RFIDRegisterTable.address[cnt] == 0x0008)
                                {
                                    version = System.Text.Encoding.ASCII.GetString(data).Trim('\0');
                                }
                                else if (RFIDCommands.RFIDRegisterTable.address[cnt] == 0x0028)
                                {
                                    build = (data[3] << 24) + (data[2] << 16) + (data[1] << 8) + (data[0]);
                                    UpdateVersion(version + "-" + build.ToString());
                                }
                                else if (RFIDCommands.RFIDRegisterTable.address[cnt] == Constants.REG_CountryEnum)
                                {
                                    value = (data[0] << 8) + (data[1]);
                                    UpdateCountry(value);
                                }
                                else if (RFIDCommands.RFIDRegisterTable.address[cnt] == Constants.REG_FrequencyChannelIndex)
                                {
                                    value = data[0];
                                    UpdateChannel(value);
                                }
                                else if (RFIDCommands.RFIDRegisterTable.address[cnt] == Constants.REG_AntennaPortConfig)
                                {
                                    UpdateInvConfig(data);
                                }
                                else if (RFIDCommands.RFIDRegisterTable.address[cnt] == Constants.REG_DuplicateEliminationRollingWindow)
                                {
                                    value = data[0];
                                    UpdateDE(value);
                                }
                                else if (RFIDCommands.RFIDRegisterTable.address[cnt] == Constants.REG_CurrentMode)
                                {
                                    if (data[0] == 1)
                                    {
                                        MessageBox.Show("Inventory is not supported in USB connection if reader in HID mode.\nIf you want to do inventory, please change reader to Normal mode.",
                                                        "Reader is in bluetooth HID mode!",
                                                        MessageBoxButtons.OK,
                                                        MessageBoxIcon.Warning);
                                    }
                                }
                                else
                                {
                                    UpdateInfo("Register Data: " + ByteArrayToHexString(data, RFIDCommands.RFIDRegisterTable.length[cnt]));
                                    if (m_start_read_oem)
                                    {
                                        for (int i = 0; i < RFIDCommands.RFIDRegisterTable.length[cnt]; i++)
                                        {
                                            m_oem_data[m_oem_data_index++] = data[i]; // OEM data
                                        }
                                    }
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
                        else if (command == Constants.SCSLRFIDStopOperation)
                        {
                            UpdateInfo("SCSLRFIDStopOperation Reply Packet.");
                            continue;
                        }
                        else if (command == Constants.SCSLRFIDRegisterReset)
                        {
                            UpdateInfo("SCSLRFIDRegisterReset Reply Packet.");
                            continue;
                        }
                        else if (command == Constants.CSLSyncTime)
                        {
                            UpdateInfo("CSLSyncTime Reply Packet.");
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
                            short raw = (short)((data[4] << 8) | data[5]);
                            rssi = (float)raw / 100;
                            phasebegin = (data[6] << 8) | data[7];
                            phaseend = (data[8] << 8) | data[9];
                            tagindex = (data[13] << 8) | data[14];
                            UpdateListView(tagindex, ByteArrayToHexString(PC, 2), ByteArrayToHexString(EPC, epclen), rssi, phasebegin, phaseend, timestamp, ByteArrayToHexString(TID, tidlen));
                            m_totaltag++;
                            continue;
                        }
                        else if (command == Constants.csl_tag_read_epc_only_recurrent)
                        {
                            timestamp = (data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3];
                            short raw = (short)((data[4] << 8) | data[5]);
                            rssi = (float)raw / 100;
                            phasebegin = (data[6] << 8) | data[7];
                            phaseend = (data[8] << 8) | data[9];
                            tagindex = (data[13] << 8) | data[14];
                            UpdateListViewIndex(tagindex, rssi, phasebegin, phaseend, timestamp);
                        }
                        else if (command == Constants.csl_tag_read_multibank_new)
                        {
                            int cnt = 0;
                            byte[] MB = new byte[m_mb_length];
                            PC[0] = data[15];
                            PC[1] = data[16];
                            epclen = ((PC[0] >> 3) & 0x1f) * 2;
                            for (cnt = 0; cnt < epclen; cnt++)
                            {
                                EPC[cnt] = data[17 + cnt];
                            }
                            int num_of_mb = data[17 + epclen];
                            int data_index = 17 + epclen + 1;
                            if (num_of_mb > 0)
                            {
                                for (cnt = 0; cnt < m_mb_length; cnt++)
                                {
                                    MB[cnt] = data[data_index + cnt];
                                }
                            }
                            timestamp = (data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3];
                            short raw = (short)((data[4] << 8) | data[5]);
                            rssi = (float)raw / 100;
                            phasebegin = (data[6] << 8) | data[7];
                            phaseend = (data[8] << 8) | data[9];
                            tagindex = (data[13] << 8) | data[14];
                            if (num_of_mb > 0)
                                UpdateListView(tagindex, ByteArrayToHexString(PC, 2), ByteArrayToHexString(EPC, epclen), rssi, phasebegin, phaseend, timestamp, ByteArrayToHexString(MB, (int)m_mb_length));
                            else
                                UpdateListView(tagindex, ByteArrayToHexString(PC, 2), ByteArrayToHexString(EPC, epclen), rssi, phasebegin, phaseend, timestamp, null);
                            m_totaltag++;
                            continue;
                        }
                        else if (command == Constants.csl_tag_read_multibank_recurrent_index_only)
                        {
                            timestamp = (data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3];
                            short raw = (short)((data[4] << 8) | data[5]);
                            rssi = (float)raw / 100;
                            phasebegin = (data[6] << 8) | data[7];
                            phaseend = (data[8] << 8) | data[9];
                            tagindex = (data[13] << 8) | data[14];
                            UpdateListViewIndex(tagindex, rssi, phasebegin, phaseend, timestamp);
                        }
                        else if (command == Constants.csl_miscellaneous_event)
                        {
                            int event_code = ((int)(data[4]) << 8) + (int)(data[5]);
                            if (datalen == 8)
                            {
                                int tagrate = ((int)(data[6]) << 8) + (int)(data[7]);
                                //UpdateInfo("tagrate: " + tagrate.ToString());
                                UpdateRate(tagrate);
                            }
                            else
                            {
                                UpdateInfo("Miscellaneous event: " + event_code.ToString("X4"));
                            }
                            continue;
                        }
                        else if (command == Constants.csl_operation_complete)
                        {
                            int status = ((int)(data[6]) << 8) + (int)(data[7]);
                            UpdateInfo("Operation complete with status: " + status.ToString("X4"));
                            ClearReadBuffer();
                            ClearRfidBuffer();
                            EnableCtrlsInventory(true);
                            m_startInventory = false;
                            timer_reset.Stop();
                            continue;
                        }
                        else if (command == Constants.csl_tag_read_compact)
                        {
                            int i;
                            timestamp = (data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3];
                            i = 6;
                            do
                            {
                                PC[0] = data[i++];
                                PC[1] = data[i++];
                                epclen = ((PC[0] >> 3) & 0x1f) * 2;
                                for (int cnt = 0; cnt < epclen; cnt++)
                                {
                                    EPC[cnt] = data[i + cnt];
                                }
                                i += epclen;
                                short raw = (short)((data[i] << 8) | data[i + 1]);
                                rssi = (float)raw / 100;
                                i += 2;
                                phasebegin = 0;
                                phaseend = 0;
                                tagindex = 0;
                                UpdateListView(tagindex, ByteArrayToHexString(PC, 2), ByteArrayToHexString(EPC, epclen), rssi, phasebegin, phaseend, timestamp, "");
                                m_totaltag++;
                            } while (i < payload_length);
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

        private void btn_rfid_poweron_Click(object sender, EventArgs e)
        {
            ushort[] address = new ushort[1];
            byte[] length = new byte[1];
            byte[,] data = new byte[1, 2];

            byte[] command = RFIDCommands.PowerOn(true);

            if (!USBSocket.TransmitData(command, command.Length))
            {
                MessageBox.Show("Device failed to transmit data.");
            }

            Thread.Sleep(100);

            address[0] = Constants.REG_EventPacketUplinkEnable;
            length[0] = 2;
            data[0, 0] = 0;
            data[0, 1] = 9;
            RFIDCommands.WriteRegisters(1, address, length, data);

            command = NotifyCommands.SetBatteryReport(true);

            if (!USBSocket.TransmitData(command, command.Length))
            {
                MessageBox.Show("Device failed to transmit data.");
            }
        }

        private void btn_rfid_poweroff_Click(object sender, EventArgs e)
        {
            byte[] command = RFIDCommands.PowerOn(false);

            if (!USBSocket.TransmitData(command, command.Length))
            {
                MessageBox.Show("Device failed to transmit data.");
            }

            command = NotifyCommands.SetBatteryReport(false);

            if (!USBSocket.TransmitData(command, command.Length))
            {
                MessageBox.Show("Device failed to transmit data.");
            }
        }

        private void btn_rfid_clear_Click(object sender, EventArgs e)
        {
            lv_tag.Items.Clear();
            UpdateTotal(0);
        }

        private void btn_clear_info_Click(object sender, EventArgs e)
        {
            tb_info.Clear();
        }

        private void btn_inventory_Click(object sender, EventArgs e)
        {
            StartInventory();
            EnableCtrlsInventory(false);
        }

        private void btn_stop_inventory_Click(object sender, EventArgs e)
        {
            StopInventory();
            EnableCtrlsInventory(true);
        }

        private void StartInventory()
        {
            //Start inventory
            if (cb_compact_mode.Checked)
            {
                if (cb_selenable.Checked)
                    RFIDCommands.SendSCommand(Constants.SCSLRFIDStartSelectCompactInventory);
                else
                    RFIDCommands.SendSCommand(Constants.SCSLRFIDStartCompactInventory);
            }
            else if (cb_selenable.Checked)
            {
                if (cb_bank1_enable.Checked || cb_bank2_enable.Checked)
                    RFIDCommands.SendSCommand(Constants.SCSLRFIDStartSelectMBInventory);
                else
                    RFIDCommands.SendSCommand(Constants.SCSLRFIDStartSelectInventory);
            }
            else
            {
                if (cb_bank1_enable.Checked || cb_bank2_enable.Checked)
                    RFIDCommands.SendSCommand(Constants.SCSLRFIDStartMBInventory);
                else
                    RFIDCommands.SendSCommand(Constants.SCSLRFIDStartSimpleInventory);
            }

            m_startInventory = true;
            m_totaltag = 0;
            m_elapsed = 0;
            timer_reset.Stop();
            timer_reset.Start();
        }

        private void StopInventory()
        {
            RFIDCommands.SendSCommand(Constants.SCSLRFIDStopOperation);

            m_startInventory = false;
            timer_reset.Stop();
        }

        private uint swapMSBLSB32bit(uint in_32bit)
        {
            int[] t_shift = new int[] {0,8,16,24};
            uint[] t_tmpVal = new uint[4];
            uint out_32bit;
            int j;
	
            out_32bit = 0;
            for(j=0; j<4; j++)
            {
	            t_tmpVal[j] = (in_32bit>>t_shift[j]) & 0xff;
	            out_32bit |= t_tmpVal[j]<<t_shift[3-j];
            }

            return out_32bit;
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
            btn_inventory.Enabled = enable;
            btn_stop_inventory.Enabled = enable;
            btn_version.Enabled = enable;
            btn_read.Enabled = enable;
            btn_invset.Enabled = enable;
            btn_selset.Enabled = enable;
            btn_mbset.Enabled = enable;
            btn_rxset.Enabled = enable;
            btn_read_register.Enabled = enable;
            btn_write_register.Enabled = enable;
            btn_read_oem.Enabled = enable;
            btn_write_oem.Enabled = enable;
        }

        private delegate void EnableCtrlsInventoryDeleg(bool enable);
        private void EnableCtrlsInventory(bool enable)
        {
            if (this.InvokeRequired)
            {
                Invoke(new EnableCtrlsInventoryDeleg(EnableCtrlsInventory), new object[] { enable });
                return;
            }
            btn_rfid_clear.Enabled = enable;
            btn_rfid_poweroff.Enabled = enable;
            btn_version.Enabled = enable;
            btn_read.Enabled = enable;
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

        private delegate void UpdateStatusDeleg(String status);
        private void UpdateStatus(String status)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateStatusDeleg(UpdateStatus), new object[] { status });
                return;
            }
            tb_status.AppendText(status);
            tb_status.AppendText("\r\n");
        }

        private delegate void UpdateVersionDeleg(String version);
        private void UpdateVersion(String version)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateInfoDeleg(UpdateVersion), new object[] { version });
                return;
            }
            tb_version.Text = version;
        }

        private delegate void UpdateCountryDeleg(int country);
        private void UpdateCountry(int country)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateCountryDeleg(UpdateCountry), new object[] { country });
                return;
            }
            comboBox_region.SelectedIndex = country;
        }

        private delegate void UpdateChannelDeleg(int channel);
        private void UpdateChannel(int channel)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateChannelDeleg(UpdateChannel), new object[] { channel });
                return;
            }
            tb_channel.Text = channel.ToString();
        }

        private delegate void UpdateDEDeleg(int interval);
        private void UpdateDE(int interval)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateDEDeleg(UpdateDE), new object[] { interval });
                return;
            }
            tb_deinterval.Text = interval.ToString();
        }

        private delegate void UpdateInvConfigDeleg(byte[] config);
        private void UpdateInvConfig(byte[] config)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateInvConfigDeleg(UpdateInvConfig), new object[] { config });
                return;
            }

            int power, rfmode;
            int session, target, toggle, algorithm, startq, minq, maxq, num_min_q_cycles, sel;
            int q_increase_use_query, q_decrease_use_query, tag_focus_enable, fast_id_enable, max_query;
            int InventoryRoundControl = 0, InventoryRoundControl2 = 0;

            power = (config[3] << 8) + (config[4]);
            InventoryRoundControl = (config[5] << 24) + (config[6] << 16) + (config[7] << 8) + (config[8]);
            InventoryRoundControl2 = (config[9] << 24) + (config[10] << 16) + (config[11] << 8) + (config[12]);
            toggle = config[13];
            rfmode = (config[14] << 8) + (config[15]);
            session = (InventoryRoundControl >> 19) & 0x03;
            target = (InventoryRoundControl >> 23) & 0x01;
            algorithm = (InventoryRoundControl >> 16) & 0x01;
            sel = (InventoryRoundControl >> 21) & 0x03;
            startq = (InventoryRoundControl) & 0x0F;
            minq = (InventoryRoundControl >> 8) & 0x0F;
            maxq = (InventoryRoundControl >> 4) & 0x0F;
            num_min_q_cycles = (InventoryRoundControl >> 12) & 0x0F;
            q_increase_use_query = (InventoryRoundControl >> 17) & 0x01;
            q_decrease_use_query = (InventoryRoundControl >> 18) & 0x01;
            tag_focus_enable = (InventoryRoundControl >> 26) & 0x01;
            fast_id_enable = (InventoryRoundControl >> 25) & 0x01;

            max_query = (InventoryRoundControl2) & 0xFF;

            tb_power.Text = power.ToString();
            comboBox_rfmode.SelectedItem = rfmode.ToString();
            comboBox_session.SelectedIndex = session;
            if (toggle == 1)
            {
                comboBox_target.SelectedIndex = 0;
            }
            else if (target == 0)
            {
                comboBox_target.SelectedIndex = 1;
            }
            else
            {
                comboBox_target.SelectedIndex = 2;
            }
            comboBox_algorithm.SelectedIndex = algorithm == 0 ? 1 : 0;
            comboBox_sel.SelectedIndex = sel;
            tb_startq.Text = startq.ToString();
            tb_minq.Text = minq.ToString();
            tb_maxq.Text = maxq.ToString();
            tb_min_q_cycle.Text = num_min_q_cycles.ToString();

            cb_q_increase_use_query.Checked = true;
            cb_q_increase_use_query.Checked = q_increase_use_query == 1 ? true : false;
            cb_q_decrease_use_query.Checked = q_decrease_use_query == 1 ? true : false;
            cb_tagfocus.Checked = tag_focus_enable == 1 ? true : false;
            cb_fastid.Checked = fast_id_enable == 1 ? true : false;

            tb_max_query.Text = max_query.ToString();
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();

            return dtDateTime;
        }

        private delegate void UpdateListViewIndexDeleg(int tagindex, float rssi, float phasebegin, float phaseend, int timestamp);
        private void UpdateListViewIndex(int tagindex, float rssi, float phasebegin, float phaseend, int timestamp)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateListViewIndexDeleg(UpdateListViewIndex), new object[] { tagindex, rssi, phasebegin, phaseend, timestamp });
                return;
            }

            int foundIndex = -1;
            int count = lv_tag.Items.Count;
            if (tagindex <= count)
            {
                foundIndex = tagindex - 1;
            }
            if (foundIndex >= 0)
            {
                ListViewItem listViewItem = lv_tag.Items[foundIndex];
                listViewItem.SubItems[0].Text = tagindex.ToString();
                listViewItem.SubItems[3].Text = rssi.ToString();
                listViewItem.SubItems[4].Text = (Convert.ToInt32(listViewItem.SubItems[4].Text) + 1).ToString();
                listViewItem.SubItems[5].Text = phasebegin.ToString();
                listViewItem.SubItems[6].Text = phaseend.ToString();
                listViewItem.SubItems[7].Text = UnixTimeStampToDateTime(timestamp).ToString();
            }
            else
            {
                UpdateInfo("Tag index not found!");
            }
        }

        private delegate void UpdateListViewDeleg(int tagindex, String PC, String EPC, float rssi, float phasebegin, float phaseend, int timestamp, String MB);
        private void UpdateListView(int tagindex, String PC, String EPC, float rssi, float phasebegin, float phaseend, int timestamp, String MB)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateListViewDeleg(UpdateListView), new object[] { tagindex, PC, EPC, rssi, phasebegin, phaseend, timestamp, MB });
                return;
            }

            int foundIndex = -1;
            int count = lv_tag.Items.Count;
            if (count > 0)
            {
                var item = lv_tag.FindItemWithText(EPC, true, 0);
                if (item != null)
                {
                    foundIndex = lv_tag.Items.IndexOf(item);
                }
            }
            if (foundIndex >= 0)
            {
                ListViewItem listViewItem = lv_tag.Items[foundIndex];
                listViewItem.SubItems[0].Text = tagindex.ToString();
                listViewItem.SubItems[1].Text = PC;
                listViewItem.SubItems[2].Text = EPC;
                listViewItem.SubItems[3].Text = rssi.ToString();
                listViewItem.SubItems[4].Text = (Convert.ToInt32(listViewItem.SubItems[4].Text)+1).ToString();
                listViewItem.SubItems[5].Text = phasebegin.ToString();
                listViewItem.SubItems[6].Text = phaseend.ToString();
                listViewItem.SubItems[7].Text = UnixTimeStampToDateTime(timestamp).ToString();
                listViewItem.SubItems[8].Text = MB;
            }
            else
            {
                string[] row = { tagindex.ToString(), PC, EPC, rssi.ToString(), "1", phasebegin.ToString(), phaseend.ToString(), UnixTimeStampToDateTime(timestamp).ToString(), MB };
                ListViewItem listViewItem = new ListViewItem(row);
                lv_tag.Items.Add(listViewItem);
                UpdateTotal(count + 1);
            }
        }

        private delegate void UpdateRateDeleg(int rate);
        private void UpdateRate(int rate)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateRateDeleg(UpdateRate), new object[] { rate });
                return;
            }
            lb_rate.Text = rate.ToString();
            lb_elapsed.Text = m_elapsed.ToString();
        }

        private delegate void UpdateTotalDeleg(int total);
        private void UpdateTotal(int total)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateTotalDeleg(UpdateTotal), new object[] { total });
                return;
            }
            lb_total.Text = total.ToString();
        }

        private delegate void ResetTimerDeleg();
        private void ResetTimer()
        {
            if (this.InvokeRequired)
            {
                Invoke(new ResetTimerDeleg(ResetTimer), new object[] { });
                return;
            }
            timer_reset.Stop();
            timer_reset.Start();
        }

        private delegate void RestartDeleg();
        private void Restart()
        {
            if (this.InvokeRequired)
            {
                Invoke(new RestartDeleg(Restart), new object[] { });
                return;
            }
            btn_rfid_poweroff_Click(null, null);
            Thread.Sleep(10);
            btn_rfid_poweron_Click(null, null);
            Thread.Sleep(500);
            btn_inventory_Click(null, null);
        }

        private void get_invset()
        {
            ushort[] address = new ushort[5];
            byte[] length = new byte[5];

            address[0] = Constants.REG_CountryEnum;
            length[0] = 2;

            address[1] = Constants.REG_AntennaPortConfig;
            length[1] = 16;

            address[2] = Constants.REG_FrequencyChannelIndex;
            length[2] = 1;

            address[3] = Constants.REG_DuplicateEliminationRollingWindow;
            length[3] = 1;

            address[4] = Constants.REG_CurrentMode;
            length[4] = 1;

            RFIDCommands.RFIDRegisterTable.ClearRegisterTable();
            RFIDCommands.RFIDRegisterTable.AddRegisterTable(address[0], length[0], null);
            RFIDCommands.RFIDRegisterTable.AddRegisterTable(address[1], length[1], null);
            RFIDCommands.RFIDRegisterTable.AddRegisterTable(address[2], length[2], null);
            RFIDCommands.RFIDRegisterTable.AddRegisterTable(address[3], length[3], null);
            RFIDCommands.RFIDRegisterTable.AddRegisterTable(address[4], length[4], null);
            RFIDCommands.ReadRegisters(5, address, length);
        }

        private void btn_invset_Click(object sender, EventArgs e)
        {
            ushort[] address = new ushort[6];
            byte[] length = new byte[6];
            byte[,] data = new byte[6,16];
            uint power, rfmode;
            uint session, target, toggle, algorithm, startq, minq, maxq, num_min_q_cycles, sel;
            uint q_increase_use_query, q_decrease_use_query, tag_focus_enable, fast_id_enable;
            uint InventoryRoundControl = 0, InventoryRoundControl2 = 0;

            power = Convert.ToUInt32(tb_power.Text);
            rfmode = Convert.ToUInt32(comboBox_rfmode.SelectedItem);
            session = (uint)comboBox_session.SelectedIndex;
            toggle = (uint)(comboBox_target.SelectedIndex == 0 ? 1 : 0); 
            target = (uint)(comboBox_target.SelectedIndex == 2 ? 1 : 0);
            algorithm = (uint)(comboBox_algorithm.SelectedIndex == 0 ? 1 : 0);
            sel = (uint)comboBox_sel.SelectedIndex;
            startq = Convert.ToUInt32(tb_startq.Text);
            minq = Convert.ToUInt32(tb_minq.Text);
            maxq = Convert.ToUInt32(tb_maxq.Text);
            num_min_q_cycles = Convert.ToUInt32(tb_min_q_cycle.Text);

            q_increase_use_query = (uint)(cb_q_increase_use_query.Checked ? 1 : 0);
            q_decrease_use_query = (uint)(cb_q_decrease_use_query.Checked ? 1 : 0);
            tag_focus_enable = (uint)(cb_tagfocus.Checked ? 1 : 0);
            fast_id_enable = (uint)(cb_fastid.Checked ? 1 : 0);     

            address[0] = Constants.REG_AntennaPortConfig;
            length[0] = 16;
            data[0, 0] = 1; // port enable
            data[0, 1] = 0; 
            data[0, 2] = 0; // dwell time = 0
            data[0, 3] = (byte)(power >> 8); 
            data[0, 4] = (byte)(power & 0xFF); // power

            InventoryRoundControl = (startq) | (maxq << 4) | (minq << 8) | (num_min_q_cycles << 12) | (algorithm << 16) |
                                    (q_increase_use_query << 17) | (q_decrease_use_query << 18) | (session << 19) |
                                    (sel << 21) | (target << 23) | (fast_id_enable << 25) | (tag_focus_enable << 26);
            data[0, 5] = (byte)(InventoryRoundControl >> 24);
            data[0, 6] = (byte)(InventoryRoundControl >> 16);
            data[0, 7] = (byte)(InventoryRoundControl >> 8);
            data[0, 8] = (byte)(InventoryRoundControl & 0xFF);
            InventoryRoundControl2 = Convert.ToUInt32(tb_max_query.Text) & 0xFF;
            data[0, 9] = (byte)(InventoryRoundControl2 >> 24);
            data[0, 10] = (byte)(InventoryRoundControl2 >> 16);
            data[0, 11] = (byte)(InventoryRoundControl2 >> 8);
            data[0, 12] = (byte)(InventoryRoundControl2 & 0xFF);
            data[0, 13] = (byte)toggle; // toggle
            data[0, 14] = (byte)(rfmode >> 8); // RF mode
            data[0, 15] = (byte)(rfmode & 0xFF); // RF mode

            address[1] = Constants.REG_DuplicateEliminationRollingWindow;
            length[1] = 1;
            data[1, 0] = (byte)Convert.ToUInt32(tb_deinterval.Text);

            // Set channel
            address[2] = Constants.REG_FrequencyChannelIndex;
            length[2] = 1;
            data[2, 0] = (byte)Convert.ToUInt32(tb_channel.Text);

            // Set region
            address[3] = Constants.REG_CountryEnum;
            length[3] = 2;
            data[3, 0] = 0;
            data[3, 1] = (byte)(comboBox_region.SelectedIndex);

            address[4] = Constants.REG_TagCacheStatus;
            length[4] = 1;
            data[4, 0] = (byte)(cb_tagcaching.Checked ? 1 : 0);
            RFIDCommands.WriteRegisters(5, address, length, data);

            /*address[0] = Constants.REG_EventPacketUplinkEnable;
            length[0] = 2;
            data[0, 0] = 0;
            data[0, 1] = 9;
            RFIDCommands.WriteRegisters(1, address, length, data);*/

            if (cb_set_default.Checked)
            {
                address[0] = Constants.OEM_AntennaPortConfig;
                address[1] = Constants.OEM_DuplicateEliminationRollingWindow;
                address[2] = Constants.OEM_FrequencyChannelIndex;
                address[3] = Constants.OEM_CountryEnum;
                address[4] = Constants.OEM_ConfigEnable;
                length[4] = 1;
                data[4, 0] = 1;
                RFIDCommands.WriteRegisters(5, address, length, data);
            }
        }

        private void btn_selset_Click(object sender, EventArgs e)
        {
            ushort[] address = new ushort[1];
            byte[] length = new byte[1];
            byte[,] data = new byte[1, 42];
            uint enable, bank, pointer, masklength, target, action, delay;
            byte[] mask = new byte[32];

            enable = (uint)(cb_selenable.Checked ? 1 : 0);
            bank = (uint)comboBox_bank.SelectedIndex;
            pointer = Convert.ToUInt32(tb_offset.Text);
            masklength = Convert.ToUInt32(tb_selLength.Text);
            target = (uint)comboBox_selTarget.SelectedIndex;
            action = (uint)comboBox_action.SelectedIndex;
            delay = 0;
            mask = Tools.HexStringToByteArray(tb_mask.Text);

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
        }

        private void btn_mbset_Click(object sender, EventArgs e)
        {
            ushort[] address = new ushort[1];
            byte[] length = new byte[1];
            byte[,] data = new byte[1, 14];
            uint enable1, bank1, pointer1, readlength1;
            uint enable2, bank2, pointer2, readlength2;

            enable1 = (uint)(cb_bank1_enable.Checked ? 1 : 0);
            bank1 = (uint)comboBox_bank1.SelectedIndex;
            pointer1 = Convert.ToUInt32(tb_bank1_offset.Text);
            readlength1 = Convert.ToUInt32(tb_bank1_length.Text);
            enable2 = (uint)(cb_bank2_enable.Checked ? 1 : 0);
            bank2 = (uint)comboBox_bank2.SelectedIndex;
            pointer2 = Convert.ToUInt32(tb_bank2_offset.Text);
            readlength2 = Convert.ToUInt32(tb_bank2_length.Text);

            m_mb_length = 0;
            if (enable1 > 0)
                m_mb_length += readlength1 * 2;
            if (enable2 > 0)
                m_mb_length += readlength2 * 2;

            address[0] = Constants.REG_MultibankReadConfig;
            length[0] = 14;
            data[0, 0] = (byte)enable1;
            data[0, 1] = (byte)bank1;
            data[0, 2] = (byte)(pointer1 >> 24);
            data[0, 3] = (byte)(pointer1 >> 16);
            data[0, 4] = (byte)(pointer1 >> 8);
            data[0, 5] = (byte)(pointer1 & 0xFF);
            data[0, 6] = (byte)readlength1;
            data[0, 7] = (byte)enable2;
            data[0, 8] = (byte)bank2;
            data[0, 9] = (byte)(pointer2 >> 24);
            data[0, 10] = (byte)(pointer2 >> 16);
            data[0, 11] = (byte)(pointer2 >> 8);
            data[0, 12] = (byte)(pointer2 & 0xFF);
            data[0, 13] = (byte)readlength2;
            RFIDCommands.WriteRegisters(1, address, length, data);
        }

        private void btn_version_Click(object sender, EventArgs e)
        {
            RFIDCommands.GetVersion();
        }

        private void readrate_Tick(object sender, EventArgs e)
        {
            m_tagRate = m_totaltag;
            m_totaltag = 0;
            if (m_startInventory) ++m_elapsed;
            //UpdateRate(m_tagRate);
        }

        private void btn_read_Click(object sender, EventArgs e)
        {
            if (lv_tag.SelectedItems.Count > 0)
            {
                m_stopReceive = true;
                this.Hide();
                using (RFIDReadForm rfidreadForm = new RFIDReadForm(lv_tag.SelectedItems[0].SubItems[2].Text))
                {
                    rfidreadForm.ShowDialog();
                }
                this.Show();
                m_stopReceive = false;
                Thread thread1 = new Thread(new ThreadStart(RecvThread));
                thread1.Start();
                btn_invset_Click(null, null);
            }
            else
                MessageBox.Show("Please select an EPC first.");
        }

        private void cb_reconnect_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.AutoReconnect = cb_reconnect.Checked;
        }

        private void timer_reset_Tick(object sender, EventArgs e)
        {
            if (m_startInventory && HID.IsOpened())
            {
                /*byte[] command = AtmelCommands.Reset();

                if (!USBSocket.TransmitData(command, command.Length))
                {
                    MessageBox.Show("Device failed to transmit data.");
                }
                UpdateStatus("Reset at " + DateTime.Now.ToString(DATETIMEFORMAT));
                m_startInventory = false;*/
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            if (HID.IsOpened())
            {
                /*byte[] command = AtmelCommands.Reset();

                if (!USBSocket.TransmitData(command, command.Length))
                {
                    MessageBox.Show("Device failed to transmit data.");
                }
                UpdateStatus("Reset at " + DateTime.Now.ToString(DATETIMEFORMAT));
                m_startInventory = false;*/
                RFIDCommands.SendSCommand(Constants.SCSLRFIDRegisterReset);
            }
        }

        private void btn_read_register_Click(object sender, EventArgs e)
        {
            ushort[] address = new ushort[1];
            byte[] length = new byte[1];

            address[0] = (ushort)Convert.ToUInt32(tb_address.Text, 16);
            length[0] = (byte)Convert.ToUInt32(tb_length.Text);

            RFIDCommands.RFIDRegisterTable.ClearRegisterTable();
            RFIDCommands.RFIDRegisterTable.AddRegisterTable(address[0], length[0], null);
            RFIDCommands.ReadRegisters(1, address, length);
        }

        private void btn_write_register_Click(object sender, EventArgs e)
        {
            ushort[] address = new ushort[1];
            byte[] length = new byte[1];
            byte[,] data = new byte[1, 32];
            byte[] value = new byte[32];

            address[0] = (ushort)Convert.ToUInt32(tb_address.Text, 16);
            length[0] = (byte)Convert.ToUInt32(tb_length.Text);
            value = Tools.HexStringToByteArray(tb_register_value.Text);
            for (int i = 0; i < value.Length; i++)
            {
                data[0, i] = value[i];
            }

            RFIDCommands.RFIDRegisterTable.ClearRegisterTable();
            RFIDCommands.RFIDRegisterTable.AddRegisterTable(address[0], length[0], null);
            RFIDCommands.WriteRegisters(1, address, length, data);
        }

        private void btn_rxset_Click(object sender, EventArgs e)
        {
            ushort[] address = new ushort[1];
            byte[] length = new byte[1];
            byte[,] data = new byte[1, 2];

            uint attenuator, pga1, pga2, pga3, mixer, rxgain;

            //FC48

            attenuator = (uint)comboBox_rx_atten.SelectedIndex;
            pga1 = (uint)comboBox_rx_pga1.SelectedIndex;
            pga2 = (uint)comboBox_rx_pga2.SelectedIndex;
            pga3 = (uint)comboBox_rx_pga3.SelectedIndex;
            mixer = (uint)comboBox_rx_mixer.SelectedIndex;
            rxgain = (attenuator) | (pga1 << 2) | (pga2 << 4) | (pga3 << 6) | (mixer << 10) | (0x0 << 12) | (0x1 << 14);

            address[0] = 0x0508;
            length[0] = 2;
            data[0, 0] = (byte)(rxgain & 0xFF);
            data[0, 1] = (byte)(rxgain >> 8);

            RFIDCommands.RFIDRegisterTable.ClearRegisterTable();
            RFIDCommands.RFIDRegisterTable.AddRegisterTable(address[0], length[0], null);
            RFIDCommands.WriteRegisters(1, address, length, data);
        }

        private void ReadOEM(FileStream fs)
        {
            ushort[] address = new ushort[16];
            byte[] length = new byte[16];

            m_start_read_oem = true;
            address[0] = 0xE800;
            length[0] = 128;
            m_oem_data_index = 0;
            for (int i = 0; i < 16; i++)
            {
                RFIDCommands.RFIDRegisterTable.ClearRegisterTable();
                RFIDCommands.RFIDRegisterTable.AddRegisterTable(address[0], length[0], null);
                RFIDCommands.ReadRegisters(1, address, length);
                address[0] += 128;

                while (m_oem_data_index < (i+1)*128) Thread.Sleep(1);
            }
            m_start_read_oem = false;

            fs.Write(m_oem_data, 0, 2048);
            fs.Close();
        }

        private delegate void ShowUpdateResultDeleg(bool success, int wait);
        private void ShowUpdateResult(bool success, int wait)
        {
            if (this.InvokeRequired)
            {
                Invoke(new ShowUpdateResultDeleg(ShowUpdateResult), new object[] { success, wait });
                return;
            }

            if (success)
            {
                progressform.UpdateProgressResult("OEM Upload Success.");
                progressform.Show();
                progressform.Refresh();
            }
            else
            {
                progressform.UpdateProgressResult("OEM Update Fail.");
                progressform.Show();
                progressform.Refresh();
            }
        }

        private delegate void ShowUpdatePercentDeleg(int percent);
        private void ShowUpdatePercent(int percent)
        {
            if (this.InvokeRequired)
            {
                Invoke(new ShowUpdatePercentDeleg(ShowUpdatePercent), new object[] { percent });
                return;
            }

            progressform.UpdateProgressPercent(percent);
            progressform.Show();
            progressform.Refresh();
        }

        private void WriteOEM(FileStream stream)
        {
            if (stream.Length != 2048)
            {
                MessageBox.Show("Incorrect OEM file size.");
                return;
            }

            int len, total_subpart, subpart = 0, size = (int)stream.Length;
            int timeout = 1000;
            byte[] subpartBuffer = new byte[240];
            byte[] readBuffer = new byte[256];
            byte[] crc = new byte[2];
            byte[] command;
            total_subpart = (int)(Math.Ceiling((double)stream.Length / 234));

            btn_rfid_poweroff_Click(null, null);
            Thread.Sleep(500);

            m_stopReceive = true;
            Thread.Sleep(500);

            while (true)
            {
                if (subpart == 0)
                {
                    subpartBuffer[0] = (byte)(size >> 24);
                    subpartBuffer[1] = (byte)(size >> 16);
                    subpartBuffer[2] = (byte)(size >> 8);
                    subpartBuffer[3] = (byte)(size);
                    command = RFIDCommands.SendOEMData(subpartBuffer, total_subpart, 0, 4);

                    stream.Seek(0, SeekOrigin.Begin);
                }
                else
                {
                    len = stream.Read(subpartBuffer, 0, 234);
                    if (len > 0)
                    {
                        command = RFIDCommands.SendOEMData(subpartBuffer, total_subpart, subpart, len);
                    }
                    else
                    {
                        MessageBox.Show("Not enough OEM data.");
                        return;
                    }
                }

                if (!USBSocket.TransmitData(command, command.Length))
                {
                    MessageBox.Show("Device failed to transmit data.");
                    return;
                }

                int bytesRead = 0;

                if (subpart == total_subpart)
                    timeout = 10000;
                else
                    timeout = 1000;

                // Make sure that we are connected to a device
                if (HID.IsOpened())
                {
                    if (USBSocket.ReceiveData(ref readBuffer, readBuffer.Length, ref bytesRead, timeout))
                    {
                        if ((readBuffer[0] == Constants.PREFIX) && (bytesRead >= 11) &&
                            (readBuffer[8] == 0x80) && (readBuffer[9] == 0x04))
                        {
                            if (readBuffer[10] == 1)
                            {
                                ShowUpdateResult(false, 0);
                                break;
                            }
                            else if (readBuffer[10] == 2)
                            {
                                ShowUpdatePercent(100);
                                ShowUpdateResult(true, 0);
                                break;
                            }
                            else
                            {
                                ShowUpdatePercent((subpart * 100) / total_subpart);
                                ++subpart;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Cannot receive correct reply.");
                            break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Reply timeout.");
                        break;
                    }
                }
                else
                {
                    MessageBox.Show("Device is not connected.");
                    break;
                }
            }
            stream.Close();

            m_stopReceive = false;
            Thread thread1 = new Thread(new ThreadStart(RecvThread));
            thread1.Start();
        }

        private void btn_read_oem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog ofile = new SaveFileDialog())
                {
                    ofile.Title = "Save an OEM file";
                    ofile.Filter = "BIN files (*.bin)|*.bin";
                    ofile.ShowDialog();
                    // If the file name is not an empty string open it for saving.
                    if (ofile.FileName != "")
                    {
                        FileStream fs = (FileStream)ofile.OpenFile();

                        Thread thread = new Thread(() => ReadOEM(fs));
                        thread.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_write_oem_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofile = new OpenFileDialog())
                {
                    ofile.Title = "Open an OEM file";
                    ofile.Filter = "BIN files (*.bin)|*.bin";
                    if (ofile.ShowDialog() == DialogResult.OK)
                    {
                        FileStream fs = (FileStream)ofile.OpenFile();

                        Thread thread = new Thread(() => WriteOEM(fs));
                        thread.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_reset_factory_Click(object sender, EventArgs e)
        {
            RFIDCommands.SendSCommand(Constants.SCSLRFIDRegisterReset);
            get_invset();
        }
    }
}
