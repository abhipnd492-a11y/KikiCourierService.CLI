using Application.Services;
using Domain.Entities;

namespace Console;

class Program
{
    private static readonly List<Offer> StandardOffers = new()
    {
        new Offer("OFR001", 10, 0, 200, 70, 200),
        new Offer("OFR002", 7, 50, 150, 100, 250),
        new Offer("OFR003", 5, 50, 250, 10, 150)
    };

    static void Main(string[] args)
    {
        try
        {
            var firstLine = System.Console.ReadLine();
            if (string.IsNullOrWhiteSpace(firstLine))
            {
                System.Console.Error.WriteLine("Error: Invalid input");
                return;
            }

            var firstLineParts = firstLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (firstLineParts.Length < 2)
            {
                System.Console.Error.WriteLine("Error: Invalid input format");
                return;
            }

            if (!decimal.TryParse(firstLineParts[0], out var baseDeliveryCost))
            {
                System.Console.Error.WriteLine("Error: Invalid base_delivery_cost");
                return;
            }

            if (!int.TryParse(firstLineParts[1], out var numberOfPackages))
            {
                System.Console.Error.WriteLine("Error: Invalid no_of_packages");
                return;
            }

            var packages = new List<Package>();
            for (int i = 0; i < numberOfPackages; i++)
            {
                var packageLine = System.Console.ReadLine();
                var package = ParsePackage(packageLine);
                if (package == null)
                {
                    System.Console.Error.WriteLine($"Error: Invalid package data");
                    return;
                }
                packages.Add(package);
            }

            var vehicleLine = System.Console.ReadLine();
            var costCalculator = new DeliveryCostCalculator(StandardOffers);

            if (string.IsNullOrWhiteSpace(vehicleLine))
            {
                // Problem 1: Cost Estimation only
                var costResults = costCalculator.CalculateCosts(baseDeliveryCost, packages);
                foreach (var result in costResults)
                {
                    System.Console.WriteLine(result.ToString());
                }
            }
            else
            {
                // Problem 2: Time Estimation
                var vehicles = ParseVehicles(vehicleLine);
                if (vehicles == null || !vehicles.Any())
                {
                    System.Console.Error.WriteLine("Error: Invalid vehicle data");
                    return;
                }

                var timeEstimator = new DeliveryTimeEstimator(costCalculator);
                var timeResults = timeEstimator.EstimateDeliveryTimes(baseDeliveryCost, packages, vehicles);

                foreach (var result in timeResults)
                {
                    System.Console.WriteLine(result.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            System.Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static Package? ParsePackage(string? line)
    {
        if (string.IsNullOrWhiteSpace(line)) return null;

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 3) return null;

        if (!decimal.TryParse(parts[1], out var weight)) return null;
        if (!decimal.TryParse(parts[2], out var distance)) return null;

        string? offerCode = parts.Length >= 4 && !string.Equals(parts[3], "NA", StringComparison.OrdinalIgnoreCase)
            ? parts[3] : null;

        try { return new Package(parts[0], weight, distance, offerCode); }
        catch { return null; }
    }

    private static List<Vehicle>? ParseVehicles(string line)
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 3) return null;

        if (!int.TryParse(parts[0], out var count)) return null;
        if (!decimal.TryParse(parts[1], out var maxSpeed)) return null;
        if (!decimal.TryParse(parts[2], out var maxWeight)) return null;

        var vehicles = new List<Vehicle>();
        for (int i = 0; i < count; i++)
        {
            try { vehicles.Add(new Vehicle(i + 1, maxWeight, maxSpeed)); }
            catch { return null; }
        }
        return vehicles;
    }
}
