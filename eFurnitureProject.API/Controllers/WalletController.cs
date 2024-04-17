using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.WalletViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class WalletController : BaseController
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }
        [Authorize(Roles = AppRole.Admin)]
        [HttpPost]
        public async Task<ApiResponse<string>> AddMoneyByUserId(UpdateWalletDTO updateWalletDTO) =>
            await _walletService.AddMoneyByUserIdAsync(updateWalletDTO);
        [Authorize(Roles = AppRole.Admin)]
        [HttpPost]
        public async Task<ApiResponse<string>> SubtractMoneyByUserId(UpdateWalletDTO updateWalletDTO) =>
            await _walletService.SubtractMoneyByUserIdAsync(updateWalletDTO);
        [HttpPost]
        public async Task<IActionResult> MoMoApi(MoMoDTO moMoDTO)
        {
            await _walletService.MoMoApi(moMoDTO);
            return StatusCode(200);
        }
    }
}
