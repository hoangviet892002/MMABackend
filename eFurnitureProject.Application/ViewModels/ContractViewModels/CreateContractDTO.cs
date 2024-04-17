using eFurnitureProject.Application.ViewModels.OrderProcessingDetailViewModels;
using eFurnitureProject.Application.ViewModels.OrderProcessingViewModels;
using eFurnitureProject.Domain.Entities;

namespace eFurnitureProject.Application.ViewModels.ContractViewModels
{
    public class CreateContractDTO
    {
        public string CustomerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public int Pay { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public virtual ICollection<CreateOrderProcessingDetailDTO> Items { get; set; }
    }
}
