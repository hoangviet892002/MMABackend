using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.VoucherDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IVoucherService
    {
        Task<ApiResponse<VoucherViewDTO>> CreateVoucherAsync(CreateVoucherDTO createVoucherDTO);
        Task<ApiResponse<Pagination<VoucherViewDTO>>> GetAllVoucher(int pageIndex, int pageSize);
        //Task<ApiResponse<Pagination<VoucherViewDTO>>> GetAllVoucherPaging(int pageIndex, int pageSize);
        Task<ApiResponse<VoucherViewDTO>> UpdateVoucher(CreateVoucherDTO createVoucherDTO, string id);
        Task<ApiResponse<VoucherViewDTO>> GetVouchertoUser(List<string>? userIDs, List<Guid> voucherIds);
        Task<ApiResponse<VoucherViewDTO>> SearchVoucherById(Guid id);
        Task<ApiResponse<bool>> DeleteVoucher(Guid ID);
        Task<ApiResponse<Pagination<VoucherViewDTO>>> Fileter(int pageIndex, int pageSize, string date);
        Task<ApiResponse<Pagination<VoucherViewDTO>>> GetVoucherByLogin(int pageIndex, int pageSize);
    }
}
