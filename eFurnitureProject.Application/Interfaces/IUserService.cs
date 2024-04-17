using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<Pagination<UserDetailViewDTO>>> GetUsersByFilter(FilterUserDTO filterUserDTO);
        Task<ApiResponse<UserDetailViewDTO>> GetUserByID(string userID);
        Task<ApiResponse<UserViewDTO>> GetUserInformationByLogin();
        Task<ApiResponse<string>> ChangePassword(UserPasswordDTO userPasswordDTO);
        Task<ApiResponse<UserViewDTO>> UpdateUser(UserUpdateDTO userUpdateDTO);
        Task<ApiResponse<string>> BanAndUnbanUser(string userID);
        Task<ApiResponse<CreateUserDTO>> CreateUser(CreateUserDTO createUserDTO);
    }
}
