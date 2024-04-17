using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class StatusOrder : BaseEntity
    {
        public string Name { get; set; }
        public int StatusCode { get; set; }
    }
}
