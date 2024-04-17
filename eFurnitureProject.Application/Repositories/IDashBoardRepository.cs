using eFurnitureProject.Application.ViewModels.DashBoardViewModel;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface IDashBoardRepository 
    {
        List<Top5UserDTO> GetTop5UsersBySpending();
        List<ProductTopDTO> GetTop5ProductsBySales();
        Task<List<ProductTop5DTO>> GetTotalProducts();
        Task<int> GetTotalFinishedOrders();
        Task<int> GetTotalUsers();
        Task<int> GetTotalProcessOrders();
        Task<double> GetTotalMoney();
        Task<List<ProductDTO>> GetTop5BestSellingProductsAsync();
    }
}
