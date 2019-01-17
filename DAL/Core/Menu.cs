using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Core
{
    public class Menu
    {
        public static List<Model.Core.Menu> Menus
        {
            get
            {
                if (_menus == null || (DateTime.Now - CacheTime).TotalDays > 1)
                {
                    using (var db = new ApplicationDbContext())
                    {
                        var dbMenus = db.Menus.ToList();
                        _menusRaw = dbMenus;
                        _menus = dbMenus.Where(m => m.ParentId == 0).ToList();
                        foreach (var menu in dbMenus)
                        {
                            if (menu.ParentId != 0)
                            {
                                var pMenu = dbMenus.FirstOrDefault(m => m.Id == menu.ParentId);
                                pMenu?.SubMenus.Add(menu);
                            }
                        }
                    }
                    CacheTime = DateTime.Now;
                }

                return _menus;
            }
        }

        public static void ClearCache()
        {
            _menus = null;
            _menusRaw = null;
            CacheTime = DateTime.MinValue;
        }
        public static DateTime CacheTime { get; set; }
        private static List<Model.Core.Menu> _menus = new List<Model.Core.Menu>();
        private static List<Model.Core.Menu> _menusRaw = new List<Model.Core.Menu>();

        public static string GetMenuUrlById(int id)
        {
            return _menusRaw.FirstOrDefault(m => m.Id == id)?.Url;
        }
    }
}
