using GrpcShared.Models;
using Microsoft.EntityFrameworkCore;
using ProcurementService.DbModels;

namespace ProcurementService.Data
{
    public class ProcurementContext : DbContext
    {
        public ProcurementContext(DbContextOptions<ProcurementContext> options) : base(options)
        {
            
        }

        public DbSet<Persons> Persons { get; set; }

        public DbSet<Users> Users { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<TeamMembers> TeamMembers { get; set; }
        public DbSet<TeamRestaurants> TeamRestaurants { get; set; }
        public DbSet<TeamRestaurantsItems> TeamRestaurantsItems { get; set; }
        public DbSet<TeamOrders> TeamOrders { get; set; }
        public DbSet<TeamOrdersItems> TeamOrdersItems { get; set; }
    }
}
