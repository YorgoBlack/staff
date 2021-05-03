using Staff.Data;
using Staff.Data.Models;
using Staff.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staff.WebApp.Helpers
{
    public class EmployeeConvert
    {
        public static EmployeeModel ToModel(Employee entity)
        {
            return new EmployeeModel()
            {
                Id = entity.Id,
                LastName = entity.LastName,
                FirstName = entity.FirstName,
                Age = entity.Age,
                DepartmentId = entity.Department.Id,
                DepartmentName = $"{entity.Department.Name} ({entity.Department.Floor})",
                GenderId = entity.Gender.Id,
                GenderName = $"{entity.Gender.Name}",
                ExperiencesNames = string.Join(",", entity.Experiences.Select(x=>x.ProgLang.Name)),
                ProgLangsIds = entity.Experiences.Select(x => x.ProgLangId).ToArray(),
            };
        }

        public static Employee ToEntity(EmployeeModel model, IEmployeeService service)
        {
            return new Employee()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.Age,
                Department = service.GetDepartments().FirstOrDefault(x => x.Id == model.DepartmentId),
                Gender = service.GetGenders().FirstOrDefault(x => x.Id == model.GenderId),
                Experiences = model.ProgLangsIds?.Select( x=> new Experience() { ProgLangId = x }).ToList(), 
            };
        }
    }
}
