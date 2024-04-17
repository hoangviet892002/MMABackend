using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.AppointmentViewModel
{
    public class FilterAppointmentDTO
    {
        
        public string? search{  get; set; }
         public int pageIndex { get; set; } = 0;
        public int pageSize { get; set; } = 10;
    }
}
