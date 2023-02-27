using FastFill_API.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Repositories
{
    public class PublicRepository : IPublicRepository
    {
        private readonly FastFillDBContext _context;
        private IUserRepository _userRepository;
        private ICompanyRepository _companyRepository;
        private IDashboardRepository _dashboardRepository;

        public PublicRepository(FastFillDBContext context)
        {
            _context = context;
        }

        public IUserRepository GetUserRepository
        {
            get
            {
                return _userRepository ??= new UserRepository(_context);
            }
        }

        public ICompanyRepository GetCompanyRepository
        {
            get
            {
                return _companyRepository ??= new CompanyRepository(_context);
            }
        }

        public IDashboardRepository GetDashboardRepository
        {
            get
            {
                return _dashboardRepository ??= new DashboardRepository(_context);
            }
        }

    }
}
