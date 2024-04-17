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
    public class OrderProcessingRepository:GenericRepository<OrderProcessing>,IOrderProcessingRepository
    {
        private readonly AppDbContext _dbContext;
        public OrderProcessingRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<OrderProcessing> GetOrderProcessingByContractId(Guid contractId)
        {
            var result = await _dbContext.OrderProcessings.FirstOrDefaultAsync(x => x.Contract.Id == contractId);
            if (result is null)
            {
                throw new Exception($"Not found order processing with contractId: {contractId}");
            }
            return result;
        }
    }
}
