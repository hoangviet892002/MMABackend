using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.FeedBackDTO;
using eFurnitureProject.Application.ViewModels.ResponseViewModel;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class ResponseService : IResponseService
    {
       private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        public ResponseService(IUnitOfWork unitOfWork, IMapper mapper,IClaimsService claimsService)
        {
            _claimsService = claimsService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ResponseDTO >> CreateResponse(CreateResponseDTO createResponseDTO,string feedBackId)
        {
            var response= new ApiResponse<ResponseDTO>();
            try
            {
                var feedbackID = Guid.Parse(feedBackId);
                var feedback = await _unitOfWork.FeedbackRepository.GetByIdAsync(feedbackID);
                if (feedback == null) throw new Exception("FeedBack not Found");
                var existFeedback = await _unitOfWork.ResponseRepository.CheckFeedback(feedbackID);
                if (existFeedback == false) throw new Exception("FeedBack has been responsed");
                feedback.Status = 2;
                var responsed = _mapper.Map<Response>(createResponseDTO);
                responsed.StaffId = _claimsService.GetCurrentUserId.ToString();
                responsed.FeedbackId = feedbackID; 
                await _unitOfWork.ResponseRepository.AddAsync(responsed);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    response.isSuccess = true;
                    response.Message = "Create successfully";
                    return response;
                }
                else throw new Exception(" Create Fail"); 
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
        public async Task<ApiResponse<Pagination<FeedBackDTO>>> GetFeedBackNotResponsed(int pageIndex, int pageSize)
        {
            var response =new ApiResponse<Pagination<FeedBackDTO>>();
            try
            {
                var feedBack = await _unitOfWork.ResponseRepository.FeedBackNotResponse(pageIndex, pageSize);
                var result = _mapper.Map<Pagination<FeedBackDTO>>(feedBack);
                if (result== null)
                {

                    response.Message = "No FeedBack";
                }
                response.Data = result;
                return response;
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
