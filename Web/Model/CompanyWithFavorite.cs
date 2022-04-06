using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Model
{
    public class CompanyWithFavorite: Company 
    {
        public bool IsFavorite { get; set; }
    }
}
