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
    public class OrderProcessingDetailConfiguration : IEntityTypeConfiguration<OrderProcessingDetail>
    {
        public void Configure(EntityTypeBuilder<OrderProcessingDetail> builder)
        {
            builder.HasKey(x => new { x.OrderProcessingId, x.ProductId });
            builder.HasOne(a => a.OrderProcessing)
                .WithMany(a => a.OrderProcessingDetail)
                .HasForeignKey(a => a.OrderProcessingId);
            builder.HasOne(a => a.Product)
                .WithMany(a => a.OrderProcessingDetail)
                .HasForeignKey(a => a.ProductId);
        }
    }
}
