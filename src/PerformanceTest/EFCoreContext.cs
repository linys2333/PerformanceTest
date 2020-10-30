using Microsoft.EntityFrameworkCore;
using PerformanceTest.Models;

namespace PerformanceTest
{
    public class EFCoreContext : DbContext
    {
        private readonly string _connectionString;

        public EFCoreContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionString);
        }

        public DbSet<TestData> TestDatas { get; set; }
    }
}