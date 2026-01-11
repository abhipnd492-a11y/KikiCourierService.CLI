using Application.Services;
using Domain.Entities;

namespace Tests;

public class DeliveryTimeEstimatorTests
{
    private readonly DeliveryTimeEstimator _estimator;
    private readonly List<Offer> _offers;

    public DeliveryTimeEstimatorTests()
    {
        _offers = new List<Offer>
        {
            new Offer("OFR001", 10, 0, 200, 70, 200),
            new Offer("OFR002", 7, 50, 150, 100, 250),
            new Offer("OFR003", 5, 50, 250, 10, 150)
        };
        var costCalculator = new DeliveryCostCalculator(_offers);
        _estimator = new DeliveryTimeEstimator(costCalculator);
    }

    [Fact]
    public void EstimateDeliveryTimes_EmptyPackages_ReturnsEmptyList()
    {
        var packages = new List<Package>();
        var vehicles = new List<Vehicle> { new Vehicle(1, 200, 70) };

        var result = _estimator.EstimateDeliveryTimes(100, packages, vehicles);

        Assert.Empty(result);
    }

    [Fact]
    public void EstimateDeliveryTimes_SinglePackageSingleVehicle_ReturnsCorrectTime()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 50, 70)
        };
        var vehicles = new List<Vehicle> { new Vehicle(1, 200, 70) };

        var result = _estimator.EstimateDeliveryTimes(100, packages, vehicles);

        Assert.Single(result);
        Assert.Equal("PKG1", result[0].PackageId);
        Assert.Equal(1m, result[0].EstimatedDeliveryTimeInHours);
    }

    [Fact]
    public void EstimateDeliveryTimes_ResultsOrderedByPackageId()
    {
        var packages = new List<Package>
        {
            new Package("PKG3", 50, 30),
            new Package("PKG1", 75, 125),
            new Package("PKG2", 100, 100)
        };
        var vehicles = new List<Vehicle> { new Vehicle(1, 200, 70) };

        var result = _estimator.EstimateDeliveryTimes(100, packages, vehicles);

        Assert.Equal("PKG1", result[0].PackageId);
        Assert.Equal("PKG2", result[1].PackageId);
        Assert.Equal("PKG3", result[2].PackageId);
    }

    [Fact]
    public void EstimateDeliveryTimes_Problem2Example_ReturnsExpectedResults()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 50, 30, "OFR001"),
            new Package("PKG2", 75, 125, "OFR008"),
            new Package("PKG3", 175, 100, "OFR003"),
            new Package("PKG4", 110, 60, "OFR002"),
            new Package("PKG5", 155, 95, "NA")
        };
        var vehicles = new List<Vehicle>
        {
            new Vehicle(1, 200, 70),
            new Vehicle(2, 200, 70)
        };

        var result = _estimator.EstimateDeliveryTimes(100, packages, vehicles);

        Assert.Equal(5, result.Count);

        var pkg1 = result.First(r => r.PackageId == "PKG1");
        var pkg2 = result.First(r => r.PackageId == "PKG2");
        var pkg3 = result.First(r => r.PackageId == "PKG3");
        var pkg4 = result.First(r => r.PackageId == "PKG4");
        var pkg5 = result.First(r => r.PackageId == "PKG5");

        Assert.Equal(4.00m, pkg1.EstimatedDeliveryTimeInHours);
        Assert.Equal(1.79m, pkg2.EstimatedDeliveryTimeInHours);
        Assert.Equal(1.43m, pkg3.EstimatedDeliveryTimeInHours);
        Assert.Equal(0.86m, pkg4.EstimatedDeliveryTimeInHours);
        Assert.Equal(4.21m, pkg5.EstimatedDeliveryTimeInHours);
    }
}
