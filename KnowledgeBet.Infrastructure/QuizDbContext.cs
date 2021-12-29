using KnowledgeBet.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBet.Infrastructure
{
    public class QuizDbContext : DbContext
    {
        private readonly ILogger logger;

        public QuizDbContext(DbContextOptions options, ILogger<QuizDbContext> logger)
            : base(options)
        {
            this.logger = logger;
        }

        public QuizDbContext(ILogger<QuizDbContext> logger)
        {
            this.logger = logger;
        }

        public void Save()
        {
            try
            {
                SaveChanges();
                logger.LogInformation("Object saved to database");
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Db update exception");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Db update exception");
                throw;
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
