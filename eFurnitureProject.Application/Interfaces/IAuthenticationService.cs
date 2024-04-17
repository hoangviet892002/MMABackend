using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.RefreshTokenModels;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<UserRegisterDTO>> RegisterAsync(UserRegisterDTO userRegisterDTO);
        Task<ApiResponse<TokenRefreshDTO>> LoginAsync(UserLoginDTO userLoginDTO);
        Task<ApiResponse<TokenRefreshDTO>> RenewTokenAsync(TokenRefreshDTO tokenRefreshDTO);
        Task<ApiResponse<string>> LogoutAsync(string refreshToken);
    }
}
