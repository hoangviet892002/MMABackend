using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public ProductRepository(
            AppDbContext context,
            ICurrentTime timeService,
            IClaimsService claimsService
        )
            : base(context, timeService, claimsService)
        {
            _dbContext = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public void UpdateProductByOrder(List<Product> product) 
        {
            _dbSet.UpdateRange(product);
        }
        public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryNameAsync(string categoryName)
        {
            var product = await _dbContext.Products
                            .Include(p => p.Category)
                            .Where(p => p.Category.Name == categoryName)
                            .Select(p => new ProductDTO
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Description = p.Description,
                                Image = p.Image,
                                InventoryQuantity = p.InventoryQuantity,
                                Status = p.Status,
                                CategoryId = p.Category.Id,
                                CategoryName = p.Category.Name
                            })
                            .ToListAsync();

            return product;
        }

        public async Task<Pagination<ProductDTO>> GetProductsByNameAsync(string productName, int pageIndex, int pageSize)
        {
            var products = await _dbContext.Products
        .Include(p => p.Category)
        .Where(p => p.Name.ToLower().Contains(productName.ToLower()))
        .Select(p => new ProductDTO
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Image = p.Image,
            InventoryQuantity = p.InventoryQuantity,
            Status = p.Status,
            Price = p.Price,
            CategoryId = p.Category.Id,
            CategoryName = p.Category.Name
        })
        .ToListAsync();

            var totalItems = products.Count;

            var paginatedProducts = products.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagination = new Pagination<ProductDTO>
            {
                Items = paginatedProducts,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = totalItems,

            };

            return pagination;
        }
        public async Task<ProductDTO> GetProductsByIDAsync(string productId)
        {
            var products = await _dbContext.Products
        .Include(p => p.Category)
         .Where(p => p.Id.ToString() == productId)
        .Select(p => new ProductDTO
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Image = p.Image,
            InventoryQuantity = p.InventoryQuantity,
            Status = p.Status,
            Price = p.Price,
            CategoryId = p.Category.Id,
            CategoryName = p.Category.Name
        })
        .FirstOrDefaultAsync();
            return products;
        }
        public async Task<Pagination<ProductDTO>> GetProductsByPriceAsync(double? minPrice, double? maxPrice, int pageIndex, int pageSize)
        {

            IQueryable<Product> query = _dbContext.Products.Include(p => p.Category);

            if (minPrice >= 0 && maxPrice >= 0 && minPrice <= maxPrice)
            {
                query = query.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            }
            else if (minPrice >= 0)
            {
                query = query.Where(p => p.Price == minPrice);
            }
            else if (maxPrice >= 0)
            {
                query = query.Where(p => p.Price == maxPrice);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            pageIndex = Math.Max(1, Math.Min(pageIndex, totalPages)); // Ensure pageIndex is within valid range

            var paginatedProducts = await query
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Image = p.Image,
                    InventoryQuantity = p.InventoryQuantity,
                    Status = p.Status,
                    Price = p.Price,
                    CategoryId = p.Category.Id,
                    CategoryName = p.Category.Name
                })
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagination = new Pagination<ProductDTO>
            {
                Items = paginatedProducts,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = totalItems,

            };

            return pagination;
        }







        public async Task<Pagination<ProductDTO>> GetProductsByCategoryIDAsync(string categoryId, int pageIndex, int pageSize)
        {

            var products = await _dbContext.Products
      .Include(p => p.Category)
      .Where(p => p.CategoryId.ToString() == categoryId)
      .Select(p => new ProductDTO
      {
          Id = p.Id,
          Name = p.Name,
          Description = p.Description,
          Image = p.Image,
          InventoryQuantity = p.InventoryQuantity,
          Status = p.Status,
          Price = p.Price,
          CategoryId = p.Category.Id,
          CategoryName = p.Category.Name
      })
      .ToListAsync();

            var totalItems = products.Count;

            var paginatedProducts = products.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagination = new Pagination<ProductDTO>
            {
                Items = paginatedProducts,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = totalItems,

            };

            return pagination;
        }

        public async Task<Pagination<ProductDTO>> GetProductsByCategoryIDAndNameAsync(String categoryId, string productName, int pageIndex,int pageSize)
        {

            var products = await _dbContext.Products
      .Include(p => p.Category)
      .Where(p => p.CategoryId.ToString() == categoryId && p.Name.ToLower().Contains(productName.ToLower()))
      .Select(p => new ProductDTO
      {
          Id = p.Id,
          Name = p.Name,
          Description = p.Description,
          Image = p.Image,
          InventoryQuantity = p.InventoryQuantity,
          Status = p.Status,
          Price = p.Price,
          CategoryId = p.Category.Id,
          CategoryName = p.Category.Name
      })
      .ToListAsync();

            var totalItems = products.Count;

            var paginatedProducts = products.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagination = new Pagination<ProductDTO>
            {
                Items = paginatedProducts,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = totalItems,

            };

            return pagination;
        }

        public async Task<Pagination<ProductDTO>> ToPaginationProduct(int pageIndex = 0, int pageSize = 10)
        {

            var query = (from product in _dbContext.Products
                         join category in _dbContext.Categories
                         on product.CategoryId equals category.Id

                         select new ProductDTO
                         {
                             Id = product.Id,
                             Name = product.Name,
                             Description = product.Description,
                             Image = product.Image,
                             InventoryQuantity = product.InventoryQuantity,
                             Status = product.Status,
                             Price = product.Price,
                             CategoryId = category.Id,
                             CategoryName = category.Name
                         });

            var totalItemsCount = await query.CountAsync();


            var products = await query
                .OrderByDescending(p => p.InventoryQuantity)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();


            var pagination = new Pagination<ProductDTO>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = totalItemsCount,
                Items = products
            };

            return pagination;
        }
        public async Task<Pagination<ProductDTO>> ToPaginationProductNotDeleted(int pageIndex = 0, int pageSize = 10)
        {

            var query = (from product in _dbContext.Products
                         join category in _dbContext.Categories
                         on product.CategoryId equals category.Id
                         where product.IsDeleted == false // san pham chua co xoa
                         select new ProductDTO
                         {
                             Id = product.Id,
                             Name = product.Name,
                             Description = product.Description,
                             Image = product.Image,
                             InventoryQuantity = product.InventoryQuantity,
                             Status = product.Status,
                             Price = product.Price,
                             CategoryId = category.Id,
                             CategoryName = category.Name
                         });

            var totalItemsCount = await query.CountAsync();


            var products = await query
                .OrderByDescending(p => p.InventoryQuantity)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();


            var pagination = new Pagination<ProductDTO>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = totalItemsCount,
                Items = products
            };

            return pagination;
        }



        public async void IncreaseQuantityProductFromImport(ICollection<ImportDetail> importDetails)
        {
            List<Product> products = new List<Product>();
            foreach (var importDetail in importDetails)
            {
                var product = await _dbContext.Products.FirstAsync(x => x.Id == importDetail.ProductId);
                product.InventoryQuantity += importDetail.Quantity;
                product.ModificationDate = _timeService.GetCurrentTime();
                product.ModificationBy = _claimsService.GetCurrentUserId;
                products.Add(product);
            }
            _dbSet.UpdateRange(products);
        }
        public async Task<int> GetQuantityByIdAsync(Guid productId) => await _dbContext.Products.Where(x => x.Id == productId)
                                                                                                .Select(x => x.InventoryQuantity)
                                                                                                .FirstAsync();
