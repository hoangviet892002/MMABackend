using eFurnitureProject.Application.Commons;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface IResponseRepository : IGenericRepository<Response>
    {

        Task<bool> CheckFeedback(Guid id);
        Task<Pagination<Feedback>> FeedBackNotResponse(int pageIndex, int pageSize);
    }
}
