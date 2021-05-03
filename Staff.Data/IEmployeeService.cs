using System;
using System.Collections.Generic;
using System.Text;

using Staff.Data.Models;

namespace Staff.Data
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetEmployees();
        IEnumerable<Department> GetDepartments();
        IEnumerable<Gender> GetGenders();
        IEnumerable<ProgLang> GetProgLangs();
        Employee GetById(int Id);
        void Update(Employee employee);
        void Delete(Employee employee);
        void Create(Employee employee);
        string LastError { get; }
    }
}
