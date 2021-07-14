using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GS247.Models.Library
{
    public class ViewBorrowedBooks
    {
        public int Id { get; set; }
        public Nullable<int> BookId { get; set; }
        public Nullable<int> SubjectId { get; set; }
        public string StudentName { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string Edition { get; set; }
        public string Publisher { get; set; }
        public string Copy { get; set; }
        public string CopiesAvailable { get; set; }
    }
}