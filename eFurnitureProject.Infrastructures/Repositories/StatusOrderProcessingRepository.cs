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
    public class StatusOrderProcessingRepository:GenericRepository<StatusOrderProcessing>,IStatusOrderProcessingRepository
    {
        private readonly AppDbContext _dbContext;
        public StatusOrderProcessingRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<bool> CheckStatusOrderProcessingExisted(string name) => await _dbContext.StatusOrderProcessings.AnyAsync(o => o.Name == name);

        public async Task<StatusOrderProcessing> GetStatusByStatusCode(int statusCode)
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.StatusCode == statusCode);
            if (result == null)
            {
                throw new Exception($"Not found any status processing with code: {statusCode}");
            }
            return result;
        }
    }
}
