using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using MealTimes.Core.Responses;
using MealTimes.Core.Service;

namespace MealTimes.Service
{
    public class BusinessService : IBusinessService
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private const decimal DEFAULT_COMMISSION_RATE = 20.00m; // 20%

        public BusinessService(
            IBusinessRepository businessRepository,
            IOrderRepository orderRepository,
            IMapper mapper)
        {
            _businessRepository = businessRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<GenericResponse<CommissionDto>> CalculateCommissionAsync(int orderId)
        {
            // Check if commission already exists
            var existingCommission = await _businessRepository.GetCommissionByOrderIdAsync(orderId);
            if (existingCommission != null)
                return GenericResponse<CommissionDto>.Success(_mapper.Map<CommissionDto>(existingCommission));

            // Get order details
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                return GenericResponse<CommissionDto>.Fail("Order not found.");

            // Calculate order total
            var orderAmount = order.OrderMeals.Sum(om => om.Meal.Price * om.Quantity);

            // Calculate commission
            var commissionRate = DEFAULT_COMMISSION_RATE;
            var commissionAmount = (orderAmount * commissionRate) / 100;
            var chefPayableAmount = orderAmount - commissionAmount;
            var platformEarning = commissionAmount;

            // Create commission record
            var commission = new Commission
            {
                OrderID = orderId,
                ChefID = order.ChefID,
                OrderAmount = orderAmount,
                CommissionRate = commissionRate,
                CommissionAmount = commissionAmount,
                ChefPayableAmount = chefPayableAmount,
                PlatformEarning = platformEarning,
                Status = "Pending"
            };

            var createdCommission = await _businessRepository.CreateCommissionAsync(commission);
            return GenericResponse<CommissionDto>.Success(_mapper.Map<CommissionDto>(createdCommission));
        }

        public async Task<GenericResponse<List<CommissionDto>>> GetCommissionsByChefAsync(int chefId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var commissions = await _businessRepository.GetCommissionsByChefAsync(chefId, startDate, endDate);
            var commissionDtos = _mapper.Map<List<CommissionDto>>(commissions);
            return GenericResponse<List<CommissionDto>>.Success(commissionDtos);
        }

        public async Task<GenericResponse<List<CommissionDto>>> GetAllCommissionsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var commissions = await _businessRepository.GetAllCommissionsAsync(startDate, endDate);
            var commissionDtos = _mapper.Map<List<CommissionDto>>(commissions);
            return GenericResponse<List<CommissionDto>>.Success(commissionDtos);
        }

        public async Task<GenericResponse<ChefPayoutDto>> CreateChefPayoutAsync(CreateChefPayoutDto dto)
        {
            // Check if payout already exists for this period
            var hasPendingPayout = await _businessRepository.HasPendingPayoutForPeriodAsync(
                dto.ChefID, dto.PeriodStart, dto.PeriodEnd);

            if (hasPendingPayout)
                return GenericResponse<ChefPayoutDto>.Fail("Payout already exists for this period.");

            // Get commissions for the period
            var commissions = await _businessRepository.GetCommissionsByChefAsync(
                dto.ChefID, dto.PeriodStart, dto.PeriodEnd);

            if (!commissions.Any())
                return GenericResponse<ChefPayoutDto>.Fail("No commissions found for the specified period.");

            // Calculate payout amounts
            var totalEarnings = commissions.Sum(c => c.OrderAmount);
            var commissionDeducted = commissions.Sum(c => c.CommissionAmount);
            var payableAmount = commissions.Sum(c => c.ChefPayableAmount);

            // Create payout record
            var payout = new ChefPayout
            {
                ChefID = dto.ChefID,
                TotalEarnings = totalEarnings,
                CommissionDeducted = commissionDeducted,
                PayableAmount = payableAmount,
                PayoutPeriod = dto.PayoutPeriod,
                PeriodStart = dto.PeriodStart,
                PeriodEnd = dto.PeriodEnd,
                Status = "Pending"
            };

            var createdPayout = await _businessRepository.CreateChefPayoutAsync(payout);
            var payoutDto = _mapper.Map<ChefPayoutDto>(createdPayout);
            payoutDto.TotalOrders = commissions.Count;

            return GenericResponse<ChefPayoutDto>.Success(payoutDto, "Chef payout created successfully.");
        }

        public async Task<GenericResponse<List<ChefPayoutDto>>> GetChefPayoutsAsync(int? chefId = null, string? status = null)
        {
            var payouts = await _businessRepository.GetChefPayoutsAsync(chefId, status);
            var payoutDtos = _mapper.Map<List<ChefPayoutDto>>(payouts);
            return GenericResponse<List<ChefPayoutDto>>.Success(payoutDtos);
        }

