using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.FeedBackDTO;
using eFurnitureProject.Application.ViewModels.ResponseViewModel;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IResponseService
    {
        Task<ApiResponse<ResponseDTO>> CreateResponse(CreateResponseDTO createResponseDTO, string feedBackId);
        Task<ApiResponse<Pagination<FeedBackDTO>>> GetFeedBackNotResponsed(int pageIndex, int pageSize);
    }
}
