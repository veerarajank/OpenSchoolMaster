using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GS247.Models.Export
{
    public class ExportClass
    {
       public string Model { get; set; }
        public string Course { get; set; }
        public string Batch { get; set; }
        public List<string> Fields { get; set; }
        public List<string> SelectedFields { get; set; }
    }
       
}