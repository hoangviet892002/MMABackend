using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using eFurnitureProject.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using FluentValidation.Results;
using System.Data.Common;
using eFurnitureProject.Domain.Enums;

namespace eFurnitureProject.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMemoryCache _memoryCache;
        private readonly IValidator<CreateProductDTO> _createProductvalidator;


        public ProductService(IMapper mapper, IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMemoryCache memoryCache, IProductRepository productRepository, IValidator<CreateProductDTO> validatorCreateProduct)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _memoryCache = memoryCache;
            _createProductvalidator = validatorCreateProduct;

        }
        public async Task<ApiResponse<ProductDTO>> CreateProductByAdmin(CreateProductDTO createProductDTO)
        {
            var response = new ApiResponse<ProductDTO>();
            try
            {
                var product = _mapper.Map<Product>(createProductDTO);
                product.Status = 1;
                ValidationResult validationResult = await _createProductvalidator.ValidateAsync(createProductDTO);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                await _unitOfWork.ProductRepository.AddAsync(product);
                var isSuccess = await _unitOfWork.SaveChangeAsync();
                if (isSuccess > 0)
                {
                    response.isSuccess = true;
                    response.Message = "Create successfully";
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = "Create Unsuccessfully";
                }
                return response;
            }
            catch (DbException ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    

        public async Task<ApiResponse<int>> CalculateTotalPages(int totalItemsCount, int pageSize)
        {
            var response = new ApiResponse<int>();
            try
            {
                int totalPages = totalItemsCount / pageSize;
                if (totalItemsCount % pageSize != 0)
                {
                    totalPages++;
                }

                return response;
            }
            catch (Exception ex)
            {
                return response;
            }
        }
        public async Task<ApiResponse<bool>> DeleteProduct(Guid productID)
        {
            var response = new ApiResponse<bool>();
            try
            {
                var exist = await _unitOfWork.ProductRepository.GetByIdAsync(productID);
                if (exist == null)
                {
                    response.isSuccess = false;
                    response.Message = "Product does not exist";
                    return response;
                }
                if (exist.IsDeleted)
                {
                    response.isSuccess = true;
                    response.Message = "Product is already deleted";
                    return response;
                }
                _unitOfWork.ProductRepository.SoftRemove(exist);
                var isSuccess = await _unitOfWork.SaveChangeAsync();
              return response;  
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;

        }

        public async Task<ApiResponse<Pagination<ProductDTO>>> getAllProduct(int pageIndex , int pageSize )
        {
            var response = new ApiResponse<Pagination<ProductDTO>>();
            var products = await _unitOfWork.ProductRepository.ToPaginationProduct(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<ProductDTO>>(products);
            response.Data = result;
            return response;
        }
        public async Task<ApiResponse<Pagination<ProductDTO>>> getAllProductNotdeleted(int pageIndex = 0, int pageSize = 10)
        {
            var response = new ApiResponse<Pagination<ProductDTO>>();
            var products = await _unitOfWork.ProductRepository.ToPaginationProductNotDeleted(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<ProductDTO>>(products);
            response.Data = result;
            return response;
        }
        public async Task<ApiResponse<ProductDTO>> GetProductByID(string id)
        {
            var response = new ApiResponse<ProductDTO>();
            try
            {
                var product = await _unitOfWork.ProductRepository.GetProductsByIDAsync(id);
                if (product == null)
                {
                    response.isSuccess = false;
                    response.Message = "not found product";
                }
                else
                {
                    var productDTO = _mapper.Map<ProductDTO>(product);
                    response.isSuccess = true;
                    response.Data = productDTO;
                    response.Message = "Find product successfully";
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }

        public async Task<ApiResponse<ProductDTO>> UpdateProductByAdmin(CreateProductDTO createProductDTO, Guid productID)
        {
            var response = new ApiResponse<ProductDTO>();
            try
            {
                var existProduct = await _unitOfWork.ProductRepository.GetByIdAsync(productID);
                ValidationResult validationResult = await _createProductvalidator.ValidateAsync(createProductDTO);

                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                else
                {
                    if (existProduct != null)
                    {
                        var updateProduct = _mapper.Map(createProductDTO, existProduct);
                        var saveProduct = await _unitOfWork.SaveChangeAsync();
                        if (saveProduct > 0)
                        {
                            response.isSuccess = true;
                            response.Message = "Update product successfully";
                            return response;
                        }
                        else
                        {
                            response.isSuccess = false;
                            response.Message = "Error";
                            return response;
                        }
                    }
                    else
                    {
                        response.isSuccess = false;
                        response.Message = "Product not found";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<ApiResponse<Pagination<ProductDTO>>> GetAll(int page, String CategoryID, string ProductName, double? minPrice, double? maxPrice, int pageSize)
        {
            var response = new ApiResponse<Pagination<ProductDTO>>();

            try
            {
                Pagination<ProductDTO> products;
                
                if (string.IsNullOrEmpty(CategoryID) && string.IsNullOrEmpty(ProductName) && minPrice <= 0 && maxPrice <= 0)
                {


                    var product = await _unitOfWork.ProductRepository.ToPaginationProduct(page, pageSize);
                    var result = _mapper.Map<Pagination<ProductDTO>>(product);
                    response.Data = result;
                    return response;
                }
                else if (!string.IsNullOrEmpty(CategoryID) && !string.IsNullOrEmpty(ProductName)&& minPrice.HasValue && minPrice >= 0 && !maxPrice.HasValue)
                {
                    products = await _unitOfWork.ProductRepository.GetProductsByCategoryIdAndNameAndMinPriceAsync(CategoryID, ProductName, minPrice, page, pageSize);
                }
                else if (!string.IsNullOrEmpty(CategoryID) && !string.IsNullOrEmpty(ProductName) && maxPrice.HasValue && maxPrice >= 0 && !minPrice.HasValue)
                {
                    products = await _unitOfWork.ProductRepository.GetProductsByCategoryIdAndNameAndMaxPriceAsync(CategoryID, ProductName, maxPrice, page, pageSize);
                }
                else if (!string.IsNullOrEmpty(CategoryID) && !string.IsNullOrEmpty(ProductName) && maxPrice.HasValue && maxPrice >= 0 && minPrice.HasValue && minPrice >= 0 && minPrice <= maxPrice)
                {
                    products = await _unitOfWork.ProductRepository.GetProductsByCategoryIdAndNameAndMinAndMaxPriceAsync(CategoryID, ProductName, minPrice, maxPrice, page, pageSize);
                }
                else if (!string.IsNullOrEmpty(CategoryID) && !string.IsNullOrEmpty(ProductName))
                {
                    products = await _unitOfWork.ProductRepository.GetProductsByCategoryIDAndNameAsync(CategoryID, ProductName, page, pageSize);
                }
               else  if (!string.IsNullOrEmpty(CategoryID)&&string.IsNullOrEmpty(ProductName)&&!minPrice.HasValue &&!maxPrice.HasValue)
                {
                    products = await _unitOfWork.ProductRepository.GetProductsByCategoryIDAsync(CategoryID, page, pageSize);
                }
                else if (!string.IsNullOrEmpty(ProductName) && string.IsNullOrEmpty(CategoryID) && !minPrice.HasValue && !maxPrice.HasValue)
                {
                    products = await _unitOfWork.ProductRepository.GetProductsByNameAsync(ProductName, page, pageSize);
                }
               else if (minPrice.HasValue && maxPrice.HasValue&& minPrice >= 0 && maxPrice >= 0 && minPrice<=maxPrice&& string.IsNullOrEmpty(ProductName) && string.IsNullOrEmpty(CategoryID))
                {
                    products = await _unitOfWork.ProductRepository.GetProductsByPriceAsync(minPrice, maxPrice, page, pageSize);
                }
               else  if (!string.IsNullOrEmpty(CategoryID) && minPrice.HasValue && minPrice >= 0 && string.IsNullOrEmpty(ProductName)&&!maxPrice.HasValue)
                {
                    products = await _unitOfWork.ProductRepository.GetProductsByCategoryIDAndMinPriceAsync(CategoryID, minPrice.Value, page, pageSize);
                }
                
                else if (!string.IsNullOrEmpty(CategoryID) && maxPrice.HasValue && maxPrice >= 0 && string.IsNullOrEmpty(ProductName) && !minPrice.HasValue)
                {
                    products = await _unitOfWork.ProductRepository.GetProductsByCategoryIDAndMaxPriceAsync(CategoryID, maxPrice.Value, page, pageSize);
                }

                else if (!string.IsNullOrEmpty(CategoryID) && minPrice.HasValue && minPrice >= 0 && maxPrice.HasValue && maxPrice >= 0 && string.IsNullOrEmpty(ProductName))
                {
                    products = await _unitOfWork.ProductRepository.GetProductsByCategoryIDAndPriceRangeAsync(CategoryID, minPrice.Value, maxPrice.Value, page, pageSize);
                }
            
                else if (!string.IsNullOrEmpty(ProductName) && minPrice.HasValue && minPrice >= 0 && string.IsNullOrEmpty(CategoryID) && !maxPrice.HasValue)
                {
                    products = await _unitOfWork.ProductRepository.GetProductsByNameAndMinPriceAsync(ProductName, minPrice.Value, page, pageSize);
                }
         
                else if (!string.IsNullOrEmpty(ProductName) && maxPrice.HasValue && maxPrice >= 0 && string.IsNullOrEmpty(CategoryID) && !minPrice.HasValue)
                {
                    products = await _unitOfWork.ProductRepository.GetProductsByNameAndMaxPriceAsync(ProductName, maxPrice.Value, page, pageSize);
                }
           
                else if (!string.IsNullOrEmpty(ProductName) && minPrice.HasValue && minPrice >= 0 && maxPrice.HasValue && maxPrice >= 0 && string.IsNullOrEmpty(CategoryID))
                {
                    products = await _unitOfWork.ProductRepository.GetProductsByNameAndPriceRangeAsync(ProductName, minPrice.Value, maxPrice.Value, page, pageSize);
                }
        
                else if (minPrice.HasValue && minPrice >= 0 && !maxPrice.HasValue && string.IsNullOrEmpty(ProductName) && string.IsNullOrEmpty(CategoryID))
                {
                    products = await _unitOfWork.ProductRepository.GetProductsByMinPriceAsync(minPrice.Value, page, pageSize);
                }
       
                else if (maxPrice.HasValue && maxPrice >= 0 &&!minPrice.HasValue && string.IsNullOrEmpty(ProductName) && string.IsNullOrEmpty(CategoryID))
                {
                    products = await _unitOfWork.ProductRepository.GetProductsByMaxPriceAsync(maxPrice.Value, page, pageSize);
                }
                else
                {
                    
                    response.isSuccess = true;
                    response.Message = "Get all products successfully";
                    return response;
                }

                var productDTOs = _mapper.Map<Pagination<ProductDTO>>(products);
                response.Data = productDTOs;
                response.isSuccess = true;
                response.Message = "Get all products successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }



        public async Task<ApiResponse<ProductDTO>> UpdateQuantityProduct(Guid productID, int quantity)
        {
            var response = new ApiResponse<ProductDTO>();
            try
            {
                var existProduct = await _unitOfWork.ProductRepository.GetByIdAsync(productID);
                if (existProduct == null)
                {
                    response.isSuccess = false;
                  
                    return response;
                }
                existProduct.InventoryQuantity = quantity;
                _unitOfWork.ProductRepository.Update(existProduct);
                await _unitOfWork.SaveChangeAsync();
                return response;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ApiResponse<bool>> UpdateProductStatus(Guid productId, ProductStatusEnum newStatus)
        {

            var response = new ApiResponse<bool>();

            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    response.Message = "Product not found";
                    return response;
                }
                product.Status = (int)newStatus;
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Message = $"Error updating product status: {ex.Message}";
            }

            return response;
        }
    }
}

    