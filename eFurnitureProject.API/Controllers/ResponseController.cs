using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.FeedBackDTO;
using eFurnitureProject.Application.ViewModels.ResponseViewModel;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class ResponseController:BaseController
    {
        private readonly IResponseService _responseService;

        public ResponseController(IResponseService responseService)
        {
            _responseService = responseService;

        }
        [HttpPost]
        public async Task<ApiResponse<ResponseDTO>> CreateResponse(CreateResponseDTO createResponseDTO, string feedBackId) => await _responseService.CreateResponse(createResponseDTO, feedBackId);
        [HttpGet]
        public async Task<ApiResponse<Pagination<FeedBackDTO>>> GetFeedBackNotResponsed(int pageIndex, int pageSize) => await _responseService.GetFeedBackNotResponsed(pageIndex, pageSize);
    }
}
