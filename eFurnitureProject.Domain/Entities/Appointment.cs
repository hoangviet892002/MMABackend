using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        
        public string Name { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string? Email { get; set; }
        public int Status { get; set; }

        public string? Time { get; set; }


        public virtual ICollection<AppointmentDetail>? AppointmentDetail { get; set; }
    }
}
