# Kiki's Courier Service - CLI Application

A command-line application for calculating delivery costs and estimating delivery times for a courier service.

## Problem Statement

Build a solution for Kiki's courier service that:
1. **Problem 1**: Calculate delivery cost with discount offers
2. **Problem 2**: Estimate delivery time using vehicle fleet optimization

## Architecture & Design Decisions

### Clean Architecture

The solution follows **Clean Architecture** principles with clear separation of concerns:

```
KikiCourierService.CLI/
├── Domain/              # Core business entities (no dependencies)
│   ├── Entities/        # Package, Offer, Vehicle
│   └── Models/          # DeliveryCostResult, DeliveryTimeResult
├── Application/         # Business logic and use cases
│   ├── Interfaces/      # Abstractions (IDeliveryCostCalculator, IDeliveryTimeEstimator)
│   └── Services/        # Implementations
├── Console/             # CLI entry point (presentation layer)
├── Tests/               # Unit tests (xUnit)
└── TestInputs/          # Sample input files
```

### SOLID Principles Applied

| Principle | Implementation |
|-----------|----------------|
| **Single Responsibility** | Each class has one purpose: `Package` holds package data, `DeliveryCostCalculator` calculates costs, `PackageSelector` selects optimal packages |
| **Open/Closed** | New offer types can be added without modifying `DeliveryCostCalculator` |
| **Liskov Substitution** | `DeliveryCostCalculator` and `DeliveryTimeEstimator` can be substituted via their interfaces |
| **Interface Segregation** | `IDeliveryCostCalculator` and `IDeliveryTimeEstimator` are focused, minimal interfaces |
| **Dependency Inversion** | `DeliveryTimeEstimator` depends on `IDeliveryCostCalculator` abstraction, not concrete implementation |

### Key Design Patterns

1. **Strategy Pattern**: Offer eligibility checking via `Offer.IsEligible()` method
2. **Immutable Entities**: `Package` and `Offer` are immutable after construction
3. **Constructor Validation**: All entities validate input in constructors, failing fast with `ArgumentException`

### Algorithm: Package Selection (Knapsack Problem)

The `PackageSelector` solves a variant of the **0/1 Knapsack Problem** with these selection criteria (in priority order):

1. **Maximize total weight** within vehicle capacity
2. **Minimize package count** (fewer packages preferred for same weight)
3. **Prefer heavier individual packages** (tiebreaker)

**Implementation**: Uses combinatorial approach generating all 2^n combinations and selecting optimal based on criteria above.

### Algorithm: Vehicle Assignment (Greedy)

The `DeliveryTimeEstimator` uses a **greedy algorithm**:

1. Select the vehicle with earliest availability
2. Assign optimal package combination to that vehicle
3. Update vehicle availability (trip time × 2 for round trip)
4. Repeat until all packages are delivered

## Build & Run

```bash
# Build the solution
dotnet build

# Run all unit tests
dotnet test

# Run CLI - Problem 1 (Cost Estimation)
cat TestInputs/problem1_input.txt | dotnet run --project Console

# Run CLI - Problem 2 (Time Estimation)
cat TestInputs/problem2_input.txt | dotnet run --project Console
```

### Windows PowerShell
```powershell
Get-Content TestInputs/problem1_input.txt | dotnet run --project Console
Get-Content TestInputs/problem2_input.txt | dotnet run --project Console
```

## Input Format

**Problem 1 - Cost Estimation:**
```
base_delivery_cost no_of_packages
pkg_id weight_in_kg distance_in_km [offer_code]
...
```

**Problem 2 - Time Estimation:**
```
base_delivery_cost no_of_packages
pkg_id weight_in_kg distance_in_km [offer_code]
...
no_of_vehicles max_speed max_weight
```

## Output Format

**Problem 1:** `pkg_id discount total_cost`

**Problem 2:** `pkg_id discount total_cost estimated_delivery_time`

## Cost Calculation Formula

```
Base Cost = base_delivery_cost + (weight × 10) + (distance × 5)
Discount = Base Cost × (discount_percentage / 100)
Total Cost = Base Cost - Discount
```

## Available Offers

| Code | Discount | Distance Range | Weight Range |
|------|----------|----------------|--------------|
| OFR001 | 10% | 0-200 km | 70-200 kg |
| OFR002 | 7% | 50-150 km | 100-250 kg |
| OFR003 | 5% | 50-250 km | 10-150 kg |

## Test Coverage

The solution includes **30 unit tests** covering:

- **Domain Entities**: Package, Offer, Vehicle validation and behavior
- **DeliveryCostCalculator**: Base cost, discount eligibility, multiple packages
- **DeliveryTimeEstimator**: Single/multiple vehicles, time calculations, ordering
- **PackageSelector**: Edge cases, optimal selection criteria, combinations

## Example

**Input (Problem 2):**
```
100 5
PKG1 50 30 OFR001
PKG2 75 125 OFR008
PKG3 175 100 OFR003
PKG4 110 60 OFR002
PKG5 155 95 NA
2 70 200
```

**Output:**
```
PKG1 0 750 4.00
PKG2 0 1475 1.79
PKG3 0 2350 1.43
PKG4 105 1395 0.86
PKG5 0 2125 4.21
```

## Technologies

- .NET 10.0
- C# with nullable reference types
- xUnit for testing
- Clean Architecture
