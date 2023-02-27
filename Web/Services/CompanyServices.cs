using FastFill_API.Interfaces;
using FastFill_API.Web.Dto;
using FastFill_API.Web.Model;
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
    public class CompanyServices
    {
        private readonly IPublicRepository _repository;
        private readonly SecurityServices _securityServices;

        public CompanyServices(IPublicRepository repository, SecurityServices securityServices)
        {
            _repository = repository;
            _securityServices = securityServices;
        }

        public async Task<IEnumerable<Company>> GetAllCompanies(int page, int pageSize, PaginationInfo paginationInfo, int userId)
        {
            IEnumerable<Company> companies = await _repository.GetCompanyRepository.GetAllCompanies(userId);
            paginationInfo.SetValues(pageSize, page, companies.Count());
            return companies.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<CompanyWithFavorite> GetCompanyByCode(string code, int userId)
        {
            CompanyWithFavorite company = await _repository.GetCompanyRepository.GetCompanyByCode(userId, code);
            return company;
        }

        public async Task<IEnumerable<CompanyWithFavorite>> SearchCompaniesByName(int page, int pageSize, PaginationInfo paginationInfo, string name, int userId)
        {
            IEnumerable<CompanyWithFavorite> companies = await _repository.GetCompanyRepository.SearchCompaniesByName(userId, name);
            paginationInfo.SetValues(pageSize, page, companies.Count());
            return companies.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<CompanyBranch>> GetAllCompaniesBranches(int page, int pageSize, PaginationInfo paginationInfo, int userId)
        {
            IEnumerable<CompanyBranch> companyBranches = await _repository.GetCompanyRepository.GetAllCompaniesBranches(userId);
            paginationInfo.SetValues(pageSize, page, companyBranches.Count());
            return companyBranches.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<CompanyBranch>> GetCompanyBranches(int page, int pageSize, PaginationInfo paginationInfo, int companyId, int userId)
        {
            IEnumerable<CompanyBranch> companyBranches = await _repository.GetCompanyRepository.GetCompanyBranches(userId, companyId);
            paginationInfo.SetValues(pageSize, page, companyBranches.Count());
            return companyBranches.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<CompanyBranchWithFavorite> GetCompanyBranchByCode(string code, int userId)
        {
            CompanyBranchWithFavorite companyBranch = await _repository.GetCompanyRepository.GetCompanyBranchByCode(userId, code);
            return companyBranch;
        }

        public async Task<IEnumerable<CompanyBranch>> GetCompanyBranchesByAddress(int page, int pageSize, PaginationInfo paginationInfo, string address, int userId)
        {
            IEnumerable<CompanyBranch> companyBranches = await _repository.GetCompanyRepository.GetCompanyBranchesByAddress(userId, address);
            paginationInfo.SetValues(pageSize, page, companyBranches.Count());
            return companyBranches.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<bool> AddCompanyToFavorite(int userId, int companyId)
        {
            return await _repository.GetCompanyRepository.AddCompanyToFavorite(userId, companyId);            
        }

        public async Task<bool> RemoveCompanyFromFavorite(int userId, int companyId)
        {
            return await _repository.GetCompanyRepository.RemoveCompanyFromFavorite(userId, companyId);
        }

        public async Task<bool> AddCompanyBranchToFavorite(int userId, int companyBranchId)
        {
            return await _repository.GetCompanyRepository.AddCompanyBranchToFavorite(userId, companyBranchId);
        }

        public async Task<bool> RemoveCompanyBranchFromFavorite(int userId, int companyBranchId)
        {
            return await _repository.GetCompanyRepository.RemoveCompanyBranchFromFavorite(userId, companyBranchId);
        }

        public async Task<IEnumerable<CompanyWithFavorite>> GetFavoriteCompanies(int page, int pageSize, PaginationInfo paginationInfo, int userId)
        {
            IEnumerable<CompanyWithFavorite> companies = await _repository.GetCompanyRepository.GetFavoriteCompanies(userId);
            paginationInfo.SetValues(pageSize, page, companies.Count());
            return companies.Skip((page - 1) * pageSize).Take(pageSize);

        }

        public async Task<IEnumerable<CompanyBranchWithFavorite>> GetFavoriteCompaniesBranches(int page, int pageSize, PaginationInfo paginationInfo, int userId)
        {
            IEnumerable<CompanyBranchWithFavorite> companiesBranches = await _repository.GetCompanyRepository.GetFavoriteCompaniesBranches(userId);
            paginationInfo.SetValues(pageSize, page, companiesBranches.Count());
            return companiesBranches.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<CompanyBranchWithFavorite>> GetFrequentlyVisitedCompaniesBranches(int page, int pageSize, PaginationInfo paginationInfo, int userId)
        {
            IEnumerable<CompanyBranchWithFavorite> companiesBranches = await _repository.GetCompanyRepository.GetFrequentlyVisitedCompaniesBranches(userId);
            paginationInfo.SetValues(pageSize, page, companiesBranches.Count());
            return companiesBranches.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<CompanyWithFavorite>> GetFrequentlyVisitedCompanies(int page, int pageSize, PaginationInfo paginationInfo, int userId)
        {
            IEnumerable<CompanyWithFavorite> companies = await _repository.GetCompanyRepository.GetFrequentlyVisitedCompanies(userId);
            paginationInfo.SetValues(pageSize, page, companies.Count());
            return companies.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<CompanyBranchWithFavorite>> SearchCompaniesBranchesByName(int page, int pageSize, PaginationInfo paginationInfo, string name, int userId)
        {
            IEnumerable<CompanyBranchWithFavorite> companiesBranches = await _repository.GetCompanyRepository.SearchCompaniesBranchesByName(userId, name);
            paginationInfo.SetValues(pageSize, page, companiesBranches.Count());
            return companiesBranches.Skip((page - 1) * pageSize).Take(pageSize);
        }


        public async Task<IEnumerable<CompanyBranchWithFavorite>> SearchCompaniesBranchesByText(int page, int pageSize, PaginationInfo paginationInfo, string name, int userId)
        {
            IEnumerable<CompanyBranchWithFavorite> companiesBranches = await _repository.GetCompanyRepository.SearchCompaniesBranchesByText(userId, name);
            paginationInfo.SetValues(pageSize, page, companiesBranches.Count());
            return companiesBranches.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<CompanyWithFavorite>> SearchCompaniesByText(int page, int pageSize, PaginationInfo paginationInfo, string name, int userId)
        {
            IEnumerable<CompanyWithFavorite> companies = await _repository.GetCompanyRepository.SearchCompaniesByText(userId, name);
            paginationInfo.SetValues(pageSize, page, companies.Count());
            return companies.Skip((page - 1) * pageSize).Take(pageSize);
        }
        public async Task<bool> AddCompanyBranch(CompanyBranch companyBranch)
        {
            try
            {
                return await _repository.GetCompanyRepository.InsertCompanyBranch(companyBranch);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateCompanyBranch(CompanyBranch companyBranch)
        {
            try
            {
                return await _repository.GetCompanyRepository.UpdateCompanyBranch(companyBranch);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCompanyBranch(CompanyBranch companyBranch)
        {
            try
            {
                return await _repository.GetCompanyRepository.DeleteCompanyBranch(companyBranch);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<CompanyBranch> GetCompanyBranchById(int id)
        {
            return await _repository.GetCompanyRepository.GetCompanyBranchById(id);

        }


        public async Task<bool> AddCompany(Company company)
        {
            try
            {
                return await _repository.GetCompanyRepository.InsertCompany(company);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateCompany(Company company)
        {
            try
            {
                return await _repository.GetCompanyRepository.UpdateCompany(company);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCompany(Company company)
        {
            try
            {
                return await _repository.GetCompanyRepository.DeleteCompany(company);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<Company> GetCompanyById(int id)
        {
            return await _repository.GetCompanyRepository.GetCompanyById(id);

        }

        public async Task<IEnumerable<User>> GetCompanyUsers(int page, int pageSize, PaginationInfo paginationInfo, int companyId)
        {
            IEnumerable<User> users = await _repository.GetCompanyRepository.GetCompanyUsers(companyId);
            paginationInfo.SetValues(pageSize, page, users.Count());
            return users.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<bool> AddUserToCompany(int userId, int companyId)
        {
            try
            {
                return await _repository.GetCompanyRepository.AddUserToCompany(userId, companyId);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> RemoveUserFromCompany(int userId, int companyId)
        {
            try
            {
                return await _repository.GetCompanyRepository.RemoveUserFromCompany(userId, companyId);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<IEnumerable<PaymentTransaction>> GetCompanyPaymentTransactions(int page, int pageSize, PaginationInfo paginationInfo, int userId, bool filterByDate, DateTime filterFromDate, DateTime filterToDate)
        {
            User user = await _repository.GetUserRepository.GetById(userId, 0);
            int? companyId = user.CompanyId;

            List<PaymentTransaction> paymentTransactions = await _repository.GetCompanyRepository.GetCompanyTransactions(companyId, filterByDate, filterFromDate, filterToDate);

            paginationInfo.SetValues(pageSize, page, paymentTransactions.Count());
            return paymentTransactions.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<MonthlyPaymentTransaction>> GetMonthlyCompanyPaymentTransactions(int page, int pageSize, PaginationInfo paginationInfo, int userId)
        {
            User user = await _repository.GetUserRepository.GetById(userId, 0);
            int? companyId = user.CompanyId;

            List<MonthlyPaymentTransaction> monthlyPaymentTransactions = await _repository.GetCompanyRepository.GetMonthlyCompanyTransactions(companyId);

            paginationInfo.SetValues(pageSize, page, monthlyPaymentTransactions.Count());
            return monthlyPaymentTransactions.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<TotalPaymentTransaction> GetTotalCompanyPaymentTransactions(int userId, bool filterByDate, DateTime filterFromDate, DateTime filterToDate)
        {
            try
            {
                User user = await _repository.GetUserRepository.GetById(userId, 0);
                int? companyId = user.CompanyId;

                return await _repository.GetCompanyRepository.GetTotalCompanyPaymentTransactions(companyId, filterByDate, filterFromDate, filterToDate);

            }
            catch (DbUpdateConcurrencyException)
            {
                return new TotalPaymentTransaction { count = 0, amount = 0.0};
            }
        }

        public async Task<int> GetCompaniesCount()
        {
            return await _repository.GetCompanyRepository.GetCompanisCount();
        }

        public async Task<IEnumerable<PaymentTransaction>> GetCompanyPaymentTransactionsPDF(int userId, bool filterByDate, DateTime filterFromDate, DateTime filterToDate)
        {
            User user = await _repository.GetUserRepository.GetById(userId, 0);
            int? companyId = user.CompanyId;

            List<PaymentTransaction> paymentTransactions = await _repository.GetCompanyRepository.GetCompanyTransactions(companyId, filterByDate, filterFromDate, filterToDate);

            return paymentTransactions;
        }

        public async Task<bool> AddCompanyPump(CompanyPump companyPump)
        {
            try
            {
                return await _repository.GetCompanyRepository.InsertCompanyPump(companyPump);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }

        public async Task<bool> UpdateCompanyPump(CompanyPump companyPump)
        {
            try
            {
                return await _repository.GetCompanyRepository.UpdateCompanyPump(companyPump);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }

        public async Task<bool> RemoveCompanyPump(CompanyPump companyPump)
        {
            try
            {
                return await _repository.GetCompanyRepository.DeleteCompanyPump(companyPump);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }



        public async Task<bool> AddGroup(Group group)
        {
            try
            {
                return await _repository.GetCompanyRepository.InsertGroup(group);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateGroup(Group group)
        {
            try
            {
                return await _repository.GetCompanyRepository.UpdateGroup(group);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteGroup(Group group)
        {
            try
            {
                return await _repository.GetCompanyRepository.DeleteGroup(group);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<Group> GetGroupById(int id)
        {
            return await _repository.GetCompanyRepository.GetGroupById(id);

        }

        public async Task<IEnumerable<Group>> GetAllGroups(int page, int pageSize, PaginationInfo paginationInfo)
        {
            IEnumerable<Group> groups = await _repository.GetCompanyRepository.GetAllGroups();
            paginationInfo.SetValues(pageSize, page, groups.Count());
            return groups.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<Group>> SearchGroupsByName(int page, int pageSize, PaginationInfo paginationInfo, string name)
        {
            IEnumerable<Group> groups = await _repository.GetCompanyRepository.SearchGroupsByName(name);
            paginationInfo.SetValues(pageSize, page, groups.Count());
            return groups.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesByGroup(int page, int pageSize, PaginationInfo paginationInfo, int userId, Totals totals, bool filterByDate, DateTime filterFromDate, DateTime filterToDate)
        {
            IEnumerable<Company> companies = await _repository.GetCompanyRepository.GetAllCompaniesByGroup(userId);

            int c = 0;
            double a = 0.0;
            foreach (var cc in companies)
            {
                TotalPaymentTransaction companyTotalPaymentTransactions = await GetTotalCompanyPaymentTransactionsByCompanyId(userId, filterByDate, filterFromDate, filterToDate, cc.Id);
                c = c + companyTotalPaymentTransactions.count;
                a = a + companyTotalPaymentTransactions.amount;
            }

            totals.SetValues(c, a);

            paginationInfo.SetValues(pageSize, page, companies.Count());
            return companies.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<PaymentTransaction>> GetCompanyPaymentTransactionsByCompanyId(int page, int pageSize, PaginationInfo paginationInfo, int companyId, bool filterByDate, DateTime filterFromDate, DateTime filterToDate)
        {
            List<PaymentTransaction> paymentTransactions = await _repository.GetCompanyRepository.GetCompanyTransactions(companyId, filterByDate, filterFromDate, filterToDate);

            paginationInfo.SetValues(pageSize, page, paymentTransactions.Count());
            return paymentTransactions.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<MonthlyPaymentTransaction>> GetMonthlyCompanyPaymentTransactionsByCompanyId(int page, int pageSize, PaginationInfo paginationInfo, int userId, int companyId)
        {

            List<MonthlyPaymentTransaction> monthlyPaymentTransactions = await _repository.GetCompanyRepository.GetMonthlyCompanyTransactions(companyId);

            paginationInfo.SetValues(pageSize, page, monthlyPaymentTransactions.Count());
            return monthlyPaymentTransactions.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<TotalPaymentTransaction> GetTotalCompanyPaymentTransactionsByCompanyId(int userId, bool filterByDate, DateTime filterFromDate, DateTime filterToDate, int companyId)
        {
            try
            {
                return await _repository.GetCompanyRepository.GetTotalCompanyPaymentTransactions(companyId, filterByDate, filterFromDate, filterToDate);

            }
            catch (DbUpdateConcurrencyException)
            {
                return new TotalPaymentTransaction { count = 0, amount = 0.0 };
            }
        }

        public async Task<IEnumerable<PaymentTransaction>> GetCompanyPaymentTransactionsPDFByCompanyId(int userId, bool filterByDate, DateTime filterFromDate, DateTime filterToDate, int companyId)
        {
            List<PaymentTransaction> paymentTransactions = await _repository.GetCompanyRepository.GetCompanyTransactions(companyId, filterByDate, filterFromDate, filterToDate);

            return paymentTransactions;
        }

    }
}
