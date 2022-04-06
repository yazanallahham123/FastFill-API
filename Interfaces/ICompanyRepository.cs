using FastFill_API.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Interfaces
{
    public interface ICompanyRepository
    {
        public Task<List<CompanyWithFavorite>> GetAllCompanies(int userId);
        public Task<CompanyWithFavorite> GetCompanyByCode(int userId, string code);
        public Task<List<CompanyWithFavorite>> SearchCompaniesByName(int userId, string name);
        public Task<List<CompanyBranchWithFavorite>> SearchCompaniesBranchesByName(int userId, string name);

        public Task<List<CompanyBranchWithFavorite>> SearchCompaniesBranchesByText(int userId, string text);
        public Task<List<CompanyWithFavorite>> SearchCompaniesByText(int userId, string text);

        public Task<List<CompanyBranchWithFavorite>> GetAllCompaniesBranches(int userId);
        public Task<List<CompanyBranchWithFavorite>> GetCompanyBranches(int userId, int companyId);
        public Task<CompanyBranchWithFavorite> GetCompanyBranchByCode(int userId, string code);
        public Task<List<CompanyBranchWithFavorite>> GetCompanyBranchesByAddress(int userId, string address);

        public Task<bool> AddCompanyToFavorite(int userId, int companyId);
        public Task<bool> RemoveCompanyFromFavorite(int userId, int companyId);

        public Task<bool> AddCompanyBranchToFavorite(int userId, int companyId);
        public Task<bool> RemoveCompanyBranchFromFavorite(int userId, int companyId);

        public Task<List<CompanyWithFavorite>> GetFavoriteCompanies(int userId);
        public Task<List<CompanyBranchWithFavorite>> GetFavoriteCompaniesBranches(int userId);

        public Task<List<CompanyWithFavorite>> GetFrequentlyVisitedCompanies(int userId);
        public Task<List<CompanyBranchWithFavorite>> GetFrequentlyVisitedCompaniesBranches(int userId);

        public Task<bool> InsertCompanyBranch(CompanyBranch companyBranch);
        public Task<bool> UpdateCompanyBranch(CompanyBranch companyBranch);
        public Task<bool> DeleteCompanyBranch(CompanyBranch companyBranch);

        public Task<CompanyBranch> GetCompanyBranchById(int id);


        public Task<bool> InsertCompany(Company company);
        public Task<bool> UpdateCompany(Company company);
        public Task<bool> DeleteCompany(Company company);

        public Task<Company> GetCompanyById(int id);

        public Task<List<User>> GetCompanyUsers(int companyId);

        public Task<bool> AddUserToCompany(int userId, int companyId);
        public Task<bool> RemoveUserFromCompany(int userId, int companyId);

        public Task<List<PaymentTransaction>> GetCompanyTransactions(int? companyId);
    }
}
