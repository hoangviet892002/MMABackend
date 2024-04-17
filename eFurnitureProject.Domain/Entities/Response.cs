using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class Response :BaseEntity

    {
        public Guid FeedbackId { get; set; }
        public string StaffId { get; set; }
        public string Details { get; set; }
        
        [ForeignKey("FeedbackId")]
        public Feedback Feedback { get; set; }

        [ForeignKey("StaffId")]
        public User Staff { get; set; }

    }
}
