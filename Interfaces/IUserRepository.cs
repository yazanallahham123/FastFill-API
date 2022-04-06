
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

        public Task<bool> Insert(User user);

        public void Delete(User user);

        public bool Exists(int id);
        public bool ExistsMobileNumber(string mobileNumber);

        public Task<bool> Update(User user);

        public Task<List<User>> GetUsers();
        public Task<List<User>> GetAdmins();
        public Task<List<User>> GetSupports();

        public Task<bool> UpdateUserProfile(int userId, string mobileNumber, string name, string imageURL);

        public Task<bool> UpdateFirebaseToken(int userId, string firebaseToken);

        public Task<bool> AddNotification(Notification notification);

        public Task<bool> AddPaymentTransaction(PaymentTransaction paymentTransaction);

        public Task<List<Notification>> GetNotifications(int userId);

        public Task<List<PaymentTransaction>> GetPaymentTransactions(int userId);

        public Task<UserCredit> TopUpUserCredit(UserCredit userCredit);

        public Task<bool> AddBankCard(BankCard bankCard);

        public Task<List<BankCard>> GetBankCards(int userId);

        public Task<BankCard> GetBankCardById(int id);

        public Task<bool> DeleteBankCard(BankCard bankCard);
    }
}
