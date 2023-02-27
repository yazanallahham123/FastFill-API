using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class EditBankCardDto
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
    }
}
