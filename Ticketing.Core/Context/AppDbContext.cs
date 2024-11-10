using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Data.Models;

namespace Ticketing.Core.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<City>().ToTable("City");
            modelBuilder.Entity<Media>().ToTable("Media");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<Status>().ToTable("Status");
            modelBuilder.Entity<Ticket>().ToTable("Ticket");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<TicketType>().ToTable("TicketType");
            modelBuilder.Entity<User_City>().ToTable("User_City").HasKey(x => new{ x.CityId, x.UserId});
            modelBuilder.Entity<User_Ticket>().ToTable("User_Ticket").HasKey(x=> new {x.TicketId, x.UserId});
        }

        public DbSet<City> City { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<User_City> User_City {  get; set; }
        public DbSet<User_Ticket> User_Ticket { get; set; }
        public DbSet<TicketType> TicketType { get; set; }
    }
}
