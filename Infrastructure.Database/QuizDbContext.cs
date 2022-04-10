using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database
{
    public class QuizDbContext : DbContext
    {
        private readonly ILogger _logger;

        public QuizDbContext(DbContextOptions<QuizDbContext> options, ILogger<QuizDbContext> logger)
            : base(options)
        {
            this._logger = logger;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOption { get; set; }
        public DbSet<GameUser> GamesByUser { get; set; }

        public DbSet<HomeComponentTile> HomeComponentTiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameUser>().HasKey(table => new
            {
                table.UserId,
                table.GameId
            });

            //Povezivanje User-a i Game-a unutar među tablice
            //1. Identifikacija veze direktno između User-a i Game-a

            //modelBuilder.Entity<User>()
            //    .HasMany(u => u.GamesPlayed)
            //    .WithMany(g => g.Players)
            //    //Među tablica
            //    .UsingEntity<GameUser>
            //    (gu => gu.HasOne<Game>().WithMany(),
            //    gu => gu.HasOne<User>().WithMany())
            //    .Property(gu => gu.HasWon)
            //    .HasDefaultValue(false);

            modelBuilder.Entity<Question>(q => q.Property(a => a.Active).HasDefaultValue(true));
        }
    }
}
