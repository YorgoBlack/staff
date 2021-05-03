using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Staff.Data;
using Staff.WebApp.Helpers;
using Staff.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Staff.WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        readonly IEmployeeService _employeeService;
        readonly IUserService _userService;
        public HomeController(IEmployeeService employeeService, IUserService userService)
        {
            _employeeService = employeeService;
            _userService = userService;
        }
        
        public IActionResult Index()
        {
            var model = new List<EmployeeModel>();
            _employeeService.GetEmployees().ToList().ForEach(x => model.Add(EmployeeConvert.ToModel(x))); 
            return View( model );
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Departments = _employeeService.GetDepartments();
            ViewBag.Genders = _employeeService.GetGenders();
            ViewBag.MultiSelectProgLangs = new MultiSelectList(_employeeService.GetProgLangs(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(EmployeeModel model)
        {
            WriteUserActionTime();
            if (ModelState.IsValid)
            {
                var entity = EmployeeConvert.ToEntity(model, _employeeService);
                _employeeService.Create( entity );
                if( _employeeService.LastError == string.Empty )
                {
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Genders = _employeeService.GetGenders();
            ViewBag.Departments = _employeeService.GetDepartments();
            ViewBag.MultiSelectProgLangs = new MultiSelectList(_employeeService.GetProgLangs(), "Id", "Name");
            return View(model);
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            var entity = _employeeService.GetEmployees().FirstOrDefault(x => x.Id == id);
            if (entity != null)
            {
                var model = EmployeeConvert.ToModel(entity);
                ViewBag.Departments = _employeeService.GetDepartments();
                ViewBag.Genders = _employeeService.GetGenders();
                ViewBag.MultiSelectProgLangs = new MultiSelectList(_employeeService.GetProgLangs(), "Id", "Name");
                return View(model);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(EmployeeModel model)
        {
            WriteUserActionTime();
            if (ModelState.IsValid)
            {
                var entity = EmployeeConvert.ToEntity(model, _employeeService);
                _employeeService.Update(entity);
                if (_employeeService.LastError == string.Empty)
                {
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Genders = _employeeService.GetGenders();
            ViewBag.Departments = _employeeService.GetDepartments();
            ViewBag.MultiSelectProgLangs = new MultiSelectList(_employeeService.GetProgLangs(), "Id", "Name");
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var entity = _employeeService.GetEmployees().FirstOrDefault(x => x.Id == id);
            if (entity != null)
            {
                var model = EmployeeConvert.ToModel(entity);
                model.IsDisabled = true;
                ViewBag.Departments = _employeeService.GetDepartments();
                ViewBag.Genders = _employeeService.GetGenders();
                ViewBag.MultiSelectProgLangs = new MultiSelectList(_employeeService.GetProgLangs(), "Id", "Name");
                return View(model);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(EmployeeModel model)
        {
            WriteUserActionTime();
            var entity = _employeeService.GetEmployees().FirstOrDefault(x => x.Id == model.Id);
            _employeeService.Delete(entity);
            return RedirectToAction("Index");
        }

        void WriteUserActionTime()
        {
            if (User.Identity.IsAuthenticated)
            {
                var u = _userService.GetUsers().FirstOrDefault(x => x.UserName == User.Identity.Name);
                if (u != null)
                {
                    u.LastActionTime = DateTime.Now;
                    _userService.Update(u);
                }
            }

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}
