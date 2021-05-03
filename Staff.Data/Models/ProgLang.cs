using System;
using System.Collections.Generic;
using System.Text;

namespace Staff.Data.Models
{
    public class ProgLang
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Experience> Experiences { get; set; } = new List<Experience>();
    }
}
