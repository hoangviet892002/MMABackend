using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.DashBoardViewModel;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class DashBoardService : IDashBoardService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DashBoardService(IUnitOfWork unitOfWork, IMapper mapper) { _unitOfWork = unitOfWork; _mapper = mapper; }
        public async Task<ApiResponse<List<Top5UserDTO>>> Top5User()
        {
            var response = new ApiResponse<List<Top5UserDTO>>();
            try
            {
                var user = _unitOfWork.DashBoardRepository.GetTop5UsersBySpending();
                var result = _mapper.Map<List<Top5UserDTO>>(user);
                response.isSuccess = true;
                response.Data = result;
                response.Message = "Top 5 users retrieved successfully.";
                return response;
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
        public async Task<ApiResponse<List<ProductTopDTO>>> Top5Product()
        {
            var response = new ApiResponse<List<ProductTopDTO>>();
            try
            {
                var product = _unitOfWork.DashBoardRepository.GetTop5ProductsBySales();
                var result = _mapper.Map<List<ProductTopDTO>>(product);

                response.isSuccess = true;
                response.Data = result;
                response.Message = "Top 5 product retrieved successfully.";
                return response;
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
        public async Task<ApiResponse<List<ProductTop5DTO>>> GetTotalProducts()
        {
            var response = new ApiResponse<List<ProductTop5DTO>>();
            try
            {
                var totalProducts = await _unitOfWork.DashBoardRepository.GetTotalProducts();
                var result = _mapper.Map<List<ProductTop5DTO>>(totalProducts);
                response.isSuccess = true;
                response.Data = result;
                response.Message = " Product retrieved successfully.";
                return response;
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
        public async Task<ApiResponse<TotalFinishedOrderDTO>> GetTotalFinishedOrders()
        {
            var response = new ApiResponse<TotalFinishedOrderDTO>();

            try
            {
                var totalFinishedOrders = await _unitOfWork.DashBoardRepository.GetTotalFinishedOrders();
                var result = _mapper.Map<int, TotalFinishedOrderDTO>(totalFinishedOrders);
                response.isSuccess = true;
                response.Data = result;
                response.Message = "Total Finish Order retrieved successfully.";
                return response;
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
        public async Task<ApiResponse<TotalUserDTO>> GetTotalUsers()
        {
            var response = new ApiResponse<TotalUserDTO>();

            try
            {
                var totalUsers = await _unitOfWork.DashBoardRepository.GetTotalUsers();
                var result = _mapper.Map<int, TotalUserDTO>(totalUsers);
                response.isSuccess=true;
                response.Data = result;
                response.Message = "Total User ";
                return response;
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
        public async Task<ApiResponse<TotalProcessOderDTO>> GetTotalProcessOder()
        {
            var response = new ApiResponse<TotalProcessOderDTO>();

            try
            {
                var totalUsers = await _unitOfWork.DashBoardRepository.GetTotalProcessOrders();
                var result = _mapper.Map<TotalProcessOderDTO>(totalUsers);
                response.isSuccess = true;
                response.Data = result;
                response.Message = "Get Total Process Orders successfully ";
                return response;
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
        public async Task<ApiResponse<TotalMoneyDTO>> GetTotalMoney()
        {
            var response = new ApiResponse<TotalMoneyDTO>();

            try
            {
                var totalMoney = await _unitOfWork.DashBoardRepository.GetTotalMoney();
                var result = _mapper.Map<TotalMoneyDTO>(totalMoney);
                response.isSuccess = true;
                response.Data = result;
                response.Message = "Get Total Money  successfully ";
                return response;
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
