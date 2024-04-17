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
    public class VoucherDetailConfiguration : IEntityTypeConfiguration<VoucherDetail>
    {
        public void Configure(EntityTypeBuilder<VoucherDetail> builder)
        {
            builder.HasKey(x => new { x.VoucherId, x.UserId });
            builder.HasOne(a => a.Voucher)
                .WithMany(a => a.VoucherDetail)
                .HasForeignKey(a => a.VoucherId);
            builder.HasOne(a => a.User)
                .WithMany(a => a.VoucherDetail)
                .HasForeignKey(a => a.UserId);
        }
    }
}
