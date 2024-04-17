using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.FeedBackDTO;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class FeedBackController : BaseController
    {
        private readonly IFeedBackService _feedBackService;

        public FeedBackController(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }
        [HttpPost]
        public async Task<ApiResponse<FeedBackDTO>> CreateFeed(CreateFeedBackDTO feedBackDTO, string productID) => await _feedBackService.CreateFeedBack(feedBackDTO, productID);

        [HttpGet]
        public async Task<ApiResponse<Pagination<FeedBackViewDTO>>> GetFeedBackJWT(int pageIndex, int PageSize) => await _feedBackService.GetFeedBackJWT(pageIndex, PageSize);
        [HttpGet]
        public async Task<ApiResponse<Pagination<Product>>> GetProductNotReviewedJWT(int pageIndex, int PageSize) => await _feedBackService.GetFeedBackNotReviewedJWT(pageIndex, PageSize);
        [HttpDelete]
        public async Task<ApiResponse<bool>> DeleteFeedBack(string id) => await _feedBackService.DeleteFeedBack(id);
    }
}
