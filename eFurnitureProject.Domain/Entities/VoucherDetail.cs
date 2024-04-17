using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class VoucherDetail
    {
        [Key, Column(Order = 1)]
        public Guid VoucherId { get; set; }
        [ForeignKey("VoucherId")]
        public Voucher? Voucher { get; set; }
        [Key, Column(Order = 2)]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public int Status { get; set; }

    }
}
