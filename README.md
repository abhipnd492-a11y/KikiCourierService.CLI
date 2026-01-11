# Kiki's Courier Service

A CLI application for calculating delivery costs and estimating delivery times.

## Project Structure

```
KikiCourierService.CLI/
├── Domain/          # Entities (Package, Offer, Vehicle)
├── Application/     # Business logic services
├── Console/         # CLI entry point
├── Tests/           # Unit tests
└── TestInputs/      # Sample input files
```

## Build & Run

```bash
# Build
dotnet build

# Run tests
dotnet test

# Run Problem 1 (Cost Estimation)
cat TestInputs/problem1_input.txt | dotnet run --project Console

# Run Problem 2 (Time Estimation)
cat TestInputs/problem2_input.txt | dotnet run --project Console
```

**Windows PowerShell:**
```powershell
Get-Content TestInputs/problem1_input.txt | dotnet run --project Console
```

## Input/Output Format

**Problem 1:**
```
base_delivery_cost no_of_packages
pkg_id weight distance [offer_code]
```
Output: `pkg_id discount total_cost`

**Problem 2:**
```
base_delivery_cost no_of_packages
pkg_id weight distance [offer_code]
no_of_vehicles max_speed max_weight
```
Output: `pkg_id discount total_cost delivery_time`

## Example

**Input:**
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

## Cost Formula

```
Base Cost = base_delivery_cost + (weight × 10) + (distance × 5)
Total Cost = Base Cost - Discount
```

## Available Offers

| Code | Discount | Distance | Weight |
|------|----------|----------|--------|
| OFR001 | 10% | 0-200 km | 70-200 kg |
| OFR002 | 7% | 50-150 km | 100-250 kg |
| OFR003 | 5% | 50-250 km | 10-150 kg |

## Technologies

- .NET 10.0 / C#
- xUnit for testing
- Clean Architecture
