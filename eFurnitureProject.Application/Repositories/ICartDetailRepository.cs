using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface ICartDetailRepository 
    {
        void UpdateQuantityProductInCart(CartDetail cartdetail);
        Task AddAsync(CartDetail cartdetail);
        void DeleteProductInCart(Guid cartId, Guid productId);
        void DeleteCart(Guid cartId);
    }
}
