using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Crypto_Program
{
    public class ReadLogin
    {
        private int amountOfLines;
        private FileStream loginStream = null;
        private StreamReader loginReader = null;
        private string line = null;
        private string[] loginArray = new string[2];
        private List<Login> loginLijst = new List<Login>();
        private Login loginObject;

        public List<Login> ReadFunction() // Leest alle users in.
        {
            try
            {        
                if(File.Exists("Users"))
                {
                    this.amountOfLines = File.ReadAllLines("Users").Length;
                    Debug.WriteLine(amountOfLines);
                    loginStream = new FileStream("Users", FileMode.Open, FileAccess.Read);
                    loginReader = new StreamReader(loginStream);
                    this.line = loginReader.ReadLine();
                    for (int i = 0; i < amountOfLines; i++)
                    {
                        this.loginArray = line.Split(';');
                        this.loginObject = new Login(loginArray[0], loginArray[1]);
                        loginLijst.Add(loginObject);
                        this.line = loginReader.ReadLine();
                    }     
                }
                else
                { }         
            }
            catch(Exception ex)
                {
                    MessageBox.Show("Error bij het inlezen van de gebruikers.");
                    Errors.Error(ex, "ReadFunctionError");
                }
            finally
            {
                if(loginStream != null)
                {
                    loginStream.Close();
                }
                if(loginReader != null)
                {
                    loginReader.Close();
                }
            }
            return this.loginLijst;
        }
    }
}
