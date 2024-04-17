using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class Import : BaseEntity
    {
        public string Name { get; set; }
        public double TotalPrice { get; set; }
        public int Status { get; set; } = 1;

        public ICollection<ImportDetail>? ImportDetail { get; set; }
    }
}
