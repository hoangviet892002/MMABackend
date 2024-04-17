using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.WalletViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IWalletService
    {
        Task<ApiResponse<string>> SubtractMoneyByUserIdAsync(UpdateWalletDTO updateWalletDTO);
        Task<ApiResponse<string>> AddMoneyByUserIdAsync(UpdateWalletDTO updateWalletDTO);
        Task<ApiResponse<string>> MoMoApi(MoMoDTO moMoDTO);
    }
}