        public async Task<GenericResponse<ChefPayoutDto>> UpdatePayoutStatusAsync(UpdatePayoutStatusDto dto)
        {
            var payout = await _businessRepository.GetChefPayoutByIdAsync(dto.PayoutID);
            if (payout == null)
                return GenericResponse<ChefPayoutDto>.Fail("Payout not found.");

            payout.Status = dto.Status;
            payout.PaymentMethod = dto.PaymentMethod;
            payout.PaymentReference = dto.PaymentReference;
            payout.Notes = dto.Notes;

            if (dto.Status == "Completed")
                payout.ProcessedAt = DateTime.UtcNow;

            var updatedPayout = await _businessRepository.UpdateChefPayoutAsync(payout);
            var payoutDto = _mapper.Map<ChefPayoutDto>(updatedPayout);

            return GenericResponse<ChefPayoutDto>.Success(payoutDto, "Payout status updated successfully.");
        }

        public async Task<GenericResponse<List<ChefPayoutDto>>> GetPendingPayoutsAsync()
        {
            var payouts = await _businessRepository.GetPendingPayoutsAsync();
            var payoutDtos = _mapper.Map<List<ChefPayoutDto>>(payouts);
            return GenericResponse<List<ChefPayoutDto>>.Success(payoutDtos);
        }

        public async Task<GenericResponse<BusinessAnalyticsDto>> GetBusinessAnalyticsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var analytics = new BusinessAnalyticsDto
            {
                TotalRevenue = await _businessRepository.GetTotalRevenueAsync(startDate, endDate),
                TotalCommissions = await _businessRepository.GetTotalCommissionRevenueAsync(startDate, endDate),
                TotalChefPayouts = await _businessRepository.GetTotalChefPayoutsAsync(startDate, endDate),
                TotalOrders = await _businessRepository.GetTotalOrdersAsync(startDate, endDate),
                TotalActiveSubscriptions = await _businessRepository.GetActiveSubscriptionsCountAsync(),
                AverageOrderValue = await _businessRepository.GetAverageOrderValueAsync(startDate, endDate)
            };

            analytics.TotalProfit = analytics.TotalRevenue - analytics.TotalChefPayouts;
            analytics.ProfitMargin = analytics.TotalRevenue > 0 ? (analytics.TotalProfit / analytics.TotalRevenue) * 100 : 0;

