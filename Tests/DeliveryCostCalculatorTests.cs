using Application.Services;
using Domain.Entities;

namespace Tests;

public class DeliveryCostCalculatorTests
{
    private readonly DeliveryCostCalculator _calculator;
    private readonly List<Offer> _offers;

    public DeliveryCostCalculatorTests()
    {
        _offers = new List<Offer>
        {
            new Offer("OFR001", 10, 0, 200, 70, 200),
            new Offer("OFR002", 7, 50, 150, 100, 250),
            new Offer("OFR003", 5, 50, 250, 10, 150)
        };
        _calculator = new DeliveryCostCalculator(_offers);
    }

    [Fact]
    public void CalculateBaseCost_ShouldReturnCorrectCost()
    {
        var baseCost = _calculator.CalculateBaseCost(100, 5, 5);
        Assert.Equal(175, baseCost);
    }

    [Fact]
    public void CalculateCost_PKG1_NoDiscountApplied()
    {
        var package = new Package("PKG1", 5, 5, "OFR001");
        var result = _calculator.CalculateCost(100, package);

        Assert.Equal("PKG1", result.PackageId);
        Assert.Equal(0, result.Discount);
        Assert.Equal(175, result.TotalCost);
    }

    [Fact]
    public void CalculateCost_PKG3_DiscountApplied()
    {
        var package = new Package("PKG3", 10, 100, "OFR003");
        var result = _calculator.CalculateCost(100, package);

        Assert.Equal("PKG3", result.PackageId);
        Assert.Equal(35, result.Discount);
        Assert.Equal(665, result.TotalCost);
    }

    [Fact]
    public void CalculateCost_NoOfferCode_NoDiscount()
    {
        var package = new Package("PKG6", 50, 50);
        var result = _calculator.CalculateCost(100, package);

        Assert.Equal("PKG6", result.PackageId);
        Assert.Equal(0, result.Discount);
        Assert.Equal(850, result.TotalCost);
    }
}
