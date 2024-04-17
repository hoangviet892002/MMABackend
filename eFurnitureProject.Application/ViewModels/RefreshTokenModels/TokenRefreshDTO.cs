using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.RefreshTokenModels
{
    public class TokenRefreshDTO
    {
        public string RefreshToken { get; set; }

        public string AccessToken { get; set; }
    }
}
