using FastFill_API.Interfaces;
using FastFill_API.Model;
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

    }
}
