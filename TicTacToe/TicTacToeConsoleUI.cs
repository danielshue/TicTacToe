using System;
using System.Collections.Generic;
using System.Threading;

namespace TicTacToe
{
    /// <summary>
    /// Implements the console-based user interface for the Tic Tac Toe game.
    /// </summary>
    /// <remarks>
    /// This class provides a text-based interface through ITickTacToeConsoleUI while maintaining
    /// compatibility with the core game logic. Key responsibilities include:
    /// 
    /// Display Features:
    /// - Color-coded symbols: X in red, O in blue
    /// - Highlighted current position
    /// - Centered score display
    /// - Clear visual feedback
    /// 
    /// Input Handling:
    /// - Arrow key navigation
    /// - Enter to confirm moves
    /// - Escape to exit
    /// - Input validation
    /// 
    /// Console Management:
    /// - Window size optimization
    /// - Cursor visibility control
    /// - Color scheme management
    /// - Screen buffer management
    /// 
    /// The class demonstrates:
    /// - Clean separation between console-specific and general UI operations
    /// - Efficient screen updates
    /// - Responsive user interaction
    /// - Platform-specific optimizations
    /// </remarks>
    public class TicTacToeConsoleUI : ITickTacToeConsoleUI
    {
        private readonly IConsole _console;
        private static readonly Dictionary<string, ConsoleColor> _colorMap = new()
        {
            { "black", ConsoleColor.Black },
            { "blue", ConsoleColor.Blue },
            { "cyan", ConsoleColor.Cyan },
            { "darkblue", ConsoleColor.DarkBlue },
            { "darkcyan", ConsoleColor.DarkCyan },
            { "darkgray", ConsoleColor.DarkGray },
            { "darkgreen", ConsoleColor.DarkGreen },
            { "darkmagenta", ConsoleColor.DarkMagenta },
            { "darkred", ConsoleColor.DarkRed },
            { "darkyellow", ConsoleColor.DarkYellow },
            { "gray", ConsoleColor.Gray },
            { "green", ConsoleColor.Green },
            { "magenta", ConsoleColor.Magenta },
            { "red", ConsoleColor.Red },
            { "white", ConsoleColor.White },
            { "yellow", ConsoleColor.Yellow }
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="TicTacToeConsoleUI"/> class.
        /// </summary>
        /// <param name="board">The Tic-Tac-Toe board.</param>
        public TicTacToeConsoleUI(ITicTacToeBoard board, IConsole console = null)
        {
            Board = board ?? throw new ArgumentNullException(nameof(board));
            _console = console ?? new SystemConsole();
        }

        /// <inheritdoc />
        public ITicTacToeBoard Board { get; set; }

        /// <inheritdoc />
        public void Clear() => _console.Clear();

        /// <inheritdoc />
        public void SetCursorPosition(int left, int top) => _console.SetCursorPosition(left, top);

        /// <inheritdoc />
        public void SetBackgroundColor(string color)
        {
            if (!_colorMap.ContainsKey(color.ToLower()))
                throw new ArgumentException($"Invalid color: {color}", nameof(color));

            _console.BackgroundColor = _colorMap[color.ToLower()];
        }

        /// <inheritdoc />
        public void SetForegroundColor(string color)
        {
            if (!_colorMap.ContainsKey(color.ToLower()))
                throw new ArgumentException($"Invalid color: {color}", nameof(color));

            _console.ForegroundColor = _colorMap[color.ToLower()];
        }

        /// <inheritdoc />
        public void ResetColor() => _console.ResetColor();

        /// <inheritdoc />
        public int WindowWidth => _console.WindowWidth;

        /// <inheritdoc />
        public int CurrentRow { get; set; }

        /// <inheritdoc />
        public int CurrentCol { get; set; }

        /// <inheritdoc />
        public IScore Score { get; set; }

        /// <inheritdoc />
        public void DisplayScore()
        {
            Clear();
            SetCursorPosition(0, 0);
            int centerPosition = (WindowWidth - Score.ToString().Length) / 2;
            if (centerPosition > 0)
            {
                SetCursorPosition(centerPosition, 0);
            }
            _console.Write(Score.ToString());
            _console.WriteLine();
        }

        /// <inheritdoc />
        public void PlayerMove(Player currentPlayer)
        {
            bool validMove = false;
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30)); // 30 second timeout per move

