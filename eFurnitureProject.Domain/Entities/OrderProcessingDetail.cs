using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class OrderProcessingDetail
    {
        public string? Detail { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        [Key, Column(Order = 1)]
        public Guid OrderProcessingId { get; set; }
        [ForeignKey("OrderProcessingId")]
        public OrderProcessing? OrderProcessing { get; set; }
        [Key, Column(Order = 2)]
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

    }
}
