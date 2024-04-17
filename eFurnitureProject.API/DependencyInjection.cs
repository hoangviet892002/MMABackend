using eFurnitureProject.API.Services;
using eFurnitureProject.API.Validator.AppointmentValidator;
using eFurnitureProject.API.Validator.AuthenticationValidator;
using eFurnitureProject.API.Validator.CategoryValidator;
using eFurnitureProject.API.Validator.ContractValidator;
using eFurnitureProject.API.Validator.ProductValidator;
using eFurnitureProject.API.Validator.UserValidator;
using eFurnitureProject.API.Validator.ImportValidator;
using eFurnitureProject.API.Validator.InventoryValidator;
using eFurnitureProject.Application;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Services;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel;
using eFurnitureProject.Application.ViewModels.CategoryViewModels;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Application.ViewModels.ImportDetailViewModels;
using eFurnitureProject.Application.ViewModels.ImportViewModels;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using eFurnitureProject.Domain.Entities;
using eFurnitureProject.Infrastructures;
using eFurnitureProject.Infrastructures.DataInitializer;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using eFurnitureProject.Application.ViewModels.VoucherDTO;
using eFurnitureProject.API.Validator.VoucherValidator;
using eFurnitureProject.Application.ViewModels.WalletViewModels;
using eFurnitureProject.API.Validator.WalletValidator;
using eFurnitureProject.Application.ViewModels.OrderViewModels;
using eFurnitureProject.API.Validator.OrderValidator;

namespace eFurnitureProject.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "eFurnitureAPI", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddHealthChecks();
            services.AddSingleton<Stopwatch>();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddHttpContextAccessor();
            services.AddHostedService<SetupIdentityDataSeeder>();
            services.AddControllers();
            services.AddLogging();

            #region Seed
            services.AddScoped<RoleInitializer>();
            services.AddScoped<AccountInitializer>();
            services.AddScoped<StatusOrderInitializer>();
            services.AddScoped<StatusOrderProcessingInitializer>();
            #endregion
            #region Validator
            services.AddTransient<IValidator<UserRegisterDTO>, UserRegisterValidation>();
            services.AddTransient<IValidator<CreateContractDTO>, CreateContractViewModelValidation>();
            services.AddTransient<IValidator<UpdateContractDTO>, UpdateContractValidation>();
            services.AddTransient<IValidator<CreateProductDTO>, CreateProductValidation>();
            services.AddTransient<IValidator<CreateAppointmentDTO>,CreateAppointmentValidation>();
            services.AddTransient<IValidator<UserPasswordDTO>, ChangePasswordValidation>();
            services.AddTransient<IValidator<UserUpdateDTO>, UpdateUserValidation>();
            services.AddTransient<IValidator<CreateUserDTO>, CreateUserValidation>();
            services.AddTransient<IValidator<CreateCategoryViewModel>, CreateCategoryViewModelValidation>();
            services.AddTransient<IValidator<CreateImportDTO>, CreateImportValidation>();
            services.AddTransient<IValidator<CreateImportDetailDTO>, CreateImportDetailValidation>();
            services.AddTransient<IValidator<UpdateImportDTO>, UpdateImportValidation>();
            services.AddTransient<IValidator<CreateVoucherDTO>, CreateVoucherValidation>();
            services.AddTransient<IValidator<UpdateWalletDTO>,UpdateWalletValidation>();
            services.AddTransient<IValidator<CreateOrderDTO>, CreateOrderValidation>();
            #endregion

            return services;
        }
    }
}
