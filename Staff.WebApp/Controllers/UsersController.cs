using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Staff.Data;
using Staff.Data.Models;
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
    public class UsersController : Controller
    {
        readonly IUserService _userService;
        
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        
        public IActionResult Index()
        {
            return RedirectToAction("Add");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new AppUsersModel()
            {
                Users = _userService
                .GetUsers()
                .Select(x => new AppUser() { Id = x.Id, LastActionTime = x.LastActionTime, UserName = x.UserName })
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AppUsersModel model)
        {
            if (ModelState.IsValid)
            {
                var u = _userService.GetUsers().FirstOrDefault(x => x.UserName == model.UserName);
                if (u != null)
                {
                    ModelState.AddModelError("", $"{model.UserName} already exists");
                    return View(new AppUsersModel()
                    {
                        Users = _userService
                        .GetUsers()
                        .Select(x => new AppUser() { Id = x.Id, LastActionTime = x.LastActionTime, UserName = x.UserName })
                    });

                }
                else
                {
                    _userService.Create(new AppUser() { UserName = model.UserName, Password = model.Password });
                    return RedirectToAction("Add", "Users");
                }
            }
            else
            {
                model.Users = _userService.GetUsers().Select(x => new AppUser() { Id = x.Id, LastActionTime = x.LastActionTime, UserName = x.UserName }); 
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var u = _userService.GetUsers().FirstOrDefault(x => x.Id == id);
            if( u != null )
            {
                _userService.Delete(u);
            }
            return RedirectToAction("Add", "Users");
        }
    }
}
