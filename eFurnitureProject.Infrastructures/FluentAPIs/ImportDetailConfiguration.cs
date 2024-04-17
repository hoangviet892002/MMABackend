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
    public class ImportDetailConfiguration : IEntityTypeConfiguration<ImportDetail>
    {
        public void Configure(EntityTypeBuilder<ImportDetail> builder)
        {
            builder.HasKey(x => new { x.ImportId, x.ProductId });
            builder.HasOne(a => a.Import)
                .WithMany(a => a.ImportDetail)
                .HasForeignKey(a => a.ImportId);
            builder.HasOne(a => a.Product)
                .WithMany(a => a.ImportDetail)
                .HasForeignKey(a => a.ProductId);
        }
    }
}

