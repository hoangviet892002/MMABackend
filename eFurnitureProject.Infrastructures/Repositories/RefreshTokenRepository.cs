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
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _dbContext;

        public RefreshTokenRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddRefreshTokenAsync(RefreshToken refresh) =>
            await _dbContext.AddAsync(refresh);

        public async Task<RefreshToken> GetRefreshTokenByTokenAsync(string token) 
        {
            var result = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
            if (result == null)
            {
                throw new Exception($"Not found any object");
            }
                return result;

        }

        public void UpdateRefreshToken(RefreshToken refresh) => 
             _dbContext.Update(refresh);
    }
}
