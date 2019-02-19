using Microsoft.EntityFrameworkCore;
using SmartUniTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartUniTest.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Data Source=SQL6005.site4now.net;Initial Catalog=DB_A43268_AhmedSM;User Id=DB_A43268_AhmedSM_admin;Password=zxc9812zxc;");
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Initial Catalog=SmarUniDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEvent>().HasKey(ue => new { ue.UserId, ue.EventId });

            modelBuilder.Entity<UserEvent>()
            .HasOne<User>( ue => ue.User)
            .WithMany(u => u.UserEvents)
            .HasForeignKey(ue => ue.UserId);


            modelBuilder.Entity<UserEvent>()
                .HasOne<Event>(ue => ue.Event)
                .WithMany(e => e.UserEvents)
                .HasForeignKey(ue => ue.EventId);
        }

    }
}
