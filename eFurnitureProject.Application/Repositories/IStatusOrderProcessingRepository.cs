using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface IStatusOrderProcessingRepository : IGenericRepository<StatusOrderProcessing>
    {
        Task<bool> CheckStatusOrderProcessingExisted(string name);
        Task<StatusOrderProcessing> GetStatusByStatusCode(int statusCode);
    }
}
