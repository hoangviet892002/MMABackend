using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel
{
    public class AppointmentDetailDTO
    {
        public Guid AppointmentId { get; set; }
        public string? UserId { get; set; }
    }
}
