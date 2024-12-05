using Account_Management.DAL.Repository.IRepository;
using Account_Management.Models.Data;
using Account_Management.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.DAL.Repository
{
    public class AuthRepository : GenericRepository<Account>, IAuthRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public AuthRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            {
                _appDbContext = appDbContext;
            }

        }

        public async Task<Account?> GetByUserName(string Username)
        {
            return await _appDbContext.Accounts
                .Include(a => a.User)
                .SingleOrDefaultAsync(u => u.Username == Username);
        }
    }
}
