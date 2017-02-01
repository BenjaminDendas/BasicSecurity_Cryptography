using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_Program
{
    public class Login
    {
        public string login { get; set; }
        public string passwoord { get; set; }

        public Login(string login, string passwoord)
        {
            this.login = login;
            this.passwoord = passwoord;
        }
    }
}
