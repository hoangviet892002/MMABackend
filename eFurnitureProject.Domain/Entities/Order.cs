using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class Order : BaseEntity
    {
        public double Price { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Name { get; set; }
        public Guid? StatusId { get; set; }
        [ForeignKey("StatusId")]
        public StatusOrder? StatusOrder { get; set; }
        public Guid? VoucherId { get; set; }
        public Voucher? Voucher { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        
        public virtual ICollection<OrderDetail>? OrderDetail { get; set; }
    }
}
