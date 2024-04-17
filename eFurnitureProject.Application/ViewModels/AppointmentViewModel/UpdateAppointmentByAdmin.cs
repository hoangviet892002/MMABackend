using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.AppointmentViewModel
{
    public class UpdateAppointmentByAdmin
    { 
        public int Status { get; set; }
        public int Time { get; set; }
        public List<String> UserID { get; set; }
    }
}
