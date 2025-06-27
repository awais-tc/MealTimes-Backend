using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using MealTimes.Core.Responses;
using MealTimes.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTimes.Service
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public DeliveryService(
            IDeliveryRepository deliveryRepository,
            IOrderRepository orderRepository,
            IMapper mapper)
        {
            _deliveryRepository = deliveryRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<GenericResponse<DeliveryDto>> AssignDeliveryAsync(DeliveryAssignDto dto)
        {
            var order = await _orderRepository.GetOrderByIdAsync(dto.OrderID);
            if (order == null)
                return GenericResponse<DeliveryDto>.Fail("Order not found");

            var delivery = _mapper.Map<Delivery>(dto);
            delivery.Status = DeliveryStatus.Assigned;

            await _deliveryRepository.AddAsync(delivery);
            await _deliveryRepository.SaveChangesAsync();

            return GenericResponse<DeliveryDto>.Success(_mapper.Map<DeliveryDto>(delivery), "Delivery assigned successfully.");
        }

        public async Task<GenericResponse<bool>> UpdateDeliveryStatusAsync(DeliveryStatusUpdateDto dto)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(dto.DeliveryID);
            if (delivery == null)
                return GenericResponse<bool>.Fail("Delivery not found.");

            if (!Enum.TryParse(dto.NewStatus, out DeliveryStatus parsedStatus))
                return GenericResponse<bool>.Fail("Invalid delivery status.");

            delivery.Status = parsedStatus;

            if(parsedStatus == DeliveryStatus.Assigned)
                delivery.PickedUpAt = DateTime.UtcNow;

            if (parsedStatus == DeliveryStatus.InTransit)
                delivery.PickedUpAt ??= DateTime.UtcNow;

            if (parsedStatus == DeliveryStatus.Delivered)
                delivery.DeliveredAt = DateTime.UtcNow;

            await _deliveryRepository.UpdateAsync(delivery);
            await _deliveryRepository.SaveChangesAsync();

            // Optional: Also update the related order’s delivery status
            var order = await _orderRepository.GetOrderByIdAsync(delivery.OrderID);
            if (order != null)
            {
                order.DeliveryStatus = parsedStatus;
                await _orderRepository.UpdateAsync(order);
                await _orderRepository.SaveChangesAsync();
            }

            return GenericResponse<bool>.Success(true, "Delivery status updated successfully.");
        }

        public async Task<GenericResponse<IEnumerable<DeliveryDto>>> GetAllDeliveriesAsync()
        {
            var deliveries = await _deliveryRepository.GetAllAsync();
            return GenericResponse<IEnumerable<DeliveryDto>>.Success(
                _mapper.Map<IEnumerable<DeliveryDto>>(deliveries));
        }

        public async Task<GenericResponse<DeliveryDto>> GetDeliveryByIdAsync(int id)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(id);
            if (delivery == null)
                return GenericResponse<DeliveryDto>.Fail("Delivery not found.");

            return GenericResponse<DeliveryDto>.Success(_mapper.Map<DeliveryDto>(delivery));
        }

        public async Task<GenericResponse<IEnumerable<DeliveryDto>>> GetDeliveriesByPersonIdAsync(int deliveryPersonId)
        {
            var deliveries = await _deliveryRepository.GetByDeliveryPersonIdAsync(deliveryPersonId);
            return GenericResponse<IEnumerable<DeliveryDto>>.Success(
                _mapper.Map<IEnumerable<DeliveryDto>>(deliveries));
        }
    }

}
