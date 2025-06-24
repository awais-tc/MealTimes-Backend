using MealTimes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTimes.Core.Repository
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<List<Order>> GetOrdersByEmployeeAsync(int employeeId);
        Task<List<Order>> GetOrdersForChefAsync(int chefId);
        Task<List<Order>> GetOrdersByCompanyAsync(int companyId);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int orderId);
        Task UpdateAsync(Order order);
        Task SaveChangesAsync();
    }
}
