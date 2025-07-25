﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        public DbSet<Message> Messages { get; set; }
        public DbSet<Bus> Bus { get; set; }
        public DbSet<DepartureTime> DepartureTime { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Love> Loves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<StudentResident>()
            //    .HasIndex(sr => sr.StudentNumber)
            //    .IsUnique();
        }
    }

}
