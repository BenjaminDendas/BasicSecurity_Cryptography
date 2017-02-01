using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Crypto_Program
{
    public class DESEncryption
    {
        private DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        private KeyAndVectorNumber kvd = new KeyAndVectorNumber();
        private byte[] dataToEncrypt;
        public string User { get; set; }
        public DESEncryption(string user)
        {
            this.User = user;
        }
      
        public void Encrypt(string inputfile, string outputfile, byte[] key, byte[] iv,string rsaPublicKey,RadioButton b)
        {
            try
            {               
                this.dataToEncrypt = File.ReadAllBytes(inputfile);
                this.des.Key = key; // Het inlezen van de Symmetrische Key.
                this.des.IV = iv;   // Het inlezen van de Initialisatie Vector.
                this.des.Mode = CipherMode.CBC;     
                this.des.Padding = PaddingMode.PKCS7;

                this.kvd.WriteIV(this.des.IV, User, rsaPublicKey,b);    // We schrijven de initialisatie Vector weg die gegenereerd werd.
                this.kvd.WriteKey(this.des.Key, User, rsaPublicKey,b);  // We schrijven de symmetrische key weg die gegenereerd werd.

                
                using (FileStream outfs = new FileStream(outputfile, FileMode.Create))
                {
                    CryptoStream cryptoStream = null;
                    try
                    {
                        // Als cryptostream geven we mee hoe dat de data 'getransformeerd' kan worden d.m.v. een symmetrisch encryptor object.
                        cryptoStream = new CryptoStream(outfs, this.des.CreateEncryptor(), CryptoStreamMode.Write); 
                        cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fout tijdens het verwerken van de data.");
                        Errors.Error(ex,this.User,"DESEncryptionError");
                    }
                    finally
                    {
                        if (cryptoStream != null)
                        {
                            cryptoStream.Close();
                        }
                    }

                }
                MessageBox.Show("Bestand is geëncrypteerd.");
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Het pad werd niet gevonden." + Environment.NewLine + ex.FileName);
                Errors.Error(ex,this.User,"FileNotFoundException");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Een error heeft zich voorgedaan tijdens het encrypteren.");
                Errors.Error(ex,this.User, "EncryptieError");
            }
            
        }
        public void Decrypt(string filePath, string savePath, byte[] key, byte[] iv, string rsaPrivateKey)
            {
                try
                {
                    FileStream infs = new FileStream(filePath, FileMode.Open);
                    using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                    {
                        des.Mode = CipherMode.CBC;                  // We zetten de CipherMode op Cipher Block Chaining.
                        des.Padding = PaddingMode.PKCS7;            // Paddingmode PKCS7 voor opvulling van de blokken die geen 64 bytes bevatten.
                        des.Key = key;                              // Instellen van de Symmetrische key die we doorgegeven hebben via de parameter.
                        des.IV = iv;                                // Instellen van de initialisatie vector die we doorgegeven hebben via de parameters.

                        using (FileStream outfs = new FileStream(savePath, FileMode.Create))
                        {
                            int count = 0;
                            int offset = 0;
                            byte[] data = new byte[64];

                            using (CryptoStream crypto = new CryptoStream(outfs, des.CreateDecryptor(), CryptoStreamMode.Write)) // Aanmaken cryptostream.
                            {
                                do
                                {
                                    count = infs.Read(data, 0, 64);  // Inlezen van data in blokken van 64 bytes.
                                    offset += count;                 // Verhogen van de offset (positie waar we starten met inlezen).
                                    crypto.Write(data, 0, count);

                                }
                                while (count > 0);
            
                                    crypto.FlushFinalBlock();       // We flushen het laatste block om ervoor te zorgen dat alle onderliggende data weggeschreven is.
                                    crypto.Close();                 // sluit CryptoStream.
                            }
                            if(outfs != null)
                            {
                                outfs.Close();
                            }    
                        }
                        if(infs != null)
                        {
                            infs.Close();
                        }    
                    }
                    MessageBox.Show("Bestand gedecrypteerd.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ongeldige Key.");
                    Errors.Error(ex, "DESDecryptError");
                    File.Delete(savePath);
                }
        }
    }
}
