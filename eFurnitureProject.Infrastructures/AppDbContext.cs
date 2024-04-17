using eFurnitureProject.Domain.Entities;
using eFurnitureProject.Infrastructures.FluentAPIs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures
{
    public class AppDbContext : IdentityDbContext<User, Role, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        #region Dbset
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentDetail> AppointmentDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Import> Imports { get; set; }
        public DbSet<ImportDetail> ImportsDetail { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrdersDetails { get; set;}
        public DbSet<OrderProcessing> OrderProcessings { get; set; }
        public DbSet<OrderProcessingDetail> OrderProcessingDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<StatusOrder> StatusOrders { get; set; }
        public DbSet<StatusOrderProcessing> StatusOrderProcessings { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherDetail> VouchersDetails { get; set;}
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<OrderTrackingStatus> OrderTrackingStatus { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppointmentDetailConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CartDetailConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ImportDetailConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDetailConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderProcessingDetailConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VoucherDetailConfiguration).Assembly);
        }
    }
}
