using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class AppointmentDetail
    {
        [Column(Order = 1)]
        public Guid AppointmentId { get; set; }
        [ForeignKey("AppointmentId")]
        public Appointment? Appointment { get; set; }
        [Column(Order = 2)]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; } 
        public  bool IsDeleted { get; set; }
    }
}
