using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.UserViewModels
{
    public class UserViewDTO 
    {
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBird { get; set; }
        public string? Gender { get; set; }
        public double? Wallet { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    
    }
}
