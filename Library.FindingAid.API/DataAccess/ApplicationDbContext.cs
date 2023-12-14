using Library.FindingAid.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Library.FindingAid.API.DataAccess
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private string connectionString;
        public ApplicationDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 18));
            optionsBuilder.UseMySql(connectionString, serverVersion, (mySqlOptions) =>
            {
                mySqlOptions.EnableRetryOnFailure();
            });
        }

        public DbSet<Collection> Collection { get; set; }
        public DbSet<Box> Box { get; set; }
        public DbSet<Folder> Folder { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Record> Record { get; set; }
        public DbSet<RecordDetail> Details { get; set; }
        public DbSet<Admin> Admin { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasKey(o => new { o.ItemNumber, o.FolderNumber, o.BoxNumber, o.AccessionNumber });
            modelBuilder.Entity<Folder>()
                .HasKey(o => new { o.FolderNumber, o.BoxNumber, o.AccessionNumber });
            modelBuilder.Entity<Box>()
                .HasKey(o => new { o.BoxNumber, o.AccessionNumber });
            modelBuilder.Entity<Collection>()
                .HasKey(o => o.AccessionNumber);


        }

        public new async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }
    }
}