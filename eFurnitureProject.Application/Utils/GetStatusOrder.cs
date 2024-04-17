using eFurnitureProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Utils
{
    public class GetStatusOrder
    {
        public static string? GetStatusName(int status)
        {
            switch (status)
            {
                case (int)OrderStatusEnum.Pending:
                    return "Pending";
                case (int)OrderStatusEnum.Delivering:
                    return "Delivering";
                case (int)OrderStatusEnum.Cancelled:
                    return "Cancelled";
                case (int)OrderStatusEnum.Delivered:
                    return "Delivered";
                case (int)OrderStatusEnum.Rejected:
                    return "Rejected";
                default:
                    return null;
            }
        }
    }
}
