using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.UserViewModels
{
    public class CreateUserDTO
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBird { get; set; }
        public string? Gender { get; set; }
        public int Role { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
