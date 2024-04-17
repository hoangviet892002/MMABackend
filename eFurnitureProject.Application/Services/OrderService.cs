using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.CartViewModels;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;
using eFurnitureProject.Application.ViewModels.OrderViewModels;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Application.ViewModels.StatusOrderViewModels;
using eFurnitureProject.Application.ViewModels.WalletViewModels;
using eFurnitureProject.Domain.Entities;
using eFurnitureProject.Domain.Enums;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly UserManager<User> _userManager;
        private readonly IValidator<CreateOrderDTO> _validatorCreateOrder;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService,
                            UserManager<User> userManager, IValidator<CreateOrderDTO> validatorCreateOrder) 
        { 
            _mapper = mapper;
            _unitOfWork = unitOfWork;  
            _claimsService = claimsService;
            _userManager = userManager;
            _validatorCreateOrder = validatorCreateOrder;
        }
        public async Task<ApiResponse<OrderViewDTO>> GetOrderByIdAsync(Guid orderId)
        {
            var response = new ApiResponse<OrderViewDTO>();
            try
            {
                var order = await _unitOfWork.OrderRepository.GetOrderByIdAsync(orderId);
                if (order == null) throw new Exception("Not found!");
                var result = _mapper.Map<OrderViewDTO>(order);
                response.Data = result;
                response.isSuccess = true;
                response.Message= "Successful!";
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

        public async Task<ApiResponse<Pagination<OrderViewDTO>>> GetOrderFilterAsync(FilterOrderDTO filterOrderDTO)
        {
            var response = new ApiResponse<Pagination<OrderViewDTO>>();
            try 
            {
                var listOrder = await _unitOfWork.OrderRepository.GetOrderByFilter
                    (filterOrderDTO.PageIndex, filterOrderDTO.PageSize,
                     filterOrderDTO.StatusCode, filterOrderDTO.FromTime,
                     filterOrderDTO.ToTime, filterOrderDTO.Search);
                if (listOrder == null) 
                {   
                    response.isSuccess = true;
                    response.Message = "Not found!";
                    return response;
                }
                var result = _mapper.Map<Pagination<OrderViewDTO>>(listOrder);
                response.Data = result; 
                response.isSuccess = true;
                response.Message = "Successful!";
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

        public async Task<ApiResponse<Pagination<OrderViewForCustomerDTO>>> GetOrderFilterByLoginAsync(FilterOrderByLoginDTO filterOrderByLogin)
        {
            var response = new ApiResponse<Pagination<OrderViewForCustomerDTO>>();
            try
            {
                var userId = _claimsService.GetCurrentUserId.ToString();
                var listOrder = await _unitOfWork.OrderRepository.GetOrderFilterByLogin
                    (filterOrderByLogin.PageIndex, filterOrderByLogin.PageSize, 
                     filterOrderByLogin.StatusCode, filterOrderByLogin.FromTime, 
                     filterOrderByLogin.ToTime, userId);
                if (listOrder == null)
                {
                    response.isSuccess = true;
                    response.Message = "Not found!";
                    return response;
                }
                var result = _mapper.Map<Pagination<OrderViewForCustomerDTO>>(listOrder);
                response.Data = result;
                response.isSuccess = true;
                response.Message = "Successful!";
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

        public async Task<ApiResponse<StatusDetailOrderViewDTO>> GetOrderStatusByOrderId(Guid orderId)
        {
            var response = new ApiResponse<StatusDetailOrderViewDTO>();
            try
            {
                var statusDetail = await _unitOfWork.OrderRepository.GetStatusOrderByOrderId(orderId);
                if (statusDetail == null)
                {
                    response.isSuccess = false;
                    response.Message = "Not found!";
                }
                var result = _mapper.Map<StatusDetailOrderViewDTO>(statusDetail);
                response.Data = result;
                response.isSuccess = true;
                response.Message = "Get status successfully!";
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

        public async Task<ApiResponse<string>> UpdateOrderStatusAsync(UpdateOrderStatusDTO updateOrderStatusDTO)
        {
            var response = new ApiResponse<string>();
            try
            {
                var newStatus = await _unitOfWork.StatusOrderRepository.GetStatusByStatusCode(updateOrderStatusDTO.StatusCode);
                var newOrder = await _unitOfWork.OrderRepository.GetByIdAsync(updateOrderStatusDTO.Id);
                if (newOrder == null) 
                {
                    response.isSuccess = false;
                    response.Message = "Not found order!";
                    return response;
                }

                var oldStatus = await _unitOfWork.StatusOrderRepository.GetByIdAsync((Guid)newOrder.StatusId);
                if (updateOrderStatusDTO.StatusCode <= oldStatus.StatusCode) 
                {
                    response.isSuccess = false;
                    response.Message = "Invalid state!";
                    return response;
                }
                newOrder.StatusId = newStatus.Id;
                _unitOfWork.OrderRepository.Update(newOrder);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (!isSuccess)
                {
                    response.isSuccess = false;
                    response.Message = "Update fail!";
                }
                response.isSuccess = true;
                response.Message = "Successful!";
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
        public async Task<ApiResponse<string>> CheckOut1(CreateOrderDTO createOrderDTO)
        {
            var response = new ApiResponse<string>();
            try
            {
                ValidationResult validationResult = await _validatorCreateOrder.ValidateAsync(createOrderDTO);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }

                bool checkVoucher = false;
                var voucherInfo = new Voucher();
                var userId = _claimsService.GetCurrentUserId.ToString();
                if (!createOrderDTO.VoucherId.IsNullOrEmpty())
                {
                    var voucherId = Guid.Parse(createOrderDTO.VoucherId);
                    //Check voucher existed
                    voucherInfo = await _unitOfWork.VoucherRepository.GetByIdAsync(voucherId);
                    if (voucherInfo == null || voucherInfo.IsDeleted || voucherInfo.Number <= 0) throw new Exception("Not found voucher!");
                    else 
                    { 
                        //Update Voucher
                        voucherInfo.Number = voucherInfo.Number - 1;
                        _unitOfWork.VoucherRepository.Update(voucherInfo);
                        await _unitOfWork.VoucherDetailRepository.AddAsync(new VoucherDetail {UserId = userId, VoucherId = voucherId});
                    }
                    //Check voucher be used
                    if (await _unitOfWork.VoucherDetailRepository.CheckVoucherBeUsedByUser(userId, voucherId))
                        throw new Exception("Voucher is used!");
                    else
                        checkVoucher = true;
                }
                else { createOrderDTO.VoucherId = null; }
                var cartDetails = await _unitOfWork.CartRepository.GetCartDetailsByUserId(userId);
                if (cartDetails.IsNullOrEmpty()) throw new Exception("Your cart has no products!");

                var createOrder = _mapper.Map<Order>(createOrderDTO);
                await _unitOfWork.OrderRepository.AddAsync(createOrder);
                var resultCreate = await _unitOfWork.SaveChangeAsync() > 0;
                if (!resultCreate) throw new Exception("Order creation failed!");
                var id = createOrder.Id;
                var price = 0d;
                // insert product from cart to orderDetail
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                List<Product> products = new List<Product>();
                foreach (var cartDetail in cartDetails)
                {
                    
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(cartDetail.ProductId);
                    if (product == null) throw new Exception($"Some products do not exist in your shopping cart!");
                    if (product.IsDeleted) throw new Exception($"{product.Name} do not exist in your shopping cart!");
                    if (product.Status != (int)ProductStatusEnum.Unlock) throw new Exception($"{product.Name} has been discontinued!");
                    if (product.InventoryQuantity <=0) throw new Exception($"{product.Name} out of stock!");
                    product.InventoryQuantity = product.InventoryQuantity - cartDetail.Quantity;
                    products.Add(product);
                    orderDetails.Add(new OrderDetail
                    {
                        OrderId = id,
                        ProductId = product.Id,
                        Quantity = cartDetail.Quantity,
                        Price = product.Price,
                    });
                    price =+ product.Price * cartDetail.Quantity;  
                }
                await _unitOfWork.OrderDetailRepository.AddRangeAsync(orderDetails);
                _unitOfWork.ProductRepository.UpdateProductByOrder(products);
                if (checkVoucher)
                {
                    if (voucherInfo.MinimumOrderValue <= price)
                    {
                        var discount = voucherInfo.Percent/100 * price;
                        if (discount > voucherInfo.MaximumDiscountAmount)
                        {
                            price = price - voucherInfo.MaximumDiscountAmount;
                        }
                        else price = price - discount;
                    }
                }
                createOrder.Price = price;
                createOrder.Address = createOrderDTO.Address;
                createOrder.Email = createOrderDTO.Email;
                createOrder.PhoneNumber = createOrderDTO.PhoneNumber;
                createOrder.StatusId = (await _unitOfWork.StatusOrderRepository.GetStatusByStatusCode((int)OrderStatusEnum.Pending)).Id;
                createOrder.Name = createOrderDTO.Name;
                createOrder.UserId = userId;
                var user = await _userManager.FindByIdAsync(userId);
                if (user.Wallet < price || user.Wallet == null) throw new Exception("Not enough money!");
                user.Wallet = user.Wallet - price;
                var cartId = await _unitOfWork.CartRepository.GetCartIdAsync();
                _unitOfWork.CartDetailRepository.DeleteCart(cartId);

                var transaction = new Transaction
                {
                    OrderId = id,
                    Amount = price,
                    From = "Wallet",
                    To = "eFurniturePay",
                    Type = "Order",
                    BalanceRemain = (double)user.Wallet,
                    UserId = userId,
                    Status = 0,
                    Description = $"Transfer {price:F2} from User wallet to eFurniturePay for paying Order",
                };
                await _unitOfWork.TransactionRepository.AddAsync(transaction);
                
                _unitOfWork.OrderRepository.Update(createOrder);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                await _userManager.UpdateAsync(user);
                if (!isSuccess) throw new Exception("Create fail!");
                response.isSuccess = true;
                response.Message = "Checkout Successfully!";

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
        public async Task<ApiResponse<string>> CheckOut(CreateOrderDTO createOrderDTO)
        {
            var response = new ApiResponse<string>();
            try
            {
                // Validate the incoming order details
                ValidationResult validationResult = await _validatorCreateOrder.ValidateAsync(createOrderDTO);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }

                var userId = _claimsService.GetCurrentUserId.ToString();
                bool checkVoucher = false;
                var voucherInfo = new Voucher();
                if (!createOrderDTO.VoucherId.IsNullOrEmpty())
                {
                    var voucherId = Guid.Parse(createOrderDTO.VoucherId);
                    voucherInfo = await _unitOfWork.VoucherRepository.GetByIdAsync(voucherId);
                    if (voucherInfo == null || voucherInfo.IsDeleted || voucherInfo.Number <= 0)
                        throw new Exception("Voucher không tồn tại hoặc không hợp lệ!");
                    if (await _unitOfWork.VoucherDetailRepository.CheckVoucherBeUsedByUser(userId, voucherId))
                        throw new Exception("Voucher này đã được sử dụng bởi người dùng này!");

                    // Giảm số lượng voucher sẵn có
                    voucherInfo.Number -= 1;
                    _unitOfWork.VoucherRepository.Update(voucherInfo);
                    await _unitOfWork.VoucherDetailRepository.AddAsync(new VoucherDetail { UserId = userId, VoucherId = voucherId });
                    checkVoucher = true;
                }

                var cartDetails = await _unitOfWork.CartRepository.GetCartDetailsByUserId(userId);
                if (cartDetails.IsNullOrEmpty()) throw new Exception("Giỏ hàng của bạn không có sản phẩm nào!");

                double totalAmount = 0;
                foreach (var cartDetail in cartDetails)
                {
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(cartDetail.ProductId);
                    if (product == null || product.IsDeleted || product.Status != (int)ProductStatusEnum.Unlock)
                        throw new Exception($"Sản phẩm {product.Name} không khả dụng.");

                    if (product.InventoryQuantity < cartDetail.Quantity)
                        throw new Exception($"Không đủ hàng tồn kho cho sản phẩm {product.Name}.");

                    totalAmount += product.Price * cartDetail.Quantity;
                }

                // Kiểm tra số dư trong ví của người dùng
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null || user.Wallet < totalAmount)
                    throw new Exception("Số dư trong ví không đủ để thanh toán!");

                foreach (var cartDetail in cartDetails)
                {
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(cartDetail.ProductId);
                    product.InventoryQuantity -= cartDetail.Quantity;
                    _unitOfWork.ProductRepository.Update(product);

                    // Tạo đơn hàng
                    var createOrder = _mapper.Map<Order>(createOrderDTO);
                    createOrder.Price = product.Price * cartDetail.Quantity;
                    createOrder.StatusId = (await _unitOfWork.StatusOrderRepository.GetStatusByStatusCode((int)OrderStatusEnum.Pending)).Id;
                    createOrder.UserId = userId;
                    await _unitOfWork.OrderRepository.AddAsync(createOrder);

                    // Thêm chi tiết đơn hàng
                    var orderDetail = new OrderDetail
                    {
                        OrderId = createOrder.Id,
                        ProductId = product.Id,
                        Quantity = cartDetail.Quantity,
                        Price = product.Price
                    };
                    await _unitOfWork.OrderDetailRepository.AddRangeAsync(new List<OrderDetail> { orderDetail });

                    // Tạo transaction cho mỗi đơn hàng
                    var transaction = new Transaction
                    {
                        OrderId = createOrder.Id,
                        Amount = createOrder.Price,
                        From = "Wallet",
                        To = "eFurniturePay",
                        Type = "Order",
                        BalanceRemain = (double)user.Wallet - createOrder.Price,
                        UserId = userId,
                        Status = 0,
                        Description = $"Chuyển khoản {createOrder.Price:F2} từ ví người dùng sang eFurniturePay để thanh toán đơn hàng"
                    };
                    await _unitOfWork.TransactionRepository.AddAsync(transaction);
                    var trackOrderStatus = new OrderTrackingStatus
                    {
                        OrderId = createOrder.Id,
                        Name = "1"
                    };
                    await _unitOfWork.OrderTrackingStatusRepository.AddAsync(trackOrderStatus);


                    // Cập nhật số dư ví người dùng
                    user.Wallet -= createOrder.Price;
                }
                var cartId = await _unitOfWork.CartRepository.GetCartIdAsync();
                _unitOfWork.CartDetailRepository.DeleteCart(cartId);
                // Cập nhật thông tin người dùng
                await _userManager.UpdateAsync(user);
                

                var isSuccess = true;
           
                if (!isSuccess) throw new Exception("Create fail!");
                response.isSuccess = true;
                response.Message = "Checkout Successfully!";
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
