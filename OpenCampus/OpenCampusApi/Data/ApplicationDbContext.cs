using OpenCampus.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace OpenCampus.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Location> Locations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<News> News { get; set; }


    }
}

