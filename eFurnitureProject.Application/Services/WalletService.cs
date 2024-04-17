using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using eFurnitureProject.Application.ViewModels.WalletViewModels;
using eFurnitureProject.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<Role> _roleManager;
        private readonly IValidator<UpdateWalletDTO> _validatorUpdateWallet;
        private readonly AppConfiguration _appConfiguration;
        public WalletService(UserManager<User> userManager, RoleManager<Role> roleManager, 
            IValidator<UpdateWalletDTO> validatorUpdateWallet, IUnitOfWork unitOfWork,
            AppConfiguration appConfiguration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _validatorUpdateWallet = validatorUpdateWallet;
            _unitOfWork = unitOfWork;
            _appConfiguration = appConfiguration;
        }
        public async Task<ApiResponse<string>> MoMoApi(MoMoDTO moMoDTO)
        {
            var response = new ApiResponse<string>();
            try
            {
                var user = new User();
                var signature = _appConfiguration.MoMoConfig.Signature;
                if (signature != moMoDTO.signature) throw new Exception("signature is wrong!");
                user = await _unitOfWork.UserRepository.GetByPhoneNumberAsync(moMoDTO.partnerId);
                if (user != null)
                {
                    user.Wallet = user.Wallet + moMoDTO.amount;
                    await _userManager.UpdateAsync(user);
                    await _unitOfWork.TransactionRepository.AddAsync(
                    new Transaction
                    {
                        Amount = moMoDTO.amount,
                        From = "MoMo",
                        To = moMoDTO.partnerId,
                        Type = "3rd",
                        BalanceRemain = (double)user.Wallet,
                        UserId = user.Id,
                        Status = 1,
                        Description = $"Transfer {moMoDTO.amount:F2} from MoMo to {moMoDTO.partnerId}. Comment: {moMoDTO.comment}. {moMoDTO.tranId}",

                    });
                }
                else 
                {
                    await _unitOfWork.TransactionRepository.AddAsync(
                    new Transaction
                    {
                        Amount = moMoDTO.amount,
                        From = "MoMo",
                        To = moMoDTO.partnerId,
                        Type = "3rd",
                        Status = 1,
                        Description = $"Transfer {moMoDTO.amount:F2} from MoMo to {moMoDTO.partnerId}. Comment: {moMoDTO.comment}. {moMoDTO.tranId}",

                    });

                }
                
                await _unitOfWork.SaveChangeAsync();
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
        public async Task<ApiResponse<string>> AddMoneyByUserIdAsync(UpdateWalletDTO updateWalletDTO)
        {
            var response = new ApiResponse<string>();
            try 
            {
                ValidationResult validationResult = await _validatorUpdateWallet.ValidateAsync(updateWalletDTO);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }

                var user = await _userManager.FindByIdAsync(updateWalletDTO.UserId);

                if (user == null) throw new Exception("Not found user!");

                var checkRole = await _userManager.GetRolesAsync(user);
                if (!checkRole.Contains(AppRole.Customer)) throw new Exception("Only add money for customer!");

                user.Wallet  = user.Wallet + updateWalletDTO.Wallet;
                var isSuccess = await _userManager.UpdateAsync(user);

                if (!isSuccess.Succeeded) throw new Exception("Top-up fail!");

                await _unitOfWork.TransactionRepository.AddAsync(
                    new Transaction {
                        Amount = updateWalletDTO.Wallet,
                        From = "Admin",
                        To = user.Name,
                        Type = "System",
                        BalanceRemain = (double)user.Wallet,
                        UserId = user.Id,
                        Status = 1,
                        Description = $"Transfer {updateWalletDTO.Wallet:F2} from eFurniturePay to User Wallet by Admin",

                    });
                var createCheck = await _unitOfWork.SaveChangeAsync() > 0;
                if (!createCheck) throw new Exception("Create transaction failed!");
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

        public async Task<ApiResponse<string>> SubtractMoneyByUserIdAsync(UpdateWalletDTO updateWalletDTO)
        {
            var response = new ApiResponse<string>();
            try
            {
                ValidationResult validationResult = await _validatorUpdateWallet.ValidateAsync(updateWalletDTO);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }

                var user = await _userManager.FindByIdAsync(updateWalletDTO.UserId)
                    ;
                if (user == null) throw new Exception("Not found user!");
                var checkRole = await _userManager.GetRolesAsync(user);

                if (!checkRole.Contains(AppRole.Customer)) throw new Exception("Only subtract money for customer!");

                if (user.Wallet < updateWalletDTO.Wallet) throw new Exception("Insufficient balance in the account");

                user.Wallet = user.Wallet - updateWalletDTO.Wallet;
                var isSuccess = await _userManager.UpdateAsync(user);

                if(!isSuccess.Succeeded) throw new Exception("Withdraw fail!");
                await _unitOfWork.TransactionRepository.AddAsync(
                    new Transaction
                    {
                        Amount = updateWalletDTO.Wallet,
                        From = "Admin",
                        To = user.Name,
                        Type = "System",
                        BalanceRemain = (double)user.Wallet,
                        UserId = user.Id,
                        Status = 0,
                        Description = $"Transfer {updateWalletDTO.Wallet:F2} from User wallet to eFurniturePay by Admin",

                    });
                var createCheck = await _unitOfWork.SaveChangeAsync() > 0;
                if (!createCheck) throw new Exception("Create transaction failed!");
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
    }
}
