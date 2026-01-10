namespace Domain.Models;

/// <summary>
/// Result of delivery cost calculation for a package
/// </summary>
public class DeliveryCostResult
{
    public string PackageId { get; }
    public decimal Discount { get; }
    public decimal TotalCost { get; }

    public DeliveryCostResult(string packageId, decimal discount, decimal totalCost)
    {
        PackageId = packageId;
        Discount = discount;
        TotalCost = totalCost;
    }

    public override string ToString()
    {
        return $"{PackageId} {Discount:F0} {TotalCost:F0}";
    }
}
