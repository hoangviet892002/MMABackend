using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.CartViewModels;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface ICartService
    {
        Task<ApiResponse<string>> addProductAsync(AddProductToCartDTO productDTO);
        Task<ApiResponse<List<CartDetailViewDTO>>> GetItemsInCartAsync();
        Task<ApiResponse<string>> DeleteProductInCartAsync(string productId);
        Task<ApiResponse<string>> IncreaseProductInCartAsync(string productId);
        Task<ApiResponse<string>> DecreaseProductIncartAsync(string productId);
    }
}
