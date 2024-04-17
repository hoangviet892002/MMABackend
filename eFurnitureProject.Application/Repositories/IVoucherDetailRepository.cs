using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface IVoucherDetailRepository
    {
        Task AddAsync(VoucherDetail voucherDetail);
        Task<bool> CheckVoucherBeUsedByUser(string userId, Guid voucherId);
    }
}
