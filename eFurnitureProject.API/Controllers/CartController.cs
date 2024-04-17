using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.CartViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    [Authorize]
    public class CartController : BaseController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            this._cartService = cartService;
        }

        [HttpPost]
        public async Task<ApiResponse<string>> AddProduct(AddProductToCartDTO product) => await _cartService.addProductAsync(product);

        [HttpGet]
        public async Task<ApiResponse<List<CartDetailViewDTO>>> GetItemsInCart() => await _cartService.GetItemsInCartAsync();

        [HttpDelete]
        public async Task<ApiResponse<string>> DeleteProductInCart(string productId) => await _cartService.DeleteProductInCartAsync(productId);

        [HttpPut]
        public async Task<ApiResponse<string>> IncreaseProductInCart(string productId) => await _cartService.IncreaseProductInCartAsync(productId);

        [HttpPut]
        public async Task<ApiResponse<string>> DecreaseProductInCart(string productId) => await _cartService.DecreaseProductIncartAsync(productId);
    } 
}
