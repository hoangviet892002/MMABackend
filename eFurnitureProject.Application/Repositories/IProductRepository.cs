using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        void UpdateProductByOrder(List<Product> product);
        Task<ProductDTO> GetProductsByIDAsync(string productId);

        Task<Pagination<ProductDTO>> GetProductsByNameAsync(string productName, int pageIndex, int pageSize);
        Task<Pagination<ProductDTO>> GetProductsByPriceAsync(double? minPrice, double? maxPrice, int pageIndex, int pageSize);

        Task<Pagination<ProductDTO>> ToPaginationProductNotDeleted(int pageIndex = 0, int pageSize = 10);
        Task<Pagination<ProductDTO>> GetProductsByCategoryIDAsync(string categoryIds, int pageIndex, int pageSize);
        Task<Pagination<ProductDTO>> ToPaginationProduct(int pageIndex = 0, int pageSize = 10);

        Task<Pagination<ProductDTO>> GetProductsByCategoryIDAndNameAsync(String categoryId, string productName, int pageIndex, int pageSize);
        void IncreaseQuantityProductFromImport(ICollection<ImportDetail> importDetails);
        Task<int> GetQuantityByIdAsync(Guid productId);
       //----------------------------Filter ---------------------------------------
        Task<Pagination<ProductDTO>> GetProductsByCategoryIDAndMaxPriceAsync(string categoryID, double value, int page, int pageSize);
        Task<Pagination<ProductDTO>> GetProductsByCategoryIDAndMinPriceAsync(string categoryID, double value, int page, int pageSize);
        Task<Pagination<ProductDTO>> GetProductsByCategoryIDAndPriceRangeAsync(string categoryID, double value1, double value2, int page, int pageSize);
        Task<Pagination<ProductDTO>> GetProductsByNameAndMinPriceAsync(string productName, double value, int page, int pageSize);
        Task<Pagination<ProductDTO>> GetProductsByNameAndMaxPriceAsync(string productName, double value, int page, int pageSize);
        Task<Pagination<ProductDTO>> GetProductsByNameAndPriceRangeAsync(string productName, double value1, double value2, int page, int pageSize);
        Task<Pagination<ProductDTO>> GetProductsByMinPriceAsync(double value, int page, int pageSize);
        Task<Pagination<ProductDTO>> GetProductsByMaxPriceAsync(double value, int page, int pageSize);
        Task<Pagination<ProductDTO>> GetProductsByCategoryIdAndNameAndMinPriceAsync(string categoryID, string productName, double? minPrice, int page, int pageSize);
        Task<Pagination<ProductDTO>> GetProductsByCategoryIdAndNameAndMaxPriceAsync(string categoryID, string productName, double? maxPrice, int page, int pageSize);
        Task<Pagination<ProductDTO>> GetProductsByCategoryIdAndNameAndMinAndMaxPriceAsync(string categoryID, string productName, double? minPrice, double? maxPrice, int page, int pageSize);
    }
}
