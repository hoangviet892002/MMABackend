using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.TransactionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService) 
        {
            _transactionService = transactionService;
        }
        [Authorize]
        [HttpGet]
        public async Task<ApiResponse<TransactionViewDTO>> GetTransactionById(Guid transactionId) =>
            await _transactionService.GetTransactionByIdAsync(transactionId);
        [Authorize]
        [HttpGet]
        public async Task<ApiResponse<Pagination<TransactionViewDTO>>> FilterTransactionByLogin([FromQuery] FilterTransactionByLoginDTO filterTransactionByLoginDTO) =>
            await _transactionService.FilterTransactionByLoginAsync(filterTransactionByLoginDTO);
        [Authorize(Roles = AppRole.Admin + "," + AppRole.Staff)]
        [HttpGet]
        public async Task<ApiResponse<Pagination<TransactionViewDTO>>> FilterTransaction([FromQuery] FilterTransactionDTO filterTransactionDTO) =>
            await _transactionService.FilterTransactionAsync(filterTransactionDTO);
    }
}
