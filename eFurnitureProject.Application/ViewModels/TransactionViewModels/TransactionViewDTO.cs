using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.TransactionViewModels
{
    public class TransactionViewDTO : BaseEntity
    {
        public double BalanceRemain { get; set; }
        public string? Type { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public double? Amount { get; set; }
        public int? Status { get; set; }
        public string? Description { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? OrderProcessingId { get; set; }
        public string? UserId { get; set; }
        
    }
}
