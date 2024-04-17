using eFurnitureProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Utils
{
    public static class GetRole
    {
        public static string? GetRoleName(int? role)
        {
            switch (role)
            {
                case (int)RolesEnum.Administrator:
                    return "Administrator";
                case (int)RolesEnum.Customer:
                    return "Customer";
                case (int)RolesEnum.Staff:
                    return "Staff";
                case (int)RolesEnum.DeliveryStaff:
                    return "DeliveryStaff";
                default:
                    return null;
            }
        }
    }
}
