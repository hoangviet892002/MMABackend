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
    public class CartDetailConfiguration : IEntityTypeConfiguration<CartDetail>
    {
        public void Configure(EntityTypeBuilder<CartDetail> builder)
        {
            builder.HasKey(x => new { x.CartId, x.ProductId });
            builder.HasOne(a => a.Cart)
                .WithMany(a => a.CartDetails)
                .HasForeignKey(a => a.CartId);  
            builder.HasOne(a => a.Product)
                .WithMany(a => a.CartDetail)
                .HasForeignKey(a => a.ProductId);
        }
    }
}
