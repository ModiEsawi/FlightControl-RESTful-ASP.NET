using Microsoft.EntityFrameworkCore;
namespace FlightControlWeb.Models
{
    /*
     * Database class.
     * defines tables in databases , and relations between entites in different tables.
     */
    public class DatabaseContext : DbContext
    {
        //tables in db
        public DbSet<FlightPlan> FlightPlans { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Segment> Segment { get; set; }
        public DbSet<Server> Servers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //each segment object in Segments table will have an id for it's flightplan/
            modelBuilder.Entity<FlightPlan>().HasMany(c => c.Segments);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //connection
            optionsBuilder.UseSqlite("Filename=MyDatabase.db");
        }
    }
}
