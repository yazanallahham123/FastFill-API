using FastFill_API.Interfaces;

using FastFill_API.Web.Dto;
using FastFill_API.Web.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly FastFillDBContext _context;

        public UserRepository(FastFillDBContext context)
        {
            _context = context;
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByMobileNumber(string mobileNumber)
        {
            return await _context.Users.Where(u => u.MobileNumber == mobileNumber).Include(x => x.Company).FirstOrDefaultAsync();
        }

        public async Task<bool> Insert(User user)
        {
            if (user != null)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public void Delete(User user)
        {
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChangesAsync();
            }
        }

        public bool Exists(int id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        public bool ExistsMobileNumber(string mobilNumber)
        {
            return (_context.Users.Any(u => u.MobileNumber == mobilNumber));
        }

        public async Task<bool> Update(User user)
        {
            if (user != null)
            {
                _context.Attach(user);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.Where(u => u.RoleId == (int)RoleType.User).ToListAsync();
        }

        public async Task<List<User>> GetCompanyUsers(int companyId)
        {
            return await _context.Users.Where(u => u.RoleId == (int)RoleType.Company && u.CompanyId == companyId).ToListAsync();
        }
        public async Task<List<User>> GetAdmins()
        {
            return await _context.Users.Where(u => u.RoleId == (int)RoleType.Admin).ToListAsync();
        }

        public async Task<List<User>> GetSupports()
        {
            return await _context.Users.Where(u => u.RoleId == (int)RoleType.Support).ToListAsync();
        }

        public async Task<bool> UpdateUserProfile(int userId, string mobileNumber, string name, string imageURL)
        {

            User? u = await _context.Users.Where((u) => u.Id == userId).FirstOrDefaultAsync();

            if (u != null)
            {
                u.MobileNumber = mobileNumber;
                u.FirstName = name;
                u.LastName = name;
                u.Username = name;
                u.ImageURL = imageURL;

                return await Update(u);                
            }
            else
                return false;
        }

        public async Task<bool> UpdateFirebaseToken(int userId, string firebaseToken)
        {

            User? u = await _context.Users.Where((u) => u.Id == userId).FirstOrDefaultAsync();

            if (u != null)
            {
                u.FirebaseToken = firebaseToken;
                await Update(u);
                return true;
            }
            else
                return false;
        }

        public async Task<bool> AddNotification(Notification notification)
        {
            if (notification != null)
            {
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public async Task<bool> AddPaymentTransaction(PaymentTransaction paymentTransaction)
        {
            if (paymentTransaction != null)
            {
                _context.PaymentTransactions.Add(paymentTransaction);
                await _context.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }


        public async Task<List<Notification>> GetNotifications(int userId)
        {
            User user = await GetById(userId);
            if (user.RoleId == 4)
                return await _context.Notifications.Where((n) => n.CompanyId == user.CompanyId && ((n.Cleared??false) == false)).OrderByDescending(x => x.Id).ToListAsync();
            else
                return await _context.Notifications.Where((n) => n.UserId == userId && ((n.Cleared??false) == false)).OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<List<PaymentTransaction>> GetPaymentTransactions(int userId)
        {
            return await _context.PaymentTransactions.Where((n) => n.UserId == userId).Include(x => x.Company).OrderByDescending(x => x.Date).ToListAsync();
        }

        public async Task<UserCredit> TopUpUserCredit(UserCredit userCredit)
        {
            if (userCredit != null)
            {
                _context.UserCredits.Add(userCredit);
                await _context.SaveChangesAsync();
                return userCredit;
            }
            else
                return null;
        }


        public async Task<bool> AddBankCard(BankCard bankCard)
        {
            if (bankCard != null)
            {
                _context.BankCards.Add(bankCard);
                await _context.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public async Task<List<BankCard>> GetBankCards(int userId)
        {
            return await _context.BankCards.Where((n) => n.UserId == userId).ToListAsync();
        }

        public async Task<BankCard> GetBankCardById(int id)
        {
            return await _context.BankCards.FindAsync(id);
        }

        public async Task<bool> DeleteBankCard(BankCard bankCard)
        {
            try
            {
                _context.BankCards.Remove(bankCard);
                await _context.SaveChangesAsync();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public async Task<UserRefillTransaction> GetSyberpayTransactionById(string transactionId)
        {
            try
            {
                return await _context.UserRefillTransactions.Where(u => u.transactionId == transactionId).Include(x => x.User).FirstOrDefaultAsync();
            }
            catch
            {
                return null;
            }

        }

        public async Task<bool> AddUserRefillTransaction(UserRefillTransaction userRefillTransaction)
        {
            if (userRefillTransaction != null)
            {
                _context.UserRefillTransactions.Add(userRefillTransaction);
                await _context.SaveChangesAsync();
                return true;
            }
            else
                return false;

        }

        public async Task<double> GetUserBalance(int userId)
        {
            double wallet = await _context.UserRefillTransactions.Where(urt => urt.UserId == userId && urt.status == true).SumAsync((urt => urt.Amount));
            double transactions = await _context.PaymentTransactions.Where(pt => pt.UserId == userId && pt.Status == true).SumAsync((pt => pt.Amount));
            return wallet - transactions;
        }

        public async Task<bool> UpdateUserLanguage(int userId, int language)
        {
            User? u = await _context.Users.Where((u) => u.Id == userId).FirstOrDefaultAsync();

            if (u != null)
            {
                u.Language = language;
                await Update(u);
                return true;
            }
            else
                return false;
        }

        public async Task<bool> LogError(int userId, string type, string message, string innerMessage, string code, string location)
        {
            try
            {
                ErrorLog errorLog = new ErrorLog();
                errorLog.Date = DateTime.Now;
                errorLog.UserId = userId;
                errorLog.Type = type;
                errorLog.Message = message;
                errorLog.InnerMessage = innerMessage;
                errorLog.Location = location;

                _context.ErrorLogs.Add(errorLog);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> ClearNotifications(int userId)
        {
            try
            {
                var sql = $"UPDATE Notifications SET Cleared = 1 WHERE UserId = " + userId.ToString();

                await _context.Database.ExecuteSqlRawAsync(sql);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserRefillStatus(string transactionId, bool status)
        {
            UserRefillTransaction? urt = await _context.UserRefillTransactions.Where((u) => u.transactionId == transactionId).FirstOrDefaultAsync();

            if (urt != null)
            {
                urt.status = status;
                _context.Attach(urt);
                _context.UserRefillTransactions.Update(urt);
                await _context.SaveChangesAsync();

                return true;
            }
            else
                return false;
        }


    }
}
