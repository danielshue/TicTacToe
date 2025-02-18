# Tic-Tac-Toe Game

[![.NET Core Desktop](https://github.com/danielshue/TicTacToe/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/danielshue/TicTacToe/actions/workflows/dotnet-desktop.yml)

## Overview
Tic-Tac-Toe is a classic two-player game where players take turns marking a space in a 3x3 grid with their respective symbols (X or O). The objective is to be the first player to get three of their symbols in a row, either horizontally, vertically, or diagonally.

## Files
- **Program.cs**: Entry point of the application that initializes the game and manages the game loop.
- **Game.cs**: Contains the `Game` class which handles the game logic, including player turns and determining the winner.
- **Board.cs**: Represents the game board and includes methods to display and update the board state.
- **Player.cs**: Represents a player in the game with properties for the player's name and symbol.
- **ComputerPlayer.cs**: Extends the `Player` class to implement an AI opponent.
- **ScoreManager.cs**: Manages the scores and keeps track of wins, losses, and draws.
- **GameInitializer.cs**: Handles the initialization of game components.
- **ITickTacToeUI.cs**: Interface for the UI components of the game.
- **Score.cs**: Represents the score data structure.
- **TicTacToe.csproj**: Project file containing configuration settings for the C# console application.
- **TicTacToeConsoleUI.cs**: Implements the console-based user interface for the game.
- **../TicTacToe.Tests/TickTacToeBoardTests.cs**: Contains unit tests for the `TickTacToeBoard` class.

## How to Run the Application
1. Ensure you have the .NET SDK installed on your machine.
2. Clone the repository or download the project files.
3. Open a terminal and navigate to the project directory.
4. Run the following command to build the project:5. After a successful build, run the application using:

## Game Rules
1. The game is played on a 3x3 grid.
2. Players take turns placing their symbol (X or O) in an empty square.
3. The first player to align three of their symbols in a row (horizontally, vertically, or diagonally) wins the game.
4. If all squares are filled and no player has three in a row, the game ends in a draw.
