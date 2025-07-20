using MealTimes.Core.DTOs;
using MealTimes.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MealTimes.Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        /// <summary>
        /// Calculate commission for a specific order
        /// </summary>
        [HttpPost("commission/calculate/{orderId}")]
        public async Task<IActionResult> CalculateCommission(int orderId)
        {
            var response = await _businessService.CalculateCommissionAsync(orderId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get commissions by chef with optional date filtering
        /// </summary>
        [HttpGet("commission/chef/{chefId}")]
        public async Task<IActionResult> GetCommissionsByChef(int chefId, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var response = await _businessService.GetCommissionsByChefAsync(chefId, startDate, endDate);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get all commissions with optional date filtering
        /// </summary>
        [HttpGet("commission/all")]
        public async Task<IActionResult> GetAllCommissions([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var response = await _businessService.GetAllCommissionsAsync(startDate, endDate);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Create chef payout for a specific period
        /// </summary>
        [HttpPost("payout/create")]
        public async Task<IActionResult> CreateChefPayout([FromBody] CreateChefPayoutDto dto)
        {
            var response = await _businessService.CreateChefPayoutAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get chef payouts with optional filtering
        /// </summary>
        [HttpGet("payout")]
        public async Task<IActionResult> GetChefPayouts([FromQuery] int? chefId = null, [FromQuery] string? status = null)
        {
            var response = await _businessService.GetChefPayoutsAsync(chefId, status);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Update payout status (mark as paid, failed, etc.)
        /// </summary>
        [HttpPut("payout/status")]
        public async Task<IActionResult> UpdatePayoutStatus([FromBody] UpdatePayoutStatusDto dto)
        {
            var response = await _businessService.UpdatePayoutStatusAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get all pending payouts
        /// </summary>
        [HttpGet("payout/pending")]
        public async Task<IActionResult> GetPendingPayouts()
        {
            var response = await _businessService.GetPendingPayoutsAsync();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get comprehensive business analytics
        /// </summary>
        [HttpGet("analytics")]
        public async Task<IActionResult> GetBusinessAnalytics([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var response = await _businessService.GetBusinessAnalyticsAsync(startDate, endDate);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Generate profit and loss report
        /// </summary>
        [HttpGet("report/profit-loss")]
        public async Task<IActionResult> GenerateProfitLossReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string period = "Monthly")
        {
            var response = await _businessService.GenerateProfitLossReportAsync(startDate, endDate, period);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get daily business metrics
        /// </summary>
        [HttpGet("metrics/daily")]
        public async Task<IActionResult> GetDailyMetrics([FromQuery] DateTime date)
        {
            var response = await _businessService.GetDailyMetricsAsync(date);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get business metrics for a date range
        /// </summary>
        [HttpGet("metrics/range")]
        public async Task<IActionResult> GetMetricsRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var response = await _businessService.GetMetricsRangeAsync(startDate, endDate);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Process daily metrics calculation
        /// </summary>
        [HttpPost("metrics/process-daily")]
        public async Task<IActionResult> ProcessDailyMetrics([FromQuery] DateTime date)
        {
            var response = await _businessService.ProcessDailyMetricsAsync(date);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Process weekly payouts for all chefs
        /// </summary>
        [HttpPost("payout/process-weekly")]
        public async Task<IActionResult> ProcessWeeklyPayouts()
        {
            var response = await _businessService.ProcessWeeklyPayoutsAsync();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Process monthly payouts for all chefs
        /// </summary>
        [HttpPost("payout/process-monthly")]
        public async Task<IActionResult> ProcessMonthlyPayouts()
        {
            var response = await _businessService.ProcessMonthlyPayoutsAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}