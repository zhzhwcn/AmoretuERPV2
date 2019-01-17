using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AmoretuERPWeb.Pages.Admin.User
{
    public class ListModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ListModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public class GridItem
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }

        public IList<GridItem> Items { get; set; }

        public void OnGet()
        {
            var users = _userManager.Users.ToList();
            Items = users.Select(u => new GridItem()
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            }).ToList();
        }
    }
}