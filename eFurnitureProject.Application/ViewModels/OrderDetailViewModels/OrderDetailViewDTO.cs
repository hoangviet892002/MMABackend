using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eFurnitureProject.Application.ViewModels.ProductDTO;

namespace eFurnitureProject.Application.ViewModels.OrderDetailViewModels
{
    public class OrderDetailViewDTO
    {
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public ProductViewDTO? Product { get; set; }
    }
}
