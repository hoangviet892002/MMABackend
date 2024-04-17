using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.CategoryViewModels;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using eFurnitureProject.Application.ViewModels.VoucherDTO;
using eFurnitureProject.Domain.Entities;
using eFurnitureProject.Application.ViewModels.OrderViewModels;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel;
using Microsoft.AspNetCore.Identity;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;
using eFurnitureProject.Application.ViewModels.ImportViewModels;
using eFurnitureProject.Application.ViewModels.ImportDetailViewModels;
using eFurnitureProject.Application.ViewModels.CartViewModels;
using eFurnitureProject.Application.ViewModels.StatusOrderViewModels;
using eFurnitureProject.Application.ViewModels.FeedBackDTO;
using eFurnitureProject.Application.ViewModels.OrderProcessingViewModels;
using eFurnitureProject.Application.ViewModels.OrderProcessingDetailViewModels;
using eFurnitureProject.Application.ViewModels.TransactionViewModels;
using eFurnitureProject.Application.ViewModels.DashBoardViewModel;

namespace eFurnitureProject.Infrastructures.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap<UserLoginDTO, User>();
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
            CreateMap<UserRegisterDTO, User>()
                .ForMember(dest => dest.PasswordHash, src => src.MapFrom(x => x.Password));
            CreateMap<ProductViewDTO, Product>();
            CreateMap<CreateVoucherDTO, Voucher>();
            CreateMap<VoucherViewDTO, Voucher>();
            CreateMap<Voucher, VoucherViewDTO>();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<ProductDTO,CreateProductDTO>().ReverseMap();
            CreateMap<Product,CreateProductDTO>();
            CreateMap<CreateProductDTO, Product>();
            CreateMap<Appointment, AppointmentDetail>();
            CreateMap<CreateContractDTO, Contract>()
                 .ForPath(dest => dest.OrderProcessing.Name, opt => opt.MapFrom(x => x.Name))
                 .ForPath(dest => dest.OrderProcessing.PhoneNumber, opt => opt.MapFrom(x => x.PhoneNumber))
                 .ForPath(dest => dest.OrderProcessing.Address, opt => opt.MapFrom(x => x.Address))
                 .ForPath(dest => dest.OrderProcessing.Price, opt => opt.MapFrom(x => x.Value))
                 .ForPath(dest => dest.OrderProcessing.Email, opt => opt.MapFrom(x => x.Email))
                 .ForPath(dest => dest.OrderProcessing.UserId, opt => opt.MapFrom(x => x.CustomerId))
                 .ForPath(dest => dest.OrderProcessing.OrderProcessingDetail, opt => opt.MapFrom(x => x.Items));
            CreateMap<Contract, ContractViewDTO>()
                 .ForMember(dest => dest.ContractID, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.StatusContract, opt => opt.MapFrom(src => src.Status))
                 .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                 .ForMember(dest => dest.CustomerContractName, opt => opt.MapFrom(src => src.Customer.Name))
                 .ForMember(dest => dest.CustomerOrderProcessName, opt => opt.MapFrom(src => src.OrderProcessing.Name))
                 .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.OrderProcessing.PhoneNumber))
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.OrderProcessing.Email))
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.OrderProcessing.Address))
                 .ForMember(dest => dest.StatusOrderProcessing, opt => opt.MapFrom(src => new StatusOrderProcessingViewDTO
                 {
                     StatusCode = src.OrderProcessing.StatusOrderProcessing.StatusCode,
                     Name = src.OrderProcessing.StatusOrderProcessing.Name
                 }));
            CreateMap<OrderProcessingDetail, OrderProcessingDetailViewDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : ""))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Product != null ? src.Product.Image : ""));
            CreateMap<Contract, ContractViewFullDTO>()
                 .ForMember(dest => dest.ContractID, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.StatusContract, opt => opt.MapFrom(src => src.Status))
                 .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                 .ForMember(dest => dest.CustomerContractName, opt => opt.MapFrom(src => src.Customer.Name))
                 .ForMember(dest => dest.CustomerOrderProcessName, opt => opt.MapFrom(src => src.OrderProcessing.Name))
                 .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.OrderProcessing.PhoneNumber))
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.OrderProcessing.Email))
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.OrderProcessing.Address))
                 .ForMember(dest => dest.StatusOrderProcessing, opt => opt.MapFrom(src => new StatusOrderProcessingViewDTO
                 {
                     StatusCode = src.OrderProcessing.StatusOrderProcessing.StatusCode,
                     Name = src.OrderProcessing.StatusOrderProcessing.Name
                 }))
                 .ForMember(dest => dest.item, opt => opt.MapFrom(src => src.OrderProcessing.OrderProcessingDetail));
            CreateMap<UpdateContractDTO, Contract>();
            CreateMap<CreateAppointmentDTO, Appointment>();
            CreateMap<AppointmentDetailDTO, AppointmentDetail>();
            CreateMap<Appointment, AppointmentDTO>();
            CreateMap<AppointmentDetail, AppointmentDetailDTO>();
            CreateMap<AppointmentDetailDTO, AppointmentDetail>();
            CreateMap<AppointmentDetail, AppointmentDetailDTO>();
            CreateMap<AppointmentDetailDTO, AppointmentDetail>().ReverseMap();
            CreateMap<CreateAppointmentDTO, AppointmentDetail>();
            CreateMap<User, AppointmentDetail>();
            CreateMap<AppointmentDetailDTO, User>();
            CreateMap<ProductViewDTO , Product>();
            CreateMap<Product,ProductViewDTO>();
            CreateMap<AppointmentDetail,CreateAppointmentDetailDTO>();
            CreateMap<Pagination<ProductDTO>, IEnumerable<ProductDTO>>();
            CreateMap<User, UserDetailViewDTO>();
            CreateMap<User, UserViewDTO>();
            CreateMap<CreateUserDTO, User>();
            CreateMap<OrderDetail, OrderDetailViewDTO>();
            CreateMap<Order, OrderViewDTO>();
            CreateMap<Order, OrderViewForCustomerDTO>()
                 .ForMember(dest => dest.StatusOrderViewDTO, opt => opt.MapFrom(src => new StatusOrderViewDTO
                 {
                     StatusCode = src.StatusOrder.StatusCode,
                     Name = src.StatusOrder.Name
                 }));
            CreateMap<CreateCategoryViewModel, Category>();
            CreateMap<Category, CategoryViewModel>()
                 .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id));
            CreateMap<OrderViewDTO, Order>();
            CreateMap<Order, OrderViewDTO>();
            CreateMap<CreateImportDTO, Import>();
            CreateMap<Import, ImportViewDTO>()
                 .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id));
            CreateMap<CreateImportDetailDTO, ImportDetail>();
            CreateMap<UpdateImportDTO, Import>();
            CreateMap<AddProductToCartDTO, CartDetail>();
            CreateMap<CartDetail, CartDetailViewDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId.ToString()))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : ""))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Product != null ? src.Product.Image : ""))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0));
            CreateMap<StatusOrder, StatusDetailOrderViewDTO>();
            CreateMap<ImportDetail, ImportDetailViewDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId.ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : ""))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Product != null ? src.Product.Image : ""));
            CreateMap<Cart, CartDetailViewDTO>();
            CreateMap<StatusOrder,StatusDetailOrderViewDTO>();
            CreateMap<Import, ImportViewFullDTO>()
                .ForMember(dest => dest.importDetailViewDTOs, opt => opt.MapFrom(src => src.ImportDetail));
            CreateMap<Feedback, FeedBackDTO>();
            CreateMap<FeedBackDTO, Feedback>();
            CreateMap<CreateFeedBackDTO, Feedback>();
            CreateMap<CreateOrderProcessingDetailDTO, OrderProcessingDetail>();
            CreateMap<CreateOrderDTO, Order>()
            .ForMember(dest => dest.VoucherId,opt => opt
            .MapFrom(src => string.IsNullOrEmpty(src.VoucherId) ? (Guid?)null : Guid.Parse(src.VoucherId)));
            CreateMap<Transaction, TransactionViewDTO>();
            CreateMap<CreateFeedBackDTO, Feedback>();
            CreateMap<OrderDetail, ProductTop5DTO>()
           .ForMember(dest => dest.TotalQuantity, opt => opt.MapFrom(src => src.Quantity));
            CreateMap<int, TotalFinishedOrderDTO>()
           .ForMember(dest => dest.TotalFinishedOrders, opt => opt.MapFrom(src => src));
            CreateMap<int, TotalUserDTO>()
          .ForMember(dest => dest.TotalUsers, opt => opt.MapFrom(src => src));
            CreateMap<int, TotalProcessOderDTO>()
             .ForMember(dest => dest.total, opt => opt.MapFrom(src => src));
            CreateMap<double, TotalMoneyDTO>()
                  .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src));
            CreateMap<ProductTopDTO, Product>();
            CreateMap<Product, ProductTopDTO>();
            CreateMap<User, Top5UserDTO>();

        }
    }
}
