using MealTimes.Core.DTOs;
using MealTimes.Core.Service;
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
    }
}
