using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Crypto_Program
{
    public class AesEncryption
    {
        private AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
        private KeyAndVectorNumber kvn = new KeyAndVectorNumber();
        public string User { get; set; }
        public AesEncryption(string user)
        {
            this.User = user;
        }

        public void AesEncrypt(string inputFile, string outputFile, string publicRSA,RadioButton b)
        {
            byte[] dataToEncrypt = File.ReadAllBytes(inputFile); // We lezen het bestand in en steken de bytes in een array van bytes.
            this.aes.Mode = CipherMode.CBC;
            this.aes.Padding = PaddingMode.PKCS7;
            Console.WriteLine(aes.KeySize);
            Console.WriteLine(aes.BlockSize);

            this.kvn.WriteIV(this.aes.IV, User, publicRSA,b);   // We schrijven de initialisatie vector weg, de IV word automatisch gemaakt 
                                                                // bij het creëren van het AESCryptoServiceProvider object.
                                                                // Het heeft een key-grootte van 256
            this.kvn.WriteKey(this.aes.Key, User, publicRSA,b); // We schrijven de Symmetrische key weg.

            using(FileStream outfs = new FileStream(outputFile,FileMode.Create))
            {
                CryptoStream cryptoStream = null;
                try
                {
                    cryptoStream = new CryptoStream(outfs, this.aes.CreateEncryptor(), CryptoStreamMode.Write); // We stellen de cryptostream in als een Encryptor
                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock(); // We flushen de laatste block om ervoor te zorgen dat in de onderliggende data source geen data meer zit.
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Fout bij het Encrypteren met AES");
                    Errors.Error(ex, "AESEncryptionError");
                }
                finally
                {
                    if(cryptoStream != null)
                    {
                        cryptoStream.Close();
                    }
                }
            }
            MessageBox.Show("Bestand is geëncrypteerd.");
        }

        public void AesDecrypt(string filepath, string savepath, byte[] key, byte[] iv, string privateRSA)
        {
            try
            {
                FileStream infs = new FileStream(filepath, FileMode.Open);
                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = key;
                    aes.IV = iv;

                    using (FileStream outfs = new FileStream(savepath, FileMode.Create))
                    {
                        int count = 0;
                        int offset = 0;
                        byte[] data = new byte[128]; // AES Schrijft weg in blokken van 128 bytes.

                        using (CryptoStream crypto = new CryptoStream(outfs, aes.CreateDecryptor(), CryptoStreamMode.Write)) // Stelt de cryptostream in als een Decryptor met als algoritme AES.
                        {
                            do
                            {
                                count = infs.Read(data, 0, 128); // We lezen in met blokken van 128 bytes
                                offset += count;
                                crypto.Write(data, 0, count);   // We encrypteren iedere blok van 128 bytes.
                            }
                            while (count > 0);
                            crypto.FlushFinalBlock();   // We flushen het laatste blok om ervoor te zorgen dat er in de onderliggende data lagen geen data meer zit.
                            crypto.Close();

                        }
                        if (outfs != null)
                        {
                            outfs.Close();
                        }
                    }
                    if (infs != null)
                    {
                        infs.Close();
                    }
                }
                MessageBox.Show("Bestand gedecrypteerd.");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Fout bij het decrypteren met AES." + Environment.NewLine + "Foutieve Key");
                Errors.Error(ex, "AES-DecryptieError");
                File.Delete(savepath);
            }
        }
                 
   }
}
