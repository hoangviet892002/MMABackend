using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.CategoryViewModels;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using eFurnitureProject.Application.ViewModels.VoucherDTO;
using eFurnitureProject.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly IValidator<CreateVoucherDTO> _createVouchervalidator;
        private readonly UserManager<User> _userManager;
        public VoucherService(UserManager<User> userManager, IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService, IValidator<CreateVoucherDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
            _createVouchervalidator = validator;
            _userManager = userManager;
        }

        public async Task<ApiResponse<Pagination<VoucherViewDTO>>> GetAllVoucher(int pageIndex, int pageSize)
        {
            var response = new ApiResponse<Pagination<VoucherViewDTO>>();

            var voucher = await _unitOfWork.VoucherRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<VoucherViewDTO>>(voucher);
            response.Data = result;
            return response;
        }

        public async Task<ApiResponse<VoucherViewDTO>> CreateVoucherAsync(CreateVoucherDTO createVoucherDTO)
        {
            var response = new ApiResponse<VoucherViewDTO>();
            try
            {

                var voucher = _mapper.Map<Voucher>(createVoucherDTO);

                ValidationResult validationResult = await _createVouchervalidator.ValidateAsync(createVoucherDTO);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                if (await _unitOfWork.VoucherRepository.CheckVoucherNameExisted(createVoucherDTO.VoucherName))
                {
                    response.isSuccess = false;
                    response.Message = "Voucher Name is existed!";
                    return response;
                }

                await _unitOfWork.VoucherRepository.AddAsync(voucher);
                var issuccess = await _unitOfWork.SaveChangeAsync();
                if (issuccess > 0)
                {
                    response.isSuccess = true;
                    response.Message = "Create Successfully";
                    return response;
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }

            }
            catch (DbException ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ApiResponse<VoucherViewDTO>> UpdateVoucher(CreateVoucherDTO createVoucherDTO, string id)
        {
            var response = new ApiResponse<VoucherViewDTO>();


            try
            {
                var voucherID = Guid.Parse(id);
                var existVoucher = await _unitOfWork.VoucherRepository.GetByIdAsync(voucherID);
                var isExist = await _unitOfWork.VoucherRepository.CheckVoucherNameExisted(voucherID, createVoucherDTO.VoucherName) ;
                if (isExist)
                {
                    response.isSuccess = false;
                    response.Message = "Voucher's name is existed, please try again";
                    return response;
                }
                ValidationResult validationResult = await _createVouchervalidator.ValidateAsync(createVoucherDTO);
                if (!validationResult.IsValid)
                {

                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                else
                {
                    if (existVoucher != null)
                    {

                        existVoucher.VoucherName = createVoucherDTO.VoucherName;
                        existVoucher.StartDate = createVoucherDTO.StartDate;
                        existVoucher.EndDate=createVoucherDTO.EndDate;
                        existVoucher.Percent= createVoucherDTO.Percent;
                        existVoucher.Number = createVoucherDTO.Number;
                        existVoucher.MinimumOrderValue= createVoucherDTO.MinimumOrderValue;
                        existVoucher.MaximumDiscountAmount=createVoucherDTO.MaximumDiscountAmount;

                        _unitOfWork.VoucherRepository.Update(existVoucher );
                      var isSuccess=  await _unitOfWork.SaveChangeAsync() > 0;
                        if (isSuccess == true)
                        {
                            response.isSuccess=true;
                            response.Data = _mapper.Map<VoucherViewDTO>(existVoucher);
                            response.Message = "Update voucher is successful!";
                        }
                        else
                        {
                            response.isSuccess=false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.Data = null;
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }

        public async Task<ApiResponse<VoucherViewDTO>> GetVouchertoUser(List<string>? userIDs, List<Guid> voucherIds)
        {
            var response = new ApiResponse<VoucherViewDTO>();
            try
            {
                foreach (var userID in userIDs)
                {
                    var user = await _userManager.FindByIdAsync(userID);
                    if (user == null)
                    {
                        response.Message = "User not found";
                        response.isSuccess = false;
                        return response;
                    }

                    foreach (var voucherId in voucherIds)
                    {
                        var existVoucher = await _unitOfWork.VoucherRepository.GetByIdAsync(voucherId);
                        if (existVoucher == null)
                        {
                            response.Message = "Voucher not found";
                            response.isSuccess = false;
                            return response;
                        }

                        var voucherDetail = new VoucherDetail
                        {
                            UserId = userID,
                            VoucherId = voucherId
                        };
                        await _unitOfWork.VoucherDetailRepository.AddAsync(voucherDetail);
                    }
                }

                await _unitOfWork.SaveChangeAsync();
                response.isSuccess = true;
                response.Message = "Vouchers assigned successfully";
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = "User already has a voucher";
            }

            return response;
        }
        public async Task<ApiResponse<bool>> DeleteVoucher(Guid ID)
        {
            var response = new ApiResponse<bool>();
            try
            {
                var exist = await _unitOfWork.VoucherRepository.GetByIdAsync(ID);
                if (exist == null)
                {
                    response.isSuccess = false;
                    response.Message = "Voucher does not exist";
                    return response;
                }
                if (exist.IsDeleted)
                {
                    response.isSuccess = true;
                    response.Message = "Voucher is already deleted";
                    return response;
                }
                _unitOfWork.VoucherRepository.SoftRemove(exist);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;

            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;

        }
        public async Task<ApiResponse<VoucherViewDTO>> SearchVoucherById(Guid id)
        {
            var response = new ApiResponse<VoucherViewDTO>();
            try
            {
                var voucher = await _unitOfWork.VoucherRepository.GetByIdAsync(id);
                if (voucher == null)
                {
                    response.isSuccess = false;
                    response.Message = "Not found";
                }
                else
                {
                    var result = _mapper.Map<VoucherViewDTO>(voucher);
                    response.isSuccess = true;
                    response.Data = result;

                }

            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public async Task<ApiResponse<Pagination<VoucherViewDTO>>> Fileter(int pageIndex, int pageSize, string date)
        {
            var response = new ApiResponse<Pagination<VoucherViewDTO>>();
            try
            {
                if (string.IsNullOrWhiteSpace(date))
                {
                    var vouchers = await _unitOfWork.VoucherRepository.ToPaginationIsNotDelete(pageIndex, pageSize);
                    var result = _mapper.Map<Pagination<VoucherViewDTO>>(vouchers);
                    response.Data = result;
                }
                else
                {
                    if (!DateTime.TryParse(date, out DateTime parsedDate))
                    {
                        response.isSuccess = false;
                        response.Message = "Invalid date format";
                        return response;
                    }

                    var voucher = await _unitOfWork.VoucherRepository.GetVoucherByDateAsync(pageIndex, pageSize, parsedDate);
                    if (voucher == null)
                    {
                        response.isSuccess = false;
                        response.Message = "Voucher not found";
                    }
                    else
                    {
                        var result = _mapper.Map<Pagination<VoucherViewDTO>>(voucher);
                        response.isSuccess = true;
                        response.Data = result;
                    }
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }


            return response;
        }
        public async Task<ApiResponse<Pagination<VoucherViewDTO>>> GetVoucherByLogin(int pageIndex, int pageSize)
          {
            var response = new ApiResponse<Pagination<VoucherViewDTO>>();
            var voucher = await _unitOfWork.VoucherRepository.GetVoucher(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<VoucherViewDTO>>(voucher);
            if (result == null)
            {
                response.Message = "no vouchers";
            }
            response.Data = result;
            return response;

        }
    } 
}
