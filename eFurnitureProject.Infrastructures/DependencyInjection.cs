using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Domain.Entities;
using eFurnitureProject.Infrastructures.Repositories;
using eFurnitureProject.Infrastructures.Mappers;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using eFurnitureProject.Application;
using eFurnitureProject.Infrastructures.DataInitializer;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using FluentValidation;

namespace eFurnitureProject.Infrastructures
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructuresService(this IServiceCollection services, string databaseConnection)
        {
            #region Service DI
            services.AddScoped<ICurrentTime, CurrentTime>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IImportService, ImportService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IResponseService ,ResponseService>();
            services.AddScoped<IFeedBackService, FeedBackService>();
            services.AddScoped<IDashBoardService,DashBoardService>();
       
            #endregion

            #region Repository DI
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IAppointmentDetailRepository, AppointmentDetailRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartDetailRepository, CartDetailRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IImportRepository, ImportRepository>();
            services.AddScoped<IImportDetailRepository, ImportDetailRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IOrderProcessingRepository, OrderProcessingRepository>();
            services.AddScoped<IOrderProcessingDetailRepository, OrderProcessingDetailRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IStatusOrderProcessingRepository, StatusOrderProcessingRepository>();
            services.AddScoped<IStatusOrderRepository, StatusOrderRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<IVoucherDetailRepository, VoucherDetailRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IResponseRepository ,ResponseRepository>();
            services.AddScoped<IDashBoardRepository,DashBoardRepository>();
            services.AddScoped<IOrderTrackingStatusRepository, OrderTrackingStatusRepository>();

            #endregion
            services.AddIdentity<User, Role>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
         

            services.Configure<IdentityOptions>(options =>
            {
                // Set your desired password requirements here
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6; // Set your desired minimum length
                options.Password.RequiredUniqueChars = 0; // Set your desired number of unique characters
            });

            // ATTENTION: if you do migration please check file README.md
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(databaseConnection));

            services.AddAutoMapper(typeof(MapperConfigurationsProfile).Assembly);


            return services;
        }
    }
}
