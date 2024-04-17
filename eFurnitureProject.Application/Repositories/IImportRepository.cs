using eFurnitureProject.Application.ViewModels.ImportViewModels;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface IImportRepository : IGenericRepository<Import>
    {
        Task AddWithDetailAsync(Import import);

        Task<Import> GetImportWithDetail(Guid importId);
    }
}
