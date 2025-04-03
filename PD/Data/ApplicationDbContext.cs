
using Microsoft.EntityFrameworkCore;
using PD.Models;
using System.Collections.Generic;
using PD.Models;

namespace PD.Data
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<FormData> FormData { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
    }

}
