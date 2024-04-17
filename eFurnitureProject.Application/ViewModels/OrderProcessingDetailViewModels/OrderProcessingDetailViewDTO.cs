using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.OrderProcessingDetailViewModels
{
    public class OrderProcessingDetailViewDTO
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string? Image { get; set; }
        public int Quantity { get; set; }
    }
}
