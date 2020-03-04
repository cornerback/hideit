using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideIt.Encryption
{
    ///RC4 Steps
    ///The steps for RC4 encryption algorithm is as follows:
    ///1- Get the data to be encrypted and the selected key.
    ///2- Create two string arrays.
    ///3- Initiate one array with numbers from 0 to 255.
    ///4- Fill the other array with the selected key.
    ///5- Randomize the first array depending on the array of the key.
    ///6- Randomize the first array within itself to generate the final key stream.
    ///7- XOR the final key stream with the data to be encrypted to give cipher text.
    public class RC4
    {
        private const int _encryptionBitLength = 256;

        /// <summary>
        /// This is the RC4 Encryption Bit Length.
        /// For example: 128, 256 (default), 512, 1024, etc...
        /// </summary>
        public Int32 EncryptionBitLength
        {
            get { return _encryptionBitLength; }
        }

        private int[] sbox;        
        private string _inputText = String.Empty;

        /// <summary>
        /// This is the text you wish to parse through RC4.
        /// It is the same method to encrypt and decrypt
        /// </summary>
        public String InputText
        {
            get { return _inputText; }
        }

        private string _RC4Key = string.Empty;

        /// <summary>
        /// This is the Key that will be used for the RC4 method.
        /// </summary>
        public string RC4Key
        {
            get { return _RC4Key; }
        }

        public RC4(string key, string message)
        {
            _inputText = message;
            _RC4Key = key;
        }

        /// <summary>
        /// This method will perform it's RC4 routine against the 'InputText' property.
        /// The result is the written to the 'Result' property
        /// </summary>
        public string DoRC4()
        {
            InitializeRC4Objects();

            int i = 0, j = 0, k = 0;
            StringBuilder cipher = new StringBuilder();
            for (int a = 0; a < _inputText.Length; a++)
            {
                i = (i + 1) % _encryptionBitLength;
                j = (j + sbox[i]) % _encryptionBitLength;
                int tempSwap = sbox[i];
                sbox[i] = sbox[j];
                sbox[j] = tempSwap;

                k = sbox[(sbox[i] + sbox[j]) % _encryptionBitLength];
                int cipherBy = ((int)_inputText[a]) ^ k;  //xor operation
                cipher.Append(Convert.ToChar(cipherBy));
            }
            return cipher.ToString();
        }

        private void InitializeRC4Objects()
        {
            sbox = new int[_encryptionBitLength];
            int[] key = new int[_encryptionBitLength];
            int n = _RC4Key.Length;
            for (int a = 0; a < _encryptionBitLength; a++)
            {
                key[a] = (int)_RC4Key[a % n];
                sbox[a] = a;
            }

            int b = 0;
            for (int a = 0; a < _encryptionBitLength; a++)
            {
                b = (b + sbox[a] + key[a]) % _encryptionBitLength;
                int tempSwap = sbox[a];
                sbox[a] = sbox[b];
                sbox[b] = tempSwap;
            }
        }
    }
}
