using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface IOrderProcessingRepository : IGenericRepository<OrderProcessing>
    {
        Task<OrderProcessing> GetOrderProcessingByContractId(Guid contractId);    
    }
}
