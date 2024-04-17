using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class OrderDetailController : BaseController
    {
        private readonly IOrderDetailService _orderDetailService;
        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }
        [Authorize]
        [HttpGet]
        public async Task<ApiResponse<IEnumerable<OrderDetailViewDTO>>> GetOrderDetailsByIdAsync(Guid orderId) =>
            await _orderDetailService.GetOrderDetailsByIdAsync(orderId);
            
    }
}
