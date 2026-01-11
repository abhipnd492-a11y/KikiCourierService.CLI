using Application.Services;
using Domain.Entities;

namespace Tests;

public class PackageSelectorTests
{
    private readonly PackageSelector _selector;

    public PackageSelectorTests()
    {
        _selector = new PackageSelector();
    }

    [Fact]
    public void SelectOptimalPackages_EmptyList_ReturnsEmptyList()
    {
        var result = _selector.SelectOptimalPackages(new List<Package>(), 100);

        Assert.Empty(result);
    }

    [Fact]
    public void SelectOptimalPackages_SinglePackageWithinLimit_ReturnsPackage()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 50, 100)
        };

        var result = _selector.SelectOptimalPackages(packages, 100);

        Assert.Single(result);
        Assert.Equal("PKG1", result[0].PackageId);
    }

    [Fact]
    public void SelectOptimalPackages_SinglePackageExceedsLimit_ReturnsEmptyList()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 150, 100)
        };

        var result = _selector.SelectOptimalPackages(packages, 100);

        Assert.Empty(result);
    }

    [Fact]
    public void SelectOptimalPackages_SelectsMaximumWeight()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 50, 30),
            new Package("PKG2", 75, 125),
            new Package("PKG3", 175, 100)
        };

        var result = _selector.SelectOptimalPackages(packages, 200);

        Assert.Equal(175, result.Sum(p => p.WeightInKg));
        Assert.Contains(result, p => p.PackageId == "PKG3");
    }

    [Fact]
    public void SelectOptimalPackages_SameWeight_PrefersFewerPackages()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 50, 30),
            new Package("PKG2", 50, 125),
            new Package("PKG3", 100, 100)
        };

        var result = _selector.SelectOptimalPackages(packages, 100);

        Assert.Single(result);
        Assert.Equal("PKG3", result[0].PackageId);
    }

    [Fact]
    public void SelectOptimalPackages_SameWeightAndCount_PrefersHeavierPackage()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 60, 30),
            new Package("PKG2", 40, 125),
            new Package("PKG3", 55, 100),
            new Package("PKG4", 45, 50)
        };

        var result = _selector.SelectOptimalPackages(packages, 100);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.PackageId == "PKG1");
        Assert.Contains(result, p => p.PackageId == "PKG2");
    }

    [Fact]
    public void SelectOptimalPackages_MultiplePackages_ReturnsOptimalCombination()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 50, 30),
            new Package("PKG2", 75, 125),
            new Package("PKG3", 175, 100),
            new Package("PKG4", 110, 60),
            new Package("PKG5", 155, 95)
        };

        var result = _selector.SelectOptimalPackages(packages, 200);

        var totalWeight = result.Sum(p => p.WeightInKg);
        Assert.True(totalWeight <= 200);
        Assert.Equal(185, totalWeight);
    }

    [Fact]
    public void SelectOptimalPackages_CombinedExactWeightMatch_ReturnsCombination()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 60, 30),
            new Package("PKG2", 40, 50)
        };

        var result = _selector.SelectOptimalPackages(packages, 100);

        Assert.Equal(2, result.Count);
        Assert.Equal(100, result.Sum(p => p.WeightInKg));
    }
}
