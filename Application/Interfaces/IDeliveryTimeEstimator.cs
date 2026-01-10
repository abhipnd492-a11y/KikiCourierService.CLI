using Domain.Entities;
using Domain.Models;

namespace Application.Interfaces;

public interface IDeliveryTimeEstimator
{
    List<DeliveryTimeResult> EstimateDeliveryTimes(
        decimal baseDeliveryCost,
        List<Package> packages,
        List<Vehicle> vehicles);
}