            try
            {
                while (!validMove && !cts.Token.IsCancellationRequested)
                {
                    DisplayBoard(CurrentRow, CurrentCol);
                    SetCursorPosition(0, 10);
                    _console.Write($"{currentPlayer.Name}'s turn ({currentPlayer.Symbol}). Use arrow keys to move, Enter to confirm: ");
                    
                    var key = _console.ReadKey(true, cts.Token);
                    
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow when CurrentRow > 0:
                            CurrentRow--;
                            break;
                        case ConsoleKey.DownArrow when CurrentRow < ITicTacToeBoard.BoardSize - 1:
                            CurrentRow++;
                            break;
                        case ConsoleKey.LeftArrow when CurrentCol > 0:
                            CurrentCol--;
                            break;
                        case ConsoleKey.RightArrow when CurrentCol < ITicTacToeBoard.BoardSize - 1:
                            CurrentCol++;
                            break;
                        case ConsoleKey.Enter:
                            if (Board.IsCellEmpty(CurrentRow, CurrentCol))
                            {
                                validMove = Board.PlaceSymbol(CurrentRow, CurrentCol, currentPlayer.Symbol);
                                if (validMove) return;
                            }
                            break;
                        case ConsoleKey.Escape:
                            cts.Cancel();
                            break;
                    }

                    if (!validMove && !cts.Token.IsCancellationRequested)
                    {
                        Clear();
                        DisplayScore();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Handle timeout or cancellation
            }

            // If we reach here without a valid move (timeout/cancel), try to find any available spot
            if (!validMove)
            {
                // Try each cell until we find an empty one
                for (int i = 0; i < ITicTacToeBoard.BoardSize && !validMove; i++)
                {
                    for (int j = 0; j < ITicTacToeBoard.BoardSize && !validMove; j++)
                    {
                        if (Board.IsCellEmpty(i, j))
                        {
                            validMove = Board.PlaceSymbol(i, j, currentPlayer.Symbol);
                            if (validMove)
                            {
                                CurrentRow = i;
                                CurrentCol = j;
                                Clear();
                                DisplayScore();
                                DisplayBoard(CurrentRow, CurrentCol); // Show final position
                                SetCursorPosition(0, 10);
                                _console.WriteLine($"Move automatically placed at position {(char)('A' + i)}{j + 1} due to timeout.");
                                return;
                            }
                        }
                    }
                }

                if (!validMove)
                {
                    // If we get here, there were no valid moves available
                    Clear();
                    DisplayScore();
                    DisplayBoard(-1, -1); // Show board without highlighting
                    SetCursorPosition(0, 10);
                    _console.WriteLine("No valid moves available.");
                }
            }
        }

        /// <inheritdoc />
        public void DisplayGameBoard(int currentRow, int currentCol) => DisplayBoard(currentRow, currentCol);

        /// <inheritdoc />
        public void DisplayGameBoard() => Display();

        /// <inheritdoc />
        public void DisplayPlayerWin(Player player)
        {
            SetCursorPosition(0, 12);
            SetForegroundColor("green");
            _console.WriteLine($"{player.Name} wins!");
            ResetColor();
        }

        /// <inheritdoc />
        public void DisplayDraw()
        {
            SetCursorPosition(0, 12);
            SetForegroundColor("yellow");
            _console.WriteLine("It's a draw!");
            ResetColor();
        }

        /// <inheritdoc />
        public bool PromptPlayAgain()
        {
            SetCursorPosition(0, 14);
            _console.Write("Do you want to play again? (y/n) ");
            string response = ReadInput().ToUpper();
            return !response.StartsWith("N");
        }

        /// <inheritdoc />
        public string ReadInput() => _console.ReadLine() ?? string.Empty;

