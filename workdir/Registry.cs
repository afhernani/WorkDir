using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace Tools.Reedit
{
	public class Reedit
	{
		/// <summary>
		/// solo lectura de Registro.Windows.
		/// Busca en el registro de windows para el usuario en curso, CurrentUser para la key pasada,
		/// devuelve su valor almacenado.
		/// </summary>
		/// <param name="KeyName">clave registro currentUser</param>
		/// <returns> string valor clave del registro</returns>
		public static string GetRegistryKey(string KeyName)
		{
			string KeyValue = "";
			RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\ToolsUser");
			if (key != null)
			{
				if (key.GetValue(KeyName) != null)
				{
					KeyValue = key.GetValue(KeyName).ToString();
				}
			}
			return KeyValue;
		}
		
		/// <summary>
		/// Asignacion de llave y valor en el registro CurrentUser
		/// </summary>
		/// <param name="KeyName"> string llave </param>
		/// <param name="KeyValue">string valor </param>
		public static void SetRegistryKey(string KeyName, string KeyValue)
		{
			RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\ToolsUser");
			key.SetValue(KeyName, KeyValue);
		}

	}
	
}
namespace Tools.Crypto
{
	public class Crypto
	{
		/// <summary>
        /// Encripta una cadena de entrada, dando como resultado una cadena encriptada
        /// devuelve true si la accion se realizo con exito y false si hubo algun error.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        public static bool EncryptFile(string inputFile, string outputFile, string CurrentPassword)
        {
            bool bSuccess = false;

            if ( CurrentPassword == "") return bSuccess;

            try
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(@CurrentPassword);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();

                bSuccess = true;
            }
            catch(Exception)
            {
                // nothing    
            }
            
            return bSuccess;
        }

        /// <summary>
        /// Desencripta una cadena de entrada.
        /// devuelve una cadena MemoryStream.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <returns></returns>
        public static MemoryStream DecryptFile(string inputFile, string CurrentPassword)
        {
            if (CurrentPassword == "") return new MemoryStream();

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
            try
            {
                MemoryStream msOut = new MemoryStream();
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(@CurrentPassword);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);

                int data;
                while ((data = cs.ReadByte()) != -1)
                {
                    msOut.WriteByte((byte)data);
                }
                cs.Close();
                fsCrypt.Close();

                return msOut;
            }
            catch 
            {
                fsCrypt.Close(); // release file lock
                return new MemoryStream();
            }
        }
		
		/// <summary>
        /// Desencripta un fichero. pasando la cadena de entrada y
        /// la cadena de salida.
        /// devuelve true si se realizó sin problemas y falso si no.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        public static bool DecryptFile(string inputFile, string outputFile, string CurrentPassword)
        {
            bool bSuccess = false;
            if (CurrentPassword == "") return bSuccess;

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
            try
            {
                FileStream fsOut = new FileStream(outputFile, FileMode.Create);
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(@CurrentPassword);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);

                int data;
                while ((data = cs.ReadByte()) != -1)
                {
                    fsOut.WriteByte((byte)data);
                }
                cs.Close();
                fsCrypt.Close();
                fsOut.Close();
                bSuccess = true;
            }
            catch
            {
                fsCrypt.Close(); // release file lock
            }
            
            return bSuccess;
        }
	}
}
