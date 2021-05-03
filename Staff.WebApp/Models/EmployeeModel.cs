using Microsoft.AspNetCore.Mvc.Rendering;
using Staff.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Staff.WebApp.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        
        [Required]
        [Range(18, 100)]
        public int Age { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int GenderId { get; set; }
        public string GenderName { get; set; }
        public string ExperiencesNames { get; set; }
        public int[] ProgLangsIds { get; set; }

        public bool IsDisabled { get; set; } = false;

    }
}