        /// <inheritdoc />
        public string GetPlayersName()
        {
            //Clear();
            //SetCursorPosition((WindowWidth - 20) / 2, 5);
            _console.CursorVisible = true;
            _console.Write("Enter your name: (Human Default) ");
            string name = ReadInput();
            _console.CursorVisible = false;
            return string.IsNullOrWhiteSpace(name) ? "Human" : name;
        }

        /// <inheritdoc />
        public void HandleDraw()
        {
            Clear();
            DisplayScore();
            DisplayGameBoard();
            DisplayDraw();
            Score.Draws++;
        }

        /// <inheritdoc />
        public void DisplayBoard(int currentRow, int currentCol)
        {
            int centerX = (WindowWidth - (ITicTacToeBoard.BoardSize * 4)) / 2;
            if (centerX < 0) centerX = 0;

            SetCursorPosition(centerX, 2);
            _console.WriteLine("   1   2   3");
            SetCursorPosition(centerX, 3);
            _console.WriteLine("  ───────────");

            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                SetCursorPosition(centerX, 4 + i * 2);
                _console.Write($"{(char)('A' + i)} ");
                
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    if (i == currentRow && j == currentCol)
                        SetBackgroundColor("darkgray");

                    char symbol = Board.BoardArray[i, j];
                    if (symbol == 'X') SetForegroundColor("red");
                    else if (symbol == 'O') SetForegroundColor("blue");

                    _console.Write($" {symbol} ");
                    ResetColor();
                    
                    if (j < ITicTacToeBoard.BoardSize - 1)
                        _console.Write("│");
                }
                
                if (i < ITicTacToeBoard.BoardSize - 1)
                {
                    SetCursorPosition(centerX, 5 + i * 2);
                    _console.WriteLine("  ───┼───┼───");
                }
            }
            
            _console.WriteLine("\n");
        }

        /// <inheritdoc />
        public void Display()
        {
            int centerX = (WindowWidth - (ITicTacToeBoard.BoardSize * 4)) / 2;
            if (centerX < 0) centerX = 0;

            SetCursorPosition(centerX, 2);
            _console.WriteLine("   1   2   3");
            SetCursorPosition(centerX, 3);
            _console.WriteLine("  ───────────");

            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                SetCursorPosition(centerX, 4 + i * 2);
                _console.Write($"{(char)('A' + i)} ");
                
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    char symbol = Board.BoardArray[i, j];
                    if (symbol == 'X') SetForegroundColor("red");
                    else if (symbol == 'O') SetForegroundColor("blue");

                    _console.Write($" {symbol} ");
                    ResetColor();
                    
                    if (j < ITicTacToeBoard.BoardSize - 1)
                        _console.Write("│");
                }
                
                if (i < ITicTacToeBoard.BoardSize - 1)
                {
                    SetCursorPosition(centerX, 5 + i * 2);
                    _console.WriteLine("  ───┼───┼───");
                }
            }
            
            _console.WriteLine("\n");
        }

        /// <inheritdoc />
        public DifficultyLevel PromptDifficultyLevel()
        {
            Clear();
            SetCursorPosition(0, 5);
            _console.WriteLine("Select difficulty level:");
            _console.WriteLine("1. Easy");
            _console.WriteLine("2. Medium");
            _console.WriteLine("3. Hard");
            _console.Write("\nEnter choice (1-3) (Default: Hard): ");

            while (true)
            {
                string input = ReadInput();
                switch (input)
                {
                    case "1": return DifficultyLevel.Easy;
                    case "2": return DifficultyLevel.Medium;
                    case "3": return DifficultyLevel.Hard;
                    default:
                        return DifficultyLevel.Hard;
                }
            }
        }

        public char GetPlayersSymbol()
        {
            while (true)
            {
                _console.Write("Do you want to play as X or O? (X goes first and is default - GetPlayersSymbol method): ");
                var input = _console.ReadLine()?.ToUpper().Trim();
                if (input == "X" || input == "O")
                {
                    return input[0];
                } else {
                    return 'X';
                }
            }
        }        
    }
}