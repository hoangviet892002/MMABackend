using eFurnitureProject.Domain.Entities;

namespace eFurnitureProject.Application.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<bool> CheckCategoryNameExisted(string name);

        Task<bool> CheckCategoryNameExisted(Guid categoryId, string name);
    }
}
