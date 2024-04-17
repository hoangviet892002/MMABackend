using eFurnitureProject.Application.Commons;
using eFurnitureProject.Domain.Entities;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eFurnitureProject.Domain.Enums;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IProductService
    {
        Task<ApiResponse<Pagination<ProductDTO>>> GetAll(int page, string CategoryID, string ProductName, double? minPrice, double? maxPrice, int pageSize);
        Task<ApiResponse<Pagination<ProductDTO>>> getAllProduct(int pageIndex = 0, int pageSize = 10);
        Task<ApiResponse<ProductDTO>> GetProductByID(string id);
        Task<ApiResponse<ProductDTO>> CreateProductByAdmin(CreateProductDTO createProductDTO);
        Task<ApiResponse<ProductDTO>> UpdateProductByAdmin(CreateProductDTO createProductDTO, Guid productID);
        Task<ApiResponse<bool>> DeleteProduct(Guid productID);
        Task<ApiResponse<Pagination<ProductDTO>>> getAllProductNotdeleted(int pageIndex = 0, int pageSize = 10);

        Task<ApiResponse<int>> CalculateTotalPages(int totalItemsCount, int pageSize);
        Task<ApiResponse<ProductDTO>> UpdateQuantityProduct(Guid productID, int quantity);
        Task<ApiResponse<bool>> UpdateProductStatus(Guid productId, ProductStatusEnum newStatus);
    }
}
