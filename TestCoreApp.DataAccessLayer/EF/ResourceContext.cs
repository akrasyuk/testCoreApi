using Microsoft.EntityFrameworkCore;
using TestCoreApp.Entities;

namespace TestCoreApp.DataAccessLayer.EF
{
    public sealed class ResourceContext : DbContext
    {
        public ResourceContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<Resource> Resources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=localhost\SQLEXPRESS;Initial Catalog=Resources; Integrated Security=True");       
        }
    }
}
