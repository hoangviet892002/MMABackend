using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class CartDetailRepository : ICartDetailRepository
    {
        private readonly AppDbContext _dbContext;

        public CartDetailRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(CartDetail cartdetail) => await _dbContext.AddAsync(cartdetail);

        public void DeleteCart(Guid cartId)
        {
            var cartDetails = _dbContext.CartDetails.Where(cd => cd.CartId == cartId).ToList();
            _dbContext.CartDetails.RemoveRange(cartDetails);
        }

        public void DeleteProductInCart(Guid cartId, Guid productId)
        {
            var itemInCart = new CartDetail()
            {
                CartId = cartId,
                ProductId = productId
            };
            _dbContext.CartDetails.Remove(itemInCart);
        }
        
        public void UpdateQuantityProductInCart(CartDetail cartdetail) => _dbContext.Update(cartdetail);
    }
}
