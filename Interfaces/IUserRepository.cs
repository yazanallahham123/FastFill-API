
using FastFill_API.Web.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetById(int id, int? roleId);

        public Task<User> GetByMobileNumber(string mobileNumber, int? roleId);

        public Task<bool> Insert(User user);

        public void Delete(User user);

        public bool Exists(int id);
        public bool ExistsMobileNumber(string mobileNumber);

        public Task<bool> Update(User user);

        public Task<List<User>> GetCompanyUsers(int companyId);

        public Task<List<User>> GetCompanyAgentUsers(int companyId);

        public Task<UserRefillTransaction> GetSyberpayTransactionById(string transactionId);

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

        public Task<bool> UpdateBankCard(BankCard bankCard);

        public Task<List<BankCard>> GetBankCards(int userId);

        public Task<BankCard> GetBankCardById(int id);

        public Task<bool> DeleteBankCard(BankCard bankCard);

        public Task<bool> AddUserRefillTransaction(UserRefillTransaction userRefillTransaction);
        public Task<double> GetUserBalance(int userId);

        public Task<bool> CheckPaymentForBushrapay(string transactionId);
        public Task<bool> CheckPaymentForFaisal(string transactionId);

        public Task<bool> UpdateUserLanguage(int userId, int language);

        public Task<bool> LogError(int userId, string type, string message, string innerMessage, string code, string location);
        public Task<bool> ClearNotifications(int userId);

        public Task<bool> ClearTransactions(int userId);

        public Task<bool> UpdateUserRefillStatus(string transactionId, bool status);

        public Task<List<User>> GetCompaniesUsers();
        public Task<TempSetting> ShowSignupInStationApp();

        public Task<int> GetUsersCount();

        public Task<bool> RemoveAccount(int userId);
        public Task<bool> Logout(int userId);

        public Task<bool> AssignPumpsAgents(int companyId, List<PumpAgentDto> pumpsAgents);
        public Task<List<CompanyPump>> GetCompanyPumps(int companyId);
        public Task<List<CompanyAgentPump>> GetPumpsAgents(int companyId);
        public Task<List<CompanyPumpState>> GetActivePumps(int companyId);

        public Task<List<String>> GetAllFireBaseTokens();

        public Task<List<UserRefill>> GetRefills(string? mobileNumber, DateTime? fromDate, DateTime? toDate, bool? status, string? transactionId, List<int>? refillSources);

        public Task<List<PaymentTransaction>> GetPaymentTransactionResults(string? mobileNumber, DateTime? fromDate, DateTime? toDate, List<int>? companies);

        public Task<bool> GenerateOtp(Otp otp);

        public Task<bool> verifyOtp(string registerId, string otpCode);
        public Task<List<Company>> getAllCompanies();

    }
}
