using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.DashBoardViewModel;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IDashBoardService 
    {
        Task<ApiResponse<List<Top5UserDTO>>> Top5User();
        Task<ApiResponse<List<ProductTopDTO>>> Top5Product();
        Task<ApiResponse<List<ProductTop5DTO>>> GetTotalProducts();
        Task<ApiResponse<TotalFinishedOrderDTO>> GetTotalFinishedOrders();
        Task<ApiResponse<TotalUserDTO>> GetTotalUsers();
        Task<ApiResponse<TotalProcessOderDTO>> GetTotalProcessOder();
        Task<ApiResponse<TotalMoneyDTO>> GetTotalMoney();
    }
}
