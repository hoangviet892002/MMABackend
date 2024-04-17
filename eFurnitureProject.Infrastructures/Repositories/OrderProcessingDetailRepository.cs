using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class OrderProcessingDetailRepository : IOrderProcessingDetailRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderProcessingDetailRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<OrderProcessingDetail>> GetOrderProcessingDetailByContractId(Guid contractId)
        {
            var result = await _dbContext.OrderProcessingDetails.Include(opd => opd.Product)
                                                                  .Where(opd => opd.OrderProcessing.Contract.Id == contractId)
                                                                  .ToListAsync();
            if (result.IsNullOrEmpty())
            {
                throw new Exception($"Not found contract with contractId: {contractId}");
            }
            return result.AsEnumerable();
        }
    }
}
