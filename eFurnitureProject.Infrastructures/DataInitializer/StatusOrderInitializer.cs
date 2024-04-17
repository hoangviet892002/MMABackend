using eFurnitureProject.Application;
using eFurnitureProject.Application.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.DataInitializer
{
    public class StatusOrderInitializer
    {
        private readonly IUnitOfWork _unitOfWork;
        public StatusOrderInitializer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task StatusOrderInitializerAsync()
        {
            string[] statusOrders = { "Pending", "Delivering", "Cancelled", "Delivered", "Rejected" };

            foreach (var statusOrder in statusOrders)
            {
                var statusOrdersExisted = await _unitOfWork.StatusOrderRepository.CheckStatusOrderExisted(statusOrder);

                if (!statusOrdersExisted)
                {
                    int statusCode = GetStatusCode(statusOrder); 
                    await _unitOfWork.StatusOrderRepository.AddAsync(new Domain.Entities.StatusOrder { Name = statusOrder, StatusCode = statusCode });
                }
            }

            await _unitOfWork.SaveChangeAsync();
        }

        private int GetStatusCode(string statusOrder)
        {
            return statusOrder switch
            {
                "Pending" => 1,
                "Delivering" => 2,
                "Cancelled" => 3,
                "Delivered" => 4,
                "Rejected" => 5,
                _ => 0,
            };
        }
    }
}
