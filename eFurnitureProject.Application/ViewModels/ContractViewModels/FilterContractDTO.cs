using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.ContractViewModels
{
    public class FilterContractDTO
    {
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public string? Search { get; set; }
        public int? StatusCode { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;
    }
}
