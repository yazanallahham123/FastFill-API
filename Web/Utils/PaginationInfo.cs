using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Utils
{
    public class PaginationInfo
    {
        public int PageSize { set; get; }
        public int PageNo { set; get; }
        public int TotalItems { set; get; }
        public int TotalPages { set; get; }
        public bool HasNext { set; get; }
        public bool HasPrevious { set; get; }

        public void SetValues(int pageSize, int page, int count)
        {

            this.PageNo = page;
            this.PageSize = pageSize;
            this.TotalItems = count;
            this.TotalPages = ((int)Math.Ceiling(((float)TotalItems / (float)pageSize)));
            this.HasNext = (page < TotalPages);
            this.HasPrevious = (page > 1) && (page <= TotalPages + 1);

        }
    }
}
