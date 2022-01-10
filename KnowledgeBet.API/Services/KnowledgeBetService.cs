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

        public List<GameDTO> GetAllPlayedGames()
        {
            var dbGames = dbContext.Games.ToList();
            var dbPlayers = dbContext.Users.ToList();
            var dbGameByUser = dbContext.GamesByUser.ToList();
            var games = new List<GameDTO>();

            foreach (var dbGame in dbGames)
            {
                try
                {
                    games.Add(new GameDTO
                    {
                        Players = dbPlayers
                            .Where(p => dbGameByUser.Any(id => id.UserId == p.Id))
                            .Select(u => new UserDTO { FirstName = u.FirstName, LastName = u.LastName })
                            .ToList(),
                        Date = dbGame.Date,
                        Winner = dbPlayers
                            .Where(p => p.Id == dbGameByUser.Where(gu => gu.HasWon == true && gu.GameId == dbGame.Id).Select(gu => gu.UserId).Single())
                            .Select(u => new UserDTO
                            {
                                FirstName = u.FirstName,
                                LastName = u.LastName
                            })
                            .Single()
                    });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "GetAllPlayedGames error while fetching data");
                }
            }

            return games;
        }
    }
}
