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
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IClaimsService _claimsService;

        public CartRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
            _claimsService = claimsService;
        }

        public async Task<Cart> GetCartAsync()
        {
            var result = await _dbSet.Include(c => c.CartDetails)
                                          .ThenInclude(cd => cd.Product)
                                          .FirstOrDefaultAsync(c => c.UserId == _claimsService.GetCurrentUserId.ToString()); ;
            if (result == null)
            {
                throw new Exception($"Not found any cart with userId: {_claimsService.GetCurrentUserId}");
            }
            return result;
        }

        public async Task<Guid> GetCartIdAsync()
        {
            var result = await _dbSet.Where(x => x.UserId == _claimsService.GetCurrentUserId.ToString())
                                       .Select(x => x.Id)
                                       .FirstOrDefaultAsync();
            if (result == Guid.Empty)
            {
                throw new Exception($"Not found any cart");
            }
            return result;
        }
        public async Task<IEnumerable<CartDetail>> GetCartDetailsByUserId(string userId)
        {
            var cartDetails = await _dbSet.Where(x => x.UserId == userId)
                                              .Include(x => x.CartDetails)
                                              .SelectMany(x => x.CartDetails).ToListAsync();
            if (cartDetails == null)
            {
                throw new Exception($"No any product in cart!");
            }
            return cartDetails;
        }
    }
}
