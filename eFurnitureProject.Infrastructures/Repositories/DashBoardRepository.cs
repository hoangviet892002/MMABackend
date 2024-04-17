using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.DashBoardViewModel;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;
using eFurnitureProject.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class DashBoardRepository : IDashBoardRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        public DashBoardRepository(AppDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }


        public List<Top5UserDTO> GetTop5UsersBySpending()
        {
            var top5Users = _dbContext.Orders
                .Where(o => o.StatusOrder.StatusCode == (int)OrderStatusEnum.Delivered) // Chỉ tính toán cho các đơn hàng đã được giao hàng
                .GroupBy(o => o.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    TotalSpending = g.Sum(o => o.Price)
                })
                .OrderByDescending(g => g.TotalSpending)
                .Take(5)
                .Join(_dbContext.Users,
                    result => result.UserId,
                    user => user.Id,
                    (result, user) => new Top5UserDTO
                    {
                        UserName = user.UserName,
                        UserEmail= user.Email,
                        PhoneNumber=user.PhoneNumber,
                        TotalMoney = result.TotalSpending
                    })
                .ToList();

            return top5Users;
        }

        public List<ProductTopDTO> GetTop5ProductsBySales()
        {
            var top5Products = _dbContext.OrdersDetails
                .GroupBy(od => od.ProductId)
                .Select(g => new ProductTopDTO
                {
                    Id = g.Key,
                    Quantitysold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(g => g.Quantitysold)
                .Take(5)
                .Join(_dbContext.Products,
                    result => result.Id,
                    product => product.Id,
                    (result, product) => new ProductTopDTO
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Image = product.Image,
                        InventoryQuantity = product.InventoryQuantity,
                        Status = product.Status,
                        CategoryId = product.CategoryId,
                        CategoryName = product.Category.Name,
                        Quantitysold = result.Quantitysold
                    })
                .ToList();

            return top5Products;
        }
        public async Task<List<ProductTop5DTO>> GetTotalProducts()
        {
            var totalProducts = await _dbContext.OrdersDetails
                .GroupBy(od => od.ProductId)
                .Select(g => new ProductTop5DTO
                {
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .ToListAsync();

            return totalProducts;
        }
        public Task<int> GetTotalFinishedOrders()
        {
            var totalFinishedOrders = _dbContext.Orders.Include(a=>a.StatusOrder)
                .CountAsync(o => o.StatusId == o.StatusOrder.Id && o.StatusOrder.StatusCode== 4); 
            return totalFinishedOrders;
        }
        public async Task<int> GetTotalUsers()
        {
            return await _dbContext.Users.CountAsync();
        }
        public async Task<int> GetTotalProcessOrders()
        {
            var processingStatuses = new List<int>
        {
            (int)OrderStatusEnum.Pending,
            (int)OrderStatusEnum.Delivering
        };

            return await _dbContext.Orders
                .Where(o => processingStatuses.Contains(o.StatusOrder.StatusCode))
                .CountAsync();
        }

        public async Task<double> GetTotalMoney()
        {
            var totalMoneyOrder = await _dbContext.Orders
                .Where(o => o.StatusOrder.StatusCode == (int)OrderStatusEnum.Delivered)
                .SumAsync(o => o.Price);

            var totalMoneyOrderProcessing = await _dbContext.OrderProcessings
                .Where(o => o.StatusOrderProcessing.StatusCode == (int)OrderStatusEnum.Delivered)
                .SumAsync(o => o.Price);

            return totalMoneyOrder + totalMoneyOrderProcessing;
        }
        public async Task<List<ProductDTO>> GetTop5BestSellingProductsAsync()
        {
            var top5BestSellingProducts = await _dbContext.Products
                .OrderByDescending(p => p.InventoryQuantity) // Sắp xếp theo số lượng bán được giảm dần
                .Take(5) // Lấy 5 sản phẩm đầu tiên
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Image = p.Image,
                    InventoryQuantity = p.InventoryQuantity,
                    Status = p.Status,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name // Lấy tên của danh mục sản phẩm
                })
                .ToListAsync();

            return top5BestSellingProducts;
        }
    }
  
}