            return GenericResponse<BusinessAnalyticsDto>.Success(analytics);
        }

        public async Task<GenericResponse<ProfitLossReportDto>> GenerateProfitLossReportAsync(DateTime startDate, DateTime endDate, string period = "Monthly")
        {
            var subscriptionRevenue = await _businessRepository.GetTotalSubscriptionRevenueAsync(startDate, endDate);
            var commissionRevenue = await _businessRepository.GetTotalCommissionRevenueAsync(startDate, endDate);
            var chefPayouts = await _businessRepository.GetTotalChefPayoutsAsync(startDate, endDate);
            var totalOrders = await _businessRepository.GetTotalOrdersAsync(startDate, endDate);
            var averageOrderValue = await _businessRepository.GetAverageOrderValueAsync(startDate, endDate);

            var totalRevenue = subscriptionRevenue + commissionRevenue;
            var operationalCosts = totalRevenue * 0.15m; // Assume 15% operational costs
            var totalExpenses = chefPayouts + operationalCosts;
            var netProfit = totalRevenue - totalExpenses;

            var report = new ProfitLossReportDto
            {
                ReportDate = DateTime.UtcNow,
                Period = period,
                SubscriptionRevenue = subscriptionRevenue,
                CommissionRevenue = commissionRevenue,
                TotalRevenue = totalRevenue,
                ChefPayouts = chefPayouts,
                OperationalCosts = operationalCosts,
                TotalExpenses = totalExpenses,
                GrossProfit = totalRevenue - chefPayouts,
                NetProfit = netProfit,
                ProfitMargin = totalRevenue > 0 ? (netProfit / totalRevenue) * 100 : 0,
                TotalOrders = totalOrders,
                AverageOrderValue = averageOrderValue
            };

            return GenericResponse<ProfitLossReportDto>.Success(report);
        }

        public async Task<GenericResponse<BusinessMetricsDto>> GetDailyMetricsAsync(DateTime date)
        {
            var metrics = await _businessRepository.GetBusinessMetricsByDateAsync(date);
            if (metrics == null)
                return GenericResponse<BusinessMetricsDto>.Fail("No metrics found for the specified date.");

            var metricsDto = _mapper.Map<BusinessMetricsDto>(metrics);
            return GenericResponse<BusinessMetricsDto>.Success(metricsDto);
        }

        public async Task<GenericResponse<List<BusinessMetricsDto>>> GetMetricsRangeAsync(DateTime startDate, DateTime endDate)
        {
            var metrics = await _businessRepository.GetBusinessMetricsRangeAsync(startDate, endDate);
            var metricsDtos = _mapper.Map<List<BusinessMetricsDto>>(metrics);
            return GenericResponse<List<BusinessMetricsDto>>.Success(metricsDtos);
        }

        public async Task<GenericResponse<bool>> ProcessDailyMetricsAsync(DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = date.Date.AddDays(1).AddTicks(-1);

            var subscriptionRevenue = await _businessRepository.GetTotalSubscriptionRevenueAsync(startOfDay, endOfDay);
            var commissionRevenue = await _businessRepository.GetTotalCommissionRevenueAsync(startOfDay, endOfDay);
            var chefPayouts = await _businessRepository.GetTotalChefPayoutsAsync(startOfDay, endOfDay);
            var totalOrders = await _businessRepository.GetTotalOrdersAsync(startOfDay, endOfDay);
            var averageOrderValue = await _businessRepository.GetAverageOrderValueAsync(startOfDay, endOfDay);

            var totalRevenue = subscriptionRevenue + commissionRevenue;
            var operationalCosts = totalRevenue * 0.15m;
            var totalExpenses = chefPayouts + operationalCosts;
            var netProfit = totalRevenue - totalExpenses;

            var metrics = new BusinessMetrics
            {
                Date = date.Date,
                SubscriptionRevenue = subscriptionRevenue,
                CommissionRevenue = commissionRevenue,
                TotalRevenue = totalRevenue,
                ChefPayouts = chefPayouts,
                OperationalCosts = operationalCosts,
                TotalExpenses = totalExpenses,
                NetProfit = netProfit,
                ProfitMargin = totalRevenue > 0 ? (netProfit / totalRevenue) * 100 : 0,
                TotalOrders = totalOrders,
                ActiveSubscriptions = await _businessRepository.GetActiveSubscriptionsCountAsync(),
                ActiveChefs = await _businessRepository.GetActiveChefsCountAsync(),
                ActiveEmployees = await _businessRepository.GetActiveEmployeesCountAsync(),
                AverageOrderValue = averageOrderValue
            };

            var existingMetrics = await _businessRepository.GetBusinessMetricsByDateAsync(date);
            if (existingMetrics != null)
            {
                metrics.MetricsID = existingMetrics.MetricsID;
                await _businessRepository.UpdateBusinessMetricsAsync(metrics);
            }
            else
            {
                await _businessRepository.CreateBusinessMetricsAsync(metrics);
            }

            return GenericResponse<bool>.Success(true, "Daily metrics processed successfully.");
        }

        public async Task<GenericResponse<bool>> ProcessWeeklyPayoutsAsync()
        {
            var endDate = DateTime.UtcNow.Date;
            var startDate = endDate.AddDays(-7);

            // Get all active chefs with orders in the past week
            var commissions = await _businessRepository.GetAllCommissionsAsync(startDate, endDate);
            var chefIds = commissions.Select(c => c.ChefID).Distinct();

            foreach (var chefId in chefIds)
            {
                var createPayoutDto = new CreateChefPayoutDto
                {
                    ChefID = chefId,
                    PayoutPeriod = "Weekly",
                    PeriodStart = startDate,
                    PeriodEnd = endDate
                };

                await CreateChefPayoutAsync(createPayoutDto);
            }

            return GenericResponse<bool>.Success(true, "Weekly payouts processed successfully.");
        }

        public async Task<GenericResponse<bool>> ProcessMonthlyPayoutsAsync()
        {
            var endDate = DateTime.UtcNow.Date;
            var startDate = new DateTime(endDate.Year, endDate.Month, 1);

            var commissions = await _businessRepository.GetAllCommissionsAsync(startDate, endDate);
            var chefIds = commissions.Select(c => c.ChefID).Distinct();

            foreach (var chefId in chefIds)
            {
                var createPayoutDto = new CreateChefPayoutDto
                {
                    ChefID = chefId,
                    PayoutPeriod = "Monthly",
                    PeriodStart = startDate,
                    PeriodEnd = endDate
                };

                await CreateChefPayoutAsync(createPayoutDto);
            }

            return GenericResponse<bool>.Success(true, "Monthly payouts processed successfully.");
        }
    }
}