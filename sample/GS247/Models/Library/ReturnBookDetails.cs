using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GS247.Models.Library
{
    public class ReturnBookDetails
    {
        public int Id { get; set; }
        public Nullable<int> BookId { get; set; }
        public string StudentName { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string IssueDate { get; set; }
        public string DueDate { get; set; }
        public Nullable<int> isReturned { get; set; }
    }
}