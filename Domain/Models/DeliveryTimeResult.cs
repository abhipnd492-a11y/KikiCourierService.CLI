namespace Domain.Models;

/// <summary>
/// Result of delivery time estimation for a package
/// </summary>
public class DeliveryTimeResult
{
    public string PackageId { get; }
    public decimal Discount { get; }
    public decimal TotalCost { get; }
    public decimal EstimatedDeliveryTimeInHours { get; }

    public DeliveryTimeResult(string packageId, decimal discount, decimal totalCost, decimal estimatedDeliveryTimeInHours)
    {
        PackageId = packageId;
        Discount = discount;
        TotalCost = totalCost;
        EstimatedDeliveryTimeInHours = estimatedDeliveryTimeInHours;
    }

    public override string ToString()
    {
        return $"{PackageId} {Discount:F0} {TotalCost:F0} {EstimatedDeliveryTimeInHours:F2}";
    }
}
