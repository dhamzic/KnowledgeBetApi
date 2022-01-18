﻿using KnowledgeBet.Core.Entities;
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

        public async Task<bool> CreateNewQuestion(NewQuestionDTO newQuestionDTO)
        {
            using var transaction = dbContext.Database.BeginTransaction();

            var question = new Core.Entities.Question
            {
                Text = newQuestionDTO.Text
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
                dbContext.SaveChanges();

                logger.LogInformation("Question saved: {@config}", question);

                transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation("Error while saving new question: {@config}", ex.Message);
                throw new Exception("Error while saving new question: {@config}." + ex.Message);
            }
        }
    }
}