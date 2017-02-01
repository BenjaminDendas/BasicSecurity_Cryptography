using Microsoft.Win32;
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Crypto_Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CryptoProgramMainWindow : Window
    {
        public string User { get; set; }
        private DESEncryption desEncryptieKlasseObject;
        private AesEncryption aesEncryptieKlasseObject;
        private string strfilename;
        private Steganografie a = null;
        private KeyAndVectorNumber kvn = new KeyAndVectorNumber();
        private string encryptWritePath;
        private string decryptWritePath;

        public CryptoProgramMainWindow(string user)
        {
            // Initializeren van de componenten
            // GUI aanpassen
            // Folders bij het opstarten aanmaken indien deze niet bestaan.
            InitializeComponent();
            encrypt_File_DES_RadioButton.IsChecked = true;
            decrypt_File_DES_RadioButton.IsChecked = true;
            this.User = user;
            this.Title = "Crypto-Program, ingelogd als gebruiker: " + User;
            this.desEncryptieKlasseObject = new DESEncryption(this.User);
            encrypt_File_PrivKeyPathLocation.Content = "Private key van " + this.User + ": ";
            decrypt_file_PrivateRSAKeyLabel.Content = "Private Key van " + this.User + ": ";
            public_rsa_Key.Content = "Public key van verzender.";
            BootFolders();
        }

        public void BootFolders() 
        {
            FolderExist(this.User);
            FolderExist(this.User, "RSA");
            FolderExist(this.User, "DES");
            FolderExist(this.User, "AES");
            FolderExist(this.User, "Bestanden");
            FolderExist(this.User, "Steganografie");
            FolderExist(this.User, "Decrypted");
            FolderExist("Logs");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
                var bericht = MessageBox.Show("Wil je de applicatie Afsluiten en terug gaan naar het login-scherm?", "Afsluiten?", MessageBoxButton.YesNo);
                if (bericht == MessageBoxResult.Yes)
                {
                    LoginWindow win = new LoginWindow();
                    win.Show();
                    this.Hide();
                }
                else
                {
                    e.Cancel = true;
                }     
        }

        #region Encrypteer File

        private void encrypt_File_BestandsLocatieButton_Click(object sender, RoutedEventArgs e)
        {
            BestandenFiles bf = new BestandenFiles(this.User);
            bf.OpenDialog();
            encrypt_File_BestandsLocatieTextBox.Text = bf.loadPath;
        }
        private void encrypt_File_Destination_PubKeyPathLocatieButton_Click(object sender, RoutedEventArgs e)
        {
            BestandenFiles bf = new BestandenFiles(this.User);
            bf.OpenDialog();
            encrypt_File_Destination_PubKeyPath.Text = bf.loadPath;
        }
        private void encrypt_FIle_PrivKeyPathLocationButton_Click(object sender, RoutedEventArgs e)
        {
            BestandenFiles bf = new BestandenFiles(this.User);
            bf.OpenDialog();
            encrypt_File_PrivKeyPathLocationTextbox.Text = bf.loadPath;
        }

        private void GenerateHash()
        {
            FileStream hashStream = null;
            StreamWriter hashWriter = null;
            try
            {
                Verzender v = new Verzender(encrypt_File_PrivKeyPathLocationTextbox.Text);
                byte[] hash = v.HashAndSign(encrypt_File_BestandsLocatieTextBox.Text);
                string basesignedHash = Convert.ToBase64String(hash); // De hash van het bestand opslaan in een base64-string
                hashStream = new FileStream(this.User + "\\" + "Bestanden" + "\\" + "SignedHash", FileMode.OpenOrCreate, FileAccess.Write);
                hashWriter = new StreamWriter(hashStream);
                hashWriter.WriteLine(basesignedHash);      
            }
            catch(Exception ex)
            {
                Errors.Error(ex, "GenerateHashError");
            }
            finally
            {
                if (hashWriter != null)
                {
                    hashWriter.Close();
                }
                if(hashStream != null)
                {
                    hashStream.Close();
                }
            }
        }

        private void encrypt_File_Button_Click(object sender, RoutedEventArgs e)
        {

            BootFolders();
            if (encrypt_File_BestandsLocatieTextBox.Text.Equals(string.Empty) ||
                encrypt_File_Destination_PubKeyPath.Text.Equals(string.Empty) ||
                encrypt_File_PrivKeyPathLocationTextbox.Text.Equals(string.Empty)
                )
            {
                MessageBox.Show("Gelieve alle velden in te vullen.");
            }
            else
            {
                //Des -encryptie
                if (encrypt_File_DES_RadioButton.IsChecked == true)
                {
                    var key = kvn.GenerateRandomWord(8);
                    var iv = kvn.GenerateRandomWord(8);
                    string path = encrypt_File_BestandsLocatieTextBox.Text;
                    FileInfo info = new FileInfo(path);
                    string filename = info.Name;
                    encryptWritePath = Directory.GetCurrentDirectory()+"\\" + this.User + "\\" + "Bestanden" + "\\" + filename + ".enc";
                    desEncryptieKlasseObject.Encrypt(encrypt_File_BestandsLocatieTextBox.Text, encryptWritePath,key, iv, encrypt_File_Destination_PubKeyPath.Text, encrypt_File_DES_RadioButton);
                    GenerateHash();
                }
                else
                {
                    //AES
                    aesEncryptieKlasseObject = new AesEncryption(User);
                    string path = encrypt_File_BestandsLocatieTextBox.Text;
                    FileInfo info = new FileInfo(path);
                    string filename = info.Name;
                    encryptWritePath = Directory.GetCurrentDirectory() + "\\" + this.User + "\\" + "Bestanden" + "\\" + "AES_" + filename + ".enc";
                    aesEncryptieKlasseObject.AesEncrypt(encrypt_File_BestandsLocatieTextBox.Text, encryptWritePath, encrypt_File_Destination_PubKeyPath.Text,encrypt_File_DES_RadioButton);
                    GenerateHash();
                }
            }
            encrypt_File_BestandsLocatieTextBox.Text = string.Empty;
            encrypt_File_Destination_PubKeyPath.Text = string.Empty;
            encrypt_File_PrivKeyPathLocationTextbox.Text = string.Empty;
            
        }
        #endregion

        #region Decrypteer File
        private void decrypt_File_GeencrypteerdeFile_FilePickerButton_Click(object sender, RoutedEventArgs e)
        {
            BestandenFiles bf = new BestandenFiles(this.User);
            bf.OpenDialog();
            decrypt_File_GeencrypteerdeFile_BestandLocatieTextBox.Text = bf.loadPath;
        }
        private void decrypt_file_PrivateRSAKeyButton_Click(object sender, RoutedEventArgs e)
        {
            BestandenFiles bf = new BestandenFiles(this.User);
            bf.OpenDialog();
            decrypt_file_PrivateRSAKeyTextBox.Text = bf.loadPath;
        }

        private void decrypt_File_SymmetricKey_FilePickerButton_Click(object sender, RoutedEventArgs e)
        {
            BestandenFiles bf = new BestandenFiles(this.User);
            bf.OpenDialog();
            decrypt_File_SymmetricKey_BestandLocatieTextBox.Text = bf.loadPath;
        }

        private void decrypt_file_IVBestandLocatieTextBox_Click(object sender, RoutedEventArgs e)
        {
            BestandenFiles bf = new BestandenFiles(this.User);
            bf.OpenDialog();
            decrypt_file_IVTextBox.Text = bf.loadPath;
        }

        private void decrypt_File_EncryptedHash_FilePickerButton_Click(object sender, RoutedEventArgs e)
        {
            BestandenFiles bf = new BestandenFiles(this.User);
            bf.OpenDialog();
            decrypt_File_EncryptedHash_BestandLocatieTextBox.Text = bf.loadPath;
        }

        public void VerifyHash() // Verifiëren van de Hash
        {
                Ontvanger o = new Ontvanger(decrypt_File_publicRsaKey.Text);
                bool test = o.VerifyHash(this.decryptWritePath, decrypt_File_EncryptedHash_BestandLocatieTextBox.Text);
                if (test)
                {
                    MessageBox.Show("Hash-controle geslaagd, het bestand is authentiek");
                }
                else
                {
                    MessageBox.Show("Hash-controle gefaald, Hash van het gedecrypteerde bestand en de verzonden hash zijn niet gelijk!","Hash-controle",MessageBoxButton.OK,MessageBoxImage.Exclamation);
                }       
        }

        private void decrypt_File_DecrypteerButton_Click(object sender, RoutedEventArgs e)
        {
            BootFolders();
            if (decrypt_File_GeencrypteerdeFile_BestandLocatieTextBox.Text.Equals(string.Empty) || 
                decrypt_File_SymmetricKey_BestandLocatieTextBox.Text.Equals(string.Empty) ||
                decrypt_file_IVTextBox.Text.Equals(string.Empty) ||
                decrypt_File_EncryptedHash_BestandLocatieTextBox.Text.Equals(string.Empty) ||
                decrypt_file_PrivateRSAKeyTextBox.Text.Equals(string.Empty))
            {
                MessageBox.Show("Gelieve alle velden in te vullen.");
            }
            else
            {
                this.decryptWritePath = null;
                string path = decrypt_File_GeencrypteerdeFile_BestandLocatieTextBox.Text;
                FileInfo info = new FileInfo(path);
                string filename = info.Name;
                if (info.Extension != string.Empty)
                {
                    this.decryptWritePath = Directory.GetCurrentDirectory() + "\\" + this.User + "\\" + "Decrypted" + "\\" + filename.Substring(0, filename.Length - 4);
                }
                else {
                    this.decryptWritePath = Directory.GetCurrentDirectory() + "\\" + this.User + "\\" + "Decrypted" + "\\" + filename;
                }

                KeyAndVectorNumber kvd = new KeyAndVectorNumber();
                if (decrypt_File_DES_RadioButton.IsChecked == true) // DES - Decryptie
                {                  
                    var key = kvd.GetKey(decrypt_File_SymmetricKey_BestandLocatieTextBox.Text, User, decrypt_file_PrivateRSAKeyTextBox.Text);
                    var iv = kvd.GetIV(decrypt_file_IVTextBox.Text, User, decrypt_file_PrivateRSAKeyTextBox.Text);
                    desEncryptieKlasseObject.Decrypt(decrypt_File_GeencrypteerdeFile_BestandLocatieTextBox.Text, this.decryptWritePath, key, iv, decrypt_file_PrivateRSAKeyTextBox.Text);
                    VerifyHash();     
                }
                else
                {
                    // AES - Decryptie
                    aesEncryptieKlasseObject = new AesEncryption(User);
                    var key = kvd.GetKey(decrypt_File_SymmetricKey_BestandLocatieTextBox.Text, User, decrypt_file_PrivateRSAKeyTextBox.Text);
                    var iv = kvd.GetIV(decrypt_file_IVTextBox.Text, User, decrypt_file_PrivateRSAKeyTextBox.Text);
                    aesEncryptieKlasseObject.AesDecrypt(decrypt_File_GeencrypteerdeFile_BestandLocatieTextBox.Text, this.decryptWritePath, key, iv, decrypt_file_PrivateRSAKeyTextBox.Text);
                    VerifyHash();
                }         
            }
            // Leegmaken van velden na decrypteren.
            decrypt_File_GeencrypteerdeFile_BestandLocatieTextBox.Text = string.Empty;
            decrypt_File_SymmetricKey_BestandLocatieTextBox.Text = string.Empty;
            decrypt_file_IVTextBox.Text = string.Empty;
            decrypt_File_EncryptedHash_BestandLocatieTextBox.Text = string.Empty;
            decrypt_file_PrivateRSAKeyTextBox.Text = string.Empty;
            decrypt_File_publicRsaKey.Text = string.Empty;
        }
        #endregion

        #region Encryteer - Steganografie

        #region select image
        private void encrypt_Steganografie_FotoPickerButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Foto's (.bmp)|*.bmp|All Files (*.*)|*.*";
            dialog.InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location;
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    strfilename = dialog.FileName;
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    if (!System.IO.Path.GetExtension(strfilename).ToUpper().Equals(".BMP"))
                    {
                        encrypt_Steganografie_Image.Source = null;
                        throw new NotSupportedException();
                    }
                    else
                    {
                        bi.UriSource = new Uri(strfilename, UriKind.RelativeOrAbsolute);
                    }

                    bi.EndInit();
                    encrypt_Steganografie_Image.Source = bi;
                }
                catch (NotSupportedException)
                {
                    MessageBox.Show("Het gekozen bestand kan niet gebruikt worden." + Environment.NewLine + "Selecteer een .bmp-bestand");
                }

            }
        }
        private void encrypt_Steganografie_plain_text_Click(object sender, RoutedEventArgs e)
        {
            BootFolders();
            Steganografie encryptPlainText;

            try
            {
                if (encrypt_plain_text_Steganografie_image.Source == null)
                {
                    throw new Errors.NotAllFilledException();
                }

                encryptPlainText = new Steganografie(new Uri(encrypt_plain_text_Steganografie_image.Source.ToString(), UriKind.RelativeOrAbsolute).LocalPath);
                encryptPlainText.TextStringImage = encrypt_steganografie_textbox.Text;

                // Bestande komen in de map van de ingelogde gebruiker terecht onder de vorm van steganografie + de naam van de foto
                encryptPlainText.WriteFile(User + "\\" + "Steganografie" + "\\" + "Steganografie" + System.IO.Path.GetFileName(strfilename));
                encryptPlainText = null;
                MessageBox.Show("Tekst succesvol verwerkt");
                encrypt_Steganografie_Image.Source = null;
            }
            catch (Errors.NotAllFilledException ex)
            {
                MessageBox.Show("Gelieve alle velden in te vullen");
                Errors.Error(ex, "FotoEx");
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show("De geselecteerde map werd niet gevonden");
                Errors.Error(ex, "FotoEx");
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Het geselecteerde bestand werd niet gevonden");
                Errors.Error(ex, "FotoEx");
            }
            catch (Errors.NotEnoughSizeInPhotoException ex)
            {
                MessageBox.Show(ex.Message);
                Errors.Error(ex, "FotoEx");
            }
            catch (Errors.EncryptionException ex)
            {
                MessageBox.Show(ex.Message);
                Errors.Error(ex, "FotoEx");
            }
            catch (Exception ex)
            {
                Errors.Error(ex, "FotoEx");
            }
        }


        private void decrypt_Steganografie_plain_text_Click(object sender, RoutedEventArgs e)
        {
            BootFolders();
            Steganografie decryptPlainText;

            try
            {
                if (decrypt_plain_text_Steganografie_image.Source == null)
                {
                    throw new Errors.NotAllFilledException();
                }

                decryptPlainText = new Steganografie(new Uri(decrypt_plain_text_Steganografie_image.Source.ToString(), UriKind.RelativeOrAbsolute).LocalPath);
                decrypt_steganografie_textblock.Text = decryptPlainText.TextStringImage;

                decrypt_Steganografie_image.Source = null;
            }
            catch (Errors.NotAllFilledException ex)
            {
                MessageBox.Show("Gelieve alle velden in te vullen");
                Errors.Error(ex, "FotoEx");
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show("De geselecteerde map werd niet gevonden");
                Errors.Error(ex, "FotoEx");
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Het geselecteerde bestand werd niet gevonden");
                Errors.Error(ex, "FotoEx");
            }
            catch (Errors.NotEnoughSizeInPhotoException ex)
            {
                MessageBox.Show(ex.Message);
                Errors.Error(ex, "FotoEx");
            }
            catch (Errors.EncryptionException ex)
            {
                MessageBox.Show(ex.Message);
                Errors.Error(ex, "FotoEx");
            }
            catch (Exception ex)
            {
                Errors.Error(ex, "FotoEx");
            }

        }
        private void encrypt_Steganografie_BestandOmTeSteganograferen_Click(object sender, RoutedEventArgs e)
        {
            BestandenFiles bf = new BestandenFiles(this.User);
            bf.OpenDialog();
            encrypt_Steganografie_BestandTextBox.Text = bf.loadPath;
        }

        private void encrypt_Steganografie_plain_text_Foto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Foto's (.bmp)|*.bmp|All Files (*.*)|*.*";
            dialog.InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location;

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    strfilename = dialog.FileName;
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    if (!System.IO.Path.GetExtension(strfilename).ToUpper().Equals(".BMP"))
                    {
                        encrypt_Steganografie_Image.Source = null;
                        throw new NotSupportedException();
                    }
                    else
                    {
                        bi.UriSource = new Uri(strfilename, UriKind.RelativeOrAbsolute);
                    }

                    bi.EndInit();
                    encrypt_plain_text_Steganografie_image.Source = bi;
                }
                catch (NotSupportedException)
                {
                    MessageBox.Show("Het gekozen bestand kan niet gebruikt worden." + Environment.NewLine + "Selecteer een .bmp-bestand");
                }
            }
        }

        private void decrypt_Steganografie_plain_text_Foto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Foto's (.bmp)|*.bmp|All Files (*.*)|*.*";
            dialog.InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location;

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    strfilename = dialog.FileName;
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    if (!System.IO.Path.GetExtension(strfilename).ToUpper().Equals(".BMP"))
                    {
                        encrypt_Steganografie_Image.Source = null;
                        throw new NotSupportedException();
                    }
                    else
                    {
                        bi.UriSource = new Uri(strfilename, UriKind.RelativeOrAbsolute);
                    }

                    bi.EndInit();
                    decrypt_plain_text_Steganografie_image.Source = bi;
                }
                catch (NotSupportedException)
                {
                    MessageBox.Show("Het gekozen bestand kan niet gebruikt worden." + Environment.NewLine + "Selecteer een .bmp-bestand");
                }
            }
        }
        #endregion

        private void encrypt_Steganografie_Button_Click(object sender, RoutedEventArgs e)
        {
            BootFolders();
            try
            {
                if (encrypt_Steganografie_BestandTextBox.Text.Equals(string.Empty) || encrypt_Steganografie_Image.Source == null)
                {
                    throw new Errors.NotAllFilledException();
                }
                Steganografie a = new Steganografie(strfilename);

                a.FileToImage(encrypt_Steganografie_BestandTextBox.Text, User + "\\" + "Steganografie" + "\\" + "Steganografie" + System.IO.Path.GetFileName(strfilename));

                a = null;
                MessageBox.Show("Tekst succesvol verwerkt");
                encrypt_Steganografie_BestandTextBox.Clear();
                encrypt_Steganografie_Image.Source = null;
            }
            catch (Errors.NotAllFilledException ex)
            {
                MessageBox.Show("Gelieve alle velden in te vullen");
                Errors.Error(ex, "FotoEx");
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show("De geselecteerde map werd niet gevonden");
                Errors.Error(ex, "FotoEx");
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Het geselecteerde bestand werd niet gevonden");
                Errors.Error(ex, "FotoEx");
            }
            catch (Errors.NotEnoughSizeInPhotoException ex)
            {
                MessageBox.Show(ex.Message);
                Errors.Error(ex, "FotoEx");
            }
            catch (Errors.EncryptionException ex)
            {
                MessageBox.Show(ex.Message);
                Errors.Error(ex, "FotoEx");
            }
            catch (Exception ex)
            {
                Errors.Error(ex, "FotoEx");
            }
        }
        #endregion

        #region Decrypteer - Steganografie
        private void decrypt_Steganografie_FotoPickerButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Foto's (.bmp)|*.bmp|All Files (*.*)|*.*";
            dialog.InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location;
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    strfilename = dialog.FileName;
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    if (!System.IO.Path.GetExtension(strfilename).ToUpper().Equals(".BMP"))
                    {
                        encrypt_Steganografie_Image.Source = null;
                        throw new NotSupportedException();
                    }
                    else
                    {
                        bi.UriSource = new Uri(strfilename, UriKind.RelativeOrAbsolute);
                    }
                    bi.EndInit();
                    decrypt_Steganografie_image.Source = bi;
                }
                catch (NotSupportedException)
                {
                    MessageBox.Show("Het gekozen bestand kan niet gebruikt worden." + Environment.NewLine + "Selecteer een .bmp-bestand");
                }
            }
        }

        private void decrypt_Steganografie_BestandOmTeSteganograferen_Click(object sender, RoutedEventArgs e)
        {
            BestandenFiles bf = new BestandenFiles(this.User);
            bf.SaveDialog();
            decrypt_Steganografie_BestandTextBox.Text = bf.savePath;
        }

        private void decrypt_Steganografie_Button_Click(object sender, RoutedEventArgs e)
        {
            BootFolders();
            FileStream writeStream = null;
            StreamWriter writer = null;
            try
            {
                if (decrypt_Steganografie_BestandTextBox.Text.Equals(string.Empty) || decrypt_Steganografie_image.Source == null)
                {
                    throw new Errors.NotAllFilledException();
                }
                a = new Steganografie(strfilename);
                a.WriteFileFromImage(decrypt_Steganografie_BestandTextBox.Text);
            }
            catch (Errors.NotAllFilledException)
            {
                MessageBox.Show("Gelieve alle velden in te vullen");
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show("De geselecteerde map werd niet gevonden");
                Errors.Error(ex, "FotoEx");
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Het geselecteerde bestand werd niet gevonden");
                Errors.Error(ex, "FotoEx");
            }
            catch (IOException ex)
            {
                MessageBox.Show("Er ging iets mis :" + Environment.NewLine + ex.Message);
            }
            catch (Errors.EncryptionException ex)
            {
                MessageBox.Show(ex.Message);
                Errors.Error(ex, "FotoEx");
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
                if (writeStream != null)
                {
                    writeStream.Close();
                }
                decrypt_Steganografie_BestandTextBox.Clear();
                decrypt_Steganografie_image.Source = null;
            }
        }

        #endregion
        private string read(string bestandsnaam)
        {
            StreamReader loginReader = null;
            FileStream tekstStream = null;
            StringBuilder tekst = null;
            try
            {
                int amountOfLines = File.ReadAllLines(bestandsnaam).Length;
                tekst = new StringBuilder();
                tekstStream = new FileStream(bestandsnaam, FileMode.Open, FileAccess.Read);
                loginReader = new StreamReader(tekstStream);
                string line = loginReader.ReadLine();
                for (int i = 0; i < amountOfLines; i++)
                {
                    tekst.Append(line + Environment.NewLine);
                    line = loginReader.ReadLine();
                }
            }
            catch(Exception ex)
            {
                Errors.Error(ex,this.User,"ReadMethodError");
            }
            finally
            {
                if (loginReader != null)
                {
                    loginReader.Close();
                }
                if(tekstStream != null)
                {
                    tekstStream.Close();
                }
            }     
            return tekst.ToString();
        }

        #region Hash
        private void decrypt_File_PublicRsaKeyButton_Click(object sender, RoutedEventArgs e)
        {
            BestandenFiles bf = new  BestandenFiles(this.User);
            bf.OpenDialog();
            decrypt_File_publicRsaKey.Text = bf.loadPath;
        }
        #endregion

        private void bestandMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var bericht = MessageBox.Show("Wil je de applicatie Afsluiten en terug gaan naar het login-scherm?", "Afsluiten?", MessageBoxButton.YesNo);
            if (bericht == MessageBoxResult.Yes)
            {
                LoginWindow win = new LoginWindow();
                win.Show();
                this.Hide();
            }
            else
            {
                
            }     
        }
        private void DecryptFileSteganografieHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Met deze functie kunt u een bestand dat verborgen werd in een file terug recupereren." + Environment.NewLine + Environment.NewLine +
                "Stappenplan: " + Environment.NewLine + Environment.NewLine +
                "Stap 1: Kies de foto waarin het verborgen bestand zit." + Environment.NewLine + Environment.NewLine +
                "Stap 2: Kies het pad naar waar je het verborgen bestand wilt schrijven." + Environment.NewLine + Environment.NewLine
                );
        }

        private void DecryptTekstSteganografieHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Met deze functie kunt u tekst (een string) uit een afbeelding halen en deze in een textblock weergeven." + Environment.NewLine + Environment.NewLine +
                            "Stappenplan: " + Environment.NewLine + Environment.NewLine +
                            "Stap 1: Selecteer de foto waar de tekst is in verborgen." + Environment.NewLine + Environment.NewLine +
                            "Klik op de knop.");
        }

        private void DecryptFileHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Met de functie decrypteer bestand, kunt u een bestand dat u ontvangen heeft decrypteren met het DES of AES algoritme." + Environment.NewLine + Environment.NewLine +
                            "Stappenplan: " + Environment.NewLine + Environment.NewLine +
                            "Stap 1: Selecteer de methode waarmee u wilt decrypteren (U vind de gebruikte methode als prefix op de Symmetrische Key die u gekregen heeft (DES_SymmetricKey of AES_SymmetricKey)" + Environment.NewLine + Environment.NewLine +
                            "Stap 2: Specifieer het pad waar het geëncrypteerde bestand zich bevindt." + Environment.NewLine + Environment.NewLine + 
                            "Stap 3: Specifieer het pad waar de symmetrische key zich bevindt." + Environment.NewLine + Environment.NewLine +
                            "Stap 4: Specifieer het pad waar de initialisatie Vector (IV) zich bevindt." + Environment.NewLine + Environment.NewLine +
                            "Stap 5: Specifieer het pad waar de geëncrypteerde hash zich bevindt." + Environment.NewLine + Environment.NewLine +
                            "Stap 6: Specifieer het pad waar uw private-key zich bevindt." + Environment.NewLine + Environment.NewLine +
                            "Stap 7: Specifeer het pad waar de public-key van de verzender zich bevindt."
                );
        }

        private void EncryptPlainTekstSteganografie_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Met deze functie kunt u zelf ingegeven tekst wegschrijven naar een foto (.bmp)" + Environment.NewLine + Environment.NewLine +
                "Stappenplan:" + Environment.NewLine + Environment.NewLine + 
                "Stap 1: Selecteer de foto naar waar u de tekst wilt schrijven" + Environment.NewLine + Environment.NewLine +
                "Stap 2: Schrijf de tekst die u wilt wegschrijven naar de foto." + Environment.NewLine + Environment.NewLine + 
                "Stap 3: Klik op de Verberg Tekst knop." + Environment.NewLine + Environment.NewLine +
                "De foto met de verborgen tekst vind u onder: " + AppDomain.CurrentDomain.BaseDirectory + this.User + @"\Steganografie");
        }

        private void EncryptFileSteganografieHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("U kunt een file wegschrijven naar een foto door gebruik te maken van steganografie." + Environment.NewLine + Environment.NewLine +
                "Stappenplan:" + Environment.NewLine + Environment.NewLine + 
                "Stap 1: Selecteer een foto waarnaar je de gegevens wilt schrijven" + Environment.NewLine + Environment.NewLine +
                "Stap 2: Selecteer het bestand dat u naar de foto wilt schrijven" + Environment.NewLine + Environment.NewLine +
                "Stap 3: Klik op de verberg bestand-knop" + Environment.NewLine + Environment.NewLine +
                "De foto met de verborgen file vind u onder : " + AppDomain.CurrentDomain.BaseDirectory  + this.User + @"\Steganografie");
        }

        private void EncryptFileHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Met de functie encrypteer bestand, kunt u eender welk bestand encrypteren met het DES of AES algoritme." + Environment.NewLine + Environment.NewLine +
                            "Stappenplan: " + Environment.NewLine + Environment.NewLine + 
                            "Stap 1: Selecteer de methode waarmee u wilt encrypteren (DES of AES)" + Environment.NewLine + Environment.NewLine +
                            "Stap 2: Selecteer de public key van de ontvanger (de persoon waarnaar u het geëncrypteerde bericht wilt sturen)" + Environment.NewLine + Environment.NewLine +
                            "Stap 3: Selecteeer uw eigen private Key om het bestand van een digitale handtekening te voorzien."
                );
        }

        private void algemeenHelpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dit programma is bedoeld om gegevens te encrypteren en decrypteren door gebruik te maken van een AES of DES algoritme of gegevens te verbergen binnen een foto m.b.v. steganografie." + Environment.NewLine + Environment.NewLine +
                            "U vind de gegenereerde bestanden onder hun gespecifieerde mappen: " + Environment.NewLine + Environment.NewLine +
                            "RSA: Public- en Private Key van de gebruiker" + Environment.NewLine + Environment.NewLine +
                            "DES: De Symmetrische Key en Initialisatie Vector (IV) voor het encrypteren/Decrypteren met behulp van het DES-algoritme." + Environment.NewLine + Environment.NewLine +
                            "AES: De Symmetrische Key en Initialisatie Vector (IV) voor het encrypteren/Decrypteren met behulp van het AES-algoritme." + Environment.NewLine + Environment.NewLine +
                            "Het geëncrypteerde bestand en de Signed-hash vind u in de Bestanden folder." + Environment.NewLine + Environment.NewLine + 
                            "Gedecodeerde bestanden zullen in de Decrypted map verschijnen." + Environment.NewLine + Environment.NewLine + 
                            "Bestanden die verborgen zijn m.b.v. steganografie zullen in de steganografie-folder verschijnen."
                );
        }

        public void FolderExist(string rootMap)
        {
            if (!(Directory.Exists(Directory.GetCurrentDirectory() + "\\" + rootMap)) == true)
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + rootMap);
                Debug.WriteLine("Root Folder aangemaakt.");
            }
            else
            {
                Debug.WriteLine("Folder bestaat al.");
            }
        }

        public void FolderExist(string gebruiker, string mapnaam)
        {
            if (!(Directory.Exists(Directory.GetCurrentDirectory() + "\\" + gebruiker.Trim() + "\\" + mapnaam.Trim())) == true)
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + gebruiker.Trim() + "\\" + mapnaam.Trim());
                Debug.WriteLine("Subfolder aangemaakt.");
            }
            else
            {
                Debug.WriteLine("Folder bestaat al.");
            }
        }




    }
}
