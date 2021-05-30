using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_BO
{
    public class LoginBo
    {
        private string username;
        private string userpin;
        public string Username => this.username;
        public string Userpin => this.userpin;

        public LoginBo(string userName, string userPin)
        {
            this.username = userName;
            this.userpin = userPin;
        }
    }
}
