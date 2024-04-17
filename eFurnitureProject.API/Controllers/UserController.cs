using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;   
        }
        [Authorize(Roles = AppRole.Admin)]
        [HttpGet]
        public async Task<ApiResponse<Pagination<UserDetailViewDTO>>> GetUserByFilter([FromQuery] FilterUserDTO filterUserDTO) =>
            await _userService.GetUsersByFilter(filterUserDTO);
        [Authorize(Roles = AppRole.Admin)]
        [HttpGet]
        public async Task<ApiResponse<UserDetailViewDTO>> GetUserByID(string userID) =>
            await _userService.GetUserByID(userID);
        [Authorize]
        [HttpGet]
        public async Task<ApiResponse<UserViewDTO>> GetUserInformationByLogin() =>
            await _userService.GetUserInformationByLogin();
        [Authorize]
        [HttpPut]
        public async Task<ApiResponse<string>> ChangePassword(UserPasswordDTO userPasswordDTO) =>
            await _userService.ChangePassword(userPasswordDTO);
        [Authorize]
        [HttpPut]
        public async Task<ApiResponse<UserViewDTO>> UpdateUser(UserUpdateDTO userUpdateDTO) =>
            await _userService.UpdateUser(userUpdateDTO);
        [Authorize(Roles = AppRole.Admin)]
        [HttpPut]
        public async Task<ApiResponse<string>> BanAndUnbanUser(string userID) =>
            await _userService.BanAndUnbanUser(userID);
        [Authorize(Roles = AppRole.Admin)]
        [HttpPost]
        public async Task<ApiResponse<CreateUserDTO>> CreateUser(CreateUserDTO createUserDTO) =>
            await _userService.CreateUser(createUserDTO);
    }
}
