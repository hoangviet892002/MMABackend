using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.DashBoardViewModel
{
    public class Top5UserDTO
    {
        public string? UserName { get; set;}
        public string? PhoneNumber { get; set;}
        public string? UserEmail { get; set;}
        public double TotalMoney {  get; set;}
    }
}
