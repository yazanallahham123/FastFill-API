
using FastFill_API.Web.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Profile
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            //User Mapper
            CreateMap<User, ChangePasswordDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserCredit, TopUpCreditDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();
            CreateMap<CompanyBranch, UpdateCompanyBranchDto>().ReverseMap();
            CreateMap<Company, UpdateCompanyDto>().ReverseMap();
            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<PaymentTransaction, PaymentTransactionDto>().ReverseMap();
            CreateMap<BankCard, AddEditBankCardDto>().ReverseMap();
        }
    }
}
