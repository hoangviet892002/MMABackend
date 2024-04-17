using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.ContractViewModels
{
    public class UpdateContractDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Value { get; set; }
        public int Status { get; set; }
    }
}
