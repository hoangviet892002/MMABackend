using eFurnitureProject.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.DataInitializer
{
    public class StatusOrderProcessingInitializer
    {
        private readonly IUnitOfWork _unitOfWork;
        public StatusOrderProcessingInitializer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task StatusOrderProcessingInitializerAsync()
        {
            string[] statusOrderProcessings = { "Pending", "Under Construction", "Complete Construction", "Delivering", "Cancelled", "Delivered", "Rejected" };

            foreach (var statusOrder in statusOrderProcessings)
            {
                var statusOrdersExisted = await _unitOfWork.StatusOrderProcessingRepository.CheckStatusOrderProcessingExisted(statusOrder);

                if (!statusOrdersExisted)
                {
                    int statusCode = GetStatusCode(statusOrder);
                    await _unitOfWork.StatusOrderProcessingRepository.AddAsync(new Domain.Entities.StatusOrderProcessing { Name = statusOrder, StatusCode = statusCode });
                }
            }

            await _unitOfWork.SaveChangeAsync();
        }

        private int GetStatusCode(string statusOrder)
        {
            return statusOrder switch
            {
                "Pending" => 1,
                "Under Construction" => 2,
                "Complete Construction" => 3,
                "Delivering" => 4,
                "Cancelled" => 5,
                "Delivered" => 6,   
                "Rejected" => 7,
                _ => 0,
            };
        }
    }
}
