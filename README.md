# Tic-Tac-Toe Game

[![.NET Core Desktop](https://github.com/danielshue/TicTacToe/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/danielshue/TicTacToe/actions/workflows/dotnet-desktop.yml)

## Overview
Tic-Tac-Toe is a classic two-player game where players take turns marking a space in a 3x3 grid with their respective symbols (X or O). The objective is to be the first player to get three of their symbols in a row, either horizontally, vertically, or diagonally.

## Files
- **Program.cs**: Entry point of the application that initializes the game and manages the game loop.
- **Game.cs**: Contains the `Game` class which handles the game logic, including player turns and determining the winner.
- **TickTacToeBoard.cs**: Implements `ITicTacToeBoard` interface and represents the game board, including methods to display and update the board state.
- **Player.cs**: Represents a player in the game with properties for the player's name and symbol.
- **ComputerPlayer.cs**: Extends the `Player` class to implement an AI opponent with different difficulty levels.
- **ScoreManager.cs**: Implements `IScoreManager` interface to manage game scores and track wins, losses, and draws.
- **GameInitializer.cs**: Implements `IGameInitializer` interface to handle the initialization of game components.
- **TicTacToeConsoleUI.cs**: Implements `ITickTacToeUI` interface for the console-based user interface.
- **SystemConsole.cs**: Implements `IConsole` interface providing console I/O operations.
- **Score.cs**: Implements `IScore` interface representing the score data structure.
- **DifficultyLevel.cs**: Defines difficulty levels for the computer player.

### Interfaces
- **ITicTacToeBoard.cs**: Interface defining the contract for the game board operations.
- **ITickTacToeUI.cs**: Interface for the UI components of the game.
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

## How to Run the Application
1. Ensure you have the .NET SDK installed on your machine (requires .NET 9.0).
2. Clone the repository or download the project files.
3. Open a terminal and navigate to the project directory.
4. Run the following command to build the project:
   ```
   dotnet build
   ```
5. Run the tests to ensure everything is working:
   ```
   dotnet test
   ```
6. Publish the project using the command:
   ```
   dotnet publish -c Release --self-contained
   ```
7. After a successful build, run the application using the appropriate architecture: [win-x64|win-arm64|linux-x64|osx-x64]:
   ```
   bin\Release\net9.0\[architecture]\publish\TicTacToe.exe
   ```
8. Follow the on-screen instructions to play the game.

## Game Rules
1. The game is played on a 3x3 grid.
2. Players take turns placing their symbol (X or O) in an empty square.
3. The first player to align three of their symbols in a row (horizontally, vertically, or diagonally) wins the game.
4. If all squares are filled and no player has three in a row, the game ends in a draw.
