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
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly AppDbContext _dbContext;
        public TransactionRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<Pagination<Transaction>> FilterTransactionAsync(
            string? search, string? type, 
            DateTime? fromTime, DateTime? toTime,
            int pageIndex,int pageSize)
        {
            var itemList = _dbSet.Where(x =>
                    (string.IsNullOrEmpty(type) || x.Type.ToLower() == type.ToLower()) &&
                    (!fromTime.HasValue || x.CreationDate >= fromTime) &&
                    (!toTime.HasValue || x.CreationDate <= toTime.Value) &&
                    (string.IsNullOrEmpty(search) ||
                        x.User.Name.Contains(search) ||
                        x.User.PhoneNumber.Contains(search) ||
                        x.User.Email.Contains(search) ||
                        x.Description.ToLower().Contains(search.ToLower()))
                        );
            var items = await itemList.
                OrderByDescending(x => x.CreationDate)
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var itemCount = await itemList.CountAsync();
            var result = new Pagination<Transaction>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };
            return result;
        }

        public async Task<Pagination<Transaction>> FilterTransactionByLoginAsync(
            string userId, DateTime? fromTime, 
            DateTime? toTime, int pageIndex, 
            int pageSize)
        {
            var itemList = _dbSet.Where(x => x.UserId == userId &&
                                           (!fromTime.HasValue || x.CreationDate >= fromTime) &&
                                           (!toTime.HasValue || x.CreationDate <= toTime.Value));
            var items = await itemList.
                OrderByDescending(x => x.CreationDate)
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var itemCount = await itemList.CountAsync();
            var result = new Pagination<Transaction>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };
            return result;
        }
    }
}
