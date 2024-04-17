using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.DashBoardViewModel;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class DashBoardController : BaseController
    {
        private readonly IDashBoardService _dashBoardService;

        public DashBoardController(IDashBoardService dashBoardService)
        {
            _dashBoardService = dashBoardService;
        }
        [HttpGet]
        public async Task<ApiResponse<TotalMoneyDTO>> GetTotalMoney() => await _dashBoardService.GetTotalMoney();
        [HttpGet]
        public async Task<ApiResponse<TotalProcessOderDTO>> GetTotalProcessOder() => await _dashBoardService.GetTotalProcessOder();
        [HttpGet]
        public async Task<ApiResponse<TotalUserDTO>> GetTotalUsers()=> await _dashBoardService.GetTotalUsers();
        [HttpGet]
        public async Task<ApiResponse<TotalFinishedOrderDTO>> GetTotalFinishedOrders()=> await _dashBoardService.GetTotalFinishedOrders();
        [HttpGet]
        public async Task<ApiResponse<List<ProductTop5DTO>>> GetTotalProducts()=> await _dashBoardService.GetTotalProducts();
        [HttpGet]
        public async Task<ApiResponse<List<ProductTopDTO>>> Top5Product() => await _dashBoardService.Top5Product();
        [HttpGet]
        public async Task<ApiResponse<List<Top5UserDTO>>> Top5User() => await _dashBoardService.Top5User();
    }
}
