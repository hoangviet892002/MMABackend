using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.OrderViewModels;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Pagination<Order>> GetOrderByFilter(int pageIndex, 
            int pageSize, int? status, DateTime? fromTime, DateTime? toTime, 
            string? search);
        Task<Pagination<Order>> GetOrderFilterByLogin(int pageIndex,
            int pageSize, int? status, DateTime? fromTime, DateTime? toTime,
            string? userId);
        Task<StatusOrder> GetStatusOrderByOrderId(Guid orderId);
        Task<Order> GetOrderByIdAsync(Guid orderId);
    }
}
