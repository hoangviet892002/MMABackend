using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.ImportViewModels;
using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class ImportRepository : GenericRepository<Import>, IImportRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;
        public ImportRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }
        public async Task AddWithDetailAsync(Import import)
        {
            foreach (var importDetail in import.ImportDetail)
            {
                import.TotalPrice += importDetail.Price * importDetail.Quantity;
            }
            import.CreatedBy = _claimsService.GetCurrentUserId;
            import.CreationDate = _timeService.GetCurrentTime();
            await _dbSet.AddAsync(import);
        }

        public async Task<Import> GetImportWithDetail(Guid importId)
        {
            var result = await _dbSet.Include(x => x.ImportDetail)
                                            .ThenInclude(id => id.Product)
                                            .FirstOrDefaultAsync(x => x.Id == importId);
            if (result == null)
            {
                throw new Exception($"Not found import with id: {importId}");
            }
            return result;
        }
    }
}
