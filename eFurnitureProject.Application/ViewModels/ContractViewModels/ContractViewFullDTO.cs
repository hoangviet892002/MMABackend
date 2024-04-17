using eFurnitureProject.Application.ViewModels.OrderProcessingDetailViewModels;
using eFurnitureProject.Application.ViewModels.OrderProcessingViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.ContractViewModels
{
    public class ContractViewFullDTO
    {
        public string ContractID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public int Pay { get; set; }
        public int StatusContract { get; set; }
        public string CustomerId { get; set; }
        public string CustomerContractName { get; set; }
        public string CustomerOrderProcessName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public StatusOrderProcessingViewDTO StatusOrderProcessing { get; set; }
        public IEnumerable<OrderProcessingDetailViewDTO> item { get; set; }
    }
}
