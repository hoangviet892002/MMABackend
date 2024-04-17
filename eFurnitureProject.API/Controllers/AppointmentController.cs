using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Services;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Net.Mime;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel;
using eFurnitureProject.Domain.Enums;

namespace eFurnitureProject.API.Controllers
{
    public class AppointmentController : BaseController
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
         _appointmentService = appointmentService;
        
        }
        [Authorize(Roles = AppRole.Customer)]
        [HttpPost]
     
        public async Task<ApiResponse<AppointmentDTO>> CreateAppointment(CreateAppointmentDTO createAppointmentDTO)=> await _appointmentService.CreateAppointment(createAppointmentDTO);


       [Authorize(Roles = AppRole.Customer)]
        [HttpPost]
        public async Task<ApiResponse<AppointmentDTO>> UpdateAppointmentByCustomer(string ID, CreateAppointmentDTO appointmentDTO) =>    
            await _appointmentService.UpdateAppointmentByCustomer(ID, appointmentDTO);

        [HttpGet]
        public async Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> GetAppointmentPaging(int page, int pageSize) => await _appointmentService.GetAppointmentPaging(page, pageSize);
      

        [HttpGet]
        public async  Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> Filter([FromQuery] FilterAppointmentDTO filterAppointment, string?  date, int status)
        {
            
           return await _appointmentService.Filter(filterAppointment,date,status);
        }
       [Authorize(Roles = AppRole.Staff + "," + AppRole.Admin)]
        [HttpPost]
        public async Task<ApiResponse<AppointmentDTO>> PickStaffForAppointment(string appointmentId, string staffId) => await _appointmentService.PickStaffForAppointment(appointmentId,  staffId);
       
        [HttpPost]
        public async Task<ApiResponse<bool>> UpdateAppointmentStatus(string appointmentId, AppointmentStatusEnum newStatus)=> await _appointmentService.UpdateAppointmentStatus(appointmentId, newStatus);
       [Authorize(Roles = AppRole.Staff + "," + AppRole.Admin)]
        [HttpDelete]
        public async Task<ApiResponse<bool>> DeleteAppointment(string ID)=> await _appointmentService.DeleteAppointment(ID);
        [HttpGet]
        public async Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> GetAppointmentByJWT(int pageIndex, int pageSize) => await _appointmentService.GetAppointmentByJWT(pageIndex, pageSize);
        [HttpGet]
        public async Task<ApiResponse<List<AppoitntmentListStaffDTO>>> GetStaffForAppointment(string appointmentID) => await _appointmentService.GetStaffForAppointment( appointmentID);
        [HttpGet]
        public async Task<ApiResponse<AppoitmentDetailViewDTO>> GetAppointmentDetail(string id) => await _appointmentService.GetAppointmentDetail(id);
    }
}
