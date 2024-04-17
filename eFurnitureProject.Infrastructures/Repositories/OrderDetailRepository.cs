using eFurnitureProject.Application.Commons;
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
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private AppDbContext _appDbContext;
        public OrderDetailRepository(AppDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
        }
        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByIdAsync(Guid id) =>
            await _appDbContext.OrdersDetails.Where(od => od.OrderId == id)
                                             .Include(od => od.Product)
                                             .ToListAsync();
        public async Task AddRangeAsync(List<OrderDetail> orderDetails) =>
            await _appDbContext.OrdersDetails.AddRangeAsync(orderDetails);
    }
}
