using Domain.Entities;

namespace Application.Services;

public class PackageSelector
{
    public List<Package> SelectOptimalPackages(List<Package> availablePackages, decimal maxWeight)
    {
        if (availablePackages == null || !availablePackages.Any())
            return new List<Package>();

        var bestCombination = new List<Package>();
        var bestWeight = 0m;
        var bestCount = int.MaxValue;

        var combinations = GetAllCombinations(availablePackages);

        foreach (var combination in combinations)
        {
            var totalWeight = combination.Sum(p => p.WeightInKg);

            if (totalWeight > maxWeight)
                continue;

            if (totalWeight > bestWeight ||
                (totalWeight == bestWeight && combination.Count < bestCount) ||
                (totalWeight == bestWeight && combination.Count == bestCount && IsHeavierCombination(combination, bestCombination)))
            {
                bestCombination = combination.ToList();
                bestWeight = totalWeight;
                bestCount = combination.Count;
            }
        }

        return bestCombination;
    }

    private IEnumerable<List<Package>> GetAllCombinations(List<Package> packages)
    {
        var count = packages.Count;

        for (int i = 1; i < (1 << count); i++)
        {
            var combination = new List<Package>();

            for (int j = 0; j < count; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    combination.Add(packages[j]);
                }
            }

            yield return combination;
        }
    }

    private bool IsHeavierCombination(List<Package> combination1, List<Package> combination2)
    {
        if (!combination1.Any() || !combination2.Any())
            return combination1.Any();

        var sorted1 = combination1.OrderByDescending(p => p.WeightInKg).ToList();
        var sorted2 = combination2.OrderByDescending(p => p.WeightInKg).ToList();

        for (int i = 0; i < Math.Min(sorted1.Count, sorted2.Count); i++)
        {
            if (sorted1[i].WeightInKg > sorted2[i].WeightInKg)
                return true;
            if (sorted1[i].WeightInKg < sorted2[i].WeightInKg)
                return false;
        }

        return false;
    }
}
