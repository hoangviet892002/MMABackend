using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class OrderProcessing : BaseEntity
    {
        public int Price { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Name { get; set; }
        public Guid StatusOrderProcessingId { get; set; }
        [ForeignKey("StatusOrderProcessingId")]
        public StatusOrderProcessing? StatusOrderProcessing { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public Contract? Contract { get; set; }
        public virtual ICollection<OrderProcessingDetail>? OrderProcessingDetail { get; set; }
    }
}
