using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart> GetCartAsync();
        Task<Guid> GetCartIdAsync();
        Task<IEnumerable<CartDetail>> GetCartDetailsByUserId(string userId);
    }
}
