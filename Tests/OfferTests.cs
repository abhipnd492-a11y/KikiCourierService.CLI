using Domain.Entities;

namespace Tests;

public class OfferTests
{
    [Fact]
    public void Constructor_ValidParameters_CreatesOffer()
    {
        var offer = new Offer("OFR001", 10, 0, 200, 70, 200);

        Assert.Equal("OFR001", offer.OfferCode);
        Assert.Equal(10, offer.DiscountPercentage);
        Assert.Equal(0, offer.MinDistance);
        Assert.Equal(200, offer.MaxDistance);
        Assert.Equal(70, offer.MinWeight);
        Assert.Equal(200, offer.MaxWeight);
    }

    [Fact]
    public void Constructor_EmptyOfferCode_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Offer("", 10, 0, 200, 70, 200));
    }

    [Fact]
    public void Constructor_NegativeDiscount_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Offer("OFR001", -10, 0, 200, 70, 200));
    }

    [Fact]
    public void Constructor_InvalidDistanceRange_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Offer("OFR001", 10, 200, 100, 70, 200));
    }

    [Fact]
    public void Constructor_InvalidWeightRange_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Offer("OFR001", 10, 0, 200, 200, 70));
    }

    [Fact]
    public void IsEligible_WithinAllRanges_ReturnsTrue()
    {
        var offer = new Offer("OFR001", 10, 0, 200, 70, 200);

        var result = offer.IsEligible(100, 100);

        Assert.True(result);
    }

    [Fact]
    public void IsEligible_WeightBelowMin_ReturnsFalse()
    {
        var offer = new Offer("OFR001", 10, 0, 200, 70, 200);

        var result = offer.IsEligible(50, 100);

        Assert.False(result);
    }

    [Fact]
    public void IsEligible_DistanceAboveMax_ReturnsFalse()
    {
        var offer = new Offer("OFR001", 10, 0, 200, 70, 200);

        var result = offer.IsEligible(100, 250);

        Assert.False(result);
    }

}
