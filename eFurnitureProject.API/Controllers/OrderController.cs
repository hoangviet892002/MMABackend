using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;
using eFurnitureProject.Application.ViewModels.OrderViewModels;
using eFurnitureProject.Application.ViewModels.StatusOrderViewModels;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }
        [Authorize(Roles = AppRole.Admin+","+AppRole.Staff+","+AppRole.DeliveryStaff)]
        [HttpGet]
        public async Task<ApiResponse<Pagination<OrderViewDTO>>> GetOrderByFilter ([FromQuery]FilterOrderDTO filterOrderDTO) =>
            await _service.GetOrderFilterAsync(filterOrderDTO);
        [Authorize(Roles = AppRole.Customer)]
        [HttpGet]
        public async Task<ApiResponse<Pagination<OrderViewForCustomerDTO>>> GetOrderFilterByLogin([FromQuery]FilterOrderByLoginDTO filterOrderByLoginDTO) =>
            await _service.GetOrderFilterByLoginAsync(filterOrderByLoginDTO);
        [Authorize]
        [HttpPut]
        public async Task<ApiResponse<string>> UpdateOrderStatus([FromBody]UpdateOrderStatusDTO updateOrderStatusDTO) =>
            await _service.UpdateOrderStatusAsync(updateOrderStatusDTO);
        [Authorize]
        [HttpGet]
        public async Task<ApiResponse<OrderViewDTO>> GetOrderById(Guid orderId) =>
            await _service.GetOrderByIdAsync(orderId);
        [Authorize]
        [HttpGet]
        public async Task<ApiResponse<StatusDetailOrderViewDTO>> GetStatusByOrderId(Guid orderId) =>
            await _service.GetOrderStatusByOrderId(orderId);
        [Authorize(Roles = AppRole.Customer)]
        [HttpPost]
        public async Task<ApiResponse<string>> CheckOut(CreateOrderDTO createOrderDTO) =>
            await _service.CheckOut(createOrderDTO);
    }
}
