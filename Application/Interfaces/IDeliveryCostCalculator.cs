using Domain.Entities;
using Domain.Models;

namespace Application.Interfaces;

public interface IDeliveryCostCalculator
{
    decimal CalculateBaseCost(decimal baseDeliveryCost, decimal weightInKg, decimal distanceInKm);
    DeliveryCostResult CalculateCost(decimal baseDeliveryCost, Package package);
    List<DeliveryCostResult> CalculateCosts(decimal baseDeliveryCost, List<Package> packages);
}
