using FastFill_API.Interfaces;
using FastFill_API.Model;
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
            return await _context.Users.Where(u => u.MobileNumber == mobileNumber).FirstOrDefaultAsync();
        }

        public void Insert(User user)
        {
            if (user != null)
            {
                _context.Users.Add(user);
                _context.SaveChangesAsync();
            }
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

        public void Update(User user)
        {
            if (user != null)
            {
                _context.Attach(user);
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.Where(u => u.RoleId == (int)RoleType.User).ToListAsync();
        }

        public async Task<List<User>> GetAdmins()
        {
            return await _context.Users.Where(u => u.RoleId == (int)RoleType.Admin).ToListAsync();
        }

        public async Task<List<User>> GetSupports()
        {
            return await _context.Users.Where(u => u.RoleId == (int)RoleType.Support).ToListAsync();
        }
    }
}
