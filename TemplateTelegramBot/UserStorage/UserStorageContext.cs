using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TemplateTelegramBot.UserStorage
{
    internal class UserStorageContext : DbContext
    {
        private readonly string _databaseName;
        private readonly string _usersTableName;
        public DbSet<RootUser>? RootUsers { get; set; }
        internal UserStorageContext(string dbName, string usersTableName)
        {
            _databaseName = dbName;
            _usersTableName = usersTableName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_databaseName}.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RootUser>().ToTable(_usersTableName);
        }
    }
}
