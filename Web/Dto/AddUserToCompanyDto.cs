﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class AddUserToCompanyDto
    {
        public int userId { get; set; }
        public int companyId { get; set; }
    }
}
