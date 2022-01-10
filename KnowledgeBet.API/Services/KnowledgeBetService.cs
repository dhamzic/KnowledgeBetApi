using KnowledgeBet.Core.Interfaces;
using KnowledgeBet.Core.Models;
using KnowledgeBet.Infrastructure;
using Newtonsoft.Json.Linq;

namespace KnowledgeBet.API.Services
{
    public class KnowledgeBetService : IKnowledgeBetService
    {
        private readonly QuizDbContext dbContext;
        private readonly ILogger logger;

        public KnowledgeBetService(
            QuizDbContext dbContext,
            ILogger<KnowledgeBetService> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public List<UserDTO> GetAllPlayers()
        {
            var dbUsers = dbContext.Users.ToList();
            var users = new List<UserDTO>();

            foreach (var dbConfig in dbUsers)
            {
                try
                {
                    users.Add(new UserDTO
                    {
                        FirstName = dbConfig.FirstName,
                        LastName = dbConfig.LastName
                    });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "GetAllPlayers error while fetching data");
                }
            }

            return users;
        }
    }
}
