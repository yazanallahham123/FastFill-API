using FastFill_API.Model;
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
        }
    }
}
