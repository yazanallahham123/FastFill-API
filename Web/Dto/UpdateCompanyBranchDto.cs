using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class UpdateCompanyBranchDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string Code { get; set; }
        public string ArabicAddress { get; set; }
        public string EnglishAddress { get; set; }

    }
}
