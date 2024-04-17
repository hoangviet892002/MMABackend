using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.TransactionViewModels
{
    public class FilterTransactionByLoginDTO
    {
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
