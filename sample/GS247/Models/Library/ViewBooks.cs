using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GS247.Models.Library
{
    public class ViewBooks
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public Nullable<int> SubjectId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> AuthorId { get; set; }
        public string Edition { get; set; }
        public string Publisher { get; set; }
        public string Copy { get; set; }
        public string BookPosition { get; set; }
        public string ShelfNo { get; set; }
        public string CopiesAvailable { get; set; }
        public string AuthorName { get; set; }
        public string SubjectName { get; set; }
    }
}