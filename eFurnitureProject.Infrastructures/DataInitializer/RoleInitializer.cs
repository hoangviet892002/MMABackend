using eFurnitureProject.Application.Commons;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.DataInitializer
{
    public  class RoleInitializer
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleInitializer(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task RoleInitializeAsync()
        {
            string[] roleNames = { AppRole.Admin, AppRole.Customer, AppRole.Staff, AppRole.DeliveryStaff };


            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    roleResult = await _roleManager.CreateAsync(new Role { Name = roleName });
                }
            }
        }
    }
}