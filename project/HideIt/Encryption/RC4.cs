using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HideIt.Encryption
{
    internal sealed class RC4
    {
        private const int _chunk = 256;
        private StringBuilder _message;

        public RC4(string key, string message)
        {
            this._message = new StringBuilder();

            //int iter = (int)Math.Floor((double)message.Length / _chunk);

            /////Applying encryption in chunks
            //int start = 0;
            //for (int i = 0; i < iter; i++)
            //{
            //    this.Encipher(key, message.Substring(start, _chunk));
            //    start += _chunk;
            //}

            /////Apply on last chunk
            //this.Encipher(key, message.Substring(start, message.Length - start));

            this.Encipher(key, message);
        }

        /// <summary>
        /// Get ASCII Integer Code
        /// </summary>
        /// <param name="ch">Get ASCII Integer Code</param>
        /// <returns>Integer code</returns>
        private static int Ord(char ch)
        {
            return (int)(Encoding.GetEncoding(1252).GetBytes(ch + "")[0]);
        }

        /// <summary>
        /// The symmetric encryption function
        /// </summary>
        /// <param name="pwd">pwd Key to encrypt with</param>
        /// <param name="data">data Content to be encrypted</param>
        private void Encipher(string pwd, string data)
        {
            int a, i, j, k, tmp, pwd_length, data_length;
            int[] key, box;
            byte[] cipher;

            pwd_length = pwd.Length;
            data_length = data.Length;
            key = new int[256];
            box = new int[256];
            cipher = new byte[data.Length];

            for (i = 0; i < 256; i++)
            {
                key[i] = Ord(pwd[i % pwd_length]);
                box[i] = i;
            }
            for (j = i = 0; i < 256; i++)
            {
                j = (j + box[i] + key[i]) % 256;
                tmp = box[i];
                box[i] = box[j];
                box[j] = tmp;
            }
            for (a = j = i = 0; i < data_length; i++)
            {
                a = (a + 1) % 256;
                j = (j + box[a]) % 256;
                tmp = box[a];
                box[a] = box[j];
                box[j] = tmp;
                k = box[((box[a] + box[j]) % 256)];
                cipher[i] = (byte)(Ord(data[i]) ^ k);
            }
            this._message.Append(Encoding.GetEncoding(1252).GetString(cipher));
        }

        /// <summary>
        /// Apply RC4 cipher on data
        /// </summary>
        /// <returns></returns>
        public string ApplyRC4()
        {
            return this._message.ToString();
        }
    }
}
