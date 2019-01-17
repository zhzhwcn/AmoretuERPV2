using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AmoretuERPWeb.Models
{
    public enum Role
    {
        管理员 = 0,
        产品组长 = 1,
        质检专员 = 2,
    }

    public class RoleHelper
    {
        public static Dictionary<Role, Dictionary<int, string>> RoleMenus = new Dictionary<Role, Dictionary<int, string>>();

        public static void LoadRoleMenus()
        {
            RoleMenus = new Dictionary<Role, Dictionary<int, string>>();
            Directory.SetCurrentDirectory(Directory.GetCurrentDirectory());
            if (File.Exists("roleMenus.json"))
            {
                var json = File.ReadAllText("roleMenus.json");
                RoleMenus = JsonConvert.DeserializeObject<Dictionary<Role, Dictionary<int, string>>>(json);
            }
        }

        public static void SaveRoleMenus()
        {
            Directory.SetCurrentDirectory(Directory.GetCurrentDirectory());
            File.WriteAllText("roleMenus.json", JsonConvert.SerializeObject(RoleMenus));
        }

        public static List<string> GetRolesByUrl(string url)
        {
            return RoleMenus.Where(r => r.Value.Any(kv => !string.IsNullOrEmpty(kv.Value) && url.StartsWith(kv.Value))).Select(r => r.Key.ToString())
                .ToList();
        }

        public static List<string> GetRolesByMenuId(int id)
        {
            return RoleMenus.Where(r => r.Value.Any(kv => kv.Key == id)).Select(r => r.Key.ToString())
                .ToList();
        }

        public static Dictionary<int, string> RoleDirectory => Enum.GetNames(typeof(Role))
            .ToDictionary(x => (int) Enum.Parse(typeof(Role), x), x => x);
    }
}
