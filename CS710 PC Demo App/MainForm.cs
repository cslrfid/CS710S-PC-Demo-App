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
using System.IO;
using System.Diagnostics;

namespace CS710_PC_Client
{
    public partial class MainForm : Form
    {
        byte[] m_read_buffer = new byte[65536]; // read ring buffer
        ushort m_read_buffer_size = 0;
        ushort m_read_buffer_head = 0;
        ushort m_read_buffer_tail = 0;

        byte[] m_rfid_buffer = new byte[65536]; // rfid ring buffer
        ushort m_rfid_buffer_size = 0;
        ushort m_rfid_buffer_head = 0;
        ushort m_rfid_buffer_tail = 0;

        bool m_Connected = false;
        bool m_isBL = false;
        static bool m_AutoReconnect = true;
        private UpdateProgressForm progressform = null;
        private string m_SN;
        bool m_stopRfidDecode = true;
        bool m_stopReceive = true;

        // Firmware versions captured from the reader during Connect(), used by the
        // automated firmware-upgrade flow.
        string m_atmelVersion = "";
        string m_btVersion = "";
        volatile bool m_autoUpgradeRunning = false;

        readonly object locker = new object();

        public static bool AutoReconnect
        {
            get { return m_AutoReconnect; }
            set
            {
                m_AutoReconnect = value;
            }
        }

        public MainForm()
        {
            InitializeComponent();

            this.CenterToScreen();

            UsbNotification.RegisterUsbDeviceNotification(this.Handle);
            EnableDeviceCtrls(false);
            HideAllButtons(this);
            UpdateDeviceList();

            progressform = new UpdateProgressForm();

            // Auto-connect if a reader is already attached at launch (no plug-in event fires
            // for a device that was present before the app started).
            if (cb_device.Items.Count > 0)
            {
                if (Connect()) m_Connected = true;
            }
        }

