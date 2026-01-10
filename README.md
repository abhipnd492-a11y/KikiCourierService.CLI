# Kiki's Courier Service - CLI Application

Command-line application for calculating delivery costs and estimating delivery times.

## Project Structure

```
KikiCourierService.CLI/
├── Domain/              # Entities and Models
├── Application/         # Business Logic Services
├── Console/             # CLI Application
├── Tests/               # Unit Tests
└── TestInputs/          # Sample Input Files
```

## Build & Run

```bash
# Build
dotnet build

# Run Tests
dotnet test

# Run CLI - Problem 1 (Cost Estimation)
cat TestInputs/problem1_input.txt | dotnet run --project Console

# Run CLI - Problem 2 (Time Estimation)
cat TestInputs/problem2_input.txt | dotnet run --project Console
```

## Input Format

**Problem 1:**
```
base_delivery_cost no_of_packages
pkg_id weight_in_kg distance_in_km offer_code
...
```

**Problem 2:**
```
base_delivery_cost no_of_packages
pkg_id weight_in_kg distance_in_km offer_code
...
no_of_vehicles max_speed max_weight
```

## Output Format

**Problem 1:** `pkg_id discount total_cost`
**Problem 2:** `pkg_id discount total_cost estimated_time`
