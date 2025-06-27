using MealTimes.Core.DTOs;
using MealTimes.Core.Responses;

namespace MealTimes.Core.Service
{
    public interface IPaymentService
    {
        Task<GenericResponse<PaymentResponseDto>> ProcessSubscriptionPaymentAsync(PaymentRequestDto dto);
        Task<GenericResponse<List<PaymentResponseDto>>> GetAllPaymentsAsync();
    }
}
