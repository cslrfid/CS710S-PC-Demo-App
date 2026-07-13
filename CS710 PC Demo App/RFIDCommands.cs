using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CS710_PC_Client
{
    class RFIDCommands
    {
        private static uint[] crc32_table = 
        {
          0x00000000, 0x04c11db7, 0x09823b6e, 0x0d4326d9,
          0x130476dc, 0x17c56b6b, 0x1a864db2, 0x1e475005, 
          0x2608edb8, 0x22c9f00f, 0x2f8ad6d6, 0x2b4bcb61, 
          0x350c9b64, 0x31cd86d3, 0x3c8ea00a, 0x384fbdbd, 
          0x4c11db70, 0x48d0c6c7, 0x4593e01e, 0x4152fda9, 
          0x5f15adac, 0x5bd4b01b, 0x569796c2, 0x52568b75,
          0x6a1936c8, 0x6ed82b7f, 0x639b0da6, 0x675a1011, 
          0x791d4014, 0x7ddc5da3, 0x709f7b7a, 0x745e66cd, 
          0x9823b6e0, 0x9ce2ab57, 0x91a18d8e, 0x95609039, 
          0x8b27c03c, 0x8fe6dd8b, 0x82a5fb52, 0x8664e6e5, 
          0xbe2b5b58, 0xbaea46ef, 0xb7a96036, 0xb3687d81,
          0xad2f2d84, 0xa9ee3033, 0xa4ad16ea, 0xa06c0b5d, 
          0xd4326d90, 0xd0f37027, 0xddb056fe, 0xd9714b49, 
          0xc7361b4c, 0xc3f706fb, 0xceb42022, 0xca753d95, 
          0xf23a8028, 0xf6fb9d9f, 0xfbb8bb46, 0xff79a6f1, 
          0xe13ef6f4, 0xe5ffeb43, 0xe8bccd9a, 0xec7dd02d,
          0x34867077, 0x30476dc0, 0x3d044b19, 0x39c556ae, 
          0x278206ab, 0x23431b1c, 0x2e003dc5, 0x2ac12072, 
          0x128e9dcf, 0x164f8078, 0x1b0ca6a1, 0x1fcdbb16, 
          0x018aeb13, 0x054bf6a4, 0x0808d07d, 0x0cc9cdca, 
          0x7897ab07, 0x7c56b6b0, 0x71159069, 0x75d48dde,
          0x6b93dddb, 0x6f52c06c, 0x6211e6b5, 0x66d0fb02, 
          0x5e9f46bf, 0x5a5e5b08, 0x571d7dd1, 0x53dc6066, 
          0x4d9b3063, 0x495a2dd4, 0x44190b0d, 0x40d816ba, 
          0xaca5c697, 0xa864db20, 0xa527fdf9, 0xa1e6e04e, 
          0xbfa1b04b, 0xbb60adfc, 0xb6238b25, 0xb2e29692,
          0x8aad2b2f, 0x8e6c3698, 0x832f1041, 0x87ee0df6, 
          0x99a95df3, 0x9d684044, 0x902b669d, 0x94ea7b2a, 
          0xe0b41de7, 0xe4750050, 0xe9362689, 0xedf73b3e, 
          0xf3b06b3b, 0xf771768c, 0xfa325055, 0xfef34de2, 
          0xc6bcf05f, 0xc27dede8, 0xcf3ecb31, 0xcbffd686,
          0xd5b88683, 0xd1799b34, 0xdc3abded, 0xd8fba05a, 
          0x690ce0ee, 0x6dcdfd59, 0x608edb80, 0x644fc637, 
          0x7a089632, 0x7ec98b85, 0x738aad5c, 0x774bb0eb, 
          0x4f040d56, 0x4bc510e1, 0x46863638, 0x42472b8f, 
          0x5c007b8a, 0x58c1663d, 0x558240e4, 0x51435d53,
          0x251d3b9e, 0x21dc2629, 0x2c9f00f0, 0x285e1d47, 
          0x36194d42, 0x32d850f5, 0x3f9b762c, 0x3b5a6b9b, 
          0x0315d626, 0x07d4cb91, 0x0a97ed48, 0x0e56f0ff, 
          0x1011a0fa, 0x14d0bd4d, 0x19939b94, 0x1d528623, 
          0xf12f560e, 0xf5ee4bb9, 0xf8ad6d60, 0xfc6c70d7,
          0xe22b20d2, 0xe6ea3d65, 0xeba91bbc, 0xef68060b, 
          0xd727bbb6, 0xd3e6a601, 0xdea580d8, 0xda649d6f, 
          0xc423cd6a, 0xc0e2d0dd, 0xcda1f604, 0xc960ebb3, 
          0xbd3e8d7e, 0xb9ff90c9, 0xb4bcb610, 0xb07daba7, 
          0xae3afba2, 0xaafbe615, 0xa7b8c0cc, 0xa379dd7b,
          0x9b3660c6, 0x9ff77d71, 0x92b45ba8, 0x9675461f, 
          0x8832161a, 0x8cf30bad, 0x81b02d74, 0x857130c3, 
          0x5d8a9099, 0x594b8d2e, 0x5408abf7, 0x50c9b640, 
          0x4e8ee645, 0x4a4ffbf2, 0x470cdd2b, 0x43cdc09c, 
          0x7b827d21, 0x7f436096, 0x7200464f, 0x76c15bf8,
          0x68860bfd, 0x6c47164a, 0x61043093, 0x65c52d24, 
          0x119b4be9, 0x155a565e, 0x18197087, 0x1cd86d30, 
          0x029f3d35, 0x065e2082, 0x0b1d065b, 0x0fdc1bec, 
          0x3793a651, 0x3352bbe6, 0x3e119d3f, 0x3ad08088, 
          0x2497d08d, 0x2056cd3a, 0x2d15ebe3, 0x29d4f654,
          0xc5a92679, 0xc1683bce, 0xcc2b1d17, 0xc8ea00a0, 
          0xd6ad50a5, 0xd26c4d12, 0xdf2f6bcb, 0xdbee767c, 
          0xe3a1cbc1, 0xe760d676, 0xea23f0af, 0xeee2ed18, 
          0xf0a5bd1d, 0xf464a0aa, 0xf9278673, 0xfde69bc4, 
          0x89b8fd09, 0x8d79e0be, 0x803ac667, 0x84fbdbd0,
          0x9abc8bd5, 0x9e7d9662, 0x933eb0bb, 0x97ffad0c, 
          0xafb010b1, 0xab710d06, 0xa6322bdf, 0xa2f33668, 
          0xbcb4666d, 0xb8757bda, 0xb5365d03, 0xb1f740b4 
        };

        public static byte downlink_seq_no = 0;

        public static class RFIDRegisterTable
        {
            public static byte count = 0;
            public static UInt16[] address = new UInt16[256];
            public static byte[] length = new byte[256];
            public static byte[,] data = new byte[256,256];

            public static void AddRegisterTable(UInt16 address, byte length, byte[] data)
            {
                RFIDRegisterTable.address[RFIDRegisterTable.count] = address;
                RFIDRegisterTable.length[RFIDRegisterTable.count] = length;
                if (data != null)
                {
                    for (int i = 0; i < length; i++)
                    {
                        RFIDRegisterTable.data[RFIDRegisterTable.count, i] = data[i];
                    }
                }
                ++RFIDRegisterTable.count;
            }

            public static void ClearRegisterTable()
            {
                RFIDRegisterTable.count = 0;
            }
        }

        public static byte[] PowerOn(bool on)
        {
            byte[] buffer = new byte[10];

            //header
            buffer[0] = Constants.PREFIX;
            buffer[1] = Constants.CONNECTION_USB;
            buffer[2] = 2; //payload length
            buffer[3] = Constants.TYPE_RFID;
            buffer[4] = Constants.RESERVE;
            buffer[5] = Constants.LINK_DOWN;
            buffer[6] = 0;
            buffer[7] = 0;

            //payload
            buffer[8] = 0x80;
            if (on)
                buffer[9] = 0x00;
            else
                buffer[9] = 0x01;

            return buffer;
        }

        public static void SendData(byte[] data, int length)
        {
            byte[] buffer = new byte[length+10];

            //header
            buffer[0] = Constants.PREFIX;
            buffer[1] = Constants.CONNECTION_USB;
            buffer[2] = (byte)(length + 2); //payload length
            buffer[3] = Constants.TYPE_RFID;
            buffer[4] = Constants.RESERVE;
            buffer[5] = Constants.LINK_DOWN;
            buffer[6] = 0;
            buffer[7] = 0;

            //payload
            buffer[8] = 0x80;
            buffer[9] = 0x02;

            for (int i = 0; i < length; i++)
            {
                buffer[i + 10] = data[i];
            }

            USBSocket.TransmitData(buffer, buffer.Length);
        }

        public static void SendSCommand(ushort command)
        {
            byte[] buffer = new byte[7];

            Tools.FromShort(Constants.COMMAND_PREFIX, out buffer[0], out buffer[1]);
            Tools.FromShort(command, out buffer[2], out buffer[3]);
            buffer[4] = downlink_seq_no++;
            Tools.FromShort(0, out buffer[5], out buffer[6]);
            
            SendData(buffer, 7);
        }

        public static void SendAdminCommand(ushort command, byte[] payload, ushort length)
        {
            byte[] buffer = new byte[7 + length];
            int i = 0;

            Tools.FromShort(Constants.COMMAND_PREFIX, out buffer[0], out buffer[1]);
            Tools.FromShort(command, out buffer[2], out buffer[3]);
            buffer[4] = downlink_seq_no++;
            Tools.FromShort(length, out buffer[5], out buffer[6]);
            for (i = 0; i < length; i++)
            {
                buffer[7 + i] = payload[i];
            }

            SendData(buffer, 7 + length);
        }

        public static void WriteRegisters(ushort count, int[] address, int[] length, byte[][] data)
        {
            int i, j;
            int payload_length = (3 * count) + 1;
            for (i = 0; i < count; i++)
            {
                payload_length += length[i];
            }
            int total_length = 7 + payload_length;
            int cmd_ind = 0;
            byte[] buffer = new byte[total_length];

            Tools.FromShort(Constants.COMMAND_PREFIX, out buffer[cmd_ind], out buffer[cmd_ind + 1]);
            Tools.FromShort(Constants.RFID_WriteReg, out buffer[cmd_ind + 2], out buffer[cmd_ind + 3]);
            buffer[cmd_ind + 4] = downlink_seq_no++;
            Tools.FromShort((ushort)(payload_length), out buffer[cmd_ind + 5], out buffer[cmd_ind + 6]);
            buffer[cmd_ind + 7] = (byte)count;
            cmd_ind = 8;
            for (i = 0; i < count; i++)
            {
                Tools.FromShort((ushort)address[i], out buffer[cmd_ind], out buffer[cmd_ind + 1]);
                buffer[cmd_ind + 2] = (byte)length[i];
                cmd_ind += 3;
                for (j = 0; j < length[i]; j++)
                {
                    buffer[cmd_ind++] = data[i][j];
                }
            }

            SendData(buffer, total_length);
        }

        public static void ReadRegisters(ushort count, ushort[] address, byte[] length)
        {
            int payload_length = (3 * count) + 1;
            int total_length = 7 + payload_length;
            int cmd_ind = 0;
            byte[] buffer = new byte[total_length];

            Tools.FromShort(Constants.COMMAND_PREFIX, out buffer[cmd_ind], out buffer[cmd_ind + 1]);
            Tools.FromShort(Constants.RFID_ReadReg, out buffer[cmd_ind + 2], out buffer[cmd_ind + 3]);
            buffer[cmd_ind + 4] = downlink_seq_no++;
            Tools.FromShort((ushort)(payload_length), out buffer[cmd_ind + 5], out buffer[cmd_ind + 6]);
            buffer[cmd_ind + 7] = (byte)count;
            cmd_ind = 8;
            for (int i = 0; i < count; i++)
            {
                Tools.FromShort((ushort)address[i], out buffer[cmd_ind], out buffer[cmd_ind + 1]);
                buffer[cmd_ind + 2] = (byte)length[i];
                cmd_ind += 3;
            }

            SendData(buffer, total_length);
        }

        public static void WriteRegisters(ushort count, ushort[] address, byte[] length, byte[,] data)
        {
            int payload_length = 1 ;
            int cmd_ind = 0;

            for (int i = 0; i < count; i++)
            {
                payload_length += length[i] + 3;
            }

            byte[] buffer = new byte[payload_length+11];

            Tools.FromShort(Constants.COMMAND_PREFIX, out buffer[cmd_ind], out buffer[cmd_ind + 1]);
            Tools.FromShort(Constants.RFID_WriteReg, out buffer[cmd_ind + 2], out buffer[cmd_ind + 3]);
            buffer[cmd_ind + 4] = downlink_seq_no++;
            Tools.FromShort((ushort)(payload_length), out buffer[cmd_ind + 5], out buffer[cmd_ind + 6]);
            buffer[cmd_ind + 7] = (byte)count;
            cmd_ind = 8;
            for (int i = 0; i < count; i++)
            {
                Tools.FromShort((ushort)address[i], out buffer[cmd_ind], out buffer[cmd_ind + 1]);
                buffer[cmd_ind + 2] = (byte)length[i];
                cmd_ind += 3;
                for (int j = 0; j < length[i]; j++)
                {
                    buffer[cmd_ind++] = data[i, j];
                }
            }

            SendData(buffer, payload_length+7);
        }

        public static void GetVersion()
        {
            ushort[] address = new ushort[2];
            byte[] length = new byte[2];

            address[0] = 0x08; // version string
            length[0] = 32;
            address[1] = 0x28; // build
            length[1] = 4;

            RFIDRegisterTable.ClearRegisterTable();
            RFIDRegisterTable.AddRegisterTable(address[0], length[0], null);
            RFIDRegisterTable.AddRegisterTable(address[1], length[1], null);

            ReadRegisters(2, address, length);
        }

        public static void SyncTime()
        {
            uint timestamp = (uint)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            byte[] payload = new byte[4];

            payload[0] = (byte)(timestamp >> 24);
            payload[1] = (byte)(timestamp >> 16);
            payload[2] = (byte)(timestamp >> 8);
            payload[3] = (byte)(timestamp);

            SendAdminCommand(Constants.CSLSyncTime, payload, 4);
        }

        private static byte[] HexStringToByteArray(String HexString)
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

        public static byte[] SendImageData(byte[] subpart_data, int total_subpart, int subpart, int length)
        {
            byte[] buffer = new byte[248];

            //header
            buffer[0] = Constants.PREFIX;
            buffer[1] = Constants.CONNECTION_USB;
            buffer[2] = 240; //payload length
            buffer[3] = Constants.TYPE_RFID;
            buffer[4] = Constants.RESERVE;
            buffer[5] = Constants.LINK_DOWN;
            buffer[6] = 0;
            buffer[7] = 0;

            //payload
            buffer[8] = 0x80;
            buffer[9] = 0x03;
            buffer[10] = (byte)(total_subpart >> 8);
            buffer[11] = (byte)total_subpart;
            buffer[12] = (byte)(subpart >> 8);
            buffer[13] = (byte)subpart;

            for (int i = 0; i < length; i++)
            {
                buffer[i + 14] = subpart_data[i];
            }
            for (int i = length; i < 234; i++)
            {
                buffer[i + 14] = 0xFF;
            }

            return buffer;
        }

        public static byte[] SendOEMData(byte[] subpart_data, int total_subpart, int subpart, int length)
        {
            byte[] buffer = new byte[248];

            //header
            buffer[0] = Constants.PREFIX;
            buffer[1] = Constants.CONNECTION_USB;
            buffer[2] = 240; //payload length
            buffer[3] = Constants.TYPE_RFID;
            buffer[4] = Constants.RESERVE;
            buffer[5] = Constants.LINK_DOWN;
            buffer[6] = 0;
            buffer[7] = 0;

            //payload
            buffer[8] = 0x80;
            buffer[9] = 0x04;
            buffer[10] = (byte)(total_subpart >> 8);
            buffer[11] = (byte)total_subpart;
            buffer[12] = (byte)(subpart >> 8);
            buffer[13] = (byte)subpart;

            for (int i = 0; i < length; i++)
            {
                buffer[i + 14] = subpart_data[i];
            }
            for (int i = length; i < 234; i++)
            {
                buffer[i + 14] = 0xFF;
            }

            return buffer;
        }
    }
}
