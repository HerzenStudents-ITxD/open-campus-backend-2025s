using OpenCampus.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace OpenCampus.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Location> Locations { get; set; }
        // сюда же позже добавишь News, Events и т.д.
    }
}

