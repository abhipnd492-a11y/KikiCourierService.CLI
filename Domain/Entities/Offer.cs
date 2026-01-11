namespace Domain.Entities;

/// <summary>
/// Discount offer with eligibility criteria
/// </summary>
public class Offer
{
    public string OfferCode { get; }
    public decimal DiscountPercentage { get; }
    public decimal MinDistance { get; }
    public decimal MaxDistance { get; }
    public decimal MinWeight { get; }
    public decimal MaxWeight { get; }

    public Offer(
        string offerCode,
        decimal discountPercentage,
        decimal minDistance,
        decimal maxDistance,
        decimal minWeight,
        decimal maxWeight)
    {
        if (string.IsNullOrWhiteSpace(offerCode))
            throw new ArgumentException("Offer code cannot be empty", nameof(offerCode));

        if (discountPercentage < 0 || discountPercentage > 100)
            throw new ArgumentException("Discount percentage must be between 0 and 100", nameof(discountPercentage));

        if (minDistance < 0 || maxDistance < minDistance)
            throw new ArgumentException("Invalid distance range", nameof(minDistance));

        if (minWeight < 0 || maxWeight < minWeight)
            throw new ArgumentException("Invalid weight range", nameof(minWeight));

        OfferCode = offerCode;
        DiscountPercentage = discountPercentage;
        MinDistance = minDistance;
        MaxDistance = maxDistance;
        MinWeight = minWeight;
        MaxWeight = maxWeight;
    }

    /// <summary>
    /// Checks if a package is eligible for this offer
    /// </summary>
    public bool IsEligible(decimal weight, decimal distance)
    {
        return weight >= MinWeight && weight <= MaxWeight &&
               distance >= MinDistance && distance <= MaxDistance;
    }
}
