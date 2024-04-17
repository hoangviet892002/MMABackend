using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.CartViewModels
{
    public class AddProductToCartDTO
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
