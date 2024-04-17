using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;
using eFurnitureProject.Application.ViewModels.OrderViewModels;
using eFurnitureProject.Application.ViewModels.StatusOrderViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IOrderService
    {
        Task<ApiResponse<OrderViewDTO>> GetOrderByIdAsync(Guid orderId);
        Task<ApiResponse<Pagination<OrderViewForCustomerDTO>>> GetOrderFilterByLoginAsync(FilterOrderByLoginDTO filterOrderByLogin);
        Task<ApiResponse<Pagination<OrderViewDTO>>> GetOrderFilterAsync(FilterOrderDTO filterOrderDTO);
        Task<ApiResponse<string>> UpdateOrderStatusAsync(UpdateOrderStatusDTO updateOrderStatusDTO);
        Task<ApiResponse<StatusDetailOrderViewDTO>> GetOrderStatusByOrderId(Guid orderId);
        Task<ApiResponse<string>> CheckOut(CreateOrderDTO createOrderDTO);
    }
}
