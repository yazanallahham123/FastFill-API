using FastFill_API.Interfaces;
using FastFill_API.Migrations;
using FastFill_API.Web.Dto;
using FastFill_API.Web.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FastFillDBContext _context;

        public UserRepository(FastFillDBContext context)
        {
            _context = context;
        }

        public async Task<User> GetById(int id, int? roleId)
        {
            if (roleId != null)
            {
                if (roleId > 0)
                {
                    return await _context.Users.Where(x => x.Id == id && x.RoleId == roleId).FirstOrDefaultAsync();
                }
                else
                    return await _context.Users.FindAsync(id);
            }
            else
                return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByMobileNumber(string mobileNumber, int? roleId)
        {

            if (roleId != null)
            {
                if (roleId > 0)
                {
                    return await _context.Users.Where(u => u.MobileNumber == mobileNumber && u.RoleId == roleId).Include(x => x.Company).Include(x => x.Group).FirstOrDefaultAsync();
                }
                else
                    return await _context.Users.Where(u => u.MobileNumber == mobileNumber).Include(x => x.Company).Include(x => x.Group).FirstOrDefaultAsync();
            }
            else
                return await _context.Users.Where(u => u.MobileNumber == mobileNumber).Include(x => x.Company).Include(x => x.Group).FirstOrDefaultAsync();
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

        public async Task<List<User>> GetCompanyAgentUsers(int companyId)
        {
            return await _context.Users.Where(u => u.RoleId == (int)RoleType.CompanyAgent && u.CompanyId == companyId).ToListAsync();
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
                List<PaymentTransaction> dailyTransactions = await _context.PaymentTransactions.Where((n) => n.Date.Date == DateTime.Now.Date.Date && n.CompanyId == paymentTransaction.CompanyId).ToListAsync();
                int dailyId = dailyTransactions.Count + 1;
                paymentTransaction.DailyId = dailyId;
                _context.PaymentTransactions.Add(paymentTransaction);
                await _context.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }


        public async Task<List<Notification>> GetNotifications(int userId)
        {
            User user = await GetById(userId, 0);
            if (user.RoleId == 4)
                return await _context.Notifications.Where((n) => (n.UserId == userId || n.CompanyId == user.CompanyId) && ((n.Cleared ?? false) == false && n.Content.Trim() != "")).OrderByDescending(x => x.Id).ToListAsync();
            else
                return await _context.Notifications.Where((n) => n.UserId == userId && ((n.Cleared ?? false) == false) && n.Content.Trim() != "").OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<List<PaymentTransaction>> GetPaymentTransactions(int userId)
        {
            return await _context.PaymentTransactions.Where((n) => n.UserId == userId && ((n.Cleared ?? false) == false)).Include(x => x.Company).OrderByDescending(x => x.Date).ThenByDescending((xx) => xx.DailyId).ToListAsync();
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

        public async Task<bool> UpdateBankCard(BankCard bankCard)
        {
            if (bankCard != null)
            {
                _context.BankCards.Update(bankCard);
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

        public async Task<bool> ClearTransactions(int userId)
        {
            try
            {
                var sql = $"UPDATE PaymentTransactions SET Cleared = 1 WHERE UserId = " + userId.ToString();

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

        public async Task<List<User>> GetCompaniesUsers()
        {
            return await _context.Users.Include(c => c.Company).Where(u => u.RoleId == (int)RoleType.Company).ToListAsync();
        }

        public async Task<TempSetting> ShowSignupInStationApp()
        {
            return await _context.TempSettings.FirstOrDefaultAsync();
        }

        public async Task<int> GetUsersCount()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<bool> RemoveAccount(int userId)
        {
            try
            {
                User u = await GetById(userId, 0);
                _context.Users.Remove(u);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await LogError(0, "1", ex.Message, ex.InnerException?.Message ?? "", "", "RemoveAccount");
                return false;
            }
        }

        public async Task<bool> Logout(int userId)
        {
            try
            {
                User u = await GetById(userId, 0);
                u.FirebaseToken = "";
                _context.Users.Update(u);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AssignPumpsAgents(int companyId, List<PumpAgentDto> pumpsAgents)
        {
            try
            {
                List<CompanyAgentPump> ap = _context.CompanyAgentPumps.Where(d => d.Agent.CompanyId == companyId).ToList();
                _context.CompanyAgentPumps.RemoveRange(ap);
                await _context.SaveChangesAsync();
                List<CompanyAgentPump> new_ap = new List<CompanyAgentPump>();
                foreach (var item in pumpsAgents)
                {
                    CompanyAgentPump agp = new CompanyAgentPump();
                    agp.AgentId = item.UserId;
                    agp.PumpId = item.PumpId;
                    new_ap.Add(agp);
                }

                _context.CompanyAgentPumps.AddRange(new_ap);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<CompanyPump>> GetCompanyPumps(int companyId)
        {
            return await _context.CompanyPumps.Where(u => u.CompanyId == companyId).ToListAsync();
        }

        public async Task<List<CompanyAgentPump>> GetPumpsAgents(int companyId)
        {
            return await _context.CompanyAgentPumps.Where(u => u.Pump.CompanyId == companyId).ToListAsync();
        }

        public async Task<List<CompanyPumpState>> GetActivePumps(int companyId)
        {
            return await _context.CompanyPumpsState.Where(u => u.CompanyAgentPump.Pump.CompanyId == companyId && u.IsOpen == true).
                Include(d => d.CompanyAgentPump).
                Include(d => d.CompanyAgentPump.Pump).
                ToListAsync();
        }

        public async Task<bool> InsertCompanyPump(CompanyPump companyPump)
        {
            try
            {
                _context.CompanyPumps.Add(companyPump);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateCompanyPump(CompanyPump companyPump)
        {
            try
            {
                _context.CompanyPumps.Update(companyPump);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCompanyPump(CompanyPump companyPump)
        {
            try
            {
                _context.CompanyPumps.Remove(companyPump);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<List<String>> GetAllFireBaseTokens()
        {
            return _context.Users.Where(d => d.FirebaseToken != null && d.FirebaseToken != "" && d.RoleId == (int)RoleType.User).Select(d => d.FirebaseToken).ToList();
        }


        public async Task<bool> CheckPaymentForBushrapay(string transactionId)
        {
            return await _context.UserRefillTransactions.AnyAsync((x) => x.transactionId == transactionId && x.RefillSourceId == (int)RefillSource.Bushrapay);
        }

        public async Task<bool> CheckPaymentForFaisal(string transactionId)
        {
            return await _context.UserRefillTransactions.AnyAsync((x) => x.transactionId == transactionId && x.RefillSourceId == (int)RefillSource.Faisal);
        }

        public async Task<List<UserRefill>> GetRefills(string? mobileNumber, DateTime? fromDate, DateTime? toDate, bool? status, string? transactionId,
            List<int>? refillSources)
        {
            List<UserRefill> userRefills = new List<UserRefill>();

            List<UserRefillTransaction> refills = _context.UserRefillTransactions.Include(u => u.User).Where(d =>
            ((mobileNumber != null) ? (d.User.MobileNumber == mobileNumber) : (d.User.MobileNumber == d.User.MobileNumber)) &&
            ((fromDate != null) ? (d.Date.Date >= Convert.ToDateTime(fromDate).Date) : (d.Date == d.Date)) &&
            ((toDate != null) ? (d.Date.Date <= Convert.ToDateTime(toDate).Date) : (d.Date == d.Date)) &&
            ((status != null) ? (d.status == status) : (d.status == d.status)) &&
            ((transactionId != null) ? (d.transactionId == transactionId) : (d.transactionId == d.transactionId))
             &&
            ((refillSources.Count == 0) || ((refillSources != null) ? (refillSources.Contains(d.RefillSourceId ?? -1)) : (d.RefillSourceId == d.RefillSourceId)))

            ).ToList();

            refills.ForEach((x) =>
            {
                UserRefill userRefill = new UserRefill();
                userRefill.Id = x.Id;
                userRefill.mobileNumber = x.User.MobileNumber;
                userRefill.userId = x.UserId;
                userRefill.date = x.Date;
                userRefill.transactionId = x.transactionId;
                userRefill.amount = x.Amount;
                userRefill.status = x.status;
                userRefill.sourceId = x.RefillSourceId;
                userRefill.source = (x.RefillSourceId == (int)RefillSource.Manual) ? "Manual" :
                (x.RefillSourceId == (int)RefillSource.Sybertech) ? "Sybertech Gateway" :
                (x.RefillSourceId == (int)RefillSource.Bushrapay) ? "Bushrapay" :
                (x.RefillSourceId == (int)RefillSource.BOK) ? "BOK" :
                (x.RefillSourceId == (int)RefillSource.Faisal) ? "Faisal Bank" :
                (x.RefillSourceId == (int)RefillSource.SybertechApp) ? "Sybertech App" : "Unknown";

                userRefills.Add(userRefill);
            });

            return userRefills;
        }

        public async Task<List<PaymentTransaction>> GetPaymentTransactionResults(string? mobileNumber, DateTime? fromDate, DateTime? toDate,
            List<int>? companies)
        {

            List<PaymentTransaction> paymentTransactionResults = _context.PaymentTransactions.Include(u => u.User).Include(c => c.Company).Where(d =>
            ((mobileNumber != null) ? (d.User.MobileNumber == mobileNumber) : (d.User.MobileNumber == d.User.MobileNumber)) &&
            ((fromDate != null) ? (d.Date.Date >= Convert.ToDateTime(fromDate).Date) : (d.Date == d.Date)) &&
            ((toDate != null) ? (d.Date.Date <= Convert.ToDateTime(toDate).Date) : (d.Date == d.Date))
             &&
            ((companies.Count == 0) || ((companies != null) ? (companies.Contains(d.CompanyId)) : (d.CompanyId == d.CompanyId)))

            ).ToList();

            return paymentTransactionResults;
        }


        public async Task<bool> GenerateOtp(Otp otp)
        {
            if (otp != null)
            {
                _context.Otps.Add(otp);
                await _context.SaveChangesAsync();
                return true;
            }
            else
                return false;

        }

        public async Task<bool> verifyOtp(string registerId, string otpCode)
        {
            try
            {
                bool res = await _context.Otps.AnyAsync((x) => x.otpCode == otpCode && x.registerId == registerId);
                if (res)
                {

                    Otp otp = await _context.Otps.Where(x => x.registerId == registerId && x.otpCode == otpCode).FirstOrDefaultAsync();
                    otp.status = true;

                    _context.Attach(otp);
                    _context.Otps.Update(otp);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public async Task<List<Company>> getAllCompanies()
        {
            try
            {
                List<Company> companies = await _context.Companies.ToListAsync();
                return companies;
            }
            catch (Exception e)
            {
                List<Company> companies = new List<Company>();
                return companies;
            }

        }
    }
}
