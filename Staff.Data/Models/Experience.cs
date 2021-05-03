using System;
using System.Collections.Generic;
using System.Text;

namespace Staff.Data.Models
{
    public class Experience
    {
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public int ProgLangId { get; set; }
        public virtual ProgLang ProgLang { get; set; }
        public bool IsDeleted { get; set; }

    }
}
