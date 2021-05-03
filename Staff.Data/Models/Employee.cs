using System;
using System.Collections.Generic;
using System.Text;

namespace Staff.Data.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Department Department { get; set; }
        public ICollection<Experience> Experiences { get; set; } = new List<Experience>();
        public bool IsDeleted { get; set; }
    }
}
