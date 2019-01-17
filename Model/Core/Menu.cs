using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Core
{
    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Url { get; set; }
        public int ParentId { get; set; }
        public bool Hide { get; set; } = true;
        public int Order { get; set; }

        public List<Menu> SubMenus { get; set; } = new List<Menu>();
    }
}
