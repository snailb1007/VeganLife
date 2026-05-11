<div align="center">

# VeganLife
*A modern cross-platform application empowering the vegan lifestyle.*

[![Language](https://img.shields.io/badge/C%23-10.0-239120.svg?style=flat-square&logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Platform](https://img.shields.io/badge/.NET-MAUI-512BD4.svg?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/en-us/apps/maui)
[![Architecture](https://img.shields.io/badge/Architecture-MVVM-5ed9c7.svg?style=flat-square)](#)

</div>

VeganLife is a cross-platform mobile and desktop application built with .NET MAUI. It is designed to support and enrich the vegan lifestyle through modern features and a seamless user experience.

## Features

- 🌱 **Vegan-First Experience** - Tailored specifically for the vegan community.
- 📱 **Cross-Platform** - Runs smoothly on Android, iOS, Windows, and macOS from a single codebase.
- 🏗️ **Clean Architecture** - Follows the MVVM (Model-View-ViewModel) pattern for robust and maintainable code.
- 🗄️ **Robust Data Management** - Includes `VeganLifeDataCenter` for structured data handling and migrations via Entity Framework Core.

## Project Structure

- **`VeganLife`**: The main .NET MAUI application containing the UI (Views), ViewModels, Models, and platform-specific code.
- **`VeganLifeDataCenter`**: The data access layer, handling database migrations and data modeling.

## Tech Stack

- **Framework:** [.NET MAUI](https://dotnet.microsoft.com/en-us/apps/maui)
- **Language:** C#
- **Architecture:** MVVM (Model-View-ViewModel)
- **Data Access:** Entity Framework Core

## Getting Started

### Prerequisites

- Visual Studio 2022 (with .NET MAUI workload installed) or Visual Studio Code with the .NET MAUI extension.
- .NET 8.0 SDK (or compatible version).

### Build & Run

1. Clone the repository:
   ```bash
   git clone https://github.com/snailb1007/VeganLife.git
   ```
2. Open the solution file `VeganLife.sln` in Visual Studio.
3. Set `VeganLife` as the startup project.
4. Select your target framework (Android, iOS, Windows, or Mac Catalyst) and hit Run.

## License
This project is proprietary and confidential.
