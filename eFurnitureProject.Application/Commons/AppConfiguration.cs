using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Commons
{
    public class AppConfiguration
    {
        public string DatabaseConnection { get; set; }

        public JwtOptions JwtOptions { get; set; }

        public MoMoConfig MoMoConfig { get; set; }
    }
}
