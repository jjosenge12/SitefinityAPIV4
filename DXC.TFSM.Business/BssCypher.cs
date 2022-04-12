using DXC.TFSM.DataAccess;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Linq;

namespace DXC.TFSM.Business
{
    public class BssCypher
    {
        readonly TFSM_PortalCotizadorEntities tFSM_PortalEntities = new TFSM_PortalCotizadorEntities();

        public Task<string> DecryptStringAES(string cipherText)
        {
            var decriptedFromJavascript = string.Empty;
            try
            {
                if (cipherText != "")
                {
                    string key = tFSM_PortalEntities.tbl_portal_Llaves.Where(x => x.Status == 1).Select(K => K.KeyValue).First().ToString();
                    byte[] keybytes = Encoding.UTF8.GetBytes(key);
                    byte[] iv = Encoding.UTF8.GetBytes(key);
                    byte[] encrypted = Convert.FromBase64String(cipherText);
                    decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
                    //var XX__decriptedFromJavascript = DecryptObjectFromBytes(encrypted, keybytes, iv);
                }
                else
                {
                    decriptedFromJavascript = "";
                }
            }
            catch (Exception) {
                decriptedFromJavascript = "";
            }

            return Task.FromResult(decriptedFromJavascript);
        }

        public static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings 
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128 / 8;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform. 
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption. 
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream 
                                // and place them in a string. 
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch(Exception ex)
                {
                    plaintext = "keyError";
                    throw ex;
                }
            }

            return plaintext;
        }


        /// <summary>
        /// Desencripta un objecto
        /// </summary>
        /// <param name="pObject">Objecto Encriptado</param>
        /// <param name="pKey">Clave 1</param>
        /// <param name="pIV">Clave 2</param>
        /// <returns>Retorna el Objecto Desencriptado</returns>
        private static object DecryptObjectFromBytes(byte[] pObject, byte[] pKey, byte[] pIV)
        {
            //Verifica si los parametros son nulo
            if (pObject == null || pObject.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (pKey == null || pKey.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (pIV == null || pIV.Length <= 0)
            {
                throw new ArgumentNullException();
            }

            //Variable que almacenara el valor del objecto encritado
            object vlDecodeObject = null;

            //Creamos el algoritmo
            System.Security.Cryptography.Rijndael rijndael = System.Security.Cryptography.Rijndael.Create();

            try
            {
                //Usamos el algoritmo
                using (rijndael)
                {
                    //Le pasamos los valores de las claves al algoritmno
                    rijndael.Key = pKey;
                    rijndael.IV = pIV;

                    //Creamos el Desencriptador
                    System.Security.Cryptography.ICryptoTransform decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);

                    //Se crean las corrientes para el cifrado
                    using (MemoryStream msDecrypt = new MemoryStream(pObject))
                    {
                        using (System.Security.Cryptography.CryptoStream csDecrypt = new System.Security.Cryptography.CryptoStream(msDecrypt, decryptor, System.Security.Cryptography.CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                //Escribe los datos en la secuencia, como un objecto (string) 
                                vlDecodeObject = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                }
            }
            catch (Exception ex) {
                throw ex;
            }

            //Retornamos el objecto desencriptado
            return vlDecodeObject;
        }

        public Task<string> GetKey() {
            var kdjflskjdfñsdkfjx = tFSM_PortalEntities.tbl_portal_Llaves.Where(x => x.Status == 1).Select(a=> a.KeyValue).First().ToString();
            return Task.FromResult(kdjflskjdfñsdkfjx);
        }
    }
}