        // Hide every Button on the form (buttons are nested inside GroupBoxes, so recurse).
        private void HideAllButtons(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Button)
                    c.Visible = false;
                else
                    HideAllButtons(c);
            }
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == UsbNotification.WmDevicechange)
            {
                // Get the message event type
                int nEventType = m.WParam.ToInt32();

                // Check for devices being connected or disconnected
                if (nEventType == UsbNotification.DbtDevicearrival ||
                    nEventType == UsbNotification.DbtDeviceremovecomplete)
                {
                    UsbNotification.DevBroadcastHdr hdr = new UsbNotification.DevBroadcastHdr();

                    // Convert lparam to DevBroadcastHdr structure
                    Marshal.PtrToStructure(m.LParam, hdr);

                    if (hdr.dbch_devicetype == UsbNotification.DbtDevtypDeviceinterface)
                    {
                        UsbNotification.DevBroadcastDeviceinterface1 devIF = new UsbNotification.DevBroadcastDeviceinterface1();

                        // Convert lparam to DevBroadcastDeviceinterface1 structure
                        Marshal.PtrToStructure(m.LParam, devIF);

                        // Get the device path from the broadcast message
                        string devicePath = new string(devIF.dbcc_name);

                        // Remove null-terminated data from the string
                        int pos = devicePath.IndexOf((char)0);
                        if (pos != -1)
                        {
                            devicePath = devicePath.Substring(0, pos);
                        }

                        // An HID device was removed
                        if (nEventType == UsbNotification.DbtDeviceremovecomplete)
                        {
                            string deviceString = "";

					        // Check if our device was removed by device path
                            if (HID.GetString(ref deviceString) == HID.HID_DEVICE_SUCCESS)
					        {
                                // Disconnect from it
                                Disconnect();
                                m_Connected = false;
					        }
                        }
                        // An HID device was connected
                        else if (nEventType == UsbNotification.DbtDevicearrival)
                        {
                            Thread.Sleep(100);
                        }

                        UpdateDeviceList();

                        if (m_AutoReconnect)
                            if (Connect()) m_Connected = true;
                    }
                }
            }
        }

        // Populate the device list combo box with connected device path strings
        // - Save previous device path string selection
        // - Fill the device list with connected device path strings
        // - Restore previous device selection
        private void UpdateDeviceList()
        {
            // Only update the combo list when the drop down list is closed
            if (!cb_device.DroppedDown)
            {
                int sel;
		        string path = "";
                string deviceString = ""; 

		        // Get previous combobox string selection
		        GetSelectedDevice(ref path);

                // Remove all strings from the combobox
                cb_device.Items.Clear();
                cb_device.ResetText();

                try
                {
                    // Iterate through each HID device with matching VID/PID
                    for (int i = 0; i < HID.GetNumHidDevices(); i++)
                    {
                        // Add path strings to the combobox
                        if (HID.GetString(ref deviceString) == HID.HID_DEVICE_SUCCESS)
                        {
                            cb_device.Items.Add(deviceString);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (cb_device.Items.Count > 0)
                {
                    sel = cb_device.FindStringExact(path, 0);

                    // Restore previous combobox selection
                    if (sel < 0)
                    {
                        cb_device.SelectedIndex = 0;
                    }
                    // Select first combobox string
                    else
                    {
                        cb_device.SelectedIndex = sel;
                    }
                }
            }
        }

        // Connect to the device with the path string selected
        // in the device list
        // - Connect to the device specified in the device list
        // - Enable/disable device combobox
        private bool Connect()
        {
            bool        connected = false;

            int status = HID.Open();


            // Attempt to open the device
			if (status == HID.HID_DEVICE_SUCCESS)
			{
				connected = true;
                GetAtmelVersion();
                if (!m_isBL)
                {
                    Thread.Sleep(500);
                    status = GetSN();
                    if (status == 1)
                    {
                        status = GetSN(); // retry one more time
                        if (status == 1)
                        {
                            MessageBox.Show("Cannot get SN.");
                        }
                    }
                    status = GetBTVersion();
                    if (status == 1)
                    {
                        status = GetBTVersion(); // retry one more time
                        if (status == 1)
                        {
                            MessageBox.Show("Cannot get bluetooth firmware version.");
                        }
                    }
                    // Note: RFID power-on is intentionally NOT started here. Its receive/decode
                    // threads read the same USB HID endpoint and would corrupt the firmware
                    // upload handshake. The automated upgrade is triggered below instead.
                }
            }
			else
			{
                MessageBox.Show("Failed to connect.");
			}

	        // Connected
	        if (connected)
	        {
		        // Update Connect/Disconnect button caption
                btn_connect.Text = "Disconnect";

		        // Disable the device combobox
		        cb_device.Enabled = false;

                if (m_isBL)
                {
                    btn_atmel_image.Enabled = true;
                }
                else
                    // Enable the device controls
                    EnableDeviceCtrls(true);

                // Kick off the automated firmware-version check / upgrade.
                StartAutoUpgrade();
	        }
	        // Disconnected
	        else
	        {
		        // Update Connect/Disconnect button caption
                btn_connect.Text = "Connect";

		        // Enable the device combobox
                cb_device.Enabled = true;

		        // Disable the device controls
		        EnableDeviceCtrls(false);
	        }

	        return connected;
        }

        // Disconnect from the currently connected device
        // - Disconnect from the current device
        // - Output any error messages
        // - Enable/disable device combobox
        private bool Disconnect()
        {
	        bool disconnected = false;

            m_stopReceive = true;
            m_stopRfidDecode = true;
            tb_info.Clear();

            // Disconnect from the current device
            int status = HID.Close();

	        // Output an error message if the close failed
	        if (status != HID.HID_DEVICE_SUCCESS)
	        {
                MessageBox.Show("Failed to disconnect.");
	        }
	        else
	        {
		        disconnected = true;
	        }

	        // Update Connect/Disconnect button caption
            btn_connect.Text = "Connect";

	        // Enable the device combobox
            cb_device.Enabled = true;

	        // Disable the device controls
	        EnableDeviceCtrls(false);

	        return disconnected;
        }

        // Get the combobox device selection
        // If a device is selected, return TRUE and return the path string
        // Otherwise, return FALSE
        private bool GetSelectedDevice(ref string path)
        {
	        bool selected = false;

	        int			sel;
	        string		selText;

	        // Get current selection index or CB_ERR(-1)
	        // if no device is selected
	        sel = cb_device.SelectedIndex;

	        if (sel >= 0)
	        {
		        // Get the selected device string
                selText = cb_device.SelectedItem.ToString();
		        selected = true;
		        path = selText;
	        }

	        return selected;
        }

        // Search for an HID device with a matching device path string
        // If the device was found return TRUE and return the device number
        // in deviceNumber
        // Otherwise return FALSE
        private bool FindDevice(string path, ref int deviceNum)
        {
	        bool found = false;
            string deviceString = "";
            
            try
            {
                // Iterate through each HID device with matching VID/PID
                for (int i = 0; i < HID.GetNumHidDevices(); i++)
                {
                    if (HID.GetString(ref deviceString) == HID.HID_DEVICE_SUCCESS)
                    {
                        if ((path.Equals(deviceString, StringComparison.OrdinalIgnoreCase)) == true)
                        {
                            found = true;
                            deviceNum = i;
                        }
                    }

                    if (found) break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

	        return found;
        }

        private void GetAtmelVersion()
        {
            byte[] buffer = new byte[128];
            int bytesRead = 0;

            byte[] command = AtmelCommands.GetVersion();

            if (!USBSocket.TransmitData(command, command.Length))
            {
                MessageBox.Show("Device failed to transmit data.");
                return;
            }

            // Make sure that we are connected to a device
            if (HID.IsOpened())
            {
                if (USBSocket.ReceiveData(ref buffer, buffer.Length, ref bytesRead, 1000))
                {
                    if ((buffer[0] == Constants.PREFIX) && (bytesRead >= 13) &&
                        (buffer[8] == 0xB0) && (buffer[9] == 0x00))
                    {
                        uint crc = ((uint)buffer[6] << 8) | (uint)buffer[7];

                        if (crc != 0 && !CRC.CheckCRC(buffer, 0, 13, crc))
                        {
                            MessageBox.Show("Wrong AtmelVersion CRC received.");
                            return;
                        }

                        m_atmelVersion = buffer[10].ToString() + "." +
                                         buffer[11].ToString() + "." +
                                         buffer[12].ToString();
                        tb_silab_version.Text = m_atmelVersion;

                        if (buffer[10] == 0 && buffer[11] == 1)
                        {
                            m_isBL = true;
                            MessageBox.Show("Atmel is in Bootloader mode. Please upgrade the Atmel firmware again.");
                        }
                        else
                            m_isBL = false;
                    }
                    else
                        MessageBox.Show("Cannot get atmel firmware version.");
                }
            }
            else
            {
                MessageBox.Show("Device is not connected.");
            }
        }

        private int GetBTVersion()
        {
            byte[] command = BluetoothCommands.GetVersion();

            if (!USBSocket.TransmitData(command, command.Length))
            {
                MessageBox.Show("Device failed to transmit data.");
                return 0;
            }

            byte[] buffer = new byte[128];
            int bytesRead = 0;

            // Make sure that we are connected to a device
            if (HID.IsOpened())
            {
                if (USBSocket.ReceiveData(ref buffer, buffer.Length, ref bytesRead, 1000))
                {
                    if ((buffer[0] == Constants.PREFIX) && (bytesRead >= 13) &&
                        (buffer[8] == 0xC0) && (buffer[9] == 0x00))
                    {
                        uint crc = ((uint)buffer[6] << 8) | (uint)buffer[7];

                        if (crc != 0 && !CRC.CheckCRC(buffer, 0, 13, crc))
                        {
                            MessageBox.Show("Wrong BTVersion CRC received.");
                            return 0;
                        }

                        m_btVersion = buffer[10].ToString() + "." +
                                      buffer[11].ToString() + "." +
                                      buffer[12].ToString();
                        tb_bt_version.Text = m_btVersion;
                    }
                    else
                        return 1;
                }
            }
            else
            {
                MessageBox.Show("Device is not connected.");
            }

            return 0;
        }

        private void RfidPowerOn()
        {
            tb_info.Clear();

            byte[] command = RFIDCommands.PowerOn(true);

            if (!USBSocket.TransmitData(command, command.Length))
            {
                MessageBox.Show("Device failed to transmit data.");
            }

            m_stopReceive = false;
            m_stopRfidDecode = false;
            Thread thread1 = new Thread(new ThreadStart(RecvThread));
            thread1.Start();
            Thread thread2 = new Thread(new ThreadStart(DecodeRfidCommands));
            thread2.Start();
        }

        private void RfidPowerOff()
        {
            byte[] command = RFIDCommands.PowerOn(false);

            if (!USBSocket.TransmitData(command, command.Length))
            {
                MessageBox.Show("Device failed to transmit data.");
            }
        }

        // Extract the firmware version embedded in a firmware file name.
        // e.g. "CS710ATMEL_V2.1.22.bin"          -> "2.1.22"
        //      "CS710_CC2652R7_APP_V1.0.16.bin"  -> "1.0.16"
        private string ExtractVersion(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "";

            int start = fileName.LastIndexOf("_V", StringComparison.OrdinalIgnoreCase);
            int end = fileName.LastIndexOf(".bin", StringComparison.OrdinalIgnoreCase);
            if (start < 0 || end < 0 || end <= start + 2)
                return "";

            start += 2; // skip "_V"
            return fileName.Substring(start, end - start).Trim();
        }

        // Launch the automated firmware-upgrade check on a background thread so the UI
        // thread stays free to repaint the progress dialog. Re-entrancy is guarded so that
        // rapid replug / post-reboot re-enumeration events cannot start overlapping runs.
        private void StartAutoUpgrade()
        {
            if (m_autoUpgradeRunning)
                return;
            m_autoUpgradeRunning = true;
            Thread t = new Thread(AutoUpgradeWorker) { IsBackground = true };
            t.Start();
        }

        private void AutoUpgradeWorker()
        {
            try
            {
                string appFolder = Application.StartupPath;
                string configPath = Path.Combine(appFolder, "config.txt");
                string firmwareFolder = Path.Combine(appFolder, "firmware");

                if (!File.Exists(configPath))
                {
                    MessageBox.Show("config.txt not found in application folder.", "Firmware Upgrade");
                    return;
                }

                string[] lines = File.ReadAllLines(configPath);
                if (lines.Length < 2)
                {
                    MessageBox.Show("config.txt must list the Atmel firmware file name on line 1 " +
                        "and the Bluetooth firmware file name on line 2.", "Firmware Upgrade");
                    return;
                }

                string atmelFile = lines[0].Trim();
                string btFile = lines[1].Trim();
                string atmelPath = Path.Combine(firmwareFolder, atmelFile);
                string btPath = Path.Combine(firmwareFolder, btFile);

                if (!File.Exists(atmelPath))
                {
                    MessageBox.Show("Atmel firmware file not found:\r\n" + atmelPath, "Firmware Upgrade");
                    return;
                }
                if (!File.Exists(btPath))
                {
                    MessageBox.Show("Bluetooth firmware file not found:\r\n" + btPath, "Firmware Upgrade");
                    return;
                }

                string atmelFileVersion = ExtractVersion(atmelFile);
                string btFileVersion = ExtractVersion(btFile);

                // Decide whether an upgrade is required.
                if (m_isBL)
                {
                    // Atmel is in bootloader mode; only the Atmel firmware can be flashed now.
                    // The reader reboots into normal mode and the next connection handles Bluetooth.
                }
                else
                {
                    bool atmelDiff = !string.Equals(m_atmelVersion, atmelFileVersion, StringComparison.OrdinalIgnoreCase);
                    bool btDiff = !string.Equals(m_btVersion, btFileVersion, StringComparison.OrdinalIgnoreCase);

                    // If either firmware differs, run BOTH upgrades (Atmel then Bluetooth).
                    if (!atmelDiff && !btDiff)
                    {
                        MessageBox.Show("Firmware is already up to date.\r\n\r\n" +
                            "Atmel: " + m_atmelVersion + "\r\n" +
                            "Bluetooth: " + m_btVersion, "Firmware Upgrade");
                        return;
                    }
                }

                // Run the Atmel upgrade first.
                bool atmelOk;
                using (Stream atmelStream = new FileStream(atmelPath, FileMode.Open, FileAccess.Read))
                {
                    atmelOk = UpdateAtmelImage(atmelStream);
                }

                if (!atmelOk)
                {
                    HideProgress();
                    MessageBox.Show("Firmware upgrade failed during the Atmel step.", "Firmware Upgrade");
                    return;
                }

                if (m_isBL)
                {
                    // Bluetooth version was not read in bootloader mode. Stop here; the reader
                    // reboots into normal mode and the next connection performs the full check.
                    HideProgress();
                    MessageBox.Show("Atmel firmware upgrade completed. Please wait for the reader to reboot.",
                        "Firmware Upgrade");
                    return;
                }

                // Keep a 5 second delay between the two upgrades.
                Thread.Sleep(5000);

                // Run the Bluetooth upgrade.
                bool btOk;
                using (Stream btStream = new FileStream(btPath, FileMode.Open, FileAccess.Read))
                {
                    btOk = UpdateBTImage(btStream);
                }

                if (!btOk)
                {
                    HideProgress();
                    MessageBox.Show("Firmware upgrade failed during the Bluetooth step.", "Firmware Upgrade");
                    return;
                }

                HideProgress();
                MessageBox.Show("Firmware upgrade completed successfully.", "Firmware Upgrade");
            }
            catch (Exception ex)
            {
                HideProgress();
                MessageBox.Show("Firmware upgrade error: " + ex.Message, "Firmware Upgrade");
            }
            finally
            {
                m_autoUpgradeRunning = false;
            }
        }

        // Hide the (TopMost) progress dialog so a following result MessageBox is visible.
        private void HideProgress()
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action(HideProgress));
                return;
            }
            progressform.Hide();
        }

        private int GetSN()
        {
            byte[] command = AtmelCommands.GetSN();

            if (!USBSocket.TransmitData(command, command.Length))
            {
                MessageBox.Show("Device failed to transmit data.");
                return 0;
            }

            byte[] buffer = new byte[128];
            int bytesRead = 0;

            // Make sure that we are connected to a device
            if (HID.IsOpened())
            {
                if (USBSocket.ReceiveData(ref buffer, buffer.Length, ref bytesRead, 1000))
                {
                    if ((buffer[0] == Constants.PREFIX) && (bytesRead >= 42) &&
                        (buffer[8] == 0xB0) && (buffer[9] == 0x04))
                    {
                        m_SN = System.Text.Encoding.UTF8.GetString(buffer, 10, 32);
                    }
                    else
                        return 1;
                }
            }
            else
            {
                MessageBox.Show("Device is not connected.");
            }

            return 0;
        }

        private void GetBatteryLevel()
        {
            byte[] command = NotifyCommands.GetBattery();

            if (!USBSocket.TransmitData(command, command.Length))
            {
                MessageBox.Show("Device failed to transmit data.");
                return;
            }

            byte[] buffer = new byte[128];
            int bytesRead = 0;

            // Make sure that we are connected to a device
            if (HID.IsOpened())
            {
                if (USBSocket.ReceiveData(ref buffer, buffer.Length, ref bytesRead, 500))
                {
                    if ((buffer[0] == Constants.PREFIX) && (bytesRead >= 12) &&
                        (buffer[8] == 0xA0) && (buffer[9] == 0x00))
                    {
                        tb_batt_lvl.Text = (buffer[10] << 8 | buffer[11]).ToString();
                    }
                    else
                        MessageBox.Show("Cannot get battery level.");
                }
            }
            else
            {
                MessageBox.Show("Device is not connected.");
            }
        }

        private void GetTriggerState()
        {
            byte[] buffer = new byte[64];
            int bytesRead = 0;

            // clear buffer
            USBSocket.ReceiveData(ref buffer, buffer.Length, ref bytesRead, 10);
            
            byte[] command = NotifyCommands.GetTriggerState();

            if (!USBSocket.TransmitData(command, command.Length))
            {
                MessageBox.Show("Device failed to transmit data.");
                return;
            }

            // Make sure that we are connected to a device
            if (HID.IsOpened())
            {
                if (USBSocket.ReceiveData(ref buffer, buffer.Length, ref bytesRead, 500))
                {
                    if ((buffer[0] == Constants.PREFIX) && (bytesRead >= 11) &&
                        (buffer[8] == 0xA0) && (buffer[9] == 0x01))
                    {
                        if (buffer[10] == 0)
                            tb_trigger_state.Text = "Released";
                        else
                            tb_trigger_state.Text = "Pushed";
                    }
                    else
                        MessageBox.Show("Cannot get trigger state.");
                }
            }
            else
            {
                MessageBox.Show("Device is not connected.");
            }
        }

        private void GetDeviceName()
        {
            byte[] command = BluetoothCommands.SendGetDeviceName();

            if (!USBSocket.TransmitData(command, command.Length))
            {
                MessageBox.Show("Device failed to transmit data.");
                return;
            }

            byte[] buffer = new byte[128];
            int bytesRead = 0;

            // Make sure that we are connected to a device
            if (HID.IsOpened())
            {
                if (USBSocket.ReceiveData(ref buffer, buffer.Length, ref bytesRead, 500))
                {
                    if ((buffer[0] == Constants.PREFIX) && (bytesRead >= 31) &&
                        (buffer[8] == 0xC0) && (buffer[9] == 0x04))
                    {
                        var str = System.Text.Encoding.ASCII.GetString(buffer, 10, 21);
                        tb_devicename.Text = str;
                    }
                    else
                        MessageBox.Show("Cannot get device name.");
                }
            }
            else
            {
                MessageBox.Show("Device is not connected.");
            }
        }

        private void SetDeviceName()
        {
            byte[] name = Encoding.ASCII.GetBytes(tb_devicename.Text);

            if (name.Length == 0)
            {
                MessageBox.Show("Device name cannot be empty!");
                return;
            }
            byte[] command = BluetoothCommands.SendSetDeviceName(name);

            if (!USBSocket.TransmitData(command, command.Length))
            {
                MessageBox.Show("Device failed to transmit data.");
                return;
            }

            byte[] buffer = new byte[128];
            int bytesRead = 0;

            // Make sure that we are connected to a device
            if (HID.IsOpened())
            {
                if (USBSocket.ReceiveData(ref buffer, buffer.Length, ref bytesRead, 500))
                {
                    if ((buffer[0] == Constants.PREFIX) && (bytesRead >= 11) &&
                        (buffer[8] == 0xC0) && (buffer[9] == 0x03))
                    {
                        if (buffer[10] == 0)
                            MessageBox.Show("Set device name success. Please re-boot device to take effective");
                        else
                            MessageBox.Show("Set device name fail.");
                    }
                    else
                        MessageBox.Show("Cannot set device name.");
                }
            }
            else
            {
                MessageBox.Show("Device is not connected.");
            }
        }

        // Enable/disable controls that should only be enabled
        // when connected to a device
        private void EnableDeviceCtrls(bool enable)
        {
            btn_rfid.Enabled = enable;
            btn_barcode.Enabled = enable;
            if (!enable)
            {
                tb_silab_version.Text = "";
                tb_bt_version.Text = "";
                tb_batt_lvl.Text = "";
                tb_trigger_state.Text = "";
                tb_devicename.Text = "";
                tb_rfid_version.Text = "";
            }
            btn_atmel_image.Enabled = enable;
            btn_atmel_bootloader.Enabled = enable;
            btn_bt_image.Enabled = enable;
            btn_get_battery.Enabled = enable;
            btn_get_trigger.Enabled = enable;
            btn_get_devicename.Enabled = enable;
            btn_set_devicename.Enabled = enable;
            btn_rfid_upgrade.Enabled = enable;
            btn_bt_config.Enabled = enable;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_stopReceive = true;
            m_stopRfidDecode = true;

            // Close the device
            HID.Close();

            UsbNotification.UnregisterUsbDeviceNotification();
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
                                RFIDCommands.GetVersion();
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
                                m_stopReceive = true;
                                m_stopRfidDecode = true;
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
                            string version = "";
                            int build = 0;
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
                                    RfidPowerOff();
                                }
                                else
                                {
                                }
                            }
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
                    }
                }
                Thread.Sleep(1); // save CPU usage
            }
            stopwatch1.Stop();
            stopwatch2.Stop();
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            if (m_Connected)
            {
                if (Disconnect()) m_Connected = false;
            }
            else
            {
                if (Connect()) m_Connected = true;
            }
        }

        private void btn_rfid_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (RFIDForm rfidForm = new RFIDForm())
            {
                rfidForm.ShowDialog();
            }
            this.Show();
        }

        private void btn_barcode_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (BarcodeForm barcodeForm = new BarcodeForm())
            {
                barcodeForm.ShowDialog();
            }
            this.Show();
        }

        private void btn_bt_config_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (BtHidConfigForm btConfigForm = new BtHidConfigForm())
            {
                btConfigForm.ShowDialog();
            }
            this.Show();
        }

        private void btn_atmel_image_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofile = new OpenFileDialog())
                {
                    ofile.Title = "Please choose Image update file";
                    ofile.Filter = "BIN files (CS710ATMEL_*.bin)|CS710ATMEL_*.bin";
                    if (ofile.ShowDialog() == DialogResult.OK)
                    {
                        /*if (!m_isBL)
                        {
                            if ((tb_rfid_version.Text.StartsWith("1") && !ofile.FileName.Contains("CS710ATMEL_V0")) ||
                                (tb_rfid_version.Text.StartsWith("2") && !ofile.FileName.Contains("CS710ATMEL_V2")))
                            {
                                MessageBox.Show("This Atmel firmware version is not allowed for this RFID version");
                                return;
                            }
                        }*/

                        Stream stream = new FileStream(ofile.FileName, FileMode.Open);

                        Thread thread = new Thread(() => UpdateAtmelImage(stream));
                        thread.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool UpdateAtmelImage(Stream stream)
        {
            if (stream.Length < 350000 || stream.Length > 450000)
            {
                MessageBox.Show("Incorrect image file size.");
                return false;
            }

            bool success = false;
            int len, total_subpart, subpart = 0, size = (int)stream.Length - 2;
            byte[] subpartBuffer = new byte[240];
            byte[] readBuffer = new byte[256];
            byte[] crc = new byte[2];
            byte[] command;
            int bytesRead = 0;
            total_subpart = (int)(Math.Ceiling((double)stream.Length / 234));

            USBSocket.ReceiveData(ref readBuffer, readBuffer.Length, ref bytesRead, 1000);

            while (true)
            {
                if (subpart == 0)
                {
                    stream.Seek(size, SeekOrigin.Begin);
                    stream.Read(crc, 0, 2);

                    subpartBuffer[0] = (byte)(size >> 24);
                    subpartBuffer[1] = (byte)(size >> 16);
                    subpartBuffer[2] = (byte)(size >> 8);
                    subpartBuffer[3] = (byte)(size);
                    subpartBuffer[4] = crc[0];
                    subpartBuffer[5] = crc[1];
                    command = AtmelCommands.SendImageData(subpartBuffer, total_subpart, 0, 6);

                    stream.Seek(0, SeekOrigin.Begin);
                }
                else
                {
                    len = stream.Read(subpartBuffer, 0, 234);
                    if (len > 0)
                    {
                        command = AtmelCommands.SendImageData(subpartBuffer, total_subpart, subpart, len);
                    }
                    else
                    {
                        MessageBox.Show("Not enough image data.");
                        return false;
                    }
                }

                if (!USBSocket.TransmitData(command, command.Length))
                {
                    MessageBox.Show("Device failed to transmit data.");
                    return false;
                }

                bytesRead = 0;

                // Make sure that we are connected to a device
                if (HID.IsOpened())
                {
                    if (USBSocket.ReceiveData(ref readBuffer, readBuffer.Length, ref bytesRead, 3000))
                    {
                        if ((readBuffer[0] == Constants.PREFIX) && (bytesRead >= 11) &&
                            (readBuffer[8] == 0xB0) && (readBuffer[9] == 0x01))
                        {
                            if (readBuffer[10] == 1)
                            {
                                ShowUpdateResult(false, 0);
                                break;
                            }
                            else if (readBuffer[10] == 2)
                            {
                                ShowUpdatePercent(100);
                                ShowUpdateResult(true, 5);
                                success = true;
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
            return success;
        }

        private void btn_atmel_bootloader_Click(object sender, EventArgs e)
        {
            /*try
            {
                using (OpenFileDialog ofile = new OpenFileDialog())
                {
                    ofile.Title = "Please choose Bootloader update file";
                    ofile.Filter = "BIN files (*.bin)|*.bin|All files (*.*)|*.*";//"BIN Files\0*.bin;\0All Files\0*.*\0";
                    if (ofile.ShowDialog() == DialogResult.OK)
                    {
                        Stream stream = new FileStream(ofile.FileName, FileMode.Open);

                        Thread thread = new Thread(() => UpdateAtmelBootloader(stream));
                        thread.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/
        }

        private void UpdateAtmelBootloader(Stream stream)
        {
            
        }

        private void btn_bt_image_Click(object sender, EventArgs e)
        {
            string model = m_SN.Substring(8, 1);
            if (model != "A" && model != "B" && model != "C" && model != "D")
            {
                MessageBox.Show("Bluetooth model is not recognized!");
            }
            else
            {
                try
                {
                    using (OpenFileDialog ofile = new OpenFileDialog())
                    {
                        ofile.Title = "Please choose Image update file";
                        switch (model)
                        {
                            case "A":
                                ofile.Filter = "BIN files (CS710_CC2652RB_APP*.bin)|CS710_CC2652RB_APP*.bin";
                                break;
                            case "B":
                                ofile.Filter = "BIN files (CS710_CC2652R7_APP*.bin)|CS710_CC2652R7_APP*.bin";
                                break;
                            case "C":
                                ofile.Filter = "BIN files (CS710_CC2652R1_APP*.bin)|CS710_CC2652R1_APP*.bin";
                                break;
                            case "D":
                                ofile.Filter = "BIN files (CS710_CC2652RSIP_APP*.bin)|CS710_CC2652RSIP_APP*.bin";
                                break;
                        }
                        if (ofile.ShowDialog() == DialogResult.OK)
                        {
                            Stream stream = new FileStream(ofile.FileName, FileMode.Open);

                            Thread thread = new Thread(() => UpdateBTImage(stream));
                            thread.Start();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private bool UpdateBTImage(Stream stream)
        {
            if (stream.Length < 100000 || stream.Length > 150000)
            {
                MessageBox.Show("Incorrect image file size.");
                return false;
            }

            bool success = false;
            int len, total_subpart, subpart = 0, size = (int)stream.Length - 2;
            int timeout = 1000;
            byte[] subpartBuffer = new byte[240];
            byte[] readBuffer = new byte[256];
            byte[] crc = new byte[2];
            byte[] command;
            int bytesRead = 0;
            total_subpart = (int)(Math.Ceiling((double)stream.Length / 234));

            USBSocket.ReceiveData(ref readBuffer, readBuffer.Length, ref bytesRead, 1000);

            while (true)
            {
                if (subpart == 0)
                {
                    stream.Seek(size, SeekOrigin.Begin);
                    stream.Read(crc, 0, 2);

                    subpartBuffer[0] = (byte)(size >> 24);
                    subpartBuffer[1] = (byte)(size >> 16);
                    subpartBuffer[2] = (byte)(size >> 8);
                    subpartBuffer[3] = (byte)(size);
                    subpartBuffer[4] = crc[0];
                    subpartBuffer[5] = crc[1];
                    command = BluetoothCommands.SendImageData(subpartBuffer, total_subpart, 0, 6);

                    stream.Seek(0, SeekOrigin.Begin);
                }
                else
                {
                    len = stream.Read(subpartBuffer, 0, 234);
                    if (len > 0)
                    {
                        command = BluetoothCommands.SendImageData(subpartBuffer, total_subpart, subpart, len);
                    }
                    else
                    {
                        MessageBox.Show("Not enough image data.");
                        return false;
                    }
                }

                if (!USBSocket.TransmitData(command, command.Length))
                {
                    MessageBox.Show("Device failed to transmit data.");
                    return false;
                }

                bytesRead = 0;

                if (subpart == total_subpart)
                    timeout = 5000;
                else
                    timeout = 1000;

                // Make sure that we are connected to a device
                if (HID.IsOpened())
                {
                    if (USBSocket.ReceiveData(ref readBuffer, readBuffer.Length, ref bytesRead, timeout))
                    {
                        if ((readBuffer[0] == Constants.PREFIX) && (bytesRead >= 11) &&
                            (readBuffer[8] == 0xC0) && (readBuffer[9] == 0x01))
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
                                success = true;
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
            return success;
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
                if (wait == 0)
                    progressform.UpdateProgressResult(String.Format("FW Upload Success. Please power reboot the reader.", wait));
                else
                    progressform.UpdateProgressResult(String.Format("FW Upload Success. Wait {0} seconds to complete.", wait));
                progressform.Show();
                progressform.Refresh();
            }
            else
            {
                progressform.UpdateProgressResult("FW Update Fail");
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

        private void btn_get_battery_Click(object sender, EventArgs e)
        {
            GetBatteryLevel();
        }

        private void btn_get_trigger_Click(object sender, EventArgs e)
        {
            GetTriggerState();
        }

        private void btn_get_devicename_Click(object sender, EventArgs e)
        {
            GetDeviceName();
        }

        private void btn_set_devicename_Click(object sender, EventArgs e)
        {
            SetDeviceName();
        }

        private void btn_rfid_upgrade_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofile = new OpenFileDialog())
                {
                    ofile.Title = "Please choose Image update file";
                    ofile.Filter = "BIN files (ex10_app*.bin)|ex10_app*.bin";
                    if (ofile.ShowDialog() == DialogResult.OK)
                    {
                        Stream stream = new FileStream(ofile.FileName, FileMode.Open);

                        Thread thread = new Thread(() => UpdateRFIDImage(stream));
                        thread.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateRFIDImage(Stream stream)
        {
            if (stream.Length < 150000 || stream.Length > 250000)
            {
                MessageBox.Show("Incorrect image file size.");
                return;
            }

            int len, total_subpart, subpart = 0, size = (int)stream.Length;
            int timeout = 1000;
            byte[] subpartBuffer = new byte[240];
            byte[] readBuffer = new byte[256];
            byte[] crc = new byte[2];
            byte[] command;
            total_subpart = (int)(Math.Ceiling((double)stream.Length / 234));

            while (true)
            {
                if (subpart == 0)
                {
                    subpartBuffer[0] = (byte)(size >> 24);
                    subpartBuffer[1] = (byte)(size >> 16);
                    subpartBuffer[2] = (byte)(size >> 8);
                    subpartBuffer[3] = (byte)(size);
                    command = RFIDCommands.SendImageData(subpartBuffer, total_subpart, 0, 4);

                    stream.Seek(0, SeekOrigin.Begin);
                }
                else
                {
                    len = stream.Read(subpartBuffer, 0, 234);
                    if (len > 0)
                    {
                        command = RFIDCommands.SendImageData(subpartBuffer, total_subpart, subpart, len);
                    }
                    else
                    {
                        MessageBox.Show("Not enough image data.");
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
                            (readBuffer[8] == 0x80) && (readBuffer[9] == 0x03))
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

        private delegate void UpdateVersionDeleg(String version);
        private void UpdateVersion(String version)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateInfoDeleg(UpdateVersion), new object[] { version });
                return;
            }
            tb_rfid_version.Text = version;
        }
    }
}
