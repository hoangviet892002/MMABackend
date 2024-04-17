using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel;
using eFurnitureProject.Application.ViewModels.FeedBackDTO;
using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface IFeedbackRepository : IGenericRepository<Feedback>
    {
        Task<bool> CheckProduct(Guid productId);
        Task<Pagination<Product>> GetProductNotFeedbackByUserID(int pageIndex, int pageSize, string userID);
        Task<Pagination<FeedBackViewDTO>> GetFeedBacksByUserID(int pageIndex, int pageSize, string userID);

    }
}
