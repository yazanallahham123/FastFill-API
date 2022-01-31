using FastFill_API.Model;
using FastFill_API.Web.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetById(int id);

        public Task<User> GetByMobileNumber(string mobileNumber);

        public void Insert(User user);

        public void Delete(User user);

        public bool Exists(int id);
        public bool ExistsMobileNumber(string mobileNumber);

        public void Update(User user);

        public Task<List<User>> GetUsers();
        public Task<List<User>> GetAdmins();
        public Task<List<User>> GetSupports();

    }
}
