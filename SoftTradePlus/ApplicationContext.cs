using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoftTradePlus.Models;

namespace SoftTradePlus
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Client> Clients{ get; set; } = null!;
        public DbSet<Manager> Managers{ get; set; } = null!;
        //public DbSet<ClientProduct> ClientProduct { get; set; } = null!;
        public DbSet<ClientStatus> ClientStatuses { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-QLF1U61;Database=SoftTrade;Trusted_Connection=True;");
        }
    }
}
