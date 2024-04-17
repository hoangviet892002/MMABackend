using eFurnitureProject.Application.ViewModels.ImportDetailViewModels;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.ImportViewModels
{
    public class CreateImportDTO
    {
        public string Name { get; set; }
        public ICollection<CreateImportDetailDTO> ImportDetail { get; set; }
    }
}
