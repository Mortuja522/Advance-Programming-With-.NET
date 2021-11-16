using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Classes
{
    public class UserClass
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Phone { get; set; }
        public string Address { get; set; }
        public int Type { get; set; }
        public int active { get; set; }
        public string Image { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }
    }
}