using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AmoretuERPWeb.Models;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AmoretuERPWeb.Pages.Admin.Menu
{
    public class EditModel : PageModel
    {
        private ApplicationDbContext _db;
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [ViewData]
        public MessageViewModel Message { get; set; }

        public class InputModel
        {
            public int Id { get; set; }
            [Required]
            [Display(Name = "名称")]
            public string Name { get; set; }
            public string Action { get; set; }
            public string Controller { get; set; }
            public string Url { get; set; }
            [Display(Name = "上级菜单")]
            public int ParentId { get; set; }
            [Display(Name = "隐藏")]
            public bool Hide { get; set; } = false;
            [Display(Name = "显示顺序")]
            public int Order { get; set; }
        }

        public void OnGet(int id = 0)
        {
            var menu = _db.Menus.FirstOrDefault(m => m.Id == id);
            Input = new InputModel();
            if (menu != null)
            {
                Input.Id = menu.Id;
                Input.Name = menu.Name;
                Input.Action = menu.Action;
                Input.Controller = menu.Controller;
                Input.Url = menu.Url;
                Input.ParentId = menu.ParentId;
                Input.Hide = menu.Hide;
                Input.Order = menu.Order;
            }
        }

        public IActionResult OnGetDelete(int id = 0)
        {
            return RedirectToPage("List");
        }

        public IActionResult OnPost(int id = 0)
        {
            var menu = _db.Menus.FirstOrDefault(m => m.Id == Input.Id);
            if (menu == null)
            {
                menu = new Model.Core.Menu();
                _db.Menus.Add(menu);
            }
            menu.Name = Input.Name;
            menu.Action = Input.Action;
            menu.Controller = Input.Controller;
            menu.Url = Input.Url;
            if (string.IsNullOrEmpty(menu.Url) && !string.IsNullOrEmpty(menu.Controller) &&
                !string.IsNullOrEmpty(menu.Action))
            {
                menu.Url = Url.Action(menu.Action, menu.Controller);
            }
            menu.ParentId = Input.ParentId;
            menu.Hide = Input.Hide;
            menu.Order = Input.Order;
            _db.SaveChanges();
            Message = new MessageViewModel()
            {
                CssClassName = "alert-success",
                Title = "提示",
                Message = "成功"
            };
            DAL.Core.Menu.ClearCache();
            return Page();
        }
    }
}