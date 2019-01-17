using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AmoretuERPWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AmoretuERPWeb.Pages.Admin.Menu
{
    public class RolesModel : PageModel
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
            ProcessMenus(menus, 0);
        }

        public void OnPost()
        {
            var menus = DAL.Core.Menu.Menus;
            SortedMenus = new List<Menu>();
            ProcessMenus(menus, 0);
            var regex = new Regex(@"Role_(\d+)");
            var postJson = Request.Form["DXMVCEditorValues"].ToString();
            var json = JsonConvert.DeserializeObject<JObject>(postJson);
            RoleHelper.RoleMenus = new Dictionary<Role, Dictionary<int, string>>();
            foreach (var obj in json)
            {
                var match = regex.Match(obj.Key);
                if(!match.Success) continue;
                var role = (Role) int.Parse(match.Groups[1].Value);
                RoleHelper.RoleMenus[role] = new Dictionary<int, string>();
                foreach (var menuId in obj.Value)
                {
                    var id = int.Parse(menuId.ToString());
                    var url = DAL.Core.Menu.GetMenuUrlById(id);
                    RoleHelper.RoleMenus[role][id] = url;
                }
            }
            RoleHelper.SaveRoleMenus();
        }

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
    }
}