using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.UserViewModels
{
    public class FilterUserDTO
    {
        public string? search { get; set; }
        public int? role { get; set; }
        public int pageIndex { get; set; } = 0;
        public int pageSize { get; set; } = 10;
    }
}
