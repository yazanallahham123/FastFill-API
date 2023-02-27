using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class InsertCompanyPumpDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Code { get; set; }
    }
}
