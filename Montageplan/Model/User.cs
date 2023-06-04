using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdministrator { get; set; }

        public User()
        {
            this.Username = "";
            this.Password = "";
            this.IsAdministrator = false;
        }

        public User(string name, string pass, bool isAdmin)
        {
            this.Username = name;
            this.Password = pass;
            this.IsAdministrator = isAdmin;
        }
    }
}
