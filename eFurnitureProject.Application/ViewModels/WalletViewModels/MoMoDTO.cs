using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.WalletViewModels
{
    public class MoMoDTO
    {
        public string signature { get; set; }
        public string tranId { get; set; }
        public string? partnerId { get; set; }
        public string? partnerName { get; set;}
        public double amount { get; set; }
        public string comment { get; set; }
    }
}
