# Tic-Tac-Toe Game

[![.NET Core Desktop](https://github.com/danielshue/TicTacToe/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/danielshue/TicTacToe/actions/workflows/dotnet-desktop.yml)

## Overview
Tic-Tac-Toe is a classic two-player game where players take turns marking a space in a 3x3 grid with their respective symbols (X or O). The objective is to be the first player to get three of their symbols in a row, either horizontally, vertically, or diagonally.

This project features three different user interfaces:
- **Console UI**: A text-based interface implementing the `ITickTacToeConsoleUI` interface.
- **Windows Forms UI**: A graphical interface that uses Windows Forms.
- **.NET MAUI**: A modern cross-platform UI that runs on Windows, macOS, iOS, and Android.

## Files
- **Program.cs**: Entry point of the application that initializes the game and manages the game loop.
- **Game.cs**: Contains the `Game` class which handles the game logic, including player turns and determining the winner.
- **TickTacToeBoard.cs**: Implements `ITicTacToeBoard` interface and represents the game board, including methods to display and update the board state.
- **Player.cs**: Represents a player in the game with properties for the player's name and symbol.
- **ComputerPlayer.cs**: Extends the `Player` class to implement an AI opponent with different difficulty levels.
- **ScoreManager.cs**: Implements `IScoreManager` interface to manage game scores and track wins, losses, and draws.
- **GameInitializer.cs**: Implements `IGameInitializer` interface to handle the initialization of game components.
- **TicTacToeConsoleUI.cs**: Implements the console-based user interface; it utilizes the new `ITickTacToeConsoleUI` interface.
- **SystemConsole.cs**: Implements `IConsole` interface providing console I/O operations.
- **Score.cs**: Implements `IScore` interface representing the score data structure.
- **DifficultyLevel.cs**: Defines difficulty levels for the computer player.

### Interfaces
- **ITicTacToeBoard.cs**: Interface defining the contract for the game board operations.
- **ITickTacToeUI.cs**: Interface for the UI components of the game common to all platforms.
- **ITickTacToeConsoleUI.cs**: Extends `ITickTacToeUI` with console-specific functionality.
- **IScoreManager.cs**: Interface defining score management operations.
- **IScore.cs**: Interface for the score data structure.
- **IGameInitializer.cs**: Interface for game initialization operations.
- **IConsole.cs**: Interface abstracting console operations.

### Test Files
The project includes comprehensive unit tests in the TicTacToe.Tests project:
- **ComputerPlayerTests.cs**: Tests for the AI opponent logic
- **GameInitializerTests.cs**: Tests for game initialization
- **GameTests.cs**: Tests for core game logic
- **PlayerTests.cs**: Tests for player functionality
- **ScoreManagerTests.cs**: Tests for score management
- **TickTacToeBoardTests.cs**: Tests for game board operations
- **TicTacToeConsoleUITests.cs**: Tests for console UI implementation

## Framework Support
- .NET 9.0 (Primary target)
- .NET 8.0 (Compatibility target)

## Dependencies
- **Core Project**:
  - Microsoft.Extensions.DependencyInjection (v8.0.0 for .NET 8.0, v9.0.2 for .NET 9.0)
  - Newtonsoft.Json (v13.0.3)

- **MAUI Project**:
  - Microsoft.Maui.Controls (v8.0.7)
  - Microsoft.Extensions.Logging.Debug (v8.0.0)

- **Test Project**:
  - MSTest (v3.8.0)
  - Moq (v4.20.72)
  - xUnit (v2.9.3)
  - Microsoft.NET.Test.Sdk (v17.13.0)

## Development Prerequisites
- **For All Platforms**:
  - .NET SDK 9.0 or later
  - Visual Studio 2022 or later / Visual Studio Code

- **For MAUI Development**:
  - Windows:
    - Visual Studio 2022 with .NET MAUI workload
    - Windows 10/11
  - macOS:
    - Visual Studio for Mac
    - macOS 11 or later
    - Xcode (for iOS development)
  - Mobile Development:
    - Android SDK (for Android development)
    - Xcode (for iOS development, requires Mac)

## How to Run the Application
1. Ensure you have the .NET SDK installed on your machine (requires .NET 9.0).
2. Clone the repository or download the project files.
3. Open a terminal and navigate to the project directory.
4. Build the project:
   ```
   dotnet build
   ```
5. Run the tests to ensure everything is working:
   ```
   dotnet test
   ```

### Running Different Versions

#### Console Version
1. Navigate to the `TicTacToe` directory:
   ```
   cd TicTacToe
   dotnet run
   ```

#### Windows Forms Version
1. Navigate to the `TicTacToe.WinForms` directory:
   ```
   cd TicTacToe.WinForms
   dotnet run
   ```

#### .NET MAUI Version
1. Ensure you have the .NET MAUI workload installed:
   ```
   dotnet workload install maui
   ```
2. Navigate to the `TicTacToe.Maui` directory:
   ```
   cd TicTacToe.Maui
   dotnet run
   ```

Note: For mobile deployment, you'll need the appropriate development environment:
- iOS: Requires a Mac with Xcode installed
- Android: Requires Android SDK
- Windows: Requires Windows 10/11
- macOS: Requires macOS 11 or later

### Publishing the Application
To publish the project as a self-contained application, run:
```
dotnet publish -c Release --self-contained -r [win-x64|win-arm64|linux-x64|osx-x64]
```
Replace `[win-x64|win-arm64|linux-x64|osx-x64]` with the appropriate runtime identifier.

## Continuous Integration / Deployment
The project uses GitHub Actions for continuous integration and testing. The workflow:
- Triggers on pushes and pull requests to the master branch
- Builds the solution in both Debug and Release configurations
- Runs all unit tests
- Uses .NET 9.0 SDK
- Runs on Windows latest runners

The build status can be seen in the badge at the top of this README.

Configuration is in `.github/workflows/dotnet-desktop.yml`.

## Testing
The project includes comprehensive unit tests using multiple testing frameworks:
- MSTest as the primary testing framework
- Moq for mocking in unit tests
- xUnit for additional testing capabilities

To run specific test categories:
```
dotnet test --filter "Category=UnitTest"
dotnet test --filter "Category=Integration"
```

## Code Quality and Testing

### Test Coverage
The test suite provides comprehensive coverage across all components:
- **Game Logic**: Tests for win detection, draw conditions, and turn management
- **AI Strategy**: Dedicated tests for each difficulty level's behavior
- **Board Management**: Validation of board state and move legality
- **UI Integration**: Tests ensuring UI-agnostic implementation
- **Score Tracking**: Verification of score management and persistence

### Testing Philosophy
- All new features must include corresponding unit tests
- Tests should be independent and reproducible
- Mock objects are used to isolate system components
- Both happy path and edge cases are covered

### Test Categories
- **Unit Tests**: Testing individual components in isolation
- **Integration Tests**: Testing component interactions
- **Strategy Tests**: Verifying AI behavior across difficulty levels
- **Platform Tests**: Ensuring consistent behavior across UI implementations

To run specific test suites:
```
dotnet test --filter "ClassName=GameTests"
dotnet test --filter "ClassName=ComputerPlayerTests"
dotnet test --filter "ClassName=TicTacToeBoardTests"
```

## Coding Standards
This project follows specific C# coding guidelines:

### Naming Conventions
- Use PascalCase for class and interface names
- Prefix private fields with an underscore (e.g., `_myField`)

### Best Practices
- Use expression-bodied members for concise property getters and small methods
- Use `var` only when the type is obvious from the right side of the assignment
- Follow standard C# naming conventions for all code elements

For detailed coding guidelines, see `code-style.md` in the root directory.

## Game Rules
1. The game is played on a 3x3 grid.
2. Players take turns placing their symbol (X or O) in an empty square.
3. The first player to align three of their symbols in a row (horizontally, vertically, or diagonally) wins the game.
4. If all squares are filled and no player has three in a row, the game ends in a draw.

## AI Gameplay Features

### Difficulty Levels
The game features three AI difficulty levels:

1. **Easy Mode**
   - Makes random moves
   - May miss obvious winning opportunities
   - Perfect for beginners or casual play

2. **Medium Mode**
   - Uses basic strategy
   - Takes winning moves when available
   - Blocks opponent's winning moves
   - Suitable for intermediate players

3. **Hard Mode** (Default)
   - Uses optimal strategy
   - Implements minimax algorithm
   - Takes center position when available
   - Creates and blocks forks
   - Challenging for experienced players

The difficulty level can be selected at the start of each game session and affects the computer player's decision-making throughout the game.

## Contributing
1. Fork the repository
2. Create a new branch for your feature
3. Follow the coding standards in `code-style.md`
4. Write or update tests for any changes
5. Submit a pull request

All contributions must follow the established coding standards and include appropriate tests.

## License
This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

Copyright (c) 2025 Daniel Shue
