using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.FeedBackDTO
{
    public class FeedBackDTO
    {
        public string? UserId { get; set; }
    
        public int Status {  get; set; }
        public string? Details { get; set; }
        public string Title { get; set; }
        public Guid ProductId { get; set; }
     
    }
}
