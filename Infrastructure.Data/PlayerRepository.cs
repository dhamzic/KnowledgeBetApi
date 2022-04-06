using AutoMapper;
using Domain.Entities.Models;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Infrastructure.Database.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ILogger _logger;
        private readonly QuizDbContext _dbContext;

        public PlayerRepository(ILogger<PlayerRepository> logger, QuizDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<List<UserModel>> GetAllPlayers()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<User, UserModel>();
                });

                var mapper = new Mapper(config);

                var dbUsers = _dbContext.Users.ToList();

                var usersModel = mapper.Map<List<UserModel>>(dbUsers);

                return usersModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error - GetAllPlayers");
                throw;
            }
        }

        public async Task<UserModel> GetPlayer(int id)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<User, UserModel>();
                });

                var mapper = new Mapper(config);

                var dbUser = _dbContext.Users.Where(u => u.Id == id).First();

                var userModel = mapper.Map<UserModel>(dbUser);

                return userModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error - GetPlayer");
                throw;
            }
        }

        public async Task<UserModel> CreateNewPlayer(string firstName, string lastName)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<User, UserModel>();
                });

                var mapper = new Mapper(config);

                var newUser = new User
                {
                    FirstName = firstName,
                    LastName = lastName
                };

                _dbContext.Add(newUser);
                await _dbContext.SaveChangesAsync();

                var newlyAddedUser = mapper.Map<UserModel>(newUser);

                _logger.LogInformation("Player created: {@newlyAddedUser}", newlyAddedUser);

                return newlyAddedUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error - CreateNewPlayer");
                throw;
            }
        }

    }
}
