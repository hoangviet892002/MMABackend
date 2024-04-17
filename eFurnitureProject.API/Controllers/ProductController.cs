
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Dynamic;
using System.Net.Mime;
using System.Net;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using eFurnitureProject.Application.Services;
using eFurnitureProject.Domain.Enums;

namespace eFurnitureProject.API.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ApiResponse<Pagination<ProductDTO>>> FilterProducts2(
          int page,
         string? categoryId,
         string? productName,
           double? minPrice, double? maxPrice,
         int pageSize)
        {
           return await _productService.GetAll(page , categoryId, productName, minPrice, maxPrice, pageSize );
           
        }
       [Authorize(Roles = AppRole.Admin)]
        [HttpPost]

        public async Task<ApiResponse<ProductDTO>> CreateProductByAdmin(CreateProductDTO createProductDTO) =>  await _productService.CreateProductByAdmin(createProductDTO);

       [Authorize(Roles = AppRole.Admin)]
        [HttpDelete]
        public async Task<ApiResponse<bool>> DeleteProduct(Guid id)=>await _productService.DeleteProduct(id);
        
        [HttpGet]
        public async Task<ApiResponse<Pagination<ProductDTO>>> getAllProduct(int pageIndex = 0, int pageSize = 10)=> await _productService.getAllProduct();
        [HttpGet]
        public async Task<ApiResponse<ProductDTO>> GetProductByID(string id) => await _productService.GetProductByID(id);   
        [Authorize(Roles = AppRole.Admin)]
        [HttpPut]
        public async Task<ApiResponse<ProductDTO>> UpdateProductByAdmin(CreateProductDTO createProductDTO, Guid productID)=>await _productService.UpdateProductByAdmin(createProductDTO, productID);
        [HttpGet]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalPages(int totalItemsCount, int pageSize) => await _productService.CalculateTotalPages(totalItemsCount, pageSize);
        [Authorize(Roles = AppRole.Staff + "," + AppRole.Admin)]
        [HttpPut]
        public async Task<ApiResponse<ProductDTO>> UpdateQuantityProduct(Guid productID, int quantity) => await _productService.UpdateQuantityProduct(productID, quantity);
        [HttpPost]
        public async Task<ApiResponse<bool>> UpdateProductStatus(Guid ProductId, ProductStatusEnum newStatus) => await _productService.UpdateProductStatus(ProductId, newStatus);
    }
}
