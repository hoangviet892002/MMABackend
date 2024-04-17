using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Application.ViewModels.OrderProcessingDetailViewModels;
using eFurnitureProject.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Data.Common;
using ValidationResult = FluentValidation.Results.ValidationResult;


namespace eFurnitureProject.Application.Services
{
    public class ContractService : IContractService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly UserManager<User> _userManager;
        private readonly IValidator<CreateContractDTO> _validatorCreateContract;
        private readonly IValidator<UpdateContractDTO> _validatorUpdateContract;

        public ContractService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService, UserManager<User> userManager, IValidator<CreateContractDTO> validatorCreateContract, IValidator<UpdateContractDTO> validatorUpdateContract)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
            _userManager = userManager;
            _validatorCreateContract = validatorCreateContract;
            _validatorUpdateContract = validatorUpdateContract;
        }

        public async Task<ApiResponse<string>> CreateContractAsync(CreateContractDTO contract)
        {
            var response = new ApiResponse<string>();
            try
            {
                var contractObj = _mapper.Map<Contract>(contract);
                ValidationResult validationResult = await _validatorCreateContract.ValidateAsync(contract);
                if (!validationResult.IsValid)
                {
                    throw new Exception(string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage)));
                }
                int totalValueItem = 0;
                foreach (var item in contractObj.OrderProcessing.OrderProcessingDetail)
                {
                    item.Price = (int)(await _unitOfWork.ProductRepository.GetByIdAsync((Guid)item.ProductId)).Price;
                    totalValueItem += item.Price * item.Quantity;
                }
                if (contract.Value < totalValueItem)
                {
                    throw new Exception($"The value of contract must be greater or equal to the total price item in the contract(>= {totalValueItem})");
                }
                contractObj.OrderProcessing.StatusOrderProcessingId = (await _unitOfWork.StatusOrderProcessingRepository.GetStatusByStatusCode(1)).Id;
                await _unitOfWork.ContractRepository.AddAsync(contractObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess == true)
                {
                    response.Message = "Create contract is successful!";
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ApiResponse<Pagination<ContractViewDTO>>> GetContractPagingAsync(int pageIndex = 0, int pageSize = 10)
        {
            var response = new ApiResponse<Pagination<ContractViewDTO>>();
            var contracts = await _unitOfWork.ContractRepository.GetContractToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<ContractViewDTO>>(contracts);
            response.Data = result;
            response.Message = "Get contract paging success!";
            return response;
        }
        public async Task<ApiResponse<ContractViewDTO>> UpdateContractAsync(Guid contractId, UpdateContractDTO contract)
        {
            var response = new ApiResponse<ContractViewDTO>();
            try
            {
                var existingContract = await _unitOfWork.ContractRepository.GetByIdAsync(contractId);
                var contractObj = _mapper.Map<Contract>(contract);
                ValidationResult validationResult = await _validatorUpdateContract.ValidateAsync(contract);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                existingContract.Title = contractObj.Title;
                existingContract.Description = contractObj.Description;
                existingContract.Value = contractObj.Value;
                existingContract.Status = contractObj.Status;
                _unitOfWork.ContractRepository.Update(existingContract);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess == true)
                {
                    response.Data = _mapper.Map<ContractViewDTO>(existingContract);
                    response.Message = "Update contract is successful!";
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ApiResponse<IEnumerable<OrderProcessingDetailViewDTO>>> GetContractItemAsync(string contractId)
        {
            ApiResponse<IEnumerable<OrderProcessingDetailViewDTO>> response = new ApiResponse<IEnumerable<OrderProcessingDetailViewDTO>>();
            try
            {
                var contractObj = await _unitOfWork.OrderProcessingDetailRepository.GetOrderProcessingDetailByContractId(Guid.Parse(contractId));
                var item = _mapper.Map<IEnumerable<OrderProcessingDetailViewDTO>>(contractObj);
                response.Data = item;
                response.Message = $"Contract contains {item.Count()} item";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.isSuccess = false;
            }
            return response;
        }
        public async Task<ApiResponse<Pagination<ContractViewDTO>>> GetContractsByLoginCustomerAsync(int pageIndex = 0, int pageSize = 10)
        {
            var response = new ApiResponse<Pagination<ContractViewDTO>>();
            var contracts = await _unitOfWork.ContractRepository.GetContractByLoginCustomerToPagination(_claimsService.GetCurrentUserId.ToString(), pageIndex, pageSize);
            var result = _mapper.Map<Pagination<ContractViewDTO>>(contracts);
            response.Data = result;
            response.Message = "Get contract paging by login success!";
            return response;
        }
        public async Task<ApiResponse<ContractViewFullDTO>> GetContractWithItemAsync(string contractId)
        {
            ApiResponse<ContractViewFullDTO> response = new ApiResponse<ContractViewFullDTO>();
            try
            {
                var contractObj = await _unitOfWork.ContractRepository.GetContractWithDetail(Guid.Parse(contractId));
                var item = _mapper.Map<ContractViewFullDTO>(contractObj);
                response.Data = item;
                response.Message = $"Get Contract item successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.isSuccess = false;
            }
            return response;
        }
        public async Task<ApiResponse<string>> UpdateStatusContractAsync(string contractId, int status)
        {
            var response = new ApiResponse<string>();
            try
            {
                if (status < 0 && status > 4)
                {
                    throw new Exception("Invalid number status");
                }
                var existingContract = await _unitOfWork.ContractRepository.GetByIdAsync(Guid.Parse(contractId));
                if (existingContract.Status == 3)
                {
                    throw new Exception("Can not change status contract anymore because you accepted contract");
                }
                if (existingContract.Status >= status && existingContract.Status != 4)
                {
                    throw new Exception($"Can not update status with value {status} again");
                }
                existingContract.Status = status;
                if (existingContract.Status == 3)
                {
                    var customer = await _userManager.FindByIdAsync(existingContract.CustomerId);
                    if (customer.Wallet < existingContract.Pay || customer.Wallet == null)
                    {
                        throw new Exception("Not enough money!");
                    }
                    customer.Wallet = customer.Wallet - existingContract.Pay;
                    var transaction = new Transaction
                    {
                        OrderProcessingId = existingContract.OrderProcessingId,
                        Amount = existingContract.Pay,
                        From = "Wallet",
                        To = "eFurniturePay",
                        Type = "OrderProcessing",
                        BalanceRemain = (double)customer.Wallet,
                        UserId = customer.Id,
                        Status = 0,
                        Description = $"Transfer {existingContract.Pay:F2} from User wallet to Furniture Pay for partial paying the contract",
                    };
                    await _unitOfWork.TransactionRepository.AddAsync(transaction);
                    await _userManager.UpdateAsync(customer);
                }
                _unitOfWork.ContractRepository.Update(existingContract);
                bool isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    response.Message = "Update status contract success";
                }
                else
                {
                    throw new Exception("Update status contract fail");
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ApiResponse<string>> UpdateStatusOrderProcessingAsync(string contractId, int status)
        {
            var response = new ApiResponse<string>();
            try
            {
                if(status == 4)
                {
                    throw new Exception("Can not set status value 4(Delivering) because depending customer");
                }
                var existingContract = await _unitOfWork.ContractRepository.GetByIdAsync(Guid.Parse(contractId));
                if (existingContract.Status != 3)
                {
                    throw new Exception("Can not update order processing because customer have not accepted the contract");
                }
                var newStatus = await _unitOfWork.StatusOrderProcessingRepository.GetStatusByStatusCode(status);
                var existingOrderProcess = await _unitOfWork.OrderProcessingRepository.GetOrderProcessingByContractId(Guid.Parse(contractId));
                var oldStatus = await _unitOfWork.StatusOrderProcessingRepository.GetByIdAsync((Guid)existingOrderProcess.StatusOrderProcessingId);
                if (oldStatus.StatusCode <= 3)
                {
                    if (newStatus.StatusCode <= oldStatus.StatusCode || newStatus.StatusCode >= 4)
                    {
                        throw new Exception("Invalid state!");
                    }
                }
                else
                {
                    if (newStatus.StatusCode <= oldStatus.StatusCode)
                    {
                        throw new Exception("Invalid state!");
                    }
                }
                existingOrderProcess.StatusOrderProcessingId = newStatus.Id;
                _unitOfWork.OrderProcessingRepository.Update(existingOrderProcess);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (!isSuccess)
                {
                    throw new Exception("Update status order processing fail!");
                }
                response.Message = "Successful!";
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        //public async Task<ApiResponse<Pagination<ContractViewDTO>>> GetContractByFilterAsync(FilterContractDTO filterContractDTO)
        //{
        //    var response = new ApiResponse<Pagination<ContractViewDTO>>();
        //    try
        //    {
        //        var listContract = await _unitOfWork.ContractRepository.GetContractByFilter
        //        (filterContractDTO.PageIndex, filterContractDTO.PageSize,
        //        filterContractDTO.StatusCode, filterContractDTO.FromTime,
        //             filterContractDTO.ToTime, filterContractDTO.Search);
        //        if (listContract == null)
        //        {
        //            response.Message = "Not found!";
        //            return response;
        //        }
        //        var result = _mapper.Map<Pagination<ContractViewDTO>>(listContract);
        //        response.Data = result;
        //        response.Message = "Successful!";
        //    }
        //    catch (Exception ex)
        //    {
        //        response.isSuccess = false;
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}
        public async Task<ApiResponse<ContractViewDTO>> SoftRemoveContractByIdAsync(Guid contractId)
        {
            var response = new ApiResponse<ContractViewDTO>();
            try
            {
                var existingContract = await _unitOfWork.ContractRepository.GetByIdAsync(contractId);
                _unitOfWork.ContractRepository.SoftRemove(existingContract);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess == true)
                {
                    response.Data = _mapper.Map<ContractViewDTO>(existingContract);
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

        public async Task<ApiResponse<string>> PayRemainingCostContractCustomerAsync(string contractId)
        {
            var response = new ApiResponse<string>();
            try
            {
                var newStatus = await _unitOfWork.StatusOrderProcessingRepository.GetStatusByStatusCode(4);
                var existingContract = await _unitOfWork.ContractRepository.GetContractWithDetail(Guid.Parse(contractId));
                if (existingContract.OrderProcessing.StatusOrderProcessing.StatusCode < 3)
                {
                    throw new Exception("Can not pay remaning cost of contract until the status of order processing is Complete Construction");
                }
                if(existingContract.OrderProcessing.StatusOrderProcessing.StatusCode > 3)
                {
                    throw new Exception("Invalid state");
                }
                var customer = await _userManager.FindByIdAsync(existingContract.CustomerId);
                var remainingCost = existingContract.Value - existingContract.Pay;
                if (customer.Wallet < remainingCost || customer.Wallet == null)
                {
                    throw new Exception("Not enough money!");
                }
                customer.Wallet = customer.Wallet - remainingCost;
                var transaction = new Transaction
                {
                    OrderProcessingId = existingContract.OrderProcessingId,
                    Amount = remainingCost,
                    From = "Wallet",
                    To = "eFurniturePay",
                    Type = "OrderProcessing",
                    BalanceRemain = (double)customer.Wallet,
                    UserId = customer.Id,
                    Status = 0,
                    Description = $"Transfer {remainingCost:F2} from User wallet to Furniture Pay for pay remaing cost contract",
                };
                await _unitOfWork.TransactionRepository.AddAsync(transaction);
                existingContract.OrderProcessing.StatusOrderProcessingId = newStatus.Id;
                _unitOfWork.OrderProcessingRepository.Update(existingContract.OrderProcessing);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                await _userManager.UpdateAsync(customer);
                if (!isSuccess)
                {
                    throw new Exception("Pay remaing cost contract fail!");
                }
                response.Message = "Pay remaing cost contract successful!";
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
