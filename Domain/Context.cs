using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Domain
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        //Data Source=./;Initial Catalog=ProcurementHub;Integrated Security=True

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
    }
}
