using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class ImportDetail
    {
        public double Price { get; set; }
        public int Quantity { get; set; }
        [Key, Column(Order = 1)]
        public Guid ImportId { get; set; }
        [ForeignKey("ImportId")]
        public Import? Import { get; set; }
        [Key, Column(Order = 2)]
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

    }
}
