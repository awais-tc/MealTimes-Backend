using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using MealTimes.Repository;
using Microsoft.EntityFrameworkCore;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return await _context.Orders
            .Include(o => o.OrderMeals)
                .ThenInclude(om => om.Meal)
            .FirstOrDefaultAsync(o => o.OrderID == order.OrderID);
    }

    public async Task<List<Order>> GetOrdersByEmployeeAsync(int employeeId)
    {
        return await _context.Orders
            .Include(o => o.Delivery)
            .Include(o => o.OrderMeals)
                .ThenInclude(om => om.Meal)
            .Where(o => o.EmployeeID == employeeId)
            .ToListAsync();
    }

    public async Task<List<Order>> GetOrdersForChefAsync(int chefId)
    {
        return await _context.Orders
            .Include(o => o.OrderMeals)
                .ThenInclude(om => om.Meal)
            .Where(o => o.ChefID == chefId)
            .ToListAsync();
    }

    public async Task<List<Order>> GetOrdersByCompanyAsync(int companyId)
    {
        return await _context.Orders
            .Include(o => o.Employee)
            .Include(o => o.OrderMeals)
                .ThenInclude(om => om.Meal)
            .Where(o => o.Employee.CompanyID == companyId)
            .ToListAsync();
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.Employee)
            .Include(o => o.Chef)
            .Include(o => o.OrderMeals)
                .ThenInclude(om => om.Meal)
            .ToListAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders
            .Include(o => o.Employee)
            .Include(o => o.Chef)
            .Include(o => o.OrderMeals)
                .ThenInclude(om => om.Meal)
            .FirstOrDefaultAsync(o => o.OrderID == orderId);
    }

    public Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
