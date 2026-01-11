using Domain.Entities;

namespace Tests;

public class VehicleTests
{
    [Fact]
    public void Constructor_ValidParameters_CreatesVehicle()
    {
        var vehicle = new Vehicle(1, 200, 70);

        Assert.Equal(1, vehicle.VehicleId);
        Assert.Equal(200, vehicle.MaxWeightCapacity);
        Assert.Equal(70, vehicle.MaxSpeedKmPerHour);
        Assert.Equal(0, vehicle.AvailableAtTime);
    }

    [Fact]
    public void Constructor_ZeroWeight_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Vehicle(1, 0, 70));
    }

    [Fact]
    public void Constructor_NegativeWeight_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Vehicle(1, -100, 70));
    }

    [Fact]
    public void Constructor_ZeroSpeed_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Vehicle(1, 200, 0));
    }

    [Fact]
    public void Constructor_NegativeSpeed_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Vehicle(1, 200, -70));
    }

    [Fact]
    public void CalculateDeliveryTime_ReturnsCorrectTime()
    {
        var vehicle = new Vehicle(1, 200, 70);

        var time = vehicle.CalculateDeliveryTime(140);

        Assert.Equal(2m, time);
    }

    [Fact]
    public void CalculateDeliveryTime_ShortDistance_ReturnsCorrectTime()
    {
        var vehicle = new Vehicle(1, 200, 70);

        var time = vehicle.CalculateDeliveryTime(35);

        Assert.Equal(0.5m, time);
    }

    [Fact]
    public void AssignTrip_UpdatesAvailability()
    {
        var vehicle = new Vehicle(1, 200, 70);

        vehicle.AssignTrip(70);

        Assert.Equal(2m, vehicle.AvailableAtTime);
    }

    [Fact]
    public void AssignTrip_MultipleTimes_AccumulatesAvailability()
    {
        var vehicle = new Vehicle(1, 200, 70);

        vehicle.AssignTrip(70);
        vehicle.AssignTrip(35);

        Assert.Equal(3m, vehicle.AvailableAtTime);
    }

    [Fact]
    public void ResetAvailability_SetsToZero()
    {
        var vehicle = new Vehicle(1, 200, 70);
        vehicle.AssignTrip(70);

        vehicle.ResetAvailability();

        Assert.Equal(0, vehicle.AvailableAtTime);
    }

    [Fact]
    public void AssignTrip_CalculatesRoundTrip()
    {
        var vehicle = new Vehicle(1, 200, 70);

        vehicle.AssignTrip(100);

        var expectedTime = (100m / 70m) * 2;
        Assert.Equal(Math.Round(expectedTime, 2), Math.Round(vehicle.AvailableAtTime, 2));
    }
}
