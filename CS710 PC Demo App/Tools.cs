using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CS710_PC_Client
{
    class Tools
    {
        public static void FromShort(ushort number, out byte byte1, out byte byte2)
        {
            byte1 = (byte)(number >> 8);
            byte2 = (byte)(number & 255);
        }

        public static UInt16 ToShort(UInt16 byte1, UInt16 byte2)
        {
            return (UInt16)((byte1 << 8) | byte2);
        }

        public static byte[] HexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                hexString = hexString.PadLeft(hexString.Length+1, '0');
                //throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The string cannot have an odd number of digits: {0}", hexString));
            }

            byte[] HexAsBytes = new byte[hexString.Length / 2];
            for (int index = 0; index < HexAsBytes.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return HexAsBytes;
        }
    }
}
