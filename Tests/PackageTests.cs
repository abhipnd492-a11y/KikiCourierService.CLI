using Domain.Entities;

namespace Tests;

public class PackageTests
{
    [Fact]
    public void Constructor_ValidParameters_CreatesPackage()
    {
        var package = new Package("PKG1", 50, 100, "OFR001");

        Assert.Equal("PKG1", package.PackageId);
        Assert.Equal(50, package.WeightInKg);
        Assert.Equal(100, package.DistanceInKm);
        Assert.Equal("OFR001", package.OfferCode);
    }

    [Fact]
    public void Constructor_NoOfferCode_CreatesPackageWithNullOffer()
    {
        var package = new Package("PKG1", 50, 100);

        Assert.Equal("PKG1", package.PackageId);
        Assert.Null(package.OfferCode);
    }

    [Fact]
    public void Constructor_EmptyPackageId_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Package("", 50, 100));
    }

    [Fact]
    public void Constructor_ZeroWeight_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Package("PKG1", 0, 100));
    }

    [Fact]
    public void Constructor_ZeroDistance_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Package("PKG1", 50, 0));
    }

}
