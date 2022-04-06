using AutoMapper;
using Domain.Entities.Models;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class GameRepository : IGameRepository
    {
        private readonly ILogger _logger;
        private readonly QuizDbContext _dbContext;

        public GameRepository(ILogger<GameRepository> logger, QuizDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<GameModel> CreateNewGame(DateTime date, List<int> players, List<int> questions, int winner)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Game, GameModel>();
                    cfg.CreateMap<User, UserModel>();
                });

                var mapper = new Mapper(config);

                var dbPlayersInGame = _dbContext.Users.Where(p => players.Contains(p.Id)).ToList();
                var dbQuestions = _dbContext.Questions.Where(q => questions.Contains(q.Id)).ToList();

                var gamePlayed = new Game
                {
                    Date = date,
                    Questions = dbQuestions.Where(p => questions.Contains(p.Id)).ToList()
                };
                List<GameUser> gameUserList = new List<GameUser>();

                foreach (var player in dbPlayersInGame)
                {
                    bool hasWon = winner == player.Id ? true : false;

                    gameUserList.Add(new GameUser
                    {
                        User = player,
                        Game = gamePlayed,
                        HasWon = hasWon
                    });
                }

                _dbContext.GamesByUser.AddRange(gameUserList);
                _dbContext.SaveChanges();
                await transaction.CommitAsync();

                _logger.LogInformation("Game created: {@gamePlayed}", gamePlayed);

                var gameModel = mapper.Map<GameModel>(gamePlayed);

                return gameModel;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogInformation("Error while saving played game: {@ex.Message}", ex.Message);
                throw new Exception("Error while saving played game: " + ex.Message);
            }
        }

        public async Task<List<PlayedGamesModel>> GetAllPlayedGames()
        {
            try
            {
                var dbGamesWithUsers = _dbContext.Games.Include(u => u.Players).ThenInclude(us=>us.User).ToList();
                var dbGameByUser = _dbContext.GamesByUser.ToList();
                var games = new List<PlayedGamesModel>();

                #region Execution Method Poiners
                //Single očekuje točno jedan rezultat.Ako se vrati više, bacit će error
                //First dohvaća samo prvi match od 0..*

                //First / Single / Last će baciti error ukoliko se ne dohvati ni jedan podatak
                //FirstOrDefault / SingleOrDefault / LastOrDefault će vratiti null, ako se ne vrati ni jedan podatak
                #endregion

                #region Paging
                //Skip(0).Take(10)
                //Skip(10).Take(10)
                #endregion

                foreach (var dbGame in dbGamesWithUsers)
                {

                    games.Add(new PlayedGamesModel
                    {
                        Players = dbGame.Players.Select(u => new UserModel { FirstName = u.User.FirstName, LastName = u.User.LastName }).ToList(),
                        Date = dbGame.Date,
                        Winner = dbGame.Players
                            .Where(p => p.User.Id == dbGameByUser.Where(gu => gu.HasWon == true && gu.GameId == dbGame.Id).Select(gu => gu.UserId).Single())
                            .Select(u => new UserModel
                            {
                                FirstName = u.User.FirstName,
                                LastName = u.User.LastName
                            })
                            .Single()
                    });
                }
                return games;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error - GetAllPlayedGames");
                throw;
            }
        }

        public async Task<GameModel> GetGame(int id)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Game, GameModel>();
                });

                var mapper = new Mapper(config);

                var dbGame = _dbContext.Games.Where(g => g.Id == id).First();

                var gameModel = mapper.Map<GameModel>(dbGame);

                return gameModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error - GetPlayer");
                throw;
            }
        }
    }
}
