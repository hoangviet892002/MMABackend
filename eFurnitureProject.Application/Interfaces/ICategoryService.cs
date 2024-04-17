using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.CategoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface ICategoryService
    {
        public Task<ApiResponse<CategoryViewModel>> CreateCategoryAsync(CreateCategoryViewModel createCategoryViewModel);
        public Task<ApiResponse<Pagination<CategoryViewModel>>> GetCategoryPagingsionAsync(int pageIndex = 0, int pageSize = 10);
        public Task<ApiResponse<CategoryViewModel>> UpdateCategoryAsync(string categoryId, CreateCategoryViewModel updateCategory);
        public Task<ApiResponse<CategoryViewModel>> SoftRemoveCategoryAsync(string categoryId);
        public Task<ApiResponse<List<CategoryViewModel>>> GetAllCategoryAsync();
    }
}
