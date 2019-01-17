using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmoretuERPWeb.Models.Admin
{
    public class MenuEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Url { get; set; }
        public int ParentId { get; set; }
        public bool Hide { get; set; } = true;
        public int Order { get; set; }
    }
}
