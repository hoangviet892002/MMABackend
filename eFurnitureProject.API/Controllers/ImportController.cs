using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.ImportViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    [Authorize(Roles = AppRole.Admin + "," + AppRole.Staff)]
    public class ImportController : BaseController
    {
        private readonly IImportService _importService;

        public ImportController(IImportService importService)
        {
            _importService = importService;
        }

        [HttpPost]
        public async Task<ApiResponse<ImportViewDTO>> CreateImportWithDetail(CreateImportDTO createImport) => await _importService.CreateImportWithDetailAsync(createImport);

        [HttpPut]
        public async Task<ApiResponse<string>> UpdateStatusImport(UpdateImportDTO updateImport) => await _importService.UpdateStatusImportAsync(updateImport);

        [HttpGet]
        public async Task<ApiResponse<Pagination<ImportViewDTO>>> GetImportPaging(int pageIndex = 0, int pageSize = 10) => await _importService.GetImportPagingAsync(pageIndex, pageSize);

        [HttpGet]
        public async Task<ApiResponse<ImportViewFullDTO>> GetImportDetail(string importID) => await _importService.GetImportDetailAsync(importID);
    }
}
