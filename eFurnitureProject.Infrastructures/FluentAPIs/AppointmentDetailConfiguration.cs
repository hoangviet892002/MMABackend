using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.FluentAPIs
{
    public class AppointmentDetailConfiguration : IEntityTypeConfiguration<AppointmentDetail>
    {
        public void Configure(EntityTypeBuilder<AppointmentDetail> builder)
        {
            builder.HasKey(x => new { x.UserId, x.AppointmentId });
            builder.HasOne(a => a.User)
                .WithMany(a => a.AppointmentDetail)
                .HasForeignKey(a => a.UserId);
            builder.HasOne(a => a.Appointment)
                .WithMany(a => a.AppointmentDetail)
                .HasForeignKey(a => a.AppointmentId);
        }
    }
}
