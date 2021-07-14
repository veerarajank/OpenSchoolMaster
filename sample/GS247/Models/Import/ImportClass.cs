using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GS247.Models.Import
{
    public class ImportClass
    {
        public string ModelName { get; set; }
        public string Action { get; set; }
        public List<string> modelHeadings { get; set; }
    }
    public class UserCreation
    {
        public String Email { get; set; }
        public bool IsSelected { get; set; }
        public String MobileNumber { get; set; }
        public String UserId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public int RoleId { get; set; }
    }


}