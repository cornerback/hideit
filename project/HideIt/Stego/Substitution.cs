using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideIt.Stego
{
    internal static class Substitution
    {       
        /// <summary>
        /// Set a bit, identified by bitNo, in data
        /// </summary>
        /// <param name="data">Byte to be updated</param>
        /// <param name="bitToSet">Bits to set in data</param>
        private static void SetLsbBit(ref byte data, byte bits)
        {
            ///If we have to substitute LSB with 1
            if (bits == 0x1)
            {
                ///OR'ing the data with 00000001 will set the LSB to 1
                data |= bits;
            }
            else ///If we have to substitute LSB with 0
            {
                ///Here we have AND. But for this we have to change the remaining bit of 'bits' to 1
                ///for e.g we have to AND data with 11111110 which will set LSB to 0
                ///For this we simply AND it with 254 = 11111110
                data &= 0xFE;
            }
        }

        /// <summary>
        /// Extracts the specified number of bits from data
        /// </summary>
        /// <param name="data">Data from which bits are to be read</param>
        /// <param name="toRead">Number of bits to read</param>
        /// <returns>Bits read from data</returns>
        private static byte GetBit(byte data, byte toRead)
        {
            byte bits = (byte)(Math.Pow(2, (int)toRead) -1);
            return (byte)(data & bits);
        }

        /// <summary>
        /// Substitute the LSB with the bit provided
        /// </summary>
        /// <param name="data">Data whose LSB will be substituted</param>
        /// <param name="subtitute">Value with which the LSB is substitued</param>        
        public static void LsbSubstitute(ref byte data, byte subtitute)
        {
            SetLsbBit(ref data, subtitute);
        }

        /// <summary>
        /// Get the value of LSB bit
        /// </summary>
        /// <param name="data">Data from which the LSB value is to be extracted</param>
        /// <returns>Value of LSB. It can either be 1 or 0</returns>
        public static byte LsbValue(byte data)
        {
            return GetBit(data, 1);
        }        
    }
}
