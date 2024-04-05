using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EF6_QueryTaker.Models.Proxies
{
    public class UserProxy
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsEngineer { get; set; }
        public bool IsUser { get; set; }
    }
}