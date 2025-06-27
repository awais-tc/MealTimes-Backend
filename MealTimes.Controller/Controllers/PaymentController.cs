using MealTimes.Core.DTOs;
using MealTimes.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MealTimes.Controller
{

    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> SubscribeToPlan([FromBody] PaymentRequestDto dto)
        {
            var result = await _paymentService.ProcessSubscriptionPaymentAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        // GET: api/payment/all
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPaymentsAsync()
        {
            var response = await _paymentService.GetAllPaymentsAsync();
            if (!response.IsSuccess)
                return NotFound(response.Message);

            return Ok(response.Data);
        }
    }
}
