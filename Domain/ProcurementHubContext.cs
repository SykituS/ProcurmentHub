using Microsoft.EntityFrameworkCore;
using ProcurementHub.Domain.Models;

namespace ProcurementHub.Domain
{
    public class ProcurementHubContext : DbContext
    {
        public ProcurementHubContext()
        { }

        public ProcurementHubContext(DbContextOptions options) : base(options)
        {
            
        }

        #region DbSets
        public DbSet<Users> Users { get; set; }
        public DbSet<Restaurants> Restaurants { get; set; }
        public DbSet<RestaurantsItemMenu> RestaurantsItemMenu { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<TeamsMembers> TeamsMembers { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(@"Data Source=.\;Initial Catalog=ProcurementHub;Integrated Security=True;MultipleActiveResultSets=true; TrustServerCertificate=True");
        }
    }
}
