using Application.Interfaces;
using Domain.Entities;
using Domain.Models;

namespace Application.Services;

public class DeliveryCostCalculator : IDeliveryCostCalculator
{
    private const decimal WeightMultiplier = 10;
    private const decimal DistanceMultiplier = 5;

    private readonly IReadOnlyList<Offer> _offers;

    public DeliveryCostCalculator(IEnumerable<Offer> offers)
    {
        _offers = offers?.ToList() ?? new List<Offer>();
    }

    public decimal CalculateBaseCost(decimal baseDeliveryCost, decimal weightInKg, decimal distanceInKm)
    {
        return baseDeliveryCost + (weightInKg * WeightMultiplier) + (distanceInKm * DistanceMultiplier);
    }

    public DeliveryCostResult CalculateCost(decimal baseDeliveryCost, Package package)
    {
        var baseCost = CalculateBaseCost(baseDeliveryCost, package.WeightInKg, package.DistanceInKm);
        var discount = CalculateDiscount(baseCost, package);
        var totalCost = baseCost - discount;

        return new DeliveryCostResult(package.PackageId, discount, totalCost);
    }

    public List<DeliveryCostResult> CalculateCosts(decimal baseDeliveryCost, List<Package> packages)
    {
        return packages.Select(p => CalculateCost(baseDeliveryCost, p)).ToList();
    }

    private decimal CalculateDiscount(decimal baseCost, Package package)
    {
        if (string.IsNullOrEmpty(package.OfferCode))
            return 0;

        var offer = _offers.FirstOrDefault(o =>
            o.OfferCode.Equals(package.OfferCode, StringComparison.OrdinalIgnoreCase));

        if (offer == null)
            return 0;

        if (!offer.IsEligible(package.WeightInKg, package.DistanceInKm))
            return 0;

        return baseCost * (offer.DiscountPercentage / 100);
    }
}
