using System;
using System.Collections.Generic;
using System.Text;

namespace LycheeClientPortal
{
    public class User
    {
        public string FullName { get; set; }
        public string AuthSysUserId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public User(string name, string id, string phone, string email)
        {
            this.FullName = name;
            this.AuthSysUserId = id;
            this.Phone = phone;
            this.Email = email;
        }
    }
}