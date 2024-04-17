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
    public class VoucherRepository : GenericRepository<Voucher>, IVoucherRepository
    {
        private readonly AppDbContext _dbContext;
        public VoucherRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Voucher>> Get(int pageIndex, int pageSize)
        {
            try
            {
                var items = await _dbSet.OrderByDescending(x => x.CreationDate)
                                    .Skip(pageIndex * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
                return items;
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }
        public async Task<bool> CheckVoucherNameExisted(string Name) =>
           await _dbContext.Vouchers.AnyAsync(u => u.VoucherName == Name && u.IsDeleted==false);
        public async Task<bool> CheckVoucherNameExisted(Guid id,string Name) =>
         await _dbContext.Vouchers.AnyAsync(u => u.VoucherName == Name && u.Id !=id  &&u.IsDeleted == false);
        
        public async Task<Pagination<Voucher>> GetVoucherByDateAsync(int pageIndex, int pageSize, DateTime date)
        {
            var voucher = await _dbContext.Vouchers.
     Where(v =>( v.StartDate.Date == date.Date || v.EndDate.Date == date.Date)&&v.IsDeleted==false)
       .Select(p => new Voucher
       {
           Id = p.Id,
           VoucherName = p.VoucherName,
  
           StartDate = p.StartDate,
           EndDate = p.EndDate,
           Percent = p.Percent,
           Number = p.Number,
           MinimumOrderValue = p.MinimumOrderValue,
           MaximumDiscountAmount = p.MaximumDiscountAmount
       })
       .ToListAsync();

            var totalItems = voucher.Count;

            var paginatedProducts = voucher.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagination = new Pagination<Voucher>
            {
                Items = paginatedProducts,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = totalItems,

            };

            return pagination;


        }

        public async Task<Pagination<Voucher>> GetVoucher(int pageIndex, int pageSize)
        {
            var voucher = await _dbContext.Vouchers.
     Where(v => v.Number>0 && v.IsDeleted==false)
       .Select(p => new Voucher
       {
           Id = p.Id,
           VoucherName = p.VoucherName,

           StartDate = p.StartDate,
           EndDate = p.EndDate,
           Percent = p.Percent,
           Number = p.Number,
           MinimumOrderValue = p.MinimumOrderValue,
           MaximumDiscountAmount = p.MaximumDiscountAmount
       })
       .ToListAsync();

            var totalItems = voucher.Count;

            var paginatedProducts = voucher.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagination = new Pagination<Voucher>
            {
                Items = paginatedProducts,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = totalItems,

            };

            return pagination;


        }
        public async Task<Voucher> GetDeletedVoucherByNameAsync(string voucherName)
        {
            return await _dbContext.Vouchers.FirstOrDefaultAsync(v => v.VoucherName == voucherName && v.IsDeleted);
        }

        public void UpdateVoucher(Voucher voucher)
        {
            _dbSet.Update(voucher);
        }
    }
}
      
