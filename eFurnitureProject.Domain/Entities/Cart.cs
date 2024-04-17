using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public string UserId { get; set; }
        [ForeignKey("UserId")]

        public User? User { get; set; }

        public virtual ICollection<CartDetail>? CartDetails { get; set; }
    }
}
