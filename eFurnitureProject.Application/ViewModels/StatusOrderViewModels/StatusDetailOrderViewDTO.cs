using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.StatusOrderViewModels
{
    public class StatusDetailOrderViewDTO : BaseEntity
    {
        public string Name { get; set; }
        public int StatusCode { get; set; }
    }
}
