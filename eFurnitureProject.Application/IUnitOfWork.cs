 using eFurnitureProject.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application
{
    public interface IUnitOfWork
    {
        public IRefreshTokenRepository RefreshTokenRepository { get; }
        public IAppointmentRepository AppointmentRepository { get; }
        public IAppointmentDetailRepository AppointmentDetailRepository { get; }
        public ICartDetailRepository CartDetailRepository { get; }
        public ICartRepository CartRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IContractRepository ContractRepository { get; }
        public IFeedbackRepository FeedbackRepository { get; }
        public IImportRepository ImportRepository { get; }
        public IImportDetailRepository ImportDetailRepository { get; }
        public IOrderDetailRepository OrderDetailRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IOrderProcessingRepository OrderProcessingRepository { get; }
        public IOrderProcessingDetailRepository OrderProcessingDetailRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IStatusOrderProcessingRepository StatusOrderProcessingRepository { get; }
        public IStatusOrderRepository StatusOrderRepository { get; }
        public ITransactionRepository TransactionRepository { get; }
        public IUserRepository UserRepository { get; }
        public IVoucherRepository VoucherRepository { get; }
        public IVoucherDetailRepository VoucherDetailRepository { get; }
        public IResponseRepository ResponseRepository { get; }
        public IDashBoardRepository DashBoardRepository { get; }

        public IOrderTrackingStatusRepository OrderTrackingStatusRepository { get; }

        public Task<int> SaveChangeAsync();
    }
}
