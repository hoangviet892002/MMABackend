using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class AppointmentDetailRepository : IAppointmentDetailRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        public AppointmentDetailRepository(AppDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public async Task AddAsync(AppointmentDetail appointmentDetail)
        {
            await _dbContext.AppointmentDetails.AddAsync(appointmentDetail);
        }
        public async Task DeleteByAppointmentIdAsync(Guid appointmentId)
        {


            var appointmentDetails = await _dbContext.AppointmentDetails
                .Where(ad => ad.AppointmentId == appointmentId) // Lọc theo AppointmentId
                .Join(_dbContext.UserRoles // Tham gia với bảng UserRoles
                    .Join(_dbContext.Roles // Tham gia với bảng Roles
                        .Where(r => r.Name == "Staff"), // Lọc các Roles có tên là "Staff"
                        ur => ur.RoleId,
                        r => r.Id,
                        (ur, r) => ur),
                    ad => ad.UserId,
                    ur => ur.UserId,
                    (ad, ur) => ad) 
                .ToListAsync();

            foreach (var ad in appointmentDetails)
            {
                ad.IsDeleted = true; // Gán IsDeleted = true cho các AppointmentDetail phù hợp
            }

            await _dbContext.SaveChangesAsync();

        }
        public async Task UpdateAsync(AppointmentDetail appointmentDetail)
        {
            _dbContext.Entry(appointmentDetail).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<AppointmentDetail>> GetByAppointmentIdAsync(Guid appointmentId)
        {
            return await _dbContext.AppointmentDetails
                .Where(ad => ad.AppointmentId == appointmentId)
                .ToListAsync();
        }
        public async Task<List<AppointmentDetail>> GetByAppointmentByStaffIdAsync(DateTime date,  string staffId)
        {
            return await _dbContext.AppointmentDetails
                .Where(ad => ad.UserId == staffId && ad.IsDeleted==false && ad.Appointment.Date.Date==date.Date)
                .ToListAsync();
        }
    }
}
