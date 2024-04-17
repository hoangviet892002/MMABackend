using eFurnitureProject.Application.ViewModels.StatusOrderViewModels;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.OrderViewModels
{
    public class OrderViewForCustomerDTO
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public double Price { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Name { get; set; }
        public StatusOrderViewDTO? StatusOrderViewDTO { get; set; }
    }
}
