using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalonCRM.Models
{
    public class BookReportModel
    {
        public ICollection<BookModel> BookList { get; set; }
    }
}