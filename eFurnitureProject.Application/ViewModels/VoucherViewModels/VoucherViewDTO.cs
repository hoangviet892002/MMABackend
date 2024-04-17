using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.VoucherDTO
{
    public class VoucherViewDTO
    {
        public Guid Id { get; set; }
        public string? VoucherName { get; set; }
       // public string? VoucherCode {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Percent { get; set; }
        public int Number { get; set; }

        public double MinimumOrderValue { get; set; } // giá trị đơn hàng tối thiểu
        public double MaximumDiscountAmount { get; set; } // Giá trị tối đa được giảm
    }
}
