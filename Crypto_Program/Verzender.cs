using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Crypto_Program
{
    public class Verzender
    {
        public RSAParameters rsaPrivKey { get; set; }
        private RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

        public Verzender(string privateKeyPath) // Inlezen van de private key
        {
            StreamReader reader = null;
            FileStream fsReader = null;
            try
            {
                fsReader = new FileStream(privateKeyPath, FileMode.Open, FileAccess.Read);
                reader = new StreamReader(fsReader);
                string txt = reader.ReadToEnd();
                rsa.FromXmlString(txt);
                this.rsaPrivKey = rsa.ExportParameters(true);
            }
            catch(Exception ex)
            {
                Errors.Error(ex, "VerzenderError");
            }
            finally
            {
                if (fsReader != null)
                {
                    fsReader.Close();
                }
                if(reader != null)
                {
                    reader.Close();
                }
            }
            
        }

        public byte[] HashAndSign(string originalFile) // Gebruikt de private key om de hash van de gegenereerde file te signen.
        {
            byte[] hashedData = null;
            try
            {
            // Genereer de Hash van de file en sign met de Private Key
            byte[] data = File.ReadAllBytes(originalFile);
            SHA1Managed hash = new SHA1Managed();

            this.rsa.ImportParameters(rsaPrivKey);
            hashedData = hash.ComputeHash(data);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error bij het genereren en Signen van de Hash.");
                Errors.Error(ex, "HashAndSignError");
            }
                      
            return rsa.SignHash(hashedData,CryptoConfig.MapNameToOID("SHA1"));
        }














    }
}
