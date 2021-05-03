using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staff.WebApp.Controllers
{
    [Authorize]
    public class NamesController : Controller
    {

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult OnFirstNameAutoComplete(string prefix)
        {
            return new JsonResult( FirstNames.Where(x => x.StartsWith(prefix)) );
        }

        readonly string[] FirstNames = { 
            "Андрей", 
            "Антон",
            "Сергей",
            "Олег",
            "Петр",
            "Елена",
            "Мария",
        };

    }
}
