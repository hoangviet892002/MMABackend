using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class Contract : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Value { get; set; }
        public int Status { get; set; } = 1;
        public int Pay { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public User? Customer { get; set; }
        public Guid? OrderProcessingId { get; set; }
        [ForeignKey("OrderProcessingId")]
        public OrderProcessing? OrderProcessing { get; set; }
    }
}
