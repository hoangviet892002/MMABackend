using eFurnitureProject.Application.Commons;
using eFurnitureProject.Domain.Entities;

namespace eFurnitureProject.Application.Repositories
{
    public interface IContractRepository : IGenericRepository<Contract>
    {
        Task<Pagination<Contract>> GetContractToPagination(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Contract>> GetContractByLoginCustomerToPagination(string customerId, int pageNumber = 0, int pageSize = 10);
        Task<Contract> GetContractWithDetail(Guid contractId);

        //Task<Pagination<Contract>> GetContractByFilter(int pageIndex, int pageSize, int? status, DateTime? fromTime, DateTime? toTime, string? search);
    }
}
