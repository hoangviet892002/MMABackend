using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.OrderViewModels
{
    public class OrderViewDTO : BaseEntity
    {
        public double Price { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Name { get; set; }
        public Guid? StatusId { get; set; }
        public StatusOrder? StatusOrder { get; set; }
        public string? UserId { get; set; }

    }
}