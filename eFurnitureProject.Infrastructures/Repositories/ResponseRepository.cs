using Azure;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class ResponseRepository  : GenericRepository<Domain.Entities.Response>, IResponseRepository
    {
        private readonly AppDbContext _dbContext;
    private readonly ICurrentTime _timeService;
    private readonly IClaimsService _claimsService;

    public ResponseRepository(
        AppDbContext context,
        ICurrentTime timeService,
        IClaimsService claimsService
    )
        : base(context, timeService, claimsService)
    {
        _dbContext = context;
        _timeService = timeService;
        _claimsService = claimsService;
    }

        public async Task<bool> CheckFeedback(Guid id)
        {
            var check = _dbContext.Feedbacks.
                Where(f=>f.Id == id && f.Status==2).Any();
            return check;
        }

        public async Task<Pagination<Feedback>> FeedBackNotResponse(int pageIndex, int pageSize)
        {
            var feedbacks = await _dbContext.Feedbacks
            .Where(f => f.Status == 2)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
            var totalCount = await _dbContext.Feedbacks.Where(f=> f.Status ==1).CountAsync();
            var pagination = new Pagination<Feedback>
            {
                Items = feedbacks,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = totalCount
            };

            return pagination;
        }
    }
}
