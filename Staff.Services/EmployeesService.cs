using Staff.Data;
using Staff.Data.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Staff.Services
{
    public class EmployeeService : IEmployeeService
    {
        readonly AppDbContext _context;
        string _lastError;

        public string LastError => _lastError;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
            _lastError = string.Empty;
        }

        public IEnumerable<Gender> GetGenders()
        {
            return _context.Genders;
        }
        public IEnumerable<ProgLang> GetProgLangs()
        {
            return _context.ProgLangs;
        }

        public IEnumerable<Department> GetDepartments()
        {
            return _context.Departments;
        }
        public IEnumerable<Employee> GetEmployees()
        {
            return _context.Employees
                .Include(g => g.Gender)
                .Include(d => d.Department)
                .Include(e => e.Experiences).ThenInclude(p => p.ProgLang);
        }

        public void Create(Employee employee)
        {
            var new_Experiences = employee.Experiences?.Select(x => new Experience() { ProgLangId = x.ProgLangId }).ToList();
            employee.Experiences?.Clear();
            _context.Add(employee);
            _context.SaveChanges();
            if (new_Experiences != null)
            {
                new_Experiences.ForEach(x => x.EmployeeId = employee.Id);
                _context.Set<Experience>().AddRange(new_Experiences);
                _context.SaveChanges();
            }
        }
        public void Update(Employee employee)
        {
            var new_Experiences = employee.Experiences?
                .Select(x => new Experience() { EmployeeId = employee.Id, ProgLangId = x.ProgLangId }).ToList();
            var old_Experiences = _context.Set<Experience>().Where(x => x.EmployeeId == employee.Id).ToList();

            employee.Experiences?.Clear();
            _context.Update(employee);
            _context.SaveChanges();

            if ( old_Experiences != null && old_Experiences.Any() )
            {
                _context.Set<Experience>().RemoveRange( new_Experiences == null ? old_Experiences : old_Experiences.Except(new_Experiences));
            }
            if (new_Experiences != null)
            {
                _context.Set<Experience>().AddRange(new_Experiences.Except(old_Experiences));
            }
            _context.SaveChanges();
        }
        public void Delete(Employee employee)
        {
            var old_Experiences = _context.Set<Experience>().Where(x => x.EmployeeId == employee.Id).ToList();
            _context.Set<Experience>().RemoveRange(old_Experiences);
            _context.SoftSaveChanges();
            employee.Experiences?.Clear();
            _context.Remove(employee);
            _context.SoftSaveChanges();
        }

        public Employee GetById(int Id)
        {
            return _context.Employees.FirstOrDefault(x => x.Id == Id);
        }

    }
}
