using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration.EFCore
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Link> Links { get; set; }

    }
}
