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
    public class KeyAndVectorNumber
    {
        
        private FileStream keyStream = null;
        private FileStream ivStream = null;
        private FileStream keyWriteStream = null;
        private FileStream ivWriteStream = null;
        
        private StreamReader ivReader = null;
        private StreamReader keyReader = null;

        private StreamWriter ivWriter = null;
        private StreamWriter keyWriter = null;     

        private CspParameters cspp = new CspParameters();
        private const string keyname = "Key01";
        private RSACryptoServiceProvider rsa;
        private StreamReader rsaReader = null;

        public byte[] keyValues { get; set; }
        public byte[] ivValues { get; set; }
        public byte[] HashValues { get; set; }

        public RSACryptoServiceProvider RSACSP
        {
            get { return rsa; }
            set { rsa = value; }
        }

        public byte[] GenerateRandomNumber(int length)
        {
            using (RNGCryptoServiceProvider randomgenerator = new RNGCryptoServiceProvider())
            {
                byte[] randomNumber = new byte[length];
                randomgenerator.GetBytes(randomNumber);

                return randomNumber;
            }
        }

        public byte[] GenerateRandomWord(int lengte) // Genereer een random woord
        {
            string result = Path.GetRandomFileName(); // Genereer een random woord
            Debug.Write(result);
            result.Replace(".",""); // Filter de . eruit.
            result = result.Substring(0, lengte); // neemt de opgegeven aantal characters op.
            char[] charArray = result.ToCharArray(); // Omzetten naar een array van characters
            Debug.Write(result);
            byte[] random = new byte[lengte];
            random = Encoding.ASCII.GetBytes(charArray);
            return random;
        }

        public byte[] GetKey(string keyFile,string user, string privateRSAKey)
        {
            try
            {
                rsaReader = new StreamReader(privateRSAKey); // Streamreader die de private key inleest.
                keyStream = new FileStream(keyFile, FileMode.Open, FileAccess.Read);
                keyReader = new StreamReader(keyStream);

                cspp.KeyContainerName = keyname;
                rsa = new RSACryptoServiceProvider(cspp); // RSA Object
                string keytxt = rsaReader.ReadToEnd();  // Leest de private key in
                rsa.FromXmlString(keytxt);
                rsa.PersistKeyInCsp = true;
                if (rsa.PublicOnly == false)
                {
                    MessageBox.Show("Private key imported.");
                }
                string tekst = keyReader.ReadToEnd();
                byte[] convertedKey = Convert.FromBase64String(tekst); // converteerd de base64 string terug naar een byte array om de Key waarden te kennen.
                keyValues = rsa.Decrypt(convertedKey, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error bij het ophalen van de Key.");
                Errors.Error(ex, "GetKeyError");
            }
            finally
            {
                if (keyStream != null)
                {
                    keyStream.Close();
                }

                if (keyReader != null)
                {
                    keyReader.Close();
                }
                if (rsaReader != null)
                {
                    rsaReader.Close();
                }
            }

            return keyValues;
        }

        public byte[] GetIV(string ivFile , string user, string privateRSA)
        {
            try
            {
                rsaReader = new StreamReader(privateRSA);
                ivStream = new FileStream(ivFile, FileMode.Open, FileAccess.Read);
                ivReader = new StreamReader(ivStream);

                cspp.KeyContainerName = keyname;
                rsa = new RSACryptoServiceProvider(cspp);
                string keytxt = rsaReader.ReadToEnd();
                rsa.FromXmlString(keytxt);
                rsa.PersistKeyInCsp = true;
                if(rsa.PublicOnly == false)
                {
                    MessageBox.Show("Private key imported.");
                }
                string tekst = ivReader.ReadToEnd();
                byte[] convertedIV = Convert.FromBase64String(tekst);
                ivValues = rsa.Decrypt(convertedIV,false);
            }
            catch(CryptographicException ex)
            {
                MessageBox.Show("U heeft geen machtiging tot dit bestand.");
                Errors.Error(ex, "GetIVError");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error bij het ophalen van de IV");
                Errors.Error(ex, "GetIVError");
            }
            finally
            {
                if (ivStream != null)
                {
                    ivStream.Close();
                }
                if (ivReader != null)
                {
                    ivReader.Close();
                }
                if(rsaReader != null)
                {
                    rsaReader.Close();
                }
            }
            return ivValues;
        }

        public void WriteKey(byte[] key, string user, string pubRSAKey,RadioButton b)
        {
            rsaReader = new StreamReader(pubRSAKey);
            this.cspp.KeyContainerName = keyname;
            this.rsa = new RSACryptoServiceProvider(cspp);
            string line = rsaReader.ReadToEnd();
            this.rsa.FromXmlString(line);
            rsa.PersistKeyInCsp = true;

            if(b.IsChecked == true)
            {
                keyWriteStream = new FileStream(user + "\\" + "DES" + "\\" + "DES_SymmetricKey", FileMode.Create, FileAccess.Write);
            }
            else
            {
                keyWriteStream = new FileStream(user + "\\" + "AES" + "\\" + "AES_SymmetricKey", FileMode.Create, FileAccess.Write);
            }
            
            keyWriter = new StreamWriter(keyWriteStream);

            try
            {
                byte[] encryptedKey = rsa.Encrypt(key, false); // Encrypteerd de key met het RSA Algorithme
                keyWriter.Write(Convert.ToBase64String(encryptedKey)); // schrijft de key weg als een base64 string naar het bestand.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error bij het wegschrijven van de Key.");
                Errors.Error(ex, "WriteKeyError");
            }
            finally
            {
                if (keyWriter != null)
                {
                    keyWriter.Close();
                }

                if (keyWriteStream != null)
                {
                    keyWriteStream.Close();
                }
            }
        }

        public void WriteIV(byte[] iv, string user, string pubRSAKey, RadioButton b)
        {
            rsaReader = new StreamReader(pubRSAKey);
            this.cspp.KeyContainerName = keyname;
            this.rsa = new RSACryptoServiceProvider(cspp);
            string line = rsaReader.ReadToEnd();
            this.rsa.FromXmlString(line); // Zet de keys in het rsa object vanuit de XML string.
            rsa.PersistKeyInCsp = true;
            if(b.IsChecked == true)
            {
                ivWriteStream = new FileStream(user + "\\" + "DES" + "\\" + "DES_IV", FileMode.Create, FileAccess.Write);
            }
            else
            {
                ivWriteStream = new FileStream(user + "\\" + "AES" + "\\" + "AES_IV", FileMode.Create, FileAccess.Write);
            }
            
            ivWriter = new StreamWriter(ivWriteStream);

            try
            {
                byte[] encryptedIV = rsa.Encrypt(iv,false);
                ivWriter.Write(Convert.ToBase64String(encryptedIV)); // Schrijft de IV weg als een base64-string naar het bestand.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error bij het wegschrijven van de IV");
                Errors.Error(ex, "WriteIVError");
            }
            finally
            {
                if (ivWriter != null)
                {
                    ivWriter.Close();
                }

                if (ivWriteStream != null)
                {
                    ivWriteStream.Close();
                }
                if (rsaReader != null)
                {
                    rsaReader.Close();
                }
            }
        }

        
    }
}
