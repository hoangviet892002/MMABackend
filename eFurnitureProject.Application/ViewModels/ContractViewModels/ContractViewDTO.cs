using eFurnitureProject.Application.ViewModels.OrderProcessingViewModels;

namespace eFurnitureProject.Application.ViewModels.ContractViewModels
{
    public class ContractViewDTO
    {
        public string ContractID { get; set; }
        public string Title { get; set; }
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
    }
}
