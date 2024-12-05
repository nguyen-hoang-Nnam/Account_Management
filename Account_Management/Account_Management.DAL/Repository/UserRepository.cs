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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public UserRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<User?> GetById(int userId)
        {
            return await _appDbContext.Users
                .Include(c => c.Account)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

    }
}
