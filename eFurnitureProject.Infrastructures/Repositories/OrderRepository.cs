using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.OrderViewModels;
using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IClaimsService _claimsService;
        private readonly ICurrentTime _currentTime;
        public OrderRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
            _currentTime = timeService;
            _claimsService = claimsService;
        }


        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _dbContext.Orders.Where(x => x.Id == orderId)
                                         .Include(x => x.StatusOrder)
                                         .Include(x => x.User).FirstOrDefaultAsync();
            if (order == null) throw new Exception("Not found!");
            return order;
        }

        public async Task<Pagination<Order>> GetOrderByFilter(int pageIndex,
            int pageSize, int? status, DateTime? fromTime, DateTime? toTime,
            string? search)
        {
            var itemList = _dbSet
            .Where(x => (!fromTime.HasValue || x.CreationDate >= fromTime) &&
            (!toTime.HasValue  || x.CreationDate <= toTime.Value) &&
                        (string.IsNullOrEmpty(search) ||
                        x.Name.Contains(search) ||
                        x.PhoneNumber.Contains(search) ||
                        x.Email.Contains(search)))
            .Include(x => x.StatusOrder)
            .Where(x => status == null || x.StatusOrder.StatusCode == status);

            var items = await itemList.
                OrderByDescending(x => x.CreationDate)
                                    .Skip((pageIndex-1) * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var itemCount = await itemList.CountAsync();
            var result = new Pagination<Order>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };
            return result;
        }

        public async Task<Pagination<Order>> GetOrderFilterByLogin(int pageIndex, 
            int pageSize, int? status, DateTime? fromTime, DateTime? toTime,
            string? userId)
        {
            var itemList = _dbSet
            .Where(x => x.UserId == userId &&
            (!fromTime.HasValue || x.CreationDate >= fromTime) &&
            (!toTime.HasValue || x.CreationDate <= toTime.Value))
            .Include(x => x.StatusOrder)
            .Where(x => status == null || x.StatusOrder.StatusCode == status);

            var items = await itemList.
                OrderByDescending(x => x.CreationDate)
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var itemCount = await itemList.CountAsync();
            var result = new Pagination<Order>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };
            return result;
        }
        public async Task<StatusOrder> GetStatusOrderByOrderId(Guid orderId)
        {
            var order = await _dbSet
                .Where(x => x.Id == orderId)
                .Include(x => x.StatusOrder)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                throw new Exception("Not Found!");
            }

            if (order.StatusOrder == null)
            {
                throw new Exception("StatusOrder not found!");
            }

            return order.StatusOrder;
        }
    }
}
