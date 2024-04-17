using eFurnitureProject.Application;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext _dbContext;

        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAppointmentDetailRepository _appointmentDetailRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IImportRepository _importRepository;
        private readonly IImportDetailRepository _importDetailRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IOrderProcessingRepository _orderProcessingRepository;
        private readonly IOrderProcessingDetailRepository _orderProcessingDetailRepository;
        private readonly IProductRepository _productRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IStatusOrderProcessingRepository _statusOrderProcessingRepository;
        private readonly IStatusOrderRepository  _statusOrderRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IVoucherDetailRepository _voucherDetailRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IResponseRepository _responseRepository;
        private readonly IDashBoardRepository _dashBoardRepository;
        private readonly IOrderTrackingStatusRepository _orderTrackingStatusRepository;
        public UnitOfWork(AppDbContext dbContext, IAppointmentRepository appointmentRepository,
            IAppointmentDetailRepository appointmentDetailRepository, ICartDetailRepository cartDetailRepository,
            ICartRepository cartRepository, ICategoryRepository categoryRepository,
            IContractRepository contractRepository, IFeedbackRepository feedbackRepository, 
            IImportRepository importRepository, IImportDetailRepository importDetailRepository, 
            IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, 
            IOrderProcessingRepository orderProcessingRepository, IOrderProcessingDetailRepository orderProcessingDetailRepository, 
            IProductRepository productRepository, IRoleRepository roleRepository, IStatusOrderProcessingRepository statusOrderProcessingRepository, 
            IStatusOrderRepository statusOrderRepository, ITransactionRepository transactionRepository, 
            IUserRepository userRepository, IVoucherRepository voucherRepository, 
            IVoucherDetailRepository voucherDetailRepository, IRefreshTokenRepository refreshTokenRepository,IResponseRepository responseRepository,IDashBoardRepository dashBoardRepository, IOrderTrackingStatusRepository orderTrackingStatusRepository)
   
        {   
            _refreshTokenRepository = refreshTokenRepository;
            _appointmentRepository = appointmentRepository;
            _dbContext = dbContext;
            _appointmentRepository = appointmentRepository;
            _appointmentDetailRepository = appointmentDetailRepository;
            _cartDetailRepository = cartDetailRepository;
            _cartRepository = cartRepository;
            _categoryRepository = categoryRepository;
            _contractRepository = contractRepository;
            _feedbackRepository = feedbackRepository;
            _importRepository = importRepository;
            _importDetailRepository = importDetailRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _orderProcessingRepository = orderProcessingRepository;
            _orderProcessingDetailRepository = orderProcessingDetailRepository;
            _productRepository = productRepository;
            _roleRepository = roleRepository;
            _statusOrderProcessingRepository = statusOrderProcessingRepository;
            _statusOrderRepository = statusOrderRepository;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _voucherRepository = voucherRepository;
            _voucherDetailRepository = voucherDetailRepository;
            _responseRepository = responseRepository;
            _dashBoardRepository = dashBoardRepository;
            _orderTrackingStatusRepository = orderTrackingStatusRepository;
        
          
        }
        public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository;
        public IAppointmentRepository AppointmentRepository => _appointmentRepository;
        public IAppointmentDetailRepository AppointmentDetailRepository => _appointmentDetailRepository;
        public ICartRepository ICartRepository => _cartRepository;
        public ICartDetailRepository CartDetailRepository => _cartDetailRepository;
        public ICategoryRepository CategoryRepository => _categoryRepository;
        public IContractRepository ContractRepository => _contractRepository;
        public IFeedbackRepository FeedbackRepository => _feedbackRepository;
        public IImportRepository ImportRepository => _importRepository;
        public IImportDetailRepository ImportDetailRepository => _importDetailRepository;
        public IOrderRepository OrderRepository => _orderRepository;
        public IOrderDetailRepository OrderDetailRepository => _orderDetailRepository;
        public IOrderProcessingRepository OrderProcessingRepository => _orderProcessingRepository;
        public IOrderProcessingDetailRepository OrderProcessingDetailRepository => _orderProcessingDetailRepository;
        public IProductRepository ProductRepository => _productRepository;
        public IRoleRepository RoleRepository => _roleRepository;
        public IStatusOrderRepository StatusOrderRepository => _statusOrderRepository;
        public IStatusOrderProcessingRepository StatusOrderProcessingRepository => _statusOrderProcessingRepository;
        public ITransactionRepository TransactionRepository => _transactionRepository;
        public IUserRepository UserRepository => _userRepository;
        public IVoucherDetailRepository VoucherDetailRepository => _voucherDetailRepository;
        public IVoucherRepository VoucherRepository => _voucherRepository;

        public ICartRepository CartRepository => _cartRepository;
        public IResponseRepository ResponseRepository => _responseRepository;
      

        public IDashBoardRepository DashBoardRepository => _dashBoardRepository;

        public IOrderTrackingStatusRepository OrderTrackingStatusRepository => _orderTrackingStatusRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
