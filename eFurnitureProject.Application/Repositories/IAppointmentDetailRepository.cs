using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface IAppointmentDetailRepository 
    {
        Task AddAsync(AppointmentDetail appointmentDetail);
        Task DeleteByAppointmentIdAsync(Guid appointmentId);
        Task UpdateAsync(AppointmentDetail appointmentDetail);
        Task<List<AppointmentDetail>> GetByAppointmentIdAsync(Guid appointmentId);

        Task<List<AppointmentDetail>> GetByAppointmentByStaffIdAsync(DateTime date, string staffId);
       
    }
}
