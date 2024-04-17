using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Application.ViewModels.OrderProcessingDetailViewModels;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IContractService
    {
        Task<ApiResponse<string>> CreateContractAsync(CreateContractDTO contract);
        Task<ApiResponse<Pagination<ContractViewDTO>>> GetContractPagingAsync(int pageIndex = 0, int pageSize = 10);
        Task<ApiResponse<Pagination<ContractViewDTO>>> GetContractsByLoginCustomerAsync(int pageIndex = 0, int pageSize = 10);
        Task<ApiResponse<ContractViewDTO>> UpdateContractAsync(Guid contractId, UpdateContractDTO contract);    
        Task<ApiResponse<ContractViewDTO>> SoftRemoveContractByIdAsync(Guid contractId);
        Task<ApiResponse<IEnumerable<OrderProcessingDetailViewDTO>>> GetContractItemAsync(string contractId);
        Task<ApiResponse<ContractViewFullDTO>> GetContractWithItemAsync(string contractId);
        Task<ApiResponse<string>> UpdateStatusContractAsync(string contractId, int status);
        Task<ApiResponse<string>> UpdateStatusOrderProcessingAsync(string contractId, int status);
        Task<ApiResponse<string>> PayRemainingCostContractCustomerAsync(string contractId);
        //Task<ApiResponse<Pagination<ContractViewDTO>>> GetContractByFilterAsync(FilterContractDTO filterContractDTO);
    }
}