//---------------------------------------------filter--------------------------------------------------------
        public async Task<Pagination<ProductDTO>> GetProductsByCategoryIDAndMaxPriceAsync(string categoryID, double value, int page, int pageSize)
        {
            var products = await _dbContext.Products
    .Include(p => p.Category)
    .Where(p => p.CategoryId.ToString() == categoryID && p.Price <= value)
    .Select(p => new ProductDTO
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Image = p.Image,
        InventoryQuantity = p.InventoryQuantity,
        Status = p.Status,
        Price = p.Price,
        CategoryId = p.Category.Id,
        CategoryName = p.Category.Name
    })
    .ToListAsync();

            var totalItems = products.Count;

            var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagination = new Pagination<ProductDTO>
            {
                Items = paginatedProducts,
                PageIndex = page,
                PageSize = pageSize,
                TotalItemsCount = totalItems,

            };

            return pagination;
        }

        public async Task<Pagination<ProductDTO>> GetProductsByCategoryIDAndMinPriceAsync(string categoryID, double value, int page, int pageSize)
        {
            var products = await _dbContext.Products
         .Where(p => p.CategoryId.ToString() == categoryID && p.Price >= value)
         .OrderBy(p => p.Name) 
         .Skip((page - 1) * pageSize)
         .Take(pageSize)
         .Select(p => new ProductDTO
         {
             Id = p.Id,
             Name = p.Name,
             Description = p.Description,
             Image = p.Image,
             InventoryQuantity = p.InventoryQuantity,
             Status = p.Status,
             Price = p.Price,
             CategoryId = p.Category.Id,
             CategoryName = p.Category.Name
         })
         .ToListAsync();

         
            var totalCount = await _dbContext.Products
                .CountAsync(p => p.CategoryId.ToString() == categoryID && p.Price >= value);

            var pagination = new Pagination<ProductDTO>
            {
                PageIndex = page,
                PageSize = pageSize,
                TotalItemsCount = totalCount,
                Items = products
            };

            return pagination;
        }

        public async Task<Pagination<ProductDTO>> GetProductsByCategoryIDAndPriceRangeAsync(string categoryID, double value1, double value2, int page, int pageSize)
        {
            var products = await _dbContext.Products
         .Where(p => p.CategoryId.ToString() == categoryID && p.Price >= value1 && p.Price <= value2)
         .OrderBy(p => p.Name) 
         .Skip((page - 1) * pageSize)
         .Take(pageSize)
         .Select(p => new ProductDTO
         {
             Id = p.Id,
             Name = p.Name,
             Description = p.Description,
             Image = p.Image,
             InventoryQuantity = p.InventoryQuantity,
             Status = p.Status,
             Price = p.Price,
             CategoryId = p.Category.Id,
             CategoryName = p.Category.Name
         })
         .ToListAsync();

     
            var totalCount = await _dbContext.Products
                .CountAsync(p => p.CategoryId.ToString() == categoryID && p.Price >= value1 && p.Price <= value2);

            var pagination = new Pagination<ProductDTO>
            {
                PageIndex = page,
                PageSize = pageSize,
                TotalItemsCount = totalCount,
                Items = products
            };

            return pagination;
        }

        public async Task<Pagination<ProductDTO>> GetProductsByNameAndMinPriceAsync(string productName, double value, int page, int pageSize)
        {
            var products = await _dbContext.Products
        .Where(p => p.Name.Contains(productName) && p.Price >= value && p.Price >= value)
        .OrderBy(p => p.Name) 
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(p => new ProductDTO
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Image = p.Image,
            InventoryQuantity = p.InventoryQuantity,
            Status = p.Status,
            Price = p.Price,
            CategoryId = p.Category.Id,
            CategoryName = p.Category.Name
        })
        .ToListAsync();

            
            var totalCount = await _dbContext.Products
                .CountAsync(p => p.Name.Contains(productName) && p.Price >= value);

            
            var pagination = new Pagination<ProductDTO>
            {
                PageIndex = page,
                PageSize = pageSize,
                TotalItemsCount = totalCount,
                Items = products
            };

            return pagination;
        }

        public async Task<Pagination<ProductDTO>> GetProductsByNameAndMaxPriceAsync(string productName, double value, int page, int pageSize)
        {
            var products = await _dbContext.Products
         .Where(p => p.Name.Contains(productName) && p.Price >= value && p.Price <= value)
         .OrderBy(p => p.Name)
         .Skip((page - 1) * pageSize)
         .Take(pageSize)
         .Select(p => new ProductDTO
         {
             Id = p.Id,
             Name = p.Name,
             Description = p.Description,
             Image = p.Image,
             InventoryQuantity = p.InventoryQuantity,
             Status = p.Status,
             Price = p.Price,
             CategoryId = p.Category.Id,
             CategoryName = p.Category.Name
         })
         .ToListAsync();


            var totalCount = await _dbContext.Products
                .CountAsync(p => p.Name.Contains(productName) && p.Price >= value && p.Price <= value);


            var pagination = new Pagination<ProductDTO>
            {
                PageIndex = page,
                PageSize = pageSize,
                TotalItemsCount = totalCount,
                Items = products
            };

            return pagination;
        }

        public async Task<Pagination<ProductDTO>> GetProductsByNameAndPriceRangeAsync(string productName, double value1, double value2, int page, int pageSize)
        {
            var products = await _dbContext.Products
        .Where(p => p.Name.Contains(productName) && p.Price >= value1 && p.Price<=value2)
        .OrderBy(p => p.Name)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(p => new ProductDTO
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Image = p.Image,
            InventoryQuantity = p.InventoryQuantity,
            Status = p.Status,
            Price = p.Price,
            CategoryId = p.Category.Id,
            CategoryName = p.Category.Name
        })
        .ToListAsync();


            var totalCount = await _dbContext.Products
                .CountAsync(p => p.Name.Contains(productName) && p.Price >= value1 && p.Price <= value2);


            var pagination = new Pagination<ProductDTO>
            {
                PageIndex = page,
                PageSize = pageSize,
                TotalItemsCount = totalCount,
                Items = products
            };

            return pagination;
        }

        public async Task<Pagination<ProductDTO>> GetProductsByMinPriceAsync(double value, int page, int pageSize)
        {
            var products = await _dbContext.Products
        .Where(p=> p.Price >= value)
        .OrderBy(p => p.Name)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(p => new ProductDTO
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Image = p.Image,
            InventoryQuantity = p.InventoryQuantity,
            Status = p.Status,
            Price = p.Price,
            CategoryId = p.Category.Id,
            CategoryName = p.Category.Name
        })
        .ToListAsync();


            var totalCount = await _dbContext.Products
                .CountAsync(p=> p.Price>=value);


            var pagination = new Pagination<ProductDTO>
            {
                PageIndex = page,
                PageSize = pageSize,
                TotalItemsCount = totalCount,
                Items = products
            };

            return pagination;
        }

        public async Task<Pagination<ProductDTO>> GetProductsByMaxPriceAsync(double value, int page, int pageSize)
        {
            var products = await _dbContext.Products
      .Where(p => p.Price <= value)
      .OrderBy(p => p.Name)
      .Skip((page - 1) * pageSize)
      .Take(pageSize)
      .Select(p => new ProductDTO
      {
          Id = p.Id,
          Name = p.Name,
          Description = p.Description,
          Image = p.Image,
          InventoryQuantity = p.InventoryQuantity,
          Status = p.Status,
          Price = p.Price,
          CategoryId = p.Category.Id,
          CategoryName = p.Category.Name
      })
      .ToListAsync();


            var totalCount = await _dbContext.Products
                .CountAsync(p => p.Price <= value);


            var pagination = new Pagination<ProductDTO>
            {
                PageIndex = page,
                PageSize = pageSize,
                TotalItemsCount = totalCount,
                Items = products
            };

            return pagination;
        }

        public async Task<Pagination<ProductDTO>> GetProductsByCategoryIdAndNameAndMinPriceAsync(string categoryID, string productName, double? minPrice, int page, int pageSize)
        {
            var products = await _dbContext.Products
     .Where(p => p.CategoryId.ToString() == categoryID && p.Name.Contains(productName) && (minPrice == null || p.Price >= minPrice))
     .OrderBy(p => p.Name)
     .Skip((page - 1) * pageSize)
     .Take(pageSize)
     .Select(p => new ProductDTO
     {
         Id = p.Id,
         Name = p.Name,
         Description = p.Description,
         Image = p.Image,
         InventoryQuantity = p.InventoryQuantity,
         Status = p.Status,
         Price = p.Price,
         CategoryId = p.Category.Id,
         CategoryName = p.Category.Name
     })
     .ToListAsync();


            var totalCount = await _dbContext.Products
                .CountAsync(p => p.CategoryId.ToString() == categoryID && p.Name.Contains(productName) && (minPrice == null || p.Price >= minPrice));


            var pagination = new Pagination<ProductDTO>
            {
                PageIndex = page,
                PageSize = pageSize,
                TotalItemsCount = totalCount,
                Items = products
            };

            return pagination;
        }

        public async Task<Pagination<ProductDTO>> GetProductsByCategoryIdAndNameAndMaxPriceAsync(string categoryID, string productName, double? maxPrice, int page, int pageSize)
        {

            var products = await _dbContext.Products
                .Where(p => p.CategoryId.ToString() == categoryID && p.Name.Contains(productName) && (maxPrice == null || p.Price <= maxPrice))
                .OrderBy(p => p.Name)
     .Skip((page - 1) * pageSize)
     .Take(pageSize)
     .Select(p => new ProductDTO
     {
         Id = p.Id,
         Name = p.Name,
         Description = p.Description,
         Image = p.Image,
         InventoryQuantity = p.InventoryQuantity,
         Status = p.Status,
         Price = p.Price,
         CategoryId = p.Category.Id,
         CategoryName = p.Category.Name
     })
     .ToListAsync();


            var totalCount = await _dbContext.Products
                .CountAsync(p => p.CategoryId.ToString() == categoryID && p.Name.Contains(productName) && (maxPrice == null || p.Price >= maxPrice));


            var pagination = new Pagination<ProductDTO>
            {
                PageIndex = page,
                PageSize = pageSize,
                TotalItemsCount = totalCount,
                Items = products
            };

            return pagination;
        }
    

        public async Task<Pagination<ProductDTO>> GetProductsByCategoryIdAndNameAndMinAndMaxPriceAsync(string categoryID, string productName, double? minPrice, double? maxPrice, int page, int pageSize)
        {
            var products = await _dbContext.Products
                 .Where(p => p.CategoryId.ToString() == categoryID && p.Name.Contains(productName) &&
            ((minPrice == null || p.Price >= minPrice) && (maxPrice == null || p.Price <= maxPrice)))
                 .OrderBy(p => p.Name)
      .Skip((page - 1) * pageSize)
      .Take(pageSize)
      .Select(p => new ProductDTO
      {
          Id = p.Id,
          Name = p.Name,
          Description = p.Description,
          Image = p.Image,
          InventoryQuantity = p.InventoryQuantity,
          Status = p.Status,
          Price = p.Price,
          CategoryId = p.Category.Id,
          CategoryName = p.Category.Name
      })
      .ToListAsync();


            var totalCount = await _dbContext.Products
                .CountAsync(p => p.CategoryId.ToString() == categoryID && p.Name.Contains(productName) &&
            ((minPrice == null || p.Price >= minPrice) && (maxPrice == null || p.Price <= maxPrice)));
                


            var pagination = new Pagination<ProductDTO>
            {
                PageIndex = page,
                PageSize = pageSize,
                TotalItemsCount = totalCount,
                Items = products
            };

            return pagination;
        }
    }
}
