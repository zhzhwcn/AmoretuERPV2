using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AmoretuERPWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AmoretuERPWeb.Pages.Admin.User
{
    public class EditModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [ViewData]
        public MessageViewModel Message { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Id { get; set; }
            [Required]
            public string UserName { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public IList<string> Roles { get; set; } = new List<string>();
        }

        public async Task<IActionResult> OnGet(string id = "")
        {
            var user = await _userManager.FindByIdAsync(id);
            Input = new InputModel();
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                Input.Id = user.Id;
                Input.UserName = user.UserName;
                Input.Email = user.Email;
                Input.Roles = roles;
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(string id = "")
        {
            var user = await _userManager.FindByIdAsync(Input.Id);
            Message = new MessageViewModel()
            {
                CssClassName = "alert-success",
                Title = "提示",
                Message = "成功"
            };
            var errors = new List<string>();
            try
            {
                foreach (var role in Input.Roles)
                {
                    if (!(await _roleManager.RoleExistsAsync(role)))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
                if (user == null)
                {
                    user = new IdentityUser(Input.UserName);
                    user.Email = Input.Email;
                    var password = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12);

                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        await _userManager.RemovePasswordAsync(user);
                        await _userManager.AddPasswordAsync(user, password);
                        Message.Message = "添加成功，密码为:" + password;
                        
                        await _userManager.AddToRolesAsync(user, Input.Roles);
                    }
                    else
                    {
                        Message.CssClassName = "alert-error";
                        Message.Message = "添加失败：" + result.ToString();
                    }
                }
                else
                {
                    IdentityResult result;
                    if (user.UserName != Input.UserName)
                    {
                        result = await _userManager.SetUserNameAsync(user, Input.UserName);
                        if (!result.Succeeded)
                        {
                            errors.AddRange(result.Errors.Select(e => e.Description));
                        }
                    }

                    if (user.Email != Input.Email)
                    {
                        result = await _userManager.SetEmailAsync(user, Input.Email);
                        if (!result.Succeeded)
                        {
                            errors.AddRange(result.Errors.Select(e => e.Description));
                        }
                    }
                    if (!string.IsNullOrEmpty(Input.Password))
                    {
                        result = await _userManager.RemovePasswordAsync(user);
                        if (!result.Succeeded)
                        {
                            errors.AddRange(result.Errors.Select(e => e.Description));
                        }
                        result = await _userManager.AddPasswordAsync(user, Input.Password);
                        if (!result.Succeeded)
                        {
                            errors.AddRange(result.Errors.Select(e => e.Description));
                        }
                    }

                    var oldRoles = await _userManager.GetRolesAsync(user);
                    result = await _userManager.RemoveFromRolesAsync(user, oldRoles);
                    if (!result.Succeeded)
                    {
                        errors.AddRange(result.Errors.Select(e => e.Description));
                    }

                    result = await _userManager.AddToRolesAsync(user, Input.Roles);
                    if (!result.Succeeded)
                    {
                        errors.AddRange(result.Errors.Select(e => e.Description));
                    }
                }
            }
            catch (Exception e)
            {
                Message.CssClassName = "alert-error";
                Message.Message = "操作失败：" + e.Message;
            }

            if (errors.Count > 0)
            {
                Message.CssClassName = "alert-error";
                Message.Message = "操作失败：<br>" + string.Join("<br>", errors);
            }
            
            return Page();
        }
    }
}