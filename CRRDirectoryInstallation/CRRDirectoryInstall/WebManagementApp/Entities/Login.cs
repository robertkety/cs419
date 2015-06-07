using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRRD_Web_Interface.Entities
{
    public class Login
    {
        public string username { get; set; }
        public string password { get; set; }
        public Login(string un, string pw)
        {
            username = un;
            password = pw;
        }        
    }
}