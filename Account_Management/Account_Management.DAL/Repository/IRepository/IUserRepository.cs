using Account_Management.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.DAL.Repository.IRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetById(int userId);
    }
}
