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
    public class Ontvanger
    {

        private RSACryptoServiceProvider rsaCP = new RSACryptoServiceProvider();
        public RSAParameters rsaPubKey { get; set; }

        public Ontvanger(string pubKeyPath)
        {
            FileStream ofs = null;
            StreamReader oReader = null;
            try
            {
                ofs = new FileStream(pubKeyPath, FileMode.Open, FileAccess.Read);
                oReader = new StreamReader(ofs);
                string txt = oReader.ReadToEnd();
                rsaCP.FromXmlString(txt);
                this.rsaPubKey = rsaCP.ExportParameters(false);
                
            }
            catch(Exception ex)
            {
                Errors.Error(ex, "OntvangerError");
            }
            finally
            {
                if(ofs != null)
                {
                    ofs.Close();
                }
                if(oReader != null)
                {
                    oReader.Close();
                }
            }       
        }

        public bool VerifyHash(string signedDataPath, string signaturePath)
        {
            bool success = false;
            byte[] hashData = null;
            byte[] signature = null;
            FileStream fs = null;
            StreamReader reader = null;
            try
            {
                byte[] signedData = File.ReadAllBytes(signedDataPath); // Leest de bytes van de hash in.
                fs = new FileStream(signaturePath, FileMode.Open, FileAccess.Read);
                reader = new StreamReader(fs);
                string txt = reader.ReadToEnd();
                signature = Convert.FromBase64String(txt);
                SHA1Managed hash = new SHA1Managed();
                
                bool dataOk = rsaCP.VerifyData(signedData, CryptoConfig.MapNameToOID("SHA1"), signature); // Controleerd de gegenereerde hash met de hash die doorgestuurd werd.
                hashData = hash.ComputeHash(signedData);
                success = true;
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error bij het verifiëren van de Hash");
                Errors.Error(ex, "VerifyHashError");
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
                if(reader != null)
                {
                    reader.Close();
                }
            }
            if(success)
            {
                return rsaCP.VerifyHash(hashData, CryptoConfig.MapNameToOID("SHA1"), signature);
            }
            else
            {
                return false;
            }
        }
    }
}
