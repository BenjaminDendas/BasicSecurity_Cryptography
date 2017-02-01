using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Crypto_Program
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private List<Login> loginLijst = new List<Login>();
        private bool found = false;
        private bool exist = false;
        public string User { get; set; }
        private Registreer reg = null;


        public LoginWindow()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            loginLijst.Clear();
            ReadLogin rl = new ReadLogin();
            loginLijst = rl.ReadFunction();
            Controleer(log_UsernameTextBox, log_UserPasswordPasswordBox);
            log_UsernameTextBox.Text = string.Empty;
            log_UserPasswordPasswordBox.Clear();
        }

        private void registreerButton_Click(object sender, RoutedEventArgs e)
        {
            loginLijst.Clear();
            ReadLogin rl = new ReadLogin();
            loginLijst = rl.ReadFunction();
            reg = new Registreer();
            TestExists(reg_usernameTextBox, reg_UserPasswordPasswordBox);      
        }
        private void TestExists(TextBox username, PasswordBox wachtwoord) // Controle of een user al bestaat.
        {
            GenerateSHA256Hash hash = new GenerateSHA256Hash();
            string gebruiker = hash.GenerateHash(username.Text);

            foreach (Login log in loginLijst)
            {
                if (log.login.Equals(gebruiker))
                 {
                            MessageBox.Show("Gebruikersnaam al in gebruik");
                            exist = true;
                            break;
                }
            }

                if (username.Text.Equals("") || wachtwoord.Password.Equals(""))
                {
                     MessageBox.Show("Gelieve alle velden in te vullen.");
                }
                else
                {
                  if (username.Text.Equals(wachtwoord.Password))
                    {
                        MessageBox.Show("Het wachtwoord mag niet hetzelfde zijn als de login.");
                     }
                        else
                            {
                                if (!exist)
                                {
                                    reg.RegistreerGebruiker(reg_usernameTextBox, reg_UserPasswordPasswordBox);
                                }
                            }
                     }
                reg_UserPasswordPasswordBox.Clear();
                reg_usernameTextBox.Clear();
                this.exist = false;
               }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var bericht = MessageBox.Show("Ben je zeker dat je de applicatie wilt afsluiten?", "Afsluiten?", MessageBoxButton.YesNo);
            if (bericht == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void Controleer(TextBox username, PasswordBox passwoord) // controleren of de ingegeven credentials overeenkomen met de opgeslagen.
        {
            GenerateSHA256Hash hash = new GenerateSHA256Hash();
            string gebruiker = hash.GenerateHash(username.Text);
            string ww = hash.GenerateHash(passwoord.Password);

            if (!found)
            {
                foreach (Login log in loginLijst)
                {
                    if (log.login.Equals(gebruiker) && log.passwoord.Equals(ww))
                    {
                        MessageBox.Show("Welkom " + username.Text);
                        User = username.Text;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    MessageBox.Show("Foutieve gegevens.");
                }
                else
                {
                    CryptoProgramMainWindow win = new CryptoProgramMainWindow(User);
                    this.Hide();
                    win.Show();
                }
            }
        }

    }
}
