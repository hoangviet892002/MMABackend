using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository

    {
        private readonly AppDbContext _dbContext;
        public CategoryRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public Task<bool> CheckCategoryNameExisted(string name) => _dbContext.Categories.AnyAsync(u => u.Name == name && u.IsDeleted != true);

        public Task<bool> CheckCategoryNameExisted(Guid categoryId, string name) => _dbContext.Categories.AnyAsync(u => u.Name == name && u.Id != categoryId && u.IsDeleted != true);
    }
}
