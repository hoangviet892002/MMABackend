using eFurnitureProject.Application.Commons;
using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface IVoucherRepository : IGenericRepository<Voucher>
    {
        Task<IEnumerable<Voucher>> Get(int pageIndex, int pageSize);
        Task<Pagination<Voucher>> GetVoucherByDateAsync(int pageIndex, int pageSize, DateTime date);
        Task<bool> CheckVoucherNameExisted(string Name);
    
        Task<Voucher> GetDeletedVoucherByNameAsync(string voucherName);
        Task<Pagination<Voucher>> GetVoucher(int pageIndex, int pageSize);
        Task<bool> CheckVoucherNameExisted(Guid id, string Name);
        void UpdateVoucher(Voucher voucher);
    }
}
