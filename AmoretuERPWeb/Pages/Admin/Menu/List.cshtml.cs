using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AmoretuERPWeb.Pages.Admin.Menu
{
    public class ListModel : PageModel
    {
        public class Menu
        {
            public int Id { get; set; }
            [Display(Name = "名称")]
            public string Name { get; set; }
            public string Action { get; set; }
            public string Controller { get; set; }
            public string Url { get; set; }
            [Display(Name = "隐藏")]
            public bool Hide { get; set; } = false;
            [Display(Name = "显示顺序")]
            public int Order { get; set; }
        }

        [BindProperty]
        public List<Menu> SortedMenus { get; set; } = new List<Menu>();

        public void OnGet()
        {
            var menus = DAL.Core.Menu.Menus;
            SortedMenus = new List<Menu>();

            void ProcessMenus(List<Model.Core.Menu> list, int i)
            {
                foreach (var menu in list)
                {
                    SortedMenus.Add(new Menu()
                    {
                        Id = menu.Id,
                        Name = new string('>', i) + menu.Name,
                        Action = menu.Action,
                        Controller = menu.Controller,
                        Url = menu.Url,
                        Hide = menu.Hide,
                        Order = menu.Order
                    });
                    if (menu.SubMenus.Count > 0)
                    {
                        ProcessMenus(menu.SubMenus, i + 1);
                    }
                }
            }
            ProcessMenus(menus, 0);
        }
    }
}