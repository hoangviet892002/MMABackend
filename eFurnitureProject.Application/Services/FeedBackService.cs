using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Application.ViewModels.FeedBackDTO;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class FeedBackService : IFeedBackService
    {
        private readonly IClaimsService _claimsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FeedBackService(IClaimsService claimsService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _claimsService = claimsService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<FeedBackDTO>> CreateFeedBack(CreateFeedBackDTO feedBackDTO, string productID)
        {
            var response = new ApiResponse<FeedBackDTO>();
            try
            {
                var proDtuctID = Guid.Parse(productID);
                var checkFeedBack = await _unitOfWork.FeedbackRepository.CheckProduct(proDtuctID);
                if (!checkFeedBack)
                {
                    response.isSuccess = false;
                    response.Message = "Fail by product";
                }
                else
                {

                    var feedback = _mapper.Map<Feedback>(feedBackDTO);
                    if (feedback.Status == 2)
                    {
                        response.isSuccess = false;
                        response.Message = " FeedBack has responsed ";
                    }
                    else
                    {
                        feedback.Status = 1;
                        feedback.ProductId = proDtuctID;
                        feedback.UserId = _claimsService.GetCurrentUserId.ToString();
                        await _unitOfWork.FeedbackRepository.AddAsync(feedback);
                        var issuccess = await _unitOfWork.SaveChangeAsync() > 0;
                        if (issuccess)
                        {
                            response.isSuccess = true;
                            response.Message = "Create Successfully";
                            return response;

                        }
                        else
                        {
                            response.isSuccess = false;
                            response.Message = "Create Fail";
                            return response;

                        }
                        }
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
        public async Task<ApiResponse<Pagination<FeedBackViewDTO>>> GetFeedBackJWT(int pageIndex, int PageSize)
        {
            var response = new ApiResponse<Pagination<FeedBackViewDTO>>();
            try
            {
                var userCurrentID = _claimsService.GetCurrentUserId.ToString();
                var feedbacks = await _unitOfWork.FeedbackRepository.GetFeedBacksByUserID(pageIndex, PageSize, userCurrentID);
                var result = _mapper.Map<Pagination<FeedBackViewDTO>>(feedbacks);
                response.Data = result;
                return response;
            }
            catch (Exception ex)
            {

                response.Data = null;
                response.isSuccess = false;
                response.Message = $" error : {ex.Message}";
            }

            return response;
        }

        public async Task<ApiResponse<Pagination<Product>>> GetFeedBackNotReviewedJWT(int pageIndex, int PageSize)
        {
            var response = new ApiResponse<Pagination<Product>>();
            try
            {
                var userCurrentID = _claimsService.GetCurrentUserId.ToString();
                var feedbacks = await _unitOfWork.FeedbackRepository.GetProductNotFeedbackByUserID(pageIndex, PageSize, userCurrentID);
                var result = _mapper.Map<Pagination<Product>>(feedbacks);
                response.Data = result;
                return response;
            }
            catch (Exception ex)
            {

                response.Data = null;
                response.isSuccess = false;
                response.Message = $" error : {ex.Message}";
            }

            return response;
        }
        public async Task<ApiResponse<bool>> DeleteFeedBack(string id)
        {
            var response = new ApiResponse<bool>();
            try
            {
                var ID=Guid.Parse(id);
                var exist = await _unitOfWork.FeedbackRepository.GetByIdAsync(ID);
                if (exist == null)
                {
                    response.isSuccess = false;
                    response.Message = "FeedBack does not exist";
                    return response;
                }
                if (exist.IsDeleted)
                {
                    response.isSuccess = true;
                    response.Message = "Product is already deleted";
                    return response;
                }
                _unitOfWork.FeedbackRepository.SoftRemove(exist);
                var isSuccess = await _unitOfWork.SaveChangeAsync();
                var issuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (issuccess)
                {
                    response.isSuccess = true;
                    response.Message = "Delete Successfully";
                    return response;

                }
                else
                {
                    response.isSuccess = false;
                    response.Message = "Delete Fail";
                    return response;

                }
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
