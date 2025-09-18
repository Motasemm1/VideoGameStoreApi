using Microsoft.EntityFrameworkCore;
using VideoGameApi.Domain;

namespace VideoGameApi.Infrastructure.Data
{
    public class VideoGameDbContext : DbContext
    {
        public VideoGameDbContext(DbContextOptions<VideoGameDbContext> options)
            : base(options)
        {
        }

        public DbSet<VideoGame> VideoGames { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<UserVideoGame> UserVideoGames { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserVideoGame>()
                .HasKey(uv => new { uv.UserId, uv.GameId });  // Composite Key

            modelBuilder.Entity<UserVideoGame>()
                .HasOne(uv => uv.User)
                .WithMany(u => u.UserVideoGames)
                .HasForeignKey(uv => uv.UserId);

            modelBuilder.Entity<UserVideoGame>()
                .HasOne(uv => uv.VideoGame)
                .WithMany(v => v.UserVideoGames)
                .HasForeignKey(uv => uv.GameId);
            modelBuilder.Entity<User>()
       .Property(u => u.Role)
       .HasConversion<string>();

            modelBuilder.Entity<Genre>().HasData(
                new Genre { GenreId = 1, Name = "FootBall" },
                new Genre { GenreId = 2, Name = "BattleField" }
            );
            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { PublisherId = 1, Name = "EaSports" },
                new Publisher { PublisherId = 2, Name = "Activision" }
            );
            modelBuilder.Entity<VideoGame>().HasData(
                new VideoGame { GameId = 1, Title = "Fifa", Description = "Football Game", Price = 150, GenreId = 1, PublisherId = 1 },
                new VideoGame { GameId = 2, Title = "Pubg", Description = "Battlefield", Price = 50, GenreId = 2, PublisherId = 2 }
            );
        }
        
    }
}