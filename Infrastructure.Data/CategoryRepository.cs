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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ILogger _logger;
        private readonly QuizDbContext _dbContext;

        public CategoryRepository(ILogger<CategoryRepository> logger, QuizDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<List<CategoryModel>> GetCategories()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Category, CategoryModel>();
                    cfg.CreateMap<Subcategory, SubcategoryModel>();
                });

                var mapper = new Mapper(config);

                var dbCategories = _dbContext.Categories.Include(c => c.Subcategories).ToList();

                var categoriesModel = mapper.Map<List<CategoryModel>>(dbCategories);

                return categoriesModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error - GetCategories");
                throw;
            }
        }
    }
}
