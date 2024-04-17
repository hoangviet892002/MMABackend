using eFurnitureProject.Application;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        public AppointmentRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService, UserManager<User> userManager) : base(context, timeService, claimsService)
        {
            _dbContext = context;
            _userManager = userManager;
        }

        public async Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentPaging(int pageIndex, int pageSize)
        {
            var userIdsByRole = new Dictionary<string, List<string>>();
            userIdsByRole["Customer"] = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();
            userIdsByRole["Staff"] = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();

            var query = _dbSet
                .Include(a => a.AppointmentDetail)
                .ThenInclude(ad => ad.User)
                .OrderByDescending(appointment => appointment.CreationDate)
                .Select(appointment => new AppoitmentDetailViewDTO
                {
                    Id = appointment.Id,
                    Name = appointment.Name,
                    Date = appointment.Date,
                    PhoneNumber = appointment.PhoneNumber,
                    Email = appointment.Email,
                    Status = appointment.Status,
                    Time = appointment.Time,
                    CustomerName = appointment.AppointmentDetail
            .Where(ad => ad.IsDeleted == false && userIdsByRole["Customer"].Contains(ad.UserId))
            .Select(ad => ad.User.UserName)
            .FirstOrDefault(),
                    StaffName = appointment.AppointmentDetail != null ?
                 appointment.AppointmentDetail
                        .Where(ad => ad.IsDeleted == false && userIdsByRole["Staff"].Contains(ad.UserId))
                        .Select(ad => ad.User != null ? ad.User.Name : null)
                        .ToList()
                 : null
                });

            var itemCount = await query.CountAsync();

            var appointmentDTOs = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new Pagination<AppoitmentDetailViewDTO>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = appointmentDTOs
            };

            return result;
        }



        public async Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentsByDateTimeAsync(int pageIndex , int pageSize ,DateTime date)
        {

            var userIdsByRole = new Dictionary<string, List<string>>();
            userIdsByRole["Customer"] = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();
            userIdsByRole["Staff"] = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();

            var query = _dbSet.Include(a => a.AppointmentDetail).ThenInclude(ad => ad.User)
                              .Where(appointment => appointment.Date.Date == date.Date)
                              .OrderByDescending(appointment => appointment.CreationDate)
                              .Select(appointment => new AppoitmentDetailViewDTO
                              {
                                  Id = appointment.Id,
                                  Name = appointment.Name,
                                  Date = appointment.Date,
                                  PhoneNumber = appointment.PhoneNumber,
                                  Email = appointment.Email,
                                  Status = appointment.Status,
                                  Time = appointment.Time,

                                  CustomerName = appointment.AppointmentDetail
                                                      .Where(ad => ad.IsDeleted == false && userIdsByRole["Customer"].Contains(ad.UserId))
                                                      .Select(ad => ad.User.UserName)
                                                      .FirstOrDefault(),

                                  StaffName = appointment.AppointmentDetail != null ?
                                              appointment.AppointmentDetail
                                                  .Where(ad => ad.IsDeleted == false && userIdsByRole["Staff"].Contains(ad.UserId))
                                                  .Select(ad => ad.User != null ? ad.User.Name : null)
                                                  .ToList()
                                              : null
                              });

            var itemCount = await query.CountAsync();

            var appointmentsDTO = await query.Skip(pageIndex * pageSize)
                                             .Take(pageSize)
                                             .ToListAsync();

            var result = new Pagination<AppoitmentDetailViewDTO>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = appointmentsDTO
            };

            return result;
        
    }

        public async Task<AppoitmentDetailViewDTO> GetAppointmentByIdAsync(Guid Id)
        {
            var userIdsByRole = new Dictionary<string, List<string>>();
            userIdsByRole["Customer"] = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();
            userIdsByRole["Staff"] = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();

            var query = _dbSet
                .Include(a => a.AppointmentDetail)
                .ThenInclude(ad => ad.User).Where(p => p.Id == Id)
                .OrderByDescending(appointment => appointment.CreationDate)
                .Select(appointment => new AppoitmentDetailViewDTO
                {
                    Id = appointment.Id,
                    Name = appointment.Name,
                    Date = appointment.Date,
                    PhoneNumber = appointment.PhoneNumber,
                    Email = appointment.Email,
                    Status = appointment.Status,
                    Time = appointment.Time,
                    CustomerName = appointment.AppointmentDetail
            .Where(ad => ad.IsDeleted == false && userIdsByRole["Customer"].Contains(ad.UserId))
            .Select(ad => ad.User.UserName)
            .FirstOrDefault(),
                    StaffName = appointment.AppointmentDetail != null ?
                 appointment.AppointmentDetail
                        .Where(ad => ad.IsDeleted == false && userIdsByRole["Staff"].Contains(ad.UserId))
                        .Select(ad => ad.User != null ? ad.User.Name : null)
                        .ToList()
                 : null
                });

            var appointmentDTO = await query.FirstOrDefaultAsync();

            return appointmentDTO;
        }




        /*    public async Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentsByNameAsync(int pageIndex, int pageSize, string appointName)
                {
                    var userIdsByRole = new Dictionary<string, List<string>>();
                    userIdsByRole["Customer"] = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();
                    userIdsByRole["Staff"] = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();

                    var query = _dbSet
                        .Include(a => a.AppointmentDetail)
                        .ThenInclude(ad => ad.User)
                        . 
                        Where(p => p.Name.ToLower().Contains(appointName.ToLower()))
                        .OrderByDescending(appointment => appointment.CreationDate)
                        .Select(appointment => new AppoitmentDetailViewDTO
                        {
                            Id = appointment.Id,
                            Name = appointment.Name,
                            Date = appointment.Date,
                            PhoneNumber = appointment.PhoneNumber,
                            Email = appointment.Email,
                            Status = appointment.Status,
                            Time = appointment.Time,
                            CustomerName = appointment.AppointmentDetail
                    .Where(ad => ad.IsDeleted == false && userIdsByRole["Customer"].Contains(ad.UserId))
                    .Select(ad => ad.User.UserName)
                    .FirstOrDefault(),
                            StaffName = appointment.AppointmentDetail != null ?
                         appointment.AppointmentDetail
                                .Where(ad => ad.IsDeleted == false && userIdsByRole["Staff"].Contains(ad.UserId))
                                .Select(ad => ad.User != null ? ad.User.Name : null)
                                .ToList()
                         : null
                        });

                    var itemCount = await query.CountAsync();

                    var appointmentDTOs = await query
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

                    var result = new Pagination<AppoitmentDetailViewDTO>
                    {
                        PageIndex = pageIndex,
                        PageSize = pageSize,
                        TotalItemsCount = itemCount,
                        Items = appointmentDTOs
                    };

                    return result;

                }*/

        public async Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentsByStatusAsync(int pageIndex, int pageSize, int status )
        {
            var userIdsByRole = new Dictionary<string, List<string>>();
            userIdsByRole["Customer"] = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();
            userIdsByRole["Staff"] = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();

            var query = _dbSet
                .Include(a => a.AppointmentDetail)
                .ThenInclude(ad => ad.User)
                .
                Where(p => p.Status==status)
                .OrderByDescending(appointment => appointment.CreationDate)
                .Select(appointment => new AppoitmentDetailViewDTO
                {
                    Id = appointment.Id,
                    Name = appointment.Name,
                    Date = appointment.Date,
                    PhoneNumber = appointment.PhoneNumber,
                    Email = appointment.Email,
                    Status = appointment.Status,
                    Time = appointment.Time,
                    CustomerName = appointment.AppointmentDetail
            .Where(ad => ad.IsDeleted == false && userIdsByRole["Customer"].Contains(ad.UserId))
            .Select(ad => ad.User.UserName)
            .FirstOrDefault(),
                    StaffName = appointment.AppointmentDetail != null ?
                 appointment.AppointmentDetail
                        .Where(ad => ad.IsDeleted == false && userIdsByRole["Staff"].Contains(ad.UserId))
                        .Select(ad => ad.User != null ? ad.User.Name : null)
                        .ToList()
                 : null
                });

            var itemCount = await query.CountAsync();

            var appointmentDTOs = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new Pagination<AppoitmentDetailViewDTO>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = appointmentDTOs
            };

            return result;

        }



        public async Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentsByUserID(int pageIndex, int pageSize, string userID)
        {
            var userIdsByRole = new Dictionary<string, List<string>>();
            userIdsByRole["Customer"] = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();
            userIdsByRole["Staff"] = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();

            var query = _dbSet
                .Include(a => a.AppointmentDetail)
                .ThenInclude(ad => ad.User)
                .Where(a => a.AppointmentDetail.Any(ad => ad.User.Id == userID))
                .OrderByDescending(appointment => appointment.CreationDate)
                .Select(appointment => new AppoitmentDetailViewDTO
                {
                    Id = appointment.Id,
                    Name = appointment.Name,
                    Date = appointment.Date,
                    PhoneNumber = appointment.PhoneNumber,
                    Email = appointment.Email,
                    Status = appointment.Status,
                    Time = appointment.Time,
                    CustomerName = appointment.AppointmentDetail
                        .Where(ad => ad.IsDeleted == false && userIdsByRole["Customer"].Contains(ad.UserId))
                        .Select(ad => ad.User.UserName)
                        .FirstOrDefault(),
                    StaffName = appointment.AppointmentDetail != null ?
                        appointment.AppointmentDetail
                            .Where(ad => ad.IsDeleted == false && userIdsByRole["Staff"].Contains(ad.UserId))
                            .Select(ad => ad.User != null ? ad.User.Name : null)
                            .ToList()
                        : null
                });

            var itemCount = await query.CountAsync();

            var appointmentDTOs = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new Pagination<AppoitmentDetailViewDTO>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = appointmentDTOs
            };

            return result;
        }
        public async Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentByFilter( string search,int  pageIndex, int pageSize)
        {
            var userIdsByRole = new Dictionary<string, List<string>>();
            userIdsByRole["Customer"] = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();
            userIdsByRole["Staff"] = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();

            var query = _dbSet
                .Include(a => a.AppointmentDetail)
                .ThenInclude(ad => ad.User)
                .Where(appointment => (appointment.Email == search ||
                                      appointment.AppointmentDetail.Any(ad => ad.User.UserName.Contains(search))))
                .OrderByDescending(appointment => appointment.CreationDate)
                .Select(appointment => new AppoitmentDetailViewDTO
                {
                    Id = appointment.Id,
                    Name = appointment.Name,
                    Date = appointment.Date,
                    PhoneNumber = appointment.PhoneNumber,
                    Email = appointment.Email,
                    Status = appointment.Status,
                    Time = appointment.Time,
                    CustomerName = appointment.AppointmentDetail
                        .Where(ad => ad.IsDeleted == false && userIdsByRole["Customer"].Contains(ad.UserId))
                        .Select(ad => ad.User.UserName)
                        .FirstOrDefault(),
                    StaffName = appointment.AppointmentDetail != null ?
                        appointment.AppointmentDetail
                            .Where(ad => ad.IsDeleted == false && userIdsByRole["Staff"].Contains(ad.UserId))
                            .Select(ad => ad.User != null ? ad.User.Name : null)
                            .ToList()
                        : null
                });

            var itemCount = await query.CountAsync();

            var appointmentDTOs = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new Pagination<AppoitmentDetailViewDTO>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = appointmentDTOs
            };

            return result;
        }

        public async Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentsBySearchDateAndStatusAsync(string search, DateTime date, int status, int pageIndex, int pageSize)
        {
            var userIdsByRole = new Dictionary<string, List<string>>();
            userIdsByRole["Customer"] = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();
            userIdsByRole["Staff"] = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();

            var query = _dbSet
                .Include(a => a.AppointmentDetail)
                .ThenInclude(ad => ad.User)
                .Where(appointment =>( appointment.Email==search ||
                                      appointment.AppointmentDetail.Any(ad => ad.User.UserName.Contains(search)))
                                     && appointment.Status == status
                                      && appointment.Date.Date == date.Date)
                .OrderByDescending(appointment => appointment.CreationDate)
                .Select(appointment => new AppoitmentDetailViewDTO
                {
                    Id = appointment.Id,
                    Name = appointment.Name,
                    Date = appointment.Date,
                    PhoneNumber = appointment.PhoneNumber,
                    Email = appointment.Email,
                    Status = appointment.Status,
                    Time = appointment.Time,
                    CustomerName = appointment.AppointmentDetail
                        .Where(ad => ad.IsDeleted == false && userIdsByRole["Customer"].Contains(ad.UserId))
                        .Select(ad => ad.User.UserName)
                        .FirstOrDefault(),
                    StaffName = appointment.AppointmentDetail != null ?
                        appointment.AppointmentDetail
                            .Where(ad => ad.IsDeleted == false && userIdsByRole["Staff"].Contains(ad.UserId))
                            .Select(ad => ad.User != null ? ad.User.Name : null)
                            .ToList()
                        : null
                });

            var itemCount = await query.CountAsync();

            var appointmentDTOs = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new Pagination<AppoitmentDetailViewDTO>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = appointmentDTOs
            };

            return result;
        }

        public async Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentsByDateAndStatusAsync(DateTime date, int status, int pageIndex, int pageSize)
        {
            var userIdsByRole = new Dictionary<string, List<string>>();
            userIdsByRole["Customer"] = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();
            userIdsByRole["Staff"] = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();

            var query = _dbSet
                .Include(a => a.AppointmentDetail)
                .ThenInclude(ad => ad.User)
                .Where(appointment =>
                                       appointment.Status == status
                                      && appointment.Date.Date == date.Date)
                .OrderByDescending(appointment => appointment.CreationDate)
                .Select(appointment => new AppoitmentDetailViewDTO
                {
                    Id = appointment.Id,
                    Name = appointment.Name,
                    Date = appointment.Date,
                    PhoneNumber = appointment.PhoneNumber,
                    Email = appointment.Email,
                    Status = appointment.Status,
                    Time = appointment.Time,
                    CustomerName = appointment.AppointmentDetail
                        .Where(ad => ad.IsDeleted == false && userIdsByRole["Customer"].Contains(ad.UserId))
                        .Select(ad => ad.User.UserName)
                        .FirstOrDefault(),
                    StaffName = appointment.AppointmentDetail != null ?
                        appointment.AppointmentDetail
                            .Where(ad => ad.IsDeleted == false && userIdsByRole["Staff"].Contains(ad.UserId))
                            .Select(ad => ad.User != null ? ad.User.Name : null)
                            .ToList()
                        : null
                });

            var itemCount = await query.CountAsync();

            var appointmentDTOs = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new Pagination<AppoitmentDetailViewDTO>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = appointmentDTOs
            };

            return result;
        }

        public async Task<Pagination<AppoitmentDetailViewDTO>> GetAppointmentsBySearchAndStatusAsync(string search, int status, int pageIndex, int pageSize)
        {
            var userIdsByRole = new Dictionary<string, List<string>>();
            userIdsByRole["Customer"] = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();
            userIdsByRole["Staff"] = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();

            var query = _dbSet
                .Include(a => a.AppointmentDetail)
                .ThenInclude(ad => ad.User).
                Where(appointment =>
               ( appointment.Email==search ||
                appointment.AppointmentDetail.Any(ad => ad.User.UserName.Contains(search))
                && appointment.Status == status))
                .OrderByDescending(appointment => appointment.CreationDate)
                .Select(appointment => new AppoitmentDetailViewDTO
                {
                    Id = appointment.Id,
                    Name = appointment.Name,
                    Date = appointment.Date,
                    PhoneNumber = appointment.PhoneNumber,
                    Email = appointment.Email,
                    Status = appointment.Status,
                    Time = appointment.Time,
                    CustomerName = appointment.AppointmentDetail
                        .Where(ad => ad.IsDeleted == false && userIdsByRole["Customer"].Contains(ad.UserId))
                        .Select(ad => ad.User.UserName)
                        .FirstOrDefault(),
                    StaffName = appointment.AppointmentDetail != null ?
                        appointment.AppointmentDetail
                            .Where(ad => ad.IsDeleted == false && userIdsByRole["Staff"].Contains(ad.UserId))
                            .Select(ad => ad.User != null ? ad.User.Name : null)
                            .ToList()
                        : null
                });

            var itemCount = await query.CountAsync();

            var appointmentDTOs = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new Pagination<AppoitmentDetailViewDTO>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = appointmentDTOs
            };

            return result;
        }
        /*public async Task<List<AppoitntmentListStaffDTO>> GetStaffInfoAsync()
        {
            var userIdsByRole = new Dictionary<string, List<string>>();
            userIdsByRole["Customer"] = (await _userManager.GetUsersInRoleAsync("Customer")).Select(u => u.Id).ToList();
            userIdsByRole["Staff"] = (await _userManager.GetUsersInRoleAsync("Staff")).Select(u => u.Id).ToList();
            return await _dbContext.
                .Select(s => new StaffInfo
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToListAsync();
        }*/

    }
}