using Microsoft.EntityFrameworkCore;

namespace WebApplication1
{
    public class BloggingContext : DbContext
    {
        //public DbSet<PlayList> PlayList { get; set; }
        public DbSet<Song> Song { get; set; }

        public string DbPath { get; }

        public BloggingContext()
        {
            DbPath = "C:\\Users\\trifo\\source\\repos\\WebApplication1\\blogging.db";
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

        }
    }
}
