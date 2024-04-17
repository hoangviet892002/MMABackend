using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.FeedBackDTO
{
    public class FeedBackViewDTO
    {
        public string? UserId { get; set; }


        public string? Details { get; set; }
        public string Title { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
