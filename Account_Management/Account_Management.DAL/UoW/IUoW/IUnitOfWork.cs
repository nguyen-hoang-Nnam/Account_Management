using Account_Management.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.DAL.UoW.IUoW
{
    public interface IUnitOfWork
    {
        ApplicationDbContext dbContext { get; }
        public Task<int> SaveChangeAsync();
    }
}
