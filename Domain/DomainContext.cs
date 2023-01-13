using Microsoft.EntityFrameworkCore;
using ProcurementHub.Domain.Models;

namespace ProcurementHub.Domain
{
    public class DomainContext : DbContext
    {
        public DomainContext(DbContextOptions<DomainContext> options) : base(options)
        {
            
        }

        #region DbSets
        public DbSet<Users> Users { get; set; }
        public DbSet<Restaurants> Restaurants { get; set; }
        public DbSet<RestaurantsItemMenu> RestaurantsItemMenu { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<UserStatistics> UserStatistics { get; set; }
        public DbSet<TeamsMembers> TeamsMembers { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Data Source=./;Initial Catalog=ProcurementHub;Integrated Security=True");
        }
    }
}
