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
    public class Registreer
    {
        private StreamWriter writer = null;
        private FileStream writeStream = null;
        private GenerateSHA256Hash hashGenerator = new GenerateSHA256Hash();
        private RSAParameters RSAParm;
        private RSAParameters privatekey;
        private RSAParameters publicKey;
        private RSACryptoServiceProvider rsa;
        private string path = null;
        private string publicPrivateKeyXML;
        private string publicOnlyxml;

        public void RegistreerGebruiker(TextBox username, PasswordBox wachtwoord)
        {
            string usernameHash = hashGenerator.GenerateHash(username.Text);
            string passwordHash = hashGenerator.GenerateHash(wachtwoord.Password);
            SchrijfWeg(usernameHash, passwordHash);
            GenerateFolderAndKeys(username.Text.Trim());
        }

        private void SchrijfWeg(string usernameHash, string passwordHash) /// Schrijft een user weg naar de Users file.
        {
            try
            {
                if (File.Exists("Users"))
                {
                    writeStream = new FileStream(@"Users", FileMode.Append, FileAccess.Write);
                    writer = new StreamWriter(writeStream);
                    writer.WriteLine(usernameHash + ";" + passwordHash);
                }
                else
                {
                    writeStream = new FileStream("Users", FileMode.CreateNew, FileAccess.Write);
                    writer = new StreamWriter(writeStream);
                    writer.WriteLine(usernameHash + ";" + passwordHash);
                }
                MessageBox.Show("Account succesvol aangemaakt");

            }
            catch (IOException e)
            {
                MessageBox.Show("Error bij het registreren van de gebruiker");
                Errors.Error(e, "RegistreerError");
            }
            catch (Exception ex)
            {
                Errors.Error(ex, "RegistreerError");
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
            }
        }

        public void GenerateFolderAndKeys(string gebruiker) // Genereerd de nodige folders en de RSA Keys voor de gebruiker.
        {
            try
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + gebruiker);
                Directory.CreateDirectory(gebruiker + "\\" + "RSA");
                Directory.CreateDirectory(gebruiker + "\\" + "DES");
                Directory.CreateDirectory(gebruiker + "\\" + "AES");
                Directory.CreateDirectory(gebruiker + "\\" + "Bestanden");
                Directory.CreateDirectory(gebruiker + "\\" + "Steganografie");
                Directory.CreateDirectory("Logs");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Onmogelijk om de nodige mappen aan te maken, heeft u de juiste rechten?");
                Errors.Error(ex, "CreerFolderError");
            }
            
            this.path = Directory.GetCurrentDirectory() + "\\" + gebruiker;
            this.rsa = new RSACryptoServiceProvider();
            try
            {
                this.RSAParm = rsa.ExportParameters(true);
                this.privatekey = rsa.ExportParameters(true);
                this.publicKey = rsa.ExportParameters(false);

                this.writer = new StreamWriter(path + "\\" + "RSA" + "\\" + "Private_" + gebruiker);
                publicPrivateKeyXML = rsa.ToXmlString(true);
                this.writer.Write(publicPrivateKeyXML);
                this.writer.Flush();

                //public key only
                this.writer = new StreamWriter(path + "\\" + "RSA" + "\\" + "Public_" + gebruiker);
                this.publicOnlyxml = rsa.ToXmlString(false);
                this.writer.Write(publicOnlyxml);
                this.writer.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Onmogelijk om RSA Keys te genereren.");
                Errors.Error(ex, "GenerateKeysError");
            }
            finally
            {
                if (this.writer != null)
                {
                    this.writer.Close();
                }
            }
        }
    }
}
