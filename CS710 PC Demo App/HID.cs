using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CS710_PC_Client
{
    class HID
    {
        // USB Parameters
        public const int VID = 0x03EB;
        public const int PID = 0x2678;
        public const int PID_1 = 0x2402;
        // HID Report IDs
        public const int ID_IN_DATA = 0x01;
        public const int ID_OUT_DATA = 0x02;
        // HID Report Sizes
        public const int SIZE_IN_DATA = 64;
        public const int SIZE_OUT_DATA = 64;
        public const int SIZE_MAX_WRITE = 62;
        public const int SIZE_MAX_READ = 62;
        // Return Codes
        public const int HID_DEVICE_SUCCESS = 0x00;
        public const int HID_DEVICE_NOT_FOUND = 0x01;
        public const int HID_DEVICE_NOT_OPENED = 0x02;
        public const int HID_DEVICE_ALREADY_OPENED = 0x03;
        public const int HID_DEVICE_TRANSFER_TIMEOUT = 0x04;
        public const int HID_DEVICE_TRANSFER_FAILED = 0x05;
        public const int HID_DEVICE_CANNOT_GET_HID_INFO = 0x06;
        public const int HID_DEVICE_HANDLE_ERROR = 0x07;
        public const int HID_DEVICE_INVALID_BUFFER_SIZE = 0x08;
        public const int HID_DEVICE_SYSTEM_CODE = 0x09;
        public const int HID_DEVICE_UNSUPPORTED_FUNCTION = 0x0A;
        public const int HID_DEVICE_UNKNOWN_ERROR = 0xFF;
        // String Types
        public const int HID_VID_STRING = 0x01;
        public const int HID_PID_STRING = 0x02;
        public const int HID_PATH_STRING = 0x03;
        public const int HID_SERIAL_STRING = 0x04;
        public const int HID_MANUFACTURER_STRING = 0x05;
        public const int HID_PRODUCT_STRING = 0x06;
        public const int MAX_PATH_LENGTH = 260;

        private const int MAX_REPORT_REQUEST_XP = 512;
        private static string HIDDeviceString = "";
        private static bool isOpen = false;

        [DllImport("AtUsbHid.dll")]
        private static extern bool findHidDevice(int vid, int pid);

        [DllImport("AtUsbHid.dll")]
        private static extern void closeDevice();

        [DllImport("AtUsbHid.dll")]
        private static extern int getOutputReportLength();

        [DllImport("AtUsbHid.dll")]
        private static extern int getInputReportLength();

        [DllImport("AtUsbHid.dll")]
        private static extern bool writeData(IntPtr buf);

        [DllImport("AtUsbHid.dll")]
        private static extern bool readData(IntPtr buffer);

        public static int GetNumHidDevices()
        {
            if (findHidDevice(VID, PID))
            {
                HIDDeviceString = "CS710 USB HID";
                closeDevice();
                return 1;
            }
            else if (findHidDevice(VID, PID_1))
            {
                HIDDeviceString = "ATSAME5X USB HID";
                closeDevice();
                return 1;
            }
            else
            {
                HIDDeviceString = "";
                return 0;
            }
        }

        public static int GetString(ref string deviceString)
        {
            int status = 0;

            deviceString = HIDDeviceString;

            return status;
        }

        public static int Open()
        {
            if (findHidDevice(VID, PID))
            {
                isOpen = true;
                HIDDeviceString = "CS710 USB HID";
                return HID_DEVICE_SUCCESS;
            }
            else if (findHidDevice(VID, PID_1))
            {
                isOpen = true;
                HIDDeviceString = "ATSAME5X USB HID";
                return HID_DEVICE_SUCCESS;
            }
            else
            {
                isOpen = false;
                HIDDeviceString = "";
                return HID_DEVICE_NOT_FOUND;
            }
        }

        public static int Close()
        {
            if (isOpen)
            {
                closeDevice();
                isOpen = false;
                HIDDeviceString = "";
            }

            return HID_DEVICE_SUCCESS;
        }

        public static int GetOutputReportBufferLength()
        {
            return getOutputReportLength();
        }

        public static int GetInputReportBufferLength()
        {
            return getInputReportLength();
        }

        public static bool IsOpened()
        {
            return isOpen;
        }

        public static bool TransmitData(byte[] buffer, int bufferSize)
        {
	        bool	success		= false;
	        int 	reportSize	= getOutputReportLength();
	        byte[]	report		= new byte[reportSize];

	        // Make sure that the device report size is adequate
	        if (reportSize >= SIZE_OUT_DATA)
	        {
		        int bytesWritten = 0;
		        int bytesToWrite = bufferSize;
                IntPtr pnt = Marshal.AllocHGlobal(reportSize);

		        // Fragment the buffer into several writes of up to SIZE_MAX_WRITE(61)
		        // bytes
		        while (bytesWritten < bytesToWrite)
		        {
			        int transferSize = Math.Min(bytesToWrite - bytesWritten, SIZE_MAX_WRITE);

			        report[0] = ID_OUT_DATA;
			        report[1] = (byte)transferSize;
                    Buffer.BlockCopy(buffer, bytesWritten, report, 2, transferSize);
                    Marshal.Copy(report, 0, pnt, reportSize);

			        // Send an output report over the interrupt endpoint
                    if (writeData(pnt) != true)
			        {
				        // Stop transmitting if there was an error
				        break;
			        }

			        bytesWritten += transferSize;
		        }

                Marshal.FreeHGlobal(pnt);

		        // Write completed successfully
		        if (bytesWritten == bytesToWrite)
		        {
			        success = true;
		        }
	        }

	        return success;
        }

        public static bool ReceiveData(ref byte[] buffer, int bufferSize, ref int bytesRead)
        {
            bool success = false;
            int reportSize = getInputReportLength();

            // Make sure that the device report size is adequate
            if (reportSize >= SIZE_IN_DATA)
            {
                // Make sure that the buffer is at least big enough to hold the maximum
                // number of input data bytes from a single report
                if (bufferSize >= SIZE_MAX_READ)
                {
                    byte[] reportBuffer = new byte[reportSize];

                    IntPtr pnt = Marshal.AllocHGlobal(reportSize);
                    bytesRead = 0;
                    if (readData(pnt))
                    {
                        Marshal.Copy(pnt, reportBuffer, 0, reportSize);

                        if (reportBuffer[0] == ID_IN_DATA)
                        {
                            reportBuffer[0] = ID_IN_DATA;
                            bytesRead = reportBuffer[1];

                            Array.Copy(reportBuffer, 2, buffer, 0, bytesRead);

                            success = true;
                        }
                    }

                    Marshal.FreeHGlobal(pnt);
                }
            }

            return success;
        }
    }
}
