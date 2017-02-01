using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Crypto_Program
{
    public class Steganografie
    {
        private bool isDecrypted = false;
        private List<byte> characterList = new List<byte>();
        private string baseImagePath;
        private string decryptedString;

        public Steganografie(string baseImagePath)
        {
            int byteReader;
            FileStream fs = null;

            this.baseImagePath = baseImagePath;

            try
            {
                fs = new FileStream(baseImagePath, FileMode.Open, FileAccess.Read);

                while ((byteReader = fs.ReadByte()) != -1)
                    characterList.Add((byte)byteReader);

                if (IsEncrypted)
                    IsEncrypted = true;
                else
                    IsEncrypted = false;

                if (characterList[0] != 66 || characterList[1] != 77) //BMP header test
                    throw new Errors.EncryptionException("Geen geldige foto, je moet een bmp gebruiken!");

            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("File '" + baseImagePath + "' was not found.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException("Map '" + Path.GetDirectoryName(Path.GetFullPath(baseImagePath)) + "' niet gevonden.");
            }
            catch (PathTooLongException)
            {
                throw new PathTooLongException("Pad is te lang, probeer de foto naar een kortere locatie te verplaatsen zoals C:\\ en probeer opnieuw");
            }
            catch (IOException e)
            {
                throw new IOException("IOException vond plaats", e);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        public void WriteFile(string destinationImageFilePath)
        {
            FileStream fs = null;
            byte[] bytes = characterList.ToArray<byte>();

            try
            {
                if (!Directory.Exists(destinationImageFilePath))
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationImageFilePath));

                fs = new FileStream(destinationImageFilePath, FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("Bestand '" + destinationImageFilePath + "' niet gevonden.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException("Map'" + destinationImageFilePath + "' niet gevonden.");
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("Onvoldoende machtigingen voor bestand: '" + destinationImageFilePath + "'");
            }
            catch (PathTooLongException)
            {
                throw new PathTooLongException("Pad is te lang, probeer de foto naar een kortere locatie te verplaatsen zoals C:\\ en probeer opnieuw.");
            }
            catch (IOException e)
            {
                throw new IOException("IOException vond plaats", e);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        public string TextStringImage
        {
            get
            {
                FileStream fs = null;
                int numbers = 0;
                string calculatedSize = String.Empty;
                StringBuilder sb = new StringBuilder();

                if (!IsEncrypted)
                    throw new Errors.EncryptionException("Bestand/Foto was niet ge-encrypteerd, dus deze kon niet worden ge-de-crypteerd!");

                //Method:
                try
                {
                    for (int i = characterList.Count - 2; i > characterList.Count - 18; i -= 2)
                        calculatedSize = (char)(characterList[i] % 16 + characterList[i - 1] % 16 * 16) + calculatedSize;

                    numbers = Int32.Parse(calculatedSize, System.Globalization.NumberStyles.HexNumber);

                    for (int i = 0; i < numbers; i++)
                        sb.Append((char)(characterList[characterList.Count - i * 2 - 18] % 16 + characterList[characterList.Count - i * 2 - 19] % 16 * 16));
                }
                catch (FileNotFoundException)
                {
                    throw new FileNotFoundException("Bestand '" + baseImagePath + "' niet gevonden, was dit het correcte pad?");
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }

                return sb.ToString();
            }

            set
            {
                char[] sizeCharArray;

                //Tests:
                if (characterList.Count - value.Length * 2 - 137 < 0)
                    throw new Errors.NotEnoughSizeInPhotoException("Data is te groot voor deze foto, u heeft met deze foto '" + (characterList.Count - 136) + "' karakters ter beschikking en u gebruikt er '" + value.Length + "'");

                sizeCharArray = String.Format("{0:X8}", value.Length).ToCharArray();

                for (int i = characterList.Count - 2, j = sizeCharArray.Length - 1; j >= 0; i -= 2, j--)
                {
                    characterList[i] = (byte)(characterList[i] / 16 * 16 + sizeCharArray[j] % 16);
                    characterList[i - 1] = (byte)(characterList[i - 1] / 16 * 16 + sizeCharArray[j] / 16);
                }

                for (int i = characterList.Count - sizeCharArray.Length * 2 - 2, j = 0; j < value.Length; i -= 2, j++)
                {
                    characterList[i] = (byte)(characterList[i] / 16 * 16 + (byte)(value.ElementAt(j) % 16));
                    characterList[i - 1] = (byte)(characterList[i - 1] / 16 * 16 + (byte)value.ElementAt(j) / 16);
                }

                IsEncrypted = true;
            }
        }

        public void FileToImage(string fromfile, string destinationFile)
        {
            FileStream fs = null;
            StringBuilder sb = new StringBuilder();
            int readBytesTest;

            try
            {
                fs = new FileStream(fromfile, FileMode.Open, FileAccess.Read);

                while ((readBytesTest = fs.ReadByte()) != -1)
                    sb.Append(String.Format("{0:X2}", readBytesTest));

                TextStringImage = sb.ToString() + "." + fromfile.Split('.')[destinationFile.Split('.').Length - 1];
                WriteFile(destinationFile);
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException("Map '" + destinationFile + "' niet gevonden.");
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("Bestand '" + destinationFile + "' niet gevonden");
            }
            catch (PathTooLongException)
            {
                throw new PathTooLongException("Pad '" + destinationFile + "' niet gevonden.");
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("Onvoldoende machtigingen voor bestand '" + destinationFile + "'");
            }
            catch (IOException)
            {
                throw new IOException("IOException: normaal kan dit niet, vraag de Author om dit na te kijken.");
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        public void WriteFileFromImage(string destinationFilePath)
        {
            string mssg;

            if (!isDecrypted)
            {
                if (!IsEncrypted)
                    throw new Errors.EncryptionException("Een bestand dat niet ge-encrypteerd is kan niet ge-de-crypteerd worden!");

                mssg = String.Empty;

                for (int i = 0; i < TextStringImage.Split('.').Length - 1; i++)
                    mssg += TextStringImage.Split('.')[i] + ".";

                if (TextStringImage.Contains('.'))
                    decryptedString = "." + TextStringImage.Split('.')[TextStringImage.Split('.').Length - 1];
                else
                    decryptedString = String.Empty;

                characterList.Clear();

                for (int i = 0; i < mssg.Length - 1; i += 2)
                    characterList.Add(Byte.Parse(Convert.ToString(mssg[i]) + Convert.ToString(mssg[i + 1]), System.Globalization.NumberStyles.HexNumber));

                isDecrypted = true;
            }

            WriteFile(destinationFilePath + decryptedString);
        }

        public bool IsEncrypted
        {
            get { return characterList[characterList.Count - 1] == 2 ? true : false; }
            set
            {
                if (value == true)
                    characterList[characterList.Count - 1] = 2;
                else
                    characterList[characterList.Count - 1] = 1;
            }
        }
    }
}
