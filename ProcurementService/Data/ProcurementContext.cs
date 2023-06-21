using GrpcShared.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}
