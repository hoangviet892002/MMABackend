using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.TransactionViewModels;
using eFurnitureProject.Application.ViewModels.VoucherDTO;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IClaimsService _claimsService;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _claimsService = claimsService;
        }

        public async Task<ApiResponse<Pagination<TransactionViewDTO>>> FilterTransactionAsync(FilterTransactionDTO filterTransactionDTO)
        {
            var response = new ApiResponse<Pagination<TransactionViewDTO>>();
            try
            {
                var transactions = await _unitOfWork.TransactionRepository.FilterTransactionAsync(filterTransactionDTO.Search,filterTransactionDTO.Type, 
                    filterTransactionDTO.FromTime, filterTransactionDTO.ToTime, 
                    filterTransactionDTO.PageIndex, filterTransactionDTO.PageSize);
                var result = _mapper.Map<Pagination<TransactionViewDTO>>(transactions);
                response.Data = result;
                response.isSuccess = true;
                response.Message = "Success!";

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
        public async Task<ApiResponse<Pagination<TransactionViewDTO>>> FilterTransactionByLoginAsync(FilterTransactionByLoginDTO filterTransactionByLoginDTO)
        {
            var response = new ApiResponse<Pagination<TransactionViewDTO>>();
            try
            {
                var userId = _claimsService.GetCurrentUserId;
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null) throw new Exception("Login fail!");
                var transactions = await _unitOfWork.TransactionRepository.FilterTransactionByLoginAsync(user.Id, filterTransactionByLoginDTO.FromTime,
                    filterTransactionByLoginDTO.ToTime, filterTransactionByLoginDTO.PageIndex, filterTransactionByLoginDTO.PageSize);
                var result = _mapper.Map<Pagination<TransactionViewDTO>>(transactions);
                response.Data = result;
                response.isSuccess = true;
                response.Message = "Success!";

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

        public async Task<ApiResponse<TransactionViewDTO>> GetTransactionByIdAsync(Guid transactionId)
        {
            var response = new ApiResponse<TransactionViewDTO>();

            try
            {
                var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(transactionId);
                var result = _mapper.Map<TransactionViewDTO>(transaction);

                if (result == null) throw new Exception("Not found!");

                response.Data = result;
                response.isSuccess = true;
                response.Message = "Success!";

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

       
    }
}
