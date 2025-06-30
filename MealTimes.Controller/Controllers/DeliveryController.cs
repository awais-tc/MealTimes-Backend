using MealTimes.Core.DTOs;
using MealTimes.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MealTimes.Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        /// <summary>
        /// Assign a delivery to a delivery person (Admin Only)
        /// </summary>
        [HttpPost("assign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignDelivery([FromBody] DeliveryAssignDto dto)
        {
            var response = await _deliveryService.AssignDeliveryAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Update delivery status (DeliveryPerson Only)
        /// </summary>
        [HttpPut("update-status")]
        //[Authorize(Roles = "DeliveryPerson")]
        public async Task<IActionResult> UpdateDeliveryStatus([FromBody] DeliveryStatusUpdateDto dto)
        {
            var response = await _deliveryService.UpdateDeliveryStatusAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDeliveries()
        {
            var response = await _deliveryService.GetAllDeliveriesAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,DeliveryPerson")]
        public async Task<IActionResult> GetDeliveryById(int id)
        {
            var response = await _deliveryService.GetDeliveryByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("person/{deliveryPersonId}")]
        [Authorize(Roles = "DeliveryPerson")]
        public async Task<IActionResult> GetDeliveriesByPersonId(int deliveryPersonId)
        {
            var response = await _deliveryService.GetDeliveriesByPersonIdAsync(deliveryPersonId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
