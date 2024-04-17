using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class Feedback : BaseEntity
    {
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public string? Details { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

    }
}
