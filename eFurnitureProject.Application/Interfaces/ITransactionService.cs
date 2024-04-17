using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.TransactionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<ApiResponse<Pagination<TransactionViewDTO>>> FilterTransactionByLoginAsync(FilterTransactionByLoginDTO filterTransactionByLoginDTO);
        Task<ApiResponse<Pagination<TransactionViewDTO>>> FilterTransactionAsync(FilterTransactionDTO filterTransactionDTO);
        Task<ApiResponse<TransactionViewDTO>> GetTransactionByIdAsync(Guid transactionId);
    }
}
