using System;
using System.Security;
using System.Windows;

namespace LATravelManager.UI.Security
{
    internal class SecureStringManipulation
    {
        public static byte[] ConvertSecureStringToByteArray(SecureString value)
        {
            //Byte array to hold the return value
            byte[] returnVal = new byte[value.Length];

            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(value);
                for (int i = 0; i < value.Length; i++)
                {
                    short unicodeChar = System.Runtime.InteropServices.Marshal.ReadInt16(valuePtr, i * 2);
                    if (unicodeChar > 255)
                    {
                        MessageBox.Show("Μόνο λατινικά η αριθμούς");
                    }
                    returnVal[i] = Convert.ToByte(unicodeChar);
                }

                return returnVal;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}