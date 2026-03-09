using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options){  }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Review> Reviews { get; set; }
   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
