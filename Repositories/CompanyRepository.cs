using AutoMapper;
using FastFill_API.Interfaces;
using FastFill_API.Web.Model;
using FastFill_API.Web.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly FastFillDBContext _context;
       
        public CompanyRepository(FastFillDBContext context)
        {
            _context = context;
        }
        public async Task<List<CompanyWithFavorite>> GetAllCompanies(int userId)
        {
            List<Company> companies = await _context.Companies.Include(x => x.CompanyBranches).ToListAsync();

            List<FavoriteCompany> favoriteCompanies = await _context.FavoriteCompanies.ToListAsync();

            List<CompanyWithFavorite> companiesWithFavorite = new List<CompanyWithFavorite>();

            companies.ForEach((c) => {
                var s = JsonConvert.SerializeObject(c);
                CompanyWithFavorite cwf = JsonConvert.DeserializeObject<CompanyWithFavorite>(s);
                if (favoriteCompanies.Exists((fc) => fc.CompanyId == c.Id && fc.UserId == userId))
                    cwf.IsFavorite = true;
                else
                    cwf.IsFavorite = false;
                companiesWithFavorite.Add(cwf);
            });

            return companiesWithFavorite;
        }

        public async Task<CompanyWithFavorite> GetCompanyByCode(int userId, string code)
        {
            Company company = await _context.Companies.Where(c => c.Code == code).Include(x => x.CompanyBranches).FirstAsync();

            List<FavoriteCompany> favoriteCompanies = await _context.FavoriteCompanies.ToListAsync();

            var s = JsonConvert.SerializeObject(company);
            CompanyWithFavorite cwf = JsonConvert.DeserializeObject<CompanyWithFavorite>(s);
            if (favoriteCompanies.Exists((fc) => fc.CompanyId == company.Id && fc.UserId == userId))
                cwf.IsFavorite = true;
            else
                cwf.IsFavorite = false;

            return cwf;

        }

        public async Task<List<CompanyWithFavorite>> SearchCompaniesByName(int userId, string name)
        {
            List<Company> companies = await _context.Companies.Where(c => c.ArabicName.StartsWith(name) || c.EnglishName.StartsWith(name)).Include(x => x.CompanyBranches).ToListAsync();

            List<FavoriteCompany> favoriteCompanies = await _context.FavoriteCompanies.ToListAsync();

            List<CompanyWithFavorite> companiesWithFavorite = new List<CompanyWithFavorite>();

            companies.ForEach((c) => {
                var s = JsonConvert.SerializeObject(c);
                CompanyWithFavorite cwf = JsonConvert.DeserializeObject<CompanyWithFavorite>(s);
                if (favoriteCompanies.Exists((fc) => fc.CompanyId == c.Id && fc.UserId == userId))
                    cwf.IsFavorite = true;
                else
                    cwf.IsFavorite = false;
                companiesWithFavorite.Add(cwf);
            });

            return companiesWithFavorite;

        }


        public async Task<List<CompanyBranchWithFavorite>> SearchCompaniesBranchesByName(int userId, string name)
        {
            List<CompanyBranch> companiesBranches = await _context.CompanyBranches.Where(c => c.ArabicName.StartsWith(name) || c.EnglishName.StartsWith(name)).Include(x => x.Company).ToListAsync();

            List<FavoriteCompanyBranch> favoriteCompaniesBranches = await _context.FavoriteCompaniesBranches.ToListAsync();

            List<CompanyBranchWithFavorite> companiesBranchesWithFavorite = new List<CompanyBranchWithFavorite>();

            companiesBranches.ForEach((c) => {
                var s = JsonConvert.SerializeObject(c);
                CompanyBranchWithFavorite cwf = JsonConvert.DeserializeObject<CompanyBranchWithFavorite>(s);
                if (favoriteCompaniesBranches.Exists((fc) => fc.CompanyBranchId == c.Id && fc.UserId == userId))
                    cwf.IsFavorite = true;
                else
                    cwf.IsFavorite = false;
                companiesBranchesWithFavorite.Add(cwf);
            });

            return companiesBranchesWithFavorite;

        }

        public async Task<List<CompanyBranchWithFavorite>> SearchCompaniesBranchesByText(int userId, string text)
        {
            List<CompanyBranch> companiesBranches = await _context.CompanyBranches.Where(c => c.ArabicName.StartsWith(text) || c.EnglishName.StartsWith(text) || c.Code == text).Include(x => x.Company).ToListAsync();

            List<FavoriteCompanyBranch> favoriteCompaniesBranches = await _context.FavoriteCompaniesBranches.ToListAsync();

            List<CompanyBranchWithFavorite> companiesBranchesWithFavorite = new List<CompanyBranchWithFavorite>();

            companiesBranches.ForEach((c) => {
                var s = JsonConvert.SerializeObject(c);
                CompanyBranchWithFavorite cwf = JsonConvert.DeserializeObject<CompanyBranchWithFavorite>(s);
                if (favoriteCompaniesBranches.Exists((fc) => fc.CompanyBranchId == c.Id && fc.UserId == userId))
                    cwf.IsFavorite = true;
                else
                    cwf.IsFavorite = false;
                companiesBranchesWithFavorite.Add(cwf);
            });

            return companiesBranchesWithFavorite;

        }

        public async Task<List<CompanyWithFavorite>> SearchCompaniesByText(int userId, string text)
        {
            List<Company> companies = await _context.Companies.Where(c => c.ArabicName.StartsWith(text) || c.EnglishName.StartsWith(text) || c.Code == text).ToListAsync();

            List<FavoriteCompany> favoriteCompanies = await _context.FavoriteCompanies.ToListAsync();

            List<CompanyWithFavorite> companiesWithFavorite = new List<CompanyWithFavorite>();

            companies.ForEach((c) => {
                var s = JsonConvert.SerializeObject(c);
                CompanyWithFavorite cwf = JsonConvert.DeserializeObject<CompanyWithFavorite>(s);
                if (favoriteCompanies.Exists((fc) => fc.CompanyId == c.Id && fc.UserId == userId))
                    cwf.IsFavorite = true;
                else
                    cwf.IsFavorite = false;
                companiesWithFavorite.Add(cwf);
            });

            return companiesWithFavorite;

        }

        public async Task<List<CompanyBranchWithFavorite>> GetAllCompaniesBranches(int userId)
        {
            List<CompanyBranch> companiesBranches = await _context.CompanyBranches.Include(x => x.Company).ToListAsync();

            List<FavoriteCompanyBranch> favoriteCompaniesBranches = await _context.FavoriteCompaniesBranches.ToListAsync();

            List<CompanyBranchWithFavorite> companiesBranchesWithFavorite = new List<CompanyBranchWithFavorite>();

            companiesBranches.ForEach((c) => {
                var s = JsonConvert.SerializeObject(c);
                CompanyBranchWithFavorite cwf = JsonConvert.DeserializeObject<CompanyBranchWithFavorite>(s);
                if (favoriteCompaniesBranches.Exists((fc) => fc.CompanyBranchId == c.Id && fc.UserId == userId))
                    cwf.IsFavorite = true;
                else
                    cwf.IsFavorite = false;
                companiesBranchesWithFavorite.Add(cwf);
            });

            return companiesBranchesWithFavorite;

        }

        public async Task<List<CompanyBranchWithFavorite>> GetCompanyBranches(int userId, int companyId)
        {

            List<CompanyBranch> companiesBranches = await _context.CompanyBranches.Where(cb => cb.CompanyId == companyId).Include(x => x.Company).ToListAsync();

            List<FavoriteCompanyBranch> favoriteCompaniesBranches = await _context.FavoriteCompaniesBranches.ToListAsync();

            List<CompanyBranchWithFavorite> companiesBranchesWithFavorite = new List<CompanyBranchWithFavorite>();

            companiesBranches.ForEach((c) => {
                var s = JsonConvert.SerializeObject(c);
                CompanyBranchWithFavorite cwf = JsonConvert.DeserializeObject<CompanyBranchWithFavorite>(s);
                if (favoriteCompaniesBranches.Exists((fc) => fc.CompanyBranchId == c.Id && fc.UserId == userId))
                    cwf.IsFavorite = true;
                else
                    cwf.IsFavorite = false;
                companiesBranchesWithFavorite.Add(cwf);
            });

            return companiesBranchesWithFavorite;
        }

        public async Task<CompanyBranchWithFavorite> GetCompanyBranchByCode(int userId, string code)
        {
            CompanyBranch companyBranch = await _context.CompanyBranches.Where(cb => cb.Code == code).FirstAsync();

            List<FavoriteCompanyBranch> favoriteCompaniesBranches = await _context.FavoriteCompaniesBranches.ToListAsync();

            var s = JsonConvert.SerializeObject(companyBranch);
            CompanyBranchWithFavorite cwf = JsonConvert.DeserializeObject<CompanyBranchWithFavorite>(s);
            if (favoriteCompaniesBranches.Exists((fc) => fc.CompanyBranchId == companyBranch.Id && fc.UserId == userId))
                cwf.IsFavorite = true;
            else
                cwf.IsFavorite = false;

            return cwf;
        }

        public async Task<List<CompanyBranchWithFavorite>> GetCompanyBranchesByAddress(int userId, string address)
        {
            List<CompanyBranch> companiesBranches = await _context.CompanyBranches.Where(cb => cb.ArabicAddress.Contains(address) || cb.EnglishAddress.Contains(address)).ToListAsync();

            List<FavoriteCompanyBranch> favoriteCompaniesBranches = await _context.FavoriteCompaniesBranches.ToListAsync();

            List<CompanyBranchWithFavorite> companiesBranchesWithFavorite = new List<CompanyBranchWithFavorite>();

            companiesBranches.ForEach((c) => {
                var s = JsonConvert.SerializeObject(c);
                CompanyBranchWithFavorite cwf = JsonConvert.DeserializeObject<CompanyBranchWithFavorite>(s);
                if (favoriteCompaniesBranches.Exists((fc) => fc.CompanyBranchId == c.Id && fc.UserId == userId))
                    cwf.IsFavorite = true;
                else
                    cwf.IsFavorite = false;
                companiesBranchesWithFavorite.Add(cwf);
            });

            return companiesBranchesWithFavorite;
        }

        public async Task<bool> AddCompanyToFavorite(int userId, int companyId)
        {
            try
            {
                List<FavoriteCompany> fac = await _context.FavoriteCompanies.ToListAsync();

                if (!fac.Exists((f) => f.CompanyId == companyId && f.UserId == userId))
                {

                    FavoriteCompany fc = new FavoriteCompany();
                    fc.CompanyId = companyId;
                    fc.UserId = userId;
                    _context.FavoriteCompanies.Add(fc);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> RemoveCompanyFromFavorite(int userId, int companyId)
        {
            try
            {
                List<FavoriteCompany> fac = await _context.FavoriteCompanies.ToListAsync();

                if (fac.Exists((f) => f.CompanyId == companyId && f.UserId == userId))
                {

                    FavoriteCompany fc = await _context.FavoriteCompanies.Where((x) => (x.CompanyId == companyId) && (x.UserId == userId)).FirstAsync();
                    _context.FavoriteCompanies.Remove(fc);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> AddCompanyBranchToFavorite(int userId, int companyBranchId)
        {
            try
            {
                List<FavoriteCompanyBranch> fcb = await _context.FavoriteCompaniesBranches.ToListAsync();

                if (!fcb.Exists((fc) => fc.CompanyBranchId == companyBranchId && fc.UserId == userId))
                {
                    FavoriteCompanyBranch fc = new FavoriteCompanyBranch();
                    fc.CompanyBranchId = companyBranchId;
                    fc.UserId = userId;
                    _context.FavoriteCompaniesBranches.Add(fc);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> RemoveCompanyBranchFromFavorite(int userId, int companyBranchId)
        {
            try
            {
                List<FavoriteCompanyBranch> fcb = await _context.FavoriteCompaniesBranches.ToListAsync();

                if (fcb.Exists((fc) => fc.CompanyBranchId == companyBranchId && fc.UserId == userId))
                {

                    FavoriteCompanyBranch fc = await _context.FavoriteCompaniesBranches.Where((x) => (x.CompanyBranchId == companyBranchId) && (x.UserId == userId)).FirstAsync();
                    _context.FavoriteCompaniesBranches.Remove(fc);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<CompanyWithFavorite>> GetFavoriteCompanies(int userId) {
            List<Company> companies = await _context.Companies.ToListAsync();
            List<FavoriteCompany> favoriteCompanies = await _context.FavoriteCompanies.ToListAsync();

            List<CompanyWithFavorite> companiesWithFavorite = new List<CompanyWithFavorite>();
            companies.ForEach((c) => {
                var s = JsonConvert.SerializeObject(c);
                CompanyWithFavorite cwf = JsonConvert.DeserializeObject<CompanyWithFavorite>(s);
                if (favoriteCompanies.Exists((fc) => fc.CompanyId == c.Id && fc.UserId == userId))
                {
                    cwf.IsFavorite = true;
                    companiesWithFavorite.Add(cwf);
                }                
            });

            return companiesWithFavorite;
        }

        public async Task<List<CompanyBranchWithFavorite>> GetFavoriteCompaniesBranches(int userId)
        {
            List<CompanyBranch> companiesBranches = await _context.CompanyBranches.ToListAsync();
            List<FavoriteCompanyBranch> favoriteCompaniesBranches = await _context.FavoriteCompaniesBranches.ToListAsync();
            List<CompanyBranchWithFavorite> companiesBranchesWithFavorite = new List<CompanyBranchWithFavorite>();

            companiesBranches.ForEach((c) => {
                var s = JsonConvert.SerializeObject(c);
                CompanyBranchWithFavorite cwf = JsonConvert.DeserializeObject<CompanyBranchWithFavorite>(s);
                if (favoriteCompaniesBranches.Exists((fc) => fc.CompanyBranchId == c.Id && fc.UserId == userId))
                {
                    cwf.IsFavorite = true;
                    companiesBranchesWithFavorite.Add(cwf);
                }                
            });

            return companiesBranchesWithFavorite;
        }

        public async Task<List<CompanyWithFavorite>> GetFrequentlyVisitedCompanies(int userId)
        {
            List<Company> companies = await _context.Companies.ToListAsync();
            List<FrequentlyVisitedCompany> frequentlyVisitedCompanies = await _context.FrequentlyVisitedCompanies.ToListAsync();
            List<FavoriteCompany> favoriteCompanies = await _context.FavoriteCompanies.ToListAsync();

            List<CompanyWithFavorite> companiesWithFavorite = new List<CompanyWithFavorite>();
            companies.ForEach((c) => {
                var s = JsonConvert.SerializeObject(c);
                CompanyWithFavorite cwf = JsonConvert.DeserializeObject<CompanyWithFavorite>(s);
                if (favoriteCompanies.Exists((fc) => fc.CompanyId == c.Id && fc.UserId == userId))
                    cwf.IsFavorite = true;
                else
                    cwf.IsFavorite = false;

                if (frequentlyVisitedCompanies.Exists((fc) => fc.CompanyId == c.Id && fc.UserId == userId))
                    companiesWithFavorite.Add(cwf);
            });

            return companiesWithFavorite;
        }

        public async Task<List<CompanyBranchWithFavorite>> GetFrequentlyVisitedCompaniesBranches(int userId)
        {
            List<CompanyBranch> companiesBranches = await _context.CompanyBranches.Include(c => c.FrequentlyVisitedCompaniesBranches.Where((fc) => fc.UserId == userId)).ToListAsync();
            List<FrequentlyVisitedCompanyBranch> frequentlyVisitedCompaniesBranches = await _context.FrequentlyVisitedCompaniesBranches.ToListAsync();
            List<FavoriteCompanyBranch> favoriteCompaniesBranches = await _context.FavoriteCompaniesBranches.ToListAsync();
            
            List<CompanyBranchWithFavorite> companiesBranchesWithFavorite = new List<CompanyBranchWithFavorite>();

            companiesBranches.ForEach((c) => {
                var s = JsonConvert.SerializeObject(c);
                CompanyBranchWithFavorite cwf = JsonConvert.DeserializeObject<CompanyBranchWithFavorite>(s);
                if (favoriteCompaniesBranches.Exists((fc) => fc.CompanyBranchId == c.Id && fc.UserId == userId))
                    cwf.IsFavorite = true;
                else
                    cwf.IsFavorite = false;


                if (frequentlyVisitedCompaniesBranches.Exists((fc) => fc.CompanyBranchId == c.Id && fc.UserId == userId))
                    companiesBranchesWithFavorite.Add(cwf);
            });

            return companiesBranchesWithFavorite;
        }

        public async Task<bool> InsertCompanyBranch(CompanyBranch companyBranch)
        {
            try
            {
                if (companyBranch != null)
                {
                    _context.CompanyBranches.Add(companyBranch);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateCompanyBranch(CompanyBranch companyBranch)
        {
            try
            {
                if (companyBranch != null)
                {
                    _context.CompanyBranches.Update(companyBranch);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            } catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteCompanyBranch(CompanyBranch companyBranch)
        {
            try
            {
                if (companyBranch != null)
                {
                    _context.CompanyBranches.Remove(companyBranch);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch {
                return false;
            }
        }

        public async Task<CompanyBranch> GetCompanyBranchById(int id)
        {
            return await _context.CompanyBranches.FindAsync(id);
        }


        public async Task<bool> InsertCompany(Company company)
        {
            try
            {
                if (company != null)
                {
                    _context.Companies.Add(company);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateCompany(Company company)
        {
            try
            {
                if (company != null)
                {
                    _context.Companies.Update(company);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteCompany(Company company)
        {
            try
            {
                if (company != null)
                {
                    _context.Companies.Remove(company);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Company> GetCompanyById(int id)
        {
            return await _context.Companies.FindAsync(id);
        }


        public async Task<List<User>> GetCompanyUsers(int companyId)
        {
            List<User> companyUsers = await _context.Users.Where((x) => x.CompanyId == companyId).ToListAsync();

            return companyUsers;
        }

        public async Task<bool> AddUserToCompany(int userId, int companyId)
        {
            try
            {
                User? user = _context.Users.Where(u => u.Id == userId).First();
                if (user != null)
                {
                    user.CompanyId = companyId;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveUserFromCompany(int userId, int companyId)
        {
            try
            {
                User? user = _context.Users.Where(u => u.Id == userId).First();
                if (user != null)
                {
                    user.CompanyId = null;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return false;

            }
            catch
            {
                return false;
            }
        }

        public async Task<List<PaymentTransaction>> GetCompanyTransactions(int? companyId, bool filterByDate, DateTime filterFromDate, DateTime filterToDate)
        {
            List<PaymentTransaction> paymentTransactions;

            if (!filterByDate)
                paymentTransactions = await _context.PaymentTransactions.Where(pt => pt.CompanyId == companyId).Include(x => x.Company).Include(u => u.User).OrderByDescending(x => x.Date).ToListAsync();
            else
            {
                paymentTransactions = await _context.PaymentTransactions.Where(pt => pt.CompanyId == companyId && pt.Date.Date >= filterFromDate && pt.Date.Date <= filterToDate).Include(x => x.Company).Include(u => u.User).OrderByDescending(x => x.Date).ToListAsync();
            }

            return paymentTransactions;
        }
    }
}
