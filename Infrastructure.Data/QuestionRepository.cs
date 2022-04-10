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
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ILogger _logger;
        private readonly QuizDbContext _dbContext;

        public QuestionRepository(ILogger<QuestionRepository> logger, QuizDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<QuestionModel> CreateNewQuestion(NewQuestionModel newQuestion)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Question, QuestionModel>();
                    cfg.CreateMap<Subcategory, SubcategoryModel>();
                    cfg.CreateMap<QuestionOption, QuestionOptionModel>();
                    cfg.CreateMap<Category, CategoryModel>();
                });

                var mapper = new Mapper(config);  

                var question = new Question
                {
                    Text = newQuestion.Text,
                    Subcategories = _dbContext.Subcategories
                        .Where(s => s.Id == newQuestion.SubcategoryId)
                        .Include(c => c.Category)
                        .ToList()
                };


                _dbContext.Questions.Add(question);
                _dbContext.SaveChanges();
                int newQuestionId = question.Id;

                List<QuestionOption> newQuestionOptions = new();
                foreach (var questionOption in newQuestion.Options)
                {
                    newQuestionOptions.Add(new QuestionOption
                    {
                        QuestionId = newQuestionId,
                        Text = questionOption.Text,
                        IsCorrect = questionOption.IsCorrect
                    });
                }
                _dbContext.QuestionOption.AddRange(newQuestionOptions);

                _dbContext.SaveChanges();
                await transaction.CommitAsync();

                _logger.LogInformation("Question saved: {@question}", question);

                var questionModel = mapper.Map<QuestionModel>(question);

                return questionModel;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogInformation("Error while saving new question: {@ex.Message}", ex.Message);
                throw;
            }
        }

        public async Task DeactivateQuestion(int questionId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var question = _dbContext.Questions
                    .Where(q => q.Id == questionId)
                    .First();

                question.Active = false;
                _dbContext.SaveChanges();
                await transaction.CommitAsync();

                _logger.LogInformation("Question deactivated: {@question}", question);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogInformation("Error while deactivating the question: {@ex.Message}", ex.Message);
                throw;
            }
        }

        public async Task DeleteQuestion(int questionId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var question = _dbContext.Questions
                    .Include(q => q.Games)
                    .Include(q => q.Options)
                    .Include(q => q.Subcategories)
                    .Where(q => q.Id == questionId)
                    .First();

                if (question.Games.Count == 0)
                {
                    _dbContext.Questions.Remove(question);
                    _dbContext.SaveChanges();
                    await transaction.CommitAsync();

                    _logger.LogInformation("Question deleted: {@question}", question);
                }
                else
                {
                    throw new Exception("The question is used in existing matches. You can deactivate it instead of deleting it.");
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogInformation("Error while deleting question: {@ex.Message}", ex.Message);
                throw new Exception("Error while deleting question: " + ex.Message);
            }
        }

        public async Task<QuestionModel> GetQuestion(int id)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Question, QuestionModel>();
                });

                var mapper = new Mapper(config);

                var dbQuestion = _dbContext.Questions.Where(q => q.Id == id).First();

                var questionModel = mapper.Map<QuestionModel>(dbQuestion);

                return questionModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error - GetQuestion");
                throw;
            }
        }
    }
}
