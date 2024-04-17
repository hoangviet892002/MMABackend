using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel
{
    public class AppoitmentDetailViewDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int Status { get; set; }
        public string? Time { get; set; }
        public string? CustomerName { get; set; }
        public List<string>? StaffName { get; set; }
    }
}
