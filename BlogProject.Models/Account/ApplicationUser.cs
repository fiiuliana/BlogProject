using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Models.Account
{
    public class ApplicationUser
    {
        public int ApplicationUserId { get; set; }
        public string Username { get; set; }
        public string Fulname{ get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
