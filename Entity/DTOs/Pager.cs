using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class Pager
    {
        public int TotalItem { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        public Pager() { }
        public Pager(int totalItem, int page, int pageSize = 3)
        {
            int totalpage = (int)Math.Ceiling((decimal)totalItem / (decimal)pageSize);
            int currentPage = page;
            int startPage = currentPage - 3;
            int endPage = currentPage + 2;
            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalpage)
            {
                endPage = totalpage;
                if (endPage > 6)
                {
                    startPage = endPage - 5;
                }
            }
            TotalItem = totalItem;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPage = totalpage;
            StartPage = startPage;
            EndPage = endPage;
        }
    }
}
