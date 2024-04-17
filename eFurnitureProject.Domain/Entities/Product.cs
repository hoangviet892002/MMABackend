using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name {get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int InventoryQuantity { get; set; }
        public int Status { get; set; }
        public double Price {  get; set; }
        [Column(Order = 2)]
        public Guid? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public virtual ICollection<CartDetail>? CartDetail { get; set; }
        public virtual ICollection<ImportDetail>? ImportDetail { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetail { get; set; }
        public virtual ICollection<OrderProcessingDetail>? OrderProcessingDetail { get; set; }

    }
}
