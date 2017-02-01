using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_Program
{
    public class GenerateSHA256Hash
    {
        private byte[] wachtwoordByteArray = null;
        public string hashString { get; set; }

        public string GenerateHash(string woord)
        {
            SHA256Managed sha256HashString = new SHA256Managed();
            try
            {
                this.wachtwoordByteArray = Encoding.UTF8.GetBytes(woord); // Converteerd het ingegeven wachtwood naar bytes op basis van UTF8 Encodering.
               
                byte[] hash = sha256HashString.ComputeHash(this.wachtwoordByteArray); // genereerd de SHA256 hash op basis van de byteArray van het wachtwoord 
                
                this.hashString = string.Empty;
                foreach (byte x in hash)
                {
                    this.hashString += String.Format("{0:x2}", x); // {0:x2} ==> Hexadecimaal, 2 characters
                }
            }
            catch (Exception ex)
            {               
                Errors.Error(ex, "GenerateSHA256HASH-Error");
            }
            finally
            {
                sha256HashString.Clear(); // Laat alle resources vrij zodat ze gebruikt kunnen worden voor andere zaken.
            }

            return this.hashString;
        }
    }
}
