using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Models
{
    public class PageViewModel
    {
        public int PageNumber { get; set; }
        public int TotalPage { get; set; }

        public PageViewModel()
        {

        }
        public PageViewModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPage = (int)Math.Ceiling(count / (decimal)pageSize);
        }

        public bool HasPreviousPage { get { return (PageNumber > 1); } }
        public bool HasNextPage { get { return (PageNumber < TotalPage); } }
    }
}
