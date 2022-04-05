using Domain.Entities.Models;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<List<UserModel>> GetAllPlayers()
        {
            var dbUsers = dbContext.Users.ToList();
            var users = new List<UserModel>();

            foreach (var dbConfig in dbUsers)
            {
                try
                {
                    users.Add(new UserModel
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

        public async Task<List<GameModel>> GetAllPlayedGames()
        {
            var dbGamesWithUsers = dbContext.Games.Include(u => u.Players).ToList();
            var dbGameByUser = dbContext.GamesByUser.ToList();
            var games = new List<GameModel>();

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
                    games.Add(new GameModel
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
                catch (Exception ex)
                {
                    logger.LogError(ex, "GetAllPlayedGames error while fetching data");
                }
            }
            return games;
        }

        public async Task<bool> CreateNewGame(NewGameModel newGameDTO)
        {
            using var transaction = dbContext.Database.BeginTransaction();

            try
            {

                var dbPlayersInGame = dbContext.Users.Where(p => newGameDTO.PlayersId.Contains(p.Id)).ToList();
                var dbQuestions = dbContext.Questions.ToList();

                var gamePlayed = new Game
                {
                    Date = newGameDTO.Date,
                    Questions = dbQuestions.Where(p => newGameDTO.QuestionsId.Contains(p.Id)).ToList()
                };
                List<GameUser> gameUserList = new List<GameUser>();

                foreach (var player in dbPlayersInGame)
                {
                    bool hasWon = newGameDTO.PlayerWinnerId == player.Id ? true : false;

                    gameUserList.Add(new GameUser
                    {
                        User = player,
                        Game = gamePlayed,
                        HasWon = hasWon
                    });
                }

                dbContext.GamesByUser.AddRange(gameUserList);
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

        public async Task<bool> CreateNewQuestion(NewQuestionModel newQuestionDTO)
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
