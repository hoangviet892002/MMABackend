using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.UserViewModels
{
    public class UserDetailViewDTO : IdentityUser
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public DateTime? DateOfBird { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public double? Wallet { get; set; }
        public bool Status { get; set; }
        public List<string>? Roles { get; set; }
    }
}
