using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Application.ViewModels.OrderProcessingDetailViewModels;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class  ContractController : BaseController
    {
        private readonly IContractService _contractService;

        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }
        [Authorize(Roles = AppRole.Admin + "," + AppRole.Staff)]
        [HttpPost]
        public async Task<ApiResponse<string>> CreateContract(CreateContractDTO contract) => await _contractService.CreateContractAsync(contract);
       
        [Authorize(Roles = AppRole.Admin + "," + AppRole.Staff)]
        [HttpGet]
        public async Task<ApiResponse<Pagination<ContractViewDTO>>> GetContractsByPage(int pageIndex = 0, int pageSize = 10) => await _contractService.GetContractPagingAsync(pageIndex, pageSize);

        [Authorize]
        [HttpGet]
        public async Task<ApiResponse<ContractViewFullDTO>> GetContractItem(string contractId) => await _contractService.GetContractWithItemAsync(contractId);

        [Authorize(Roles = AppRole.Customer)]
        [HttpGet]
        public async Task<ApiResponse<Pagination<ContractViewDTO>>> GetContractsByLoginCustomer(int pageIndex = 0, int pageSize = 10) => await _contractService.GetContractsByLoginCustomerAsync(pageIndex, pageSize);

        [Authorize(Roles = AppRole.Customer)]
        [HttpPut]
        public async Task<ApiResponse<string>> UpdateStatusContract(string contractId, int status) => await _contractService.UpdateStatusContractAsync(contractId, status);

        [Authorize(Roles = AppRole.Admin + "," + AppRole.Staff)]
        [HttpPut]
        public async Task<ApiResponse<string>> UpdateStatusOrderProcessing(string contractId, int status) => await _contractService.UpdateStatusOrderProcessingAsync(contractId, status);

        [Authorize(Roles = AppRole.Customer)]
        [HttpPut]
        public async Task<ApiResponse<string>> PayRemainingCostContractCustomer(string contractId) => await _contractService.PayRemainingCostContractCustomerAsync(contractId);
    }
}