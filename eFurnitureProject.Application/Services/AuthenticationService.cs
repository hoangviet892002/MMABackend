using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Utils;
using eFurnitureProject.Application.ViewModels.RefreshTokenModels;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using eFurnitureProject.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace eFurnitureProject.Application.Services
{

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        private readonly SignInManager<User> _signInManager;
        private readonly AppConfiguration _appConfiguration;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IValidator<UserRegisterDTO> _validatorRegister;
        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime,
            SignInManager<User> signInManager, AppConfiguration appConfiguration, UserManager<User> userManager,
            IValidator<UserRegisterDTO> validatorRegister, RoleManager<Role> roleManager)
        {

            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _mapper = mapper;
            _appConfiguration = appConfiguration;
            _userManager = userManager;
            _validatorRegister = validatorRegister;
            _roleManager = roleManager;
        }
        public async Task<ApiResponse<TokenRefreshDTO>> LoginAsync(UserLoginDTO userLoginDTO)
        {
            var response = new ApiResponse<TokenRefreshDTO>();
            try
            {
                var result = await _signInManager.PasswordSignInAsync(
                    userLoginDTO.UserName, userLoginDTO.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await _unitOfWork.UserRepository.GetUserByUserNameAndPassword
                        (userLoginDTO.UserName, userLoginDTO.Password);
                    var userRole = await _userManager.GetRolesAsync(user);

                    var refreshToken = GenerateJsonWebTokenString.GenerateRefreshToken();

                    await _userManager.UpdateAsync(user);

                    var token = user.GenerateJsonWebToken(
                        _appConfiguration,
                        _appConfiguration.JwtOptions.Secret,
                        _currentTime.GetCurrentTime(),
                        userRole,
                        refreshToken
                        );
                    var refreshTokenEntity = new RefreshToken
                    {
                        JwtId = token.AccessToken,
                        UserId = user.Id,
                        Token = refreshToken,
                        IsUsed = false,
                        IsRevoked = false,
                        IssuedAt = DateTime.UtcNow,
                        ExpiredAt = DateTime.UtcNow.AddMonths(1)
                    };
                    _unitOfWork.RefreshTokenRepository.UpdateRefreshToken(refreshTokenEntity);
                    await _unitOfWork.SaveChangeAsync();
                    response.Data = token;
                    response.isSuccess = true;
                    response.Message = "Login is successful!";
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = "Username or password is not correct!";
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

        public async Task<ApiResponse<UserRegisterDTO>> RegisterAsync(UserRegisterDTO userRegisterDTO)
        {
            var response = new ApiResponse<UserRegisterDTO>();
            try
            {
                ValidationResult validationResult = await _validatorRegister.ValidateAsync(userRegisterDTO);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }

                if (await _unitOfWork.UserRepository.CheckUserNameExisted(userRegisterDTO.Email))
                {
                    response.isSuccess = false;
                    response.Message = "UserName is existed!";
                }
                else
                if (await _unitOfWork.UserRepository.CheckUserNameExisted(userRegisterDTO.UserName))
                {
                    response.isSuccess = false;
                    response.Message = "Email is existed!";
                }
                else
                if (await _unitOfWork.UserRepository.CheckPhoneNumberExisted(userRegisterDTO.PhoneNumber))
                {
                    response.isSuccess = false;
                    response.Message = "PhoneNumber is existed!";
                }
                else
                {
                    var user = _mapper.Map<User>(userRegisterDTO);
                    user.Wallet = 0;
                    var identityResult = await _userManager.CreateAsync(user, user.PasswordHash);
                    if (identityResult.Succeeded == true)
                    {
                        if (!await _roleManager.RoleExistsAsync(AppRole.Customer))
                        {
                            await _roleManager.CreateAsync(new Role { Name = AppRole.Customer });
                        }
                        await _userManager.AddToRoleAsync(user, AppRole.Customer);
                        var cart = new Cart()
                        {
                            UserId = user.Id
                        };
                        await _unitOfWork.CartRepository.AddAsync(cart);
                        await _unitOfWork.SaveChangeAsync();
                        response.Data = userRegisterDTO;
                        response.isSuccess = true;
                        response.Message = "Register is successful!";

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

        public async Task<ApiResponse<TokenRefreshDTO>> RenewTokenAsync(TokenRefreshDTO tokenRefreshDTO)
        {
            var response = new ApiResponse<TokenRefreshDTO>();
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var tokenValidateParam = new TokenValidationParameters
            {

                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _appConfiguration.JwtOptions.Issuer,
                ValidAudience = _appConfiguration.JwtOptions.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfiguration.JwtOptions.Secret)),
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };
            try
            {

                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRefreshDTO.AccessToken, tokenValidateParam, out var validatedToken);
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                    {
                        response.isSuccess = false;
                        response.Message = "Refresh token does not exist";
                        return response;
                    }
                }
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expireDate = GenerateJsonWebTokenString.ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                {
                    response.isSuccess = false;
                    response.Message = "Access token has not yet expired";
                    return response;
                }
                var storedToken = await _unitOfWork.RefreshTokenRepository.GetRefreshTokenByTokenAsync(tokenRefreshDTO.RefreshToken);
                if (storedToken == null)
                {
                    response.isSuccess = false;
                    response.Message = "Refresh token does not exist";
                    return response;
                }
                if (storedToken.IsUsed)
                {
                    response.isSuccess = false;
                    response.Message = "Refresh token has been used";
                    return response;
                }
                if (storedToken.IsRevoked)
                {
                    response.isSuccess = false;
                    response.Message = "Refresh token has been revoked";
                    return response;
                }
                /*var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;*/


                if (storedToken.JwtId != tokenRefreshDTO.AccessToken)
                {
                    response.isSuccess = false;
                    response.Message = "Refresh token do not match!";
                    return response;
                }

                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _unitOfWork.RefreshTokenRepository.UpdateRefreshToken(storedToken);
                await _unitOfWork.SaveChangeAsync();
                var refreshToken = GenerateJsonWebTokenString.GenerateRefreshToken();
                var user = await _userManager.FindByIdAsync(storedToken.UserId);
                var userRole = await _userManager.GetRolesAsync(user);
                var token = user.GenerateJsonWebToken(_appConfiguration,
                        _appConfiguration.JwtOptions.Secret,
                        _currentTime.GetCurrentTime(),
                        userRole,
                        refreshToken
                        );

                var refreshTokenEntity = new RefreshToken
                {
                    JwtId = token.AccessToken,
                    UserId = user.Id,
                    Token = refreshToken,
                    IsUsed = false,
                    IsRevoked = false,
                    IssuedAt = DateTime.UtcNow,
                    ExpiredAt = DateTime.UtcNow.AddMonths(1)
                };
                _unitOfWork.RefreshTokenRepository.UpdateRefreshToken(refreshTokenEntity);
                await _unitOfWork.SaveChangeAsync();
                response.Data = token;
                response.isSuccess = true;
                response.Message = "Refresh Successful!";
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;

            }
            return response;
        }
        public async Task<ApiResponse<string>> LogoutAsync(string refreshToken)
        {
            var response = new ApiResponse<string>();
            try
            {
                var storedToken = await _unitOfWork.RefreshTokenRepository.GetRefreshTokenByTokenAsync(refreshToken);
                if (storedToken == null)
                {
                    response.isSuccess = false;
                    response.Message = "Refresh token does not exist";
                    return response;
                }
                if (storedToken.IsRevoked || storedToken.IsUsed)
                {
                    response.isSuccess = false;
                    response.Message = "Refresh token has been revoked";
                    return response;
                }
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _unitOfWork.RefreshTokenRepository.UpdateRefreshToken(storedToken);
                await _unitOfWork.SaveChangeAsync();
                response.isSuccess = true;
                response.Message = "Logout Successful!";
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
                return response;

            }
            return response;
        }
    }
}
