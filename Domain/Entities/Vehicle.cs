namespace Domain.Entities;

/// <summary>
/// Represents a delivery vehicle with capacity and speed constraints
/// </summary>
public class Vehicle
{
    public int VehicleId { get; }
    public decimal MaxWeightCapacity { get; }
    public decimal MaxSpeedKmPerHour { get; }
    public decimal AvailableAtTime { get; private set; }

    public Vehicle(int vehicleId, decimal maxWeightCapacity, decimal maxSpeedKmPerHour)
    {
        if (maxWeightCapacity <= 0)
            throw new ArgumentException("Max weight capacity must be greater than 0", nameof(maxWeightCapacity));

        if (maxSpeedKmPerHour <= 0)
            throw new ArgumentException("Max speed must be greater than 0", nameof(maxSpeedKmPerHour));

        VehicleId = vehicleId;
        MaxWeightCapacity = maxWeightCapacity;
        MaxSpeedKmPerHour = maxSpeedKmPerHour;
        AvailableAtTime = 0;
    }

    /// <summary>
    /// Calculate delivery time for a given distance
    /// </summary>
    public decimal CalculateDeliveryTime(decimal distanceInKm)
    {
        return distanceInKm / MaxSpeedKmPerHour;
    }

    /// <summary>
    /// Assigns a delivery trip and updates availability
    /// </summary>
    public void AssignTrip(decimal maxDistanceInTrip)
    {
        var tripTime = CalculateDeliveryTime(maxDistanceInTrip);
        // Vehicle takes round trip time (deliver + return)
        AvailableAtTime += tripTime * 2;
    }

    /// <summary>
    /// Resets vehicle availability (for testing)
    /// </summary>
    public void ResetAvailability()
    {
        AvailableAtTime = 0;
    }
}
