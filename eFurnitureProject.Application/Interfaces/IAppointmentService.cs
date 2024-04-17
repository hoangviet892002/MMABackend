using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel;
using eFurnitureProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<ApiResponse<AppointmentDTO>> CreateAppointment(CreateAppointmentDTO createAppointmentDTO);
        Task<ApiResponse<AppointmentDTO>> UpdateAppointmentByCustomer(string ID,CreateAppointmentDTO appointmentDTO);
        Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> GetAppointmentPaging(int page, int amout);
        Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> Filter(FilterAppointmentDTO filterAppointment, string date, int status);
       Task<ApiResponse<AppointmentDTO>> PickStaffForAppointment(string appointmentId, string staffId);
        Task<ApiResponse<bool>> UpdateAppointmentStatus(string appointmentId, AppointmentStatusEnum newStatus);
        Task<ApiResponse<bool>> DeleteAppointment(string ID);
        Task<ApiResponse<Pagination<AppoitmentDetailViewDTO>>> GetAppointmentByJWT(int pageIndex, int pageSize);
        Task<ApiResponse<List<AppoitntmentListStaffDTO>>> GetStaffForAppointment(string appointmentID);
        Task<ApiResponse<AppoitmentDetailViewDTO>> GetAppointmentDetail(string id);
    }
}
