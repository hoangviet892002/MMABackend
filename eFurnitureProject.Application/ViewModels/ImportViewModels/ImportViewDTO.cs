using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.ImportViewModels
{
    public class ImportViewDTO
    {
        public String Id { get; set; }
        public string Name { get; set; }
        public double TotalPrice { get; set; }
        public int Status { get; set; }
    }
}
