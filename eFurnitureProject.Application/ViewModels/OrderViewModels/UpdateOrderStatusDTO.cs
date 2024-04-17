using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.OrderViewModels
{
    public class UpdateOrderStatusDTO
    {
        public Guid Id { get; set; }
        public int StatusCode { get; set; }
    }
}
