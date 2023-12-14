using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.FindingAid.API.Models;
using Library.FindingAid.API.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Library.FindingAid.API.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IApplicationDbContext dbContext;

        public AdminRepository(IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAdmin(string UserId)
        {
            await dbContext.Admin.AddAsync(new Admin { UserId = UserId });
            await dbContext.SaveChanges();
        }

        public async Task RemoveAdmin(string UserId)
        {
            dbContext.Admin.Remove(await dbContext.Admin.FirstAsync(s => s.UserId == UserId));
            await dbContext.SaveChanges();
        }

        public async Task<List<Admin>> GetAll()
        {
            return await dbContext.Admin.ToListAsync();
        }

        public async Task<bool> IsAdmin(string UserId)
        {
            return await dbContext.Admin.AnyAsync(s => s.UserId == UserId);
        }
    }
}
