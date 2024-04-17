using eFurnitureProject.Application.Commons;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByPhoneNumberAsync(string phoneNumber);
        Task<List<string>> GetRolesByUserId(string userId);
        Task<Pagination<User>> GetUsersByFilter
        (string search, string role, int pageIndex = 1, int pageSize = 10);
        Task<bool> CheckPhoneNumberExisted(string phoneNumber);
        Task<bool> CheckEmailNameExisted(string emailName);
        Task<bool> CheckUserNameExisted(string userName);
        Task<User> GetUserByUserNameAndPassword(string username, string password);
        Task AddAsync(User user);
    }
}
