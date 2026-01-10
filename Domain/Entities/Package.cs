namespace Domain.Entities;

/// <summary>
/// Represents a package for delivery
/// </summary>
public class Package
{
    public string PackageId { get; }
    public decimal WeightInKg { get; }
    public decimal DistanceInKm { get; }
    public string? OfferCode { get; }

    public Package(string packageId, decimal weightInKg, decimal distanceInKm, string? offerCode = null)
    {
        if (string.IsNullOrWhiteSpace(packageId))
            throw new ArgumentException("Package ID cannot be empty", nameof(packageId));

        if (weightInKg <= 0)
            throw new ArgumentException("Weight must be greater than 0", nameof(weightInKg));

        if (distanceInKm <= 0)
            throw new ArgumentException("Distance must be greater than 0", nameof(distanceInKm));

        PackageId = packageId;
        WeightInKg = weightInKg;
        DistanceInKm = distanceInKm;
        OfferCode = offerCode;
    }
}
