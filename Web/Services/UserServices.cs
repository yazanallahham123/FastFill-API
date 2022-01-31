using FastFill_API.Interfaces;
using FastFill_API.Model;
using FastFill_API.Web.Dto;
using FastFill_API.Web.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Services
{
    public class UserServices
    {

        private readonly IPublicRepository _repository;
        private readonly SecurityServices _securityServices;

        public UserServices(IPublicRepository repository, SecurityServices securityServices)
        {
            _repository = repository;
            _securityServices = securityServices;
        }

        //Add new user
        public bool AddUser(User user, RoleType role)
        {
            try
            {
                var hashSalt = _securityServices.EncryptPassword(user.Password);
                user.Password = hashSalt.Hash;
                user.StoredSalt = hashSalt.Salt;
                user.RoleId = (int)role;
                _repository.GetUserRepository.Insert(user);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }


        public bool ExistsMobileNumber(string mobileNumber)
        {
            return _repository.GetUserRepository.ExistsMobileNumber(mobileNumber);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _repository.GetUserRepository.GetById(id);

        }

        public async Task<IEnumerable<User>> GetUsers(int page, int pageSize, PaginationInfo paginationInfo)
        {
            IEnumerable<User> users = await _repository.GetUserRepository.GetUsers();
            paginationInfo.SetValues(pageSize, page, users.Count());
            return users.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<User>> GetAdmins(int page, int pageSize, PaginationInfo paginationInfo)
        {
            IEnumerable<User> users = await _repository.GetUserRepository.GetAdmins();
            paginationInfo.SetValues(pageSize, page, users.Count());
            return users.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<User>> GetSupports(int page, int pageSize, PaginationInfo paginationInfo)
        {
            IEnumerable<User> users = await _repository.GetUserRepository.GetSupports();
            paginationInfo.SetValues(pageSize, page, users.Count());
            return users.Skip((page - 1) * pageSize).Take(pageSize);
        }


        public bool Update(User user)
        {
            try
            {
                _repository.GetUserRepository.Update(user);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public bool Exists(int id)
        {
            return _repository.GetUserRepository.Exists(id);
        }

        public bool DeleteUser(User user)
        {
            try
            {
                _repository.GetUserRepository.Delete(user);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<User> GetByMobileNumber(string mobileNumber)
        {
            return await _repository.GetUserRepository.GetByMobileNumber(mobileNumber);
        }

        public bool ChangePassword(User oldUser, string newPassword)
        {
            try
            {
                var hashSalt = _securityServices.EncryptPassword(newPassword);
                oldUser.Password = hashSalt.Hash;
                oldUser.StoredSalt = hashSalt.Salt;
                _repository.GetUserRepository.Update(oldUser);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

    }
}
