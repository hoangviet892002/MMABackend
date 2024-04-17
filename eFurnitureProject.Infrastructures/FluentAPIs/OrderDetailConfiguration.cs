using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.FluentAPIs
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(x => new { x.OrderId, x.ProductId });
            builder.HasOne(a => a.Order)
                .WithMany(a => a.OrderDetail)
                .HasForeignKey(a => a.OrderId);
            builder.HasOne(a => a.Product)
                .WithMany(a => a.OrderDetail)
                .HasForeignKey(a => a.ProductId);
        }
    }
}