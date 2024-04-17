using eFurnitureProject.Application.ViewModels.OrderProcessingDetailViewModels;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.OrderProcessingViewModels
{
    public class CreateOrderProcessingDTO
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public virtual ICollection<CreateOrderProcessingDetailDTO> Items { get; set; }

    }
}
