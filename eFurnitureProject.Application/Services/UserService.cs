using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Application.Utils;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using eFurnitureProject.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimsService _claimsService;
        private readonly ICurrentTime _currentTime;
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IValidator<UserPasswordDTO> _userPasswordValidation;
        private readonly IValidator<UserUpdateDTO> _userUpdateValidation;
        private readonly IValidator<CreateUserDTO> _createUserValidation;

        public UserService(IUnitOfWork unitOfWork, ICurrentTime currentTime,
            IClaimsService claimsService, IMapper mapper,
            RoleManager<Role> roleManager,UserManager<User> userManager,
            IValidator<UserPasswordDTO> userPasswordValidation,
            IValidator<UserUpdateDTO> userUpdateValidation,
            IValidator<CreateUserDTO> createUserValidation)

        {
            _claimsService = claimsService;
            _currentTime = currentTime; 
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
            _userPasswordValidation = userPasswordValidation;
            _userUpdateValidation = userUpdateValidation;
            _createUserValidation = createUserValidation;
        }


        public async Task<ApiResponse<Pagination<UserDetailViewDTO>>> GetUsersByFilter(FilterUserDTO filterUserDTO)
        {
            var response = new ApiResponse<Pagination<UserDetailViewDTO>>();
            try
            {
                
                var pagination = await _unitOfWork.UserRepository.GetUsersByFilter
                (filterUserDTO.search, GetRole.GetRoleName(filterUserDTO.role), filterUserDTO.pageIndex, filterUserDTO.pageSize);

                var usersDto = _mapper.Map<List<UserDetailViewDTO>>(pagination.Items);

                foreach (var userDto in usersDto)
                {
                    var roles = await _unitOfWork.UserRepository.GetRolesByUserId(userDto.Id);
                    userDto.Roles = roles;
                }
                if (usersDto == null) 
                {
                    response.isSuccess = false;
                    response.Message = "Not found users!";
                }
                var paginationDTO = new Pagination<UserDetailViewDTO>
                {
                    PageIndex = pagination.PageIndex,
                    PageSize = pagination.PageSize,
                    TotalItemsCount = pagination.TotalItemsCount,
                    Items = usersDto
                };

                response.Data = paginationDTO;
                response.isSuccess = true;
                response.Message = "Get information successful!";
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

        public async Task<ApiResponse<UserDetailViewDTO>> GetUserByID(string userID)
        {
            var response = new ApiResponse<UserDetailViewDTO>();
            try
            {
                var user = await _userManager.FindByIdAsync(userID);
                var userData = _mapper.Map<UserDetailViewDTO>(user);
                if (user == null)
                {
                    response.isSuccess = false;
                    response.Message = "Get information fail!";
                    return response;
                }
                response.Data = userData;
                response.isSuccess = true;
                response.Message = "Get information successful!";
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

        public async Task<ApiResponse<UserViewDTO>> GetUserInformationByLogin()
        {
            var response = new ApiResponse<UserViewDTO>();
            try
            {
                var userID = _claimsService.GetCurrentUserId;
                var user = await _userManager.FindByIdAsync(userID.ToString());
                var userData = _mapper.Map<UserViewDTO>(user);
                if (user == null)
                {
                    response.isSuccess = false;
                    response.Message = "Get information fail!";
                    return response;
                }
                response.Data = userData;
                response.isSuccess = true;
                response.Message = "Get information successful!";
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

        public async Task<ApiResponse<string>> ChangePassword(UserPasswordDTO userPasswordDTO)
        {
            var response = new ApiResponse<string>();
            try
            {
                ValidationResult validationResult = await _userPasswordValidation.ValidateAsync(userPasswordDTO);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                var userID = _claimsService.GetCurrentUserId;
                var user = await _userManager.FindByIdAsync(userID.ToString());
                var newUser = await _userManager.ChangePasswordAsync(user, userPasswordDTO.OldPassword, userPasswordDTO.NewPassword);
                if (!newUser.Succeeded)
                {
                    response.isSuccess = false;
                    response.Message = "Change password fail!";
                    return response;
                }
                response.isSuccess = true;
                response.Message = "Change password successful!";
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

        public async Task<ApiResponse<UserViewDTO>> UpdateUser(UserUpdateDTO userUpdateDTO)
        {
            var response = new ApiResponse<UserViewDTO>();
            try
            {
                ValidationResult validationResult = await _userUpdateValidation.ValidateAsync(userUpdateDTO);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                var userID = _claimsService.GetCurrentUserId;
                var user = await _userManager.FindByIdAsync(userID.ToString());
                if (user == null) {
                    response.isSuccess = false;
                    response.Message = "login to access!";
                    return response;
                }
                if (userUpdateDTO.Email != null && userUpdateDTO.Email != await _userManager.GetEmailAsync(user))
                {
                    if (await _unitOfWork.UserRepository.CheckEmailNameExisted(userUpdateDTO.Email))
                    {
                        response.isSuccess = false;
                        response.Message = "Email is existed!";
                        return response;
                    }
                    user.Email = userUpdateDTO.Email;
                }

                if (userUpdateDTO.PhoneNumber != null && userUpdateDTO.PhoneNumber != await _userManager.GetPhoneNumberAsync(user))
                {
                    if (await _unitOfWork.UserRepository.CheckPhoneNumberExisted(userUpdateDTO.PhoneNumber))
                    {
                        response.isSuccess = false;
                        response.Message = "PhoneNumber is existed!";
                        return response;
                    }
                    user.PhoneNumber = userUpdateDTO.PhoneNumber;
                }
                if (userUpdateDTO.Gender != null) user.Gender = userUpdateDTO.Gender;
                if (userUpdateDTO.DateOfBird != null) user.DateOfBird = userUpdateDTO.DateOfBird;
                if (userUpdateDTO.Address != null) user.Address = userUpdateDTO.Address;
                if (userUpdateDTO.Name != null) user.Name = userUpdateDTO.Name;
                var newUser = await _userManager.UpdateAsync(user);
                var userData = _mapper.Map<UserViewDTO>(user);
                if (!newUser.Succeeded)
                {
                    response.isSuccess = false;
                    response.Message = "Update information fail!";
                    return response;
                }
                response.Data = userData;
                response.isSuccess = true;
                response.Message = "Update information successful!";
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

        public async Task<ApiResponse<string>> BanAndUnbanUser(string userID)
        {
            var response = new ApiResponse<string>();
            try
            {
                var user = await _userManager.FindByIdAsync(userID);

                if (user != null)
                {
                    if (user.LockoutEnd == null || user.LockoutEnd <= DateTimeOffset.UtcNow)
                    {
                        
                        var lockoutEndDate = DateTimeOffset.MaxValue; 
                        await _userManager.SetLockoutEndDateAsync(user, lockoutEndDate);
                        response.Message = "User has been banned.";
                    }
                    else
                    {
                        
                        await _userManager.SetLockoutEndDateAsync(user, null); 
                        response.Message = "User has been unbanned.";
                    }

                    response.isSuccess = true;
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = "User not found.";
                }
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

        public async Task<ApiResponse<CreateUserDTO>> CreateUser(CreateUserDTO createUserDTO)
        {
            var response = new ApiResponse<CreateUserDTO>();
            try
            {
                ValidationResult validationResult = await _createUserValidation.ValidateAsync(createUserDTO);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                if (await _unitOfWork.UserRepository.CheckUserNameExisted(createUserDTO.Email))
                {
                    response.isSuccess = false;
                    response.Message = "UserName is existed!";
                    return response;
                }
                if (await _unitOfWork.UserRepository.CheckUserNameExisted(createUserDTO.UserName))
                {
                    response.isSuccess = false;
                    response.Message = "Email is existed!";
                    return response;
                }
                if (await _unitOfWork.UserRepository.CheckPhoneNumberExisted(createUserDTO.PhoneNumber))
                {
                    response.isSuccess = false;
                    response.Message = "PhoneNumber is existed!";
                    return response;
                }
                var user = _mapper.Map<User>(createUserDTO);
                user.Wallet = 0;
                var identityResult = await _userManager.CreateAsync(user, createUserDTO.Password);
                if (identityResult.Succeeded == true)
                {
                    var role = GetRole.GetRoleName(createUserDTO.Role);
                    if (role != null)
                    {
                        if (!await _roleManager.RoleExistsAsync(role))
                        {
                            await _roleManager.CreateAsync(new Role { Name = role });
                        }
                        await _userManager.AddToRoleAsync(user, role);
                        response.Data = createUserDTO;
                        response.isSuccess = true;
                        response.Message = "Register is successful!";
                    }
                    else
                    {
                        response.isSuccess = false;
                        response.Message = "Get role from user fail!";
                    }
                }
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
