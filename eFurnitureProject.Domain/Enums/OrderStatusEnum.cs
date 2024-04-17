using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Enums
{
    public enum OrderStatusEnum
    {
        Pending = 1, //Chờ Xác Nhận
        Delivering = 2, //Đang Giao Hàng
        Cancelled = 3, //Bị Hủy
        Delivered = 4, //Đã Giao Hàng
        Rejected = 5, //Từ Chối Xác Nhận
    }
}
