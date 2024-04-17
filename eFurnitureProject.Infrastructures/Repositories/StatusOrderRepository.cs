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
    public class StatusOrderRepository:GenericRepository<StatusOrder>,IStatusOrderRepository
    {
        private readonly AppDbContext _dbContext;
        public StatusOrderRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<bool> CheckStatusOrderExisted(string name) =>
            await _dbContext.StatusOrders.AnyAsync(o => o.Name == name);

        public async Task<StatusOrder> GetStatusByStatusCode(int statusCode)
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.StatusCode == statusCode);
            if (result == null)
            {
                throw new Exception();
            }
            return result;
        }
    }
}
