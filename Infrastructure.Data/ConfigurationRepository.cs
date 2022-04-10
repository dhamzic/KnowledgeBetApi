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
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly ILogger _logger;
        private readonly QuizDbContext _dbContext;

        public ConfigurationRepository(ILogger<ConfigurationRepository> logger, QuizDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<List<HomeComponentTileModel>> GetHomeComponentTiles()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<HomeComponentTile, HomeComponentTileModel>();
                });

                var mapper = new Mapper(config);

                var dbHomeTiles = _dbContext.HomeComponentTiles.ToList();

                var homeTilesModel = mapper.Map<List<HomeComponentTileModel>>(dbHomeTiles);

                return homeTilesModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error - GetHomeComponentTiles");
                throw;
            }
        }
    }
}
