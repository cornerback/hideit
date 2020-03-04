using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HideIt.Encryption
{

    internal sealed class Ghost
    {
        /// Static members initialization...
        private static byte[][] sbox = new byte[][]
                         {new byte[] {4,10,9,2,13,8,0,14,6,11,1,12,7,15,5,3},
						 new byte[] {14,11,4,12,6,13,15,10,2,3,8,1,0,7,5,9},
						 new byte[] {5,8,1,13,10,3,4,2,14,15,12,7,6,0,9,11},
						 new byte[] {7,13,10,1,0,8,9,15,14,4,6,12,11,2,5,3},
						 new byte[] {6,12,7,1,5,15,13,8,4,10,9,14,0,3,11,2},
						 new byte[] {4,11,10,0,7,2,1,13,3,6,8,5,9,12,15,14},
						 new byte[] {13,11,4,1,3,15,5,9,0,10,14,7,6,8,2,12},
						 new byte[] {1,15,13,0,5,7,10,4,9,2,3,14,6,11,8,12}};

        private static byte[] skround = new byte[] 
                        { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 
                            4, 5, 6, 7, 8, 8, 7, 6, 5, 4, 3, 2, 1 };

        private uint[] pkeys = new uint[8];

        private const byte padByte = (byte)'$';

        private string _key = null;

        public Ghost()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public Ghost(string key)
        {
            this.MakeSubKeys(key);
        }

        public string Key
        {
            get { return this._key; }
            set
            {
                this._key = value;
                if (string.IsNullOrEmpty(this._key))
                {
                    this.MakeSubKeys(this._key);
                }
            }
        }

        private void MakeSubKeys(string key)
        {
            for (uint i = 0; i < 32; i += 4)
            {
                uint pkr = i / 4;

                pkeys[pkr] = key[(int)i];
                pkeys[pkr] = pkeys[pkr] << 8; pkeys[pkr] |= key[(int)i + 1];
                pkeys[pkr] = pkeys[pkr] << 8; pkeys[pkr] |= key[(int)i + 2];
                pkeys[pkr] = pkeys[pkr] << 8; pkeys[pkr] |= key[(int)i + 3];
            }
        }

        public byte[] Encrypt(byte[] pdata)
        {
            int padlen = 0;
            uint bufflen = (uint)(pdata.Length + this.Padding((uint)pdata.Length, out padlen));
            if (padlen != 0)
            {
                for (uint i = (uint)pdata.Length; i < bufflen; i++)
                    pdata[i] = padByte;
            }

            byte[] block = new byte[8];
            byte[] cdata = new byte[bufflen + 1];//one extra to hold the padding length

            for (uint i = 0; i < bufflen - 1; i += 8)
            {
                this.SubBYTEs(block, pdata, i, 8);
                cdata = this.ApplyGost(cdata, block, i, false);
            }

            return cdata;
        }

        public byte[] Decrypt(byte[] cdata)
        {
            int padlen = 0;
            uint bufflen = (uint)(cdata.Length + this.Padding((uint)cdata.Length, out padlen));
            if (padlen != 0)
            {
                for (uint i = (uint)cdata.Length; i < bufflen; i++)
                    cdata[i] = padByte;
            }

            byte[] block = new byte[8];
            byte[] pdata = new byte[bufflen];

            for (uint i = 0; i < bufflen; i += 8)
            {
                this.SubBYTEs(block, cdata, i, 8);
                pdata = this.ApplyGost(pdata, block, i, true);
            }

            return pdata;
        }

        private int Padding(uint filelen, out int padlen)
        {
            padlen = (int)filelen % 8;
            if (padlen == 0)
                return 0;
            return (8 - padlen);
        }

        private void SubBYTEs(byte[] block, byte[] data, uint pos, int len)
        {
            int j = 0;
            for (uint i = pos; i < (uint)len + pos; i++)
                block[j++] = data[i];
        }

        private byte[] ApplyGost(byte[] data, byte[] block, uint pos, bool isdecrypting)
        {
            uint R, L, R_1;
            R = L = 0;

            R = block[4];
            R <<= 8; R |= block[5];
            R <<= 8; R |= block[6];
            R <<= 8; R |= block[7];

            L = block[0];
            L <<= 8; L |= block[1];
            L <<= 8; L |= block[2];
            L <<= 8; L |= block[3];

            for (uint i = 0; i < 32; i++)
            {
                R_1 = R;

                if (isdecrypting)
                    R = this.GhostFunction(R, i);
                else
                    R = this.GhostFunction(R, 31 - i);

                R = R ^ L;
                L = R_1;
            }

            uint ander = 4278190080;

            data[pos++] = (byte)((ander & R) >> 24); ander >>= 8;
            data[pos++] = (byte)((ander & R) >> 16); ander >>= 8;
            data[pos++] = (byte)((ander & R) >> 8); ander >>= 8;
            data[pos++] = (byte)(ander & R);

            ander = 4278190080;

            data[pos++] = (byte)((ander & L) >> 24); ander >>= 8;
            data[pos++] = (byte)((ander & L) >> 16); ander >>= 8;
            data[pos++] = (byte)((ander & L) >> 8); ander >>= 8;
            data[pos++] = (byte)(ander & L);

            return data;
        }

        private uint GhostFunction(uint Ri_1, uint round)
        {
            uint R = pkeys[skround[round] - 1] ^ Ri_1;

            uint Ri = R;
            uint ander = 15;

            Ri |= (uint)sbox[0][(R & ander)];

            ander <<= 4; Ri |= (uint)sbox[1][((R & ander) >> 4)] << 4;
            ander <<= 4; Ri |= (uint)sbox[2][((R & ander) >> 8)] << 8;
            ander <<= 4; Ri |= (uint)sbox[3][((R & ander) >> 12)] << 12;
            ander <<= 4; Ri |= (uint)sbox[4][((R & ander) >> 16)] << 16;
            ander <<= 4; Ri |= (uint)sbox[5][((R & ander) >> 20)] << 20;
            ander <<= 4; Ri |= (uint)sbox[6][((R & ander) >> 24)] << 24;
            ander <<= 4; Ri |= (uint)sbox[7][((R & ander) >> 28)] << 28;

            return Ri << 11 | Ri >> (32 - 11);
        }
    }
}
