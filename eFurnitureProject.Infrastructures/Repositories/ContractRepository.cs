using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class ContractRepository : GenericRepository<Domain.Entities.Contract>, IContractRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IClaimsService _claimsService;

        public ContractRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
            _claimsService = claimsService;
        }

        public async Task<Pagination<Domain.Entities.Contract>> GetContractByLoginCustomerToPagination(string customerId, int pageIndex = 0, int pageSize = 10)
        {
            var itemList =  _dbSet.Where(ct => ct.CustomerId == customerId);
            var itemCount = itemList.Count();
            var items = await itemList.OrderByDescending(ct => ct.CreationDate)
                                     .Skip(pageIndex * pageSize)
                                     .Take(pageSize)
                                     .Include(ct => ct.OrderProcessing)
                                     .ThenInclude(op => op.StatusOrderProcessing)
                                     .Include(ct => ct.Customer)
                                     .AsNoTracking()
                                     .ToListAsync();

            var result = new Pagination<Domain.Entities.Contract>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };
            return result;
        }


        public async Task<Pagination<Domain.Entities.Contract>> GetContractToPagination(int pageIndex = 0, int pageSize = 10)
        {
            var itemCount = await _dbSet.CountAsync();
            var items = await _dbSet.OrderByDescending(ct => ct.CreationDate)
                                     .Skip(pageIndex * pageSize)
                                     .Take(pageSize)
                                     .Include(ct => ct.OrderProcessing)
                                     .ThenInclude(op => op.StatusOrderProcessing)
                                     .Include(ct => ct.Customer)
                                     .AsNoTracking()
                                     .ToListAsync();

            var result = new Pagination<Domain.Entities.Contract>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };
            return result;
        }

        public async Task<Domain.Entities.Contract> GetContractWithDetail(Guid contractId)
        {
            var result = await _dbContext.Contracts.Include(ct => ct.OrderProcessing)
                                                             .ThenInclude(op => op.StatusOrderProcessing)
                                                           .Include(ct => ct.OrderProcessing)
                                                             .ThenInclude(op => op.OrderProcessingDetail)
                                                            .ThenInclude(opd => opd.Product)
                                                            .Include(ct => ct.Customer)
                                                            .FirstOrDefaultAsync(ct => ct.Id == contractId);
            if (result is null)
            {
                throw new Exception($"Not found contract with contractId: {contractId}");
            }
            return result;
        }
    }
        //public async Task<Pagination<Domain.Entities.Contract>> GetContractByFilter(int pageIndex, int pageSize, int? status, DateTime? fromTime, DateTime? toTime, string? search)
        //{
        //    var itemList = _dbSet.Where(x => (!fromTime.HasValue || x.CreationDate >= fromTime)
        //                                               && (!toTime.HasValue || x.CreationDate <= toTime.Value)
        //                                               && (string.IsNullOrEmpty(search)
        //                                               || x.Title.Contains(search) || x.Customer.Name.Contains(search)
        //                                               || x.OrderProcessing.Name.Contains(search)));

        //    var items = await _dbSet.OrderByDescending(ct => ct.CreationDate)
        //                             .Skip(pageIndex * pageSize)
        //                             .Take(pageSize)
        //                             .Include(ct => ct.OrderProcessing)
        //                             .ThenInclude(op => op.StatusOrderProcessing)
        //                             .Include(ct => ct.Customer)
        //                             .AsNoTracking()
        //                             .ToListAsync();

        //    var itemCount = await itemList.CountAsync();
        //    var result = new Pagination<Domain.Entities.Contract>()
        //    {
        //        PageIndex = pageIndex,
        //        PageSize = pageSize,
        //        TotalItemsCount = itemCount,
        //        Items = items,
        //    };
        //    return result;
        //}

}
