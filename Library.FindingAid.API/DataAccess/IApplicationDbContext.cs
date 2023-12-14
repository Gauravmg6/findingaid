using Library.FindingAid.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.FindingAid.API.DataAccess
{
    public interface IApplicationDbContext
    {
        DbSet<Collection> Collection { get; set; }
        DbSet<Box> Box { get; set; }
        DbSet<Folder> Folder { get; set; }
        DbSet<Item> Item { get; set; }
        DbSet<Record> Record { get; set; }
        DbSet<RecordDetail> Details { get; set; }
        DbSet<Admin> Admin { get; set; }

        Task<int> SaveChanges();
    }
}