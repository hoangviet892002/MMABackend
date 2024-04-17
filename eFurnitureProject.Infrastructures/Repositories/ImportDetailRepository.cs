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
    public class ImportDetailRepository : IImportDetailRepository
    {
        private AppDbContext _appDbContext;
        public ImportDetailRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<ImportDetail>> GetImportDetailsByIdAsync(Guid id) => await _appDbContext.ImportsDetail.Where(x => x.ImportId == id)
                                                                                                                           .Include(x => x.Product)
                                                                                                                           .ToListAsync();
    }
}
