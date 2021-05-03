using System;
using System.Collections.Generic;
using System.Text;

namespace Staff.Data.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime LastActionTime { get; set; }

    }
}
