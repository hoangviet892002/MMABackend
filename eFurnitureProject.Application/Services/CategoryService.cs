using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.CategoryViewModels;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;
using FluentValidation;
using ValidationResult = FluentValidation.Results.ValidationResult;


namespace eFurnitureProject.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCategoryViewModel> _validatorCategory;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateCategoryViewModel> validatorCategory)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validatorCategory = validatorCategory;
        }

        public async Task<ApiResponse<CategoryViewModel>> CreateCategoryAsync(CreateCategoryViewModel category)
        {
            var response = new ApiResponse<CategoryViewModel>();
            var isExisted = await _unitOfWork.CategoryRepository.CheckCategoryNameExisted(category.Name);
            if (isExisted)
            {
                response.isSuccess = false;
                response.Message = "Category's name is existed, please try again";
                return response;
            }
            try
            {
                var categoryObj = _mapper.Map<Category>(category);
                ValidationResult validationResult = await _validatorCategory.ValidateAsync(category);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                await _unitOfWork.CategoryRepository.AddAsync(categoryObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess == true)
                {
                    response.Data = _mapper.Map<CategoryViewModel>(categoryObj);
                    response.Message = "Create category is successful!";
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiResponse<List<CategoryViewModel>>> GetAllCategoryAsync()
        {
            var response = new ApiResponse<List<CategoryViewModel>>();
            var categories = await _unitOfWork.CategoryRepository.GetAllIsNotDeleteAsync();
            var result = _mapper.Map<List<CategoryViewModel>>(categories);
            response.Data = result;
            response.Message = $"Have {result.Count} category.";
            return response;
        }

        public async Task<ApiResponse<Pagination<CategoryViewModel>>> GetCategoryPagingsionAsync(int pageIndex = 0, int pageSize = 10)
        {
            var response = new ApiResponse<Pagination<CategoryViewModel>>();
            var categories = await _unitOfWork.CategoryRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<CategoryViewModel>>(categories);
            response.Data = result;
            return response;
        }

        public async Task<ApiResponse<CategoryViewModel>> SoftRemoveCategoryAsync(string categoryId)
        {
            var response = new ApiResponse<CategoryViewModel>();
            try
            {
                var existingCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(Guid.Parse(categoryId));
                _unitOfWork.CategoryRepository.SoftRemove(existingCategory);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess == true)
                {
                    response.Data = _mapper.Map<CategoryViewModel>(existingCategory);
                    response.Message = "Remove is successful!";
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiResponse<CategoryViewModel>> UpdateCategoryAsync(string categoryId, CreateCategoryViewModel updateCategory)
        {
            var response = new ApiResponse<CategoryViewModel>();
            var categoryGuidId = Guid.Parse(categoryId);
            var isExisted = await _unitOfWork.CategoryRepository.CheckCategoryNameExisted(categoryGuidId, updateCategory.Name);
            if (isExisted)
            {
                response.isSuccess = false;
                response.Message = "Category's name is existed, please try again";
                return response;
            }
            try
            {
                var existingCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryGuidId);
                ValidationResult validationResult = await _validatorCategory.ValidateAsync(updateCategory);
                if (validationResult.IsValid == false)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                existingCategory.Name = updateCategory.Name;
                _unitOfWork.CategoryRepository.Update(existingCategory);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess == true)
                {
                    response.Data = _mapper.Map<CategoryViewModel>(existingCategory);
                    response.Message = "Update category is successful!";
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
