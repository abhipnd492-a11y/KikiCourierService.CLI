using Application.Interfaces;
using Domain.Entities;
using Domain.Models;

namespace Application.Services;

public class DeliveryTimeEstimator : IDeliveryTimeEstimator
{
    private readonly IDeliveryCostCalculator _costCalculator;
    private readonly PackageSelector _packageSelector;

    public DeliveryTimeEstimator(IDeliveryCostCalculator costCalculator)
    {
        _costCalculator = costCalculator;
        _packageSelector = new PackageSelector();
    }

    public List<DeliveryTimeResult> EstimateDeliveryTimes(
        decimal baseDeliveryCost,
        List<Package> packages,
        List<Vehicle> vehicles)
    {
        var results = new Dictionary<string, DeliveryTimeResult>();
        var remainingPackages = packages.ToList();

        foreach (var vehicle in vehicles)
        {
            vehicle.ResetAvailability();
        }

        while (remainingPackages.Any())
        {
            var vehicle = vehicles.OrderBy(v => v.AvailableAtTime).First();
            var departureTime = vehicle.AvailableAtTime;

            var selectedPackages = _packageSelector.SelectOptimalPackages(
                remainingPackages,
                vehicle.MaxWeightCapacity);

            if (!selectedPackages.Any())
                break;

            var maxDistance = selectedPackages.Max(p => p.DistanceInKm);

            foreach (var package in selectedPackages)
            {
                var costResult = _costCalculator.CalculateCost(baseDeliveryCost, package);
                var deliveryTime = departureTime + vehicle.CalculateDeliveryTime(package.DistanceInKm);

                results[package.PackageId] = new DeliveryTimeResult(
                    package.PackageId,
                    costResult.Discount,
                    costResult.TotalCost,
                    Math.Round(deliveryTime, 2));
            }

            vehicle.AssignTrip(maxDistance);

            foreach (var package in selectedPackages)
            {
                remainingPackages.RemoveAll(p => p.PackageId == package.PackageId);
            }
        }

        return results.Values
            .OrderBy(r => r.PackageId)
            .ToList();
    }
}
