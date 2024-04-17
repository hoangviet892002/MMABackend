using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.CartViewModels;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Domain.Entities;

namespace eFurnitureProject.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> addProductAsync(AddProductToCartDTO productDTO)
        {
            ApiResponse<string> response = new ApiResponse<string>();
            try
            {
                if (productDTO.Quantity <= 0)
                {
                    throw new Exception("Quanity must greater than 0");
                }
                var inventoryQuantity = await _unitOfWork.ProductRepository.GetQuantityByIdAsync(productDTO.ProductId);
                var cartObj = await _unitOfWork.CartRepository.GetCartAsync();
                var cartDetail = _mapper.Map<CartDetail>(productDTO);
                cartDetail.CartId = cartObj.Id;
                var existingDetail = cartObj.CartDetails.FirstOrDefault(x => x.ProductId == cartDetail.ProductId);
                if (existingDetail is null)
                {
                    if (cartDetail.Quantity > inventoryQuantity)
                    {
                        throw new Exception("The quantity exceeds with inventory");
                    }
                    await _unitOfWork.CartDetailRepository.AddAsync(cartDetail);
                }
                else
                {
                    existingDetail.Quantity += productDTO.Quantity;
                    if (existingDetail.Quantity > inventoryQuantity)
                    {
                        throw new Exception("The quantity exceeds with inventory");
                    }
                    _unitOfWork.CartDetailRepository.UpdateQuantityProductInCart(existingDetail);
                }
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess == true)
                {
                    response.Message = "Add product into cart successful!";
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiResponse<string>> DecreaseProductIncartAsync(string productId)
        {
            ApiResponse<string> response = new ApiResponse<string>();
            try
            {
                var productGuid = Guid.Parse(productId);
                var cartObj = await _unitOfWork.CartRepository.GetCartAsync();
                var cartDetailItem = cartObj.CartDetails.First(x => x.ProductId == productGuid);
                cartDetailItem.Quantity--;
                if(cartDetailItem.Quantity == 0)
                {
                    throw new Exception("Minimum of quantity is 1, can not decrease more");
                }
                _unitOfWork.CartDetailRepository.UpdateQuantityProductInCart(cartDetailItem);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess == true)
                {
                    response.Message = "Decrease quantity of product in cart successfully!";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.isSuccess = false;
            }
            return response;
        }

        public async Task<ApiResponse<string>> DeleteProductInCartAsync(string productId)
        {
            ApiResponse<string> response = new ApiResponse<string>();
            try
            {
                var cartId = await _unitOfWork.CartRepository.GetCartIdAsync();
                _unitOfWork.CartDetailRepository.DeleteProductInCart(cartId, Guid.Parse(productId));
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess == true)
                {
                    response.Message = "Delete product in cart successfully!";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.isSuccess = false;
            }
            return response;
        }

        public async Task<ApiResponse<List<CartDetailViewDTO>>> GetItemsInCartAsync()
        {
            ApiResponse<List<CartDetailViewDTO>> response = new ApiResponse<List<CartDetailViewDTO>>();
            try
            {
                var cartObj = await _unitOfWork.CartRepository.GetCartAsync();
                var item = _mapper.Map<List<CartDetailViewDTO>>(cartObj.CartDetails);
                response.Data = item;
                response.Message = $"Cart contains {item.Count} item";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.isSuccess = false;
            }
            return response;
        }

        public async Task<ApiResponse<string>> IncreaseProductInCartAsync(string productId)
        {
            ApiResponse<string> response = new ApiResponse<string>();
            try
            {
                Guid productGuid = Guid.Parse(productId);
                var inventoryQuantity = await _unitOfWork.ProductRepository.GetQuantityByIdAsync(productGuid);
                var cartObj = await _unitOfWork.CartRepository.GetCartAsync();
                var cartDetailItem = cartObj.CartDetails.First(x => x.ProductId == productGuid);
                cartDetailItem.Quantity++;
                if (cartDetailItem.Quantity > inventoryQuantity)
                {
                    throw new Exception("The quantity of the product exceeds to inventory");
                }
                _unitOfWork.CartDetailRepository.UpdateQuantityProductInCart(cartDetailItem);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess == true)
                {
                    response.Message = "Increase quantity of product in cart successfully!";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.isSuccess = false;
            }
            return response;
        }
    }
}
