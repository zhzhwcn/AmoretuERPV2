using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmoretuERPWeb.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace AmoretuERPWeb.Controllers.Admin
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(int id = 0)
        {
            var viewModel = new MenuEditViewModel();
            return View(viewModel);
        }
    }
}