using FastFill_API.Interfaces;

using FastFill_API.Web.Dto;
using FastFill_API.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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
        public async Task<bool> AddUser(User user, RoleType role)
        {
            try
            {
                var hashSalt = _securityServices.EncryptPassword(user.Password);
                user.Password = hashSalt.Hash;
                user.StoredSalt = hashSalt.Salt;
                user.RoleId = (int)role;
                return await _repository.GetUserRepository.Insert(user);
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

        public async Task<bool> UpdateUserProfile(int userId, string mobileNumber, string name, string imageURL) {

            try
            {
                return await _repository.GetUserRepository.UpdateUserProfile(userId, mobileNumber, name, imageURL);                
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            
        }

        public async Task<bool> UpdateFirebaseToken(int userId, string firebaseToken)
        {

            try
            {
                return await _repository.GetUserRepository.UpdateFirebaseToken(userId, firebaseToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }
        public async Task<bool> Update(User user)
        {
            try
            {
                var hashSalt = _securityServices.EncryptPassword(user.Password);
                user.Password = hashSalt.Hash;
                user.StoredSalt = hashSalt.Salt;
                return await _repository.GetUserRepository.Update(user);
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

        public async Task<bool> AddNotification(Notification notification) {
            try
            {
                return await _repository.GetUserRepository.AddNotification(notification);
                
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> AddPaymentTransaction(PaymentTransaction paymentTransaction)
        {
            try
            {
                return await _repository.GetUserRepository.AddPaymentTransaction(paymentTransaction);

            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Notification>> GetNotifications(int page, int pageSize, PaginationInfo paginationInfo, int userId)
        {
            IEnumerable<Notification> notifications = await _repository.GetUserRepository.GetNotifications(userId);
            paginationInfo.SetValues(pageSize, page, notifications.Count());
            return notifications.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<PaymentTransaction>> GetPaymentTransactions(int page, int pageSize, PaginationInfo paginationInfo, int userId)
        {
            IEnumerable<PaymentTransaction> paymentTransactions = await _repository.GetUserRepository.GetPaymentTransactions(userId);
            paginationInfo.SetValues(pageSize, page, paymentTransactions.Count());
            return paymentTransactions.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<string> Upload(IFormFile file)
        {
            var ext = System.IO.Path.GetExtension(file.FileName);
            var fileName = "attachment_" + System.Guid.NewGuid().ToString() + ext;

            var path = Directory.GetCurrentDirectory() + @"\attachments\" + fileName;

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                await file.CopyToAsync(stream);
            }

            string fileURL = "http://fastfill.developitech.com/attachments/" + fileName;

            return fileURL;
        }

        public async Task<UserCredit> TopUpUserCredit(UserCredit userCredit)
        {
            try
            {
                return await _repository.GetUserRepository.TopUpUserCredit(userCredit);

            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }
        }

        public async Task<bool> AddBankCard(BankCard bankCard)
        {
            try
            {
                return await _repository.GetUserRepository.AddBankCard(bankCard);

            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<IEnumerable<BankCard>> GetBankCards(int page, int pageSize, PaginationInfo paginationInfo, int userId)
        {
            IEnumerable<BankCard> bankCards = await _repository.GetUserRepository.GetBankCards(userId);
            paginationInfo.SetValues(pageSize, page, bankCards.Count());
            return bankCards.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<bool> DeleteBankCard(BankCard bankCard)
        {
            try
            {
                return await _repository.GetUserRepository.DeleteBankCard(bankCard);

            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<BankCard> GetBankCardById(int id)
        {
            return await _repository.GetUserRepository.GetBankCardById(id);
        }

    }

}
