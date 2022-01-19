using KnowledgeBet.Core.Entities;
using KnowledgeBet.Core.Interfaces;
using KnowledgeBet.Core.Models;
using KnowledgeBet.Infrastructure;
using Microsoft.EntityFrameworkCore;
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
            var dbGamesWithUsers = dbContext.Games.Include(u => u.Players).ToList();
            var dbGameByUser = dbContext.GamesByUser.ToList();
            var games = new List<GameDTO>();

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
                try
                {
                    games.Add(new GameDTO
                    {
                        Players = dbGame.Players.Select(u => new UserDTO { FirstName = u.FirstName, LastName = u.LastName }).ToList(),
                        Date = dbGame.Date,
                        Winner = dbGame.Players
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

        public async Task<bool> CreateNewGame(NewGameDTO newGameDTO)
        {
            using var transaction = dbContext.Database.BeginTransaction();

            try
            {
                var dbPlayersInGame = dbContext.Users.Where(p => newGameDTO.PlayersId.Contains(p.Id)).ToList();
                var dbQuestions = dbContext.Questions.ToList();

                var newGame = new Game
                {
                    Date = newGameDTO.Date,
                    Players = dbPlayersInGame,
                    Questions = dbQuestions.Where(p => newGameDTO.QuestionsId.Contains(p.Id)).ToList()
                };

                dbContext.Games.Add(newGame);
                dbContext.Save();
                int newGameId = newGame.Id;

                //Winner
                dbContext.GamesByUser.Add(new GameUser
                {
                    GameId = newGameId,
                    UserId = newGameDTO.PlayerWinnerId,
                    HasWon = true
                });

                //Other players
                var otherPlayersInGame = dbPlayersInGame.Where(p => dbPlayersInGame.Select(pg => pg.Id).Contains(newGameDTO.PlayerWinnerId)).ToList();
                foreach (var otherPlayer in otherPlayersInGame)
                {
                    dbContext.GamesByUser.Add(new GameUser
                    {
                        GameId = newGameId,
                        UserId = otherPlayer.Id,
                        HasWon = false
                    });
                }
                dbContext.Save();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                logger.LogInformation("Error while saving played game: {@ex.Message}", ex.Message);
                throw new Exception("Error while saving played game: " + ex.Message);
            }
            return true;
        }

        public async Task<bool> CreateNewQuestion(NewQuestionDTO newQuestionDTO)
        {
            using var transaction = dbContext.Database.BeginTransaction();

            var question = new Question
            {
                Text = newQuestionDTO.Text,
                Subcategories = dbContext.Subcategories.Where(s => s.Id == newQuestionDTO.SubcategoryId).ToList()
            };

            try
            {
                dbContext.Questions.Add(question);
                dbContext.Save();
                int newQuestionId = question.Id;

                List<QuestionOption> newQuestionOptions = new List<QuestionOption>();
                foreach (var questionOption in newQuestionDTO.Options)
                {
                    newQuestionOptions.Add(new QuestionOption
                    {
                        QuestionId = newQuestionId,
                        Text = questionOption.Text,
                        IsCorrect = questionOption.IsCorrect
                    });
                }
                dbContext.QuestionOption.AddRange(newQuestionOptions);
                dbContext.Save();
                transaction.Commit();

                logger.LogInformation("Question saved: {@question}", question);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation("Error while saving new question: {@ex.Message}", ex.Message);
                throw new Exception("Error while saving new question: " + ex.Message);
            }
        }

        public async Task<bool> DeleteQuestion(int questionId)
        {
            using var transaction = dbContext.Database.BeginTransaction();

            try
            {
                var question = dbContext.Questions
                    .Include(q => q.Games)
                    .Include(q => q.Options)
                    .Include(q => q.Subcategories)
                    .Where(q => q.Id == questionId)
                    .First();
                if (question.Games.Count == 0)
                {
                    dbContext.Questions.Remove(question);
                    dbContext.Save();
                    transaction.Commit();

                    logger.LogInformation("Question deleted: {@question}", question);
                    return true;
                }
                else
                {
                    throw new Exception("The question is used in past games. You can deactivate it instead of deleting it.");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("Error while deleting question: {@ex.Message}", ex.Message);
                throw new Exception("Error while deleting question: " + ex.Message);
            }
        }
        public async Task<bool> DeactivateQuestion(int questionId)
        {
            using var transaction = dbContext.Database.BeginTransaction();

            try
            {
                var question = dbContext.Questions
                    .Where(q => q.Id == questionId)
                    .First();

                question.Active = false;
                dbContext.Save();
                transaction.Commit();

                logger.LogInformation("Question deactivated: {@question}", question);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation("Error while deactivating the question: {@ex.Message}", ex.Message);
                throw new Exception("Error while deactivating the question: " + ex.Message);
            }
        }
    }
}
