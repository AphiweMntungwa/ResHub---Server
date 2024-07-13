using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResHub.Models;
using System.Xml;

namespace ResHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<StudentResident>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<Residence> Residence { get; set; }
        public DbSet<Events> Events { get; set; }   
        public DbSet<EventResidence> EventResidents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<StudentResident>()
            //    .HasIndex(sr => sr.StudentNumber)
            //    .IsUnique();

            // Configuring the many-to-many relationship
            modelBuilder.Entity<EventResidence>()
                .HasKey(er => new { er.ResidenceId, er.EventId });

            modelBuilder.Entity<EventResidence>()
                .HasOne(er => er.Residence)
                .WithMany(r => r.EventResidences)
                .HasForeignKey(er => er.ResidenceId);

            modelBuilder.Entity<EventResidence>()
                .HasOne(er => er.Event)
                .WithMany(e => e.EventResidences)
                .HasForeignKey(er => er.EventId);
        }
    }

}
