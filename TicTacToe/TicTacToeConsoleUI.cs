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
            while (!validMove)
            {
                DisplayBoard(CurrentRow, CurrentCol);
                SetCursorPosition(0, 10);
                _console.Write($"{currentPlayer.Name}'s turn ({currentPlayer.Symbol}). Use arrow keys to move, Enter to confirm: ");
                
                var key = _console.ReadKey(true);
                
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
                        }
                        break;
                }

                if (!validMove)
                {
                    Clear();
                    DisplayScore();
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
            return response.StartsWith("Y");
        }

        /// <inheritdoc />
        public string ReadInput() => _console.ReadLine() ?? string.Empty;

        /// <inheritdoc />
        public string GetPlayersName()
        {
            Clear();
            SetCursorPosition((WindowWidth - 20) / 2, 5);
            _console.CursorVisible = true;
            _console.Write("Enter your name: ");
            string name = ReadInput();
            _console.CursorVisible = false;
            return string.IsNullOrWhiteSpace(name) ? "Player" : name;
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
                    _console.WriteLine("  ─┼───┼─");
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
                    _console.WriteLine("  ─┼───┼─");
                }
            }
            
            _console.WriteLine("\n");
        }

        /// <summary>
        /// Shows the cursor in the console.
        /// </summary>
        /// <remarks>
        /// This method sets the <see cref="Console.CursorVisible"/> property to true, making the 
        /// cursor visible in the console. It is typically used when user input is required, such as 
        /// when prompting the player for their name or asking if they want to play again.
        /// </remarks>
        private void ShowCursor() => _console.CursorVisible = true;

        /// <summary>
        /// Hides the cursor in the console.
        /// </summary>
        /// <remarks>
        /// This method sets the <see cref="Console.CursorVisible"/> property to false, making the cursor 
        /// invisible in the console. It is typically used to improve the visual appearance of the console 
        /// during game play, where the cursor might otherwise be distracting.
        /// </remarks>
        private void HideCursor() => _console.CursorVisible = false;

        /// <inheritdoc />
        public DifficultyLevel PromptDifficultyLevel()
        {
            Clear();
            SetCursorPosition(0, 5);
            _console.WriteLine("Select difficulty level:");
            _console.WriteLine("1. Easy");
            _console.WriteLine("2. Medium");
            _console.WriteLine("3. Hard");
            _console.Write("\nEnter choice (1-3): ");

            while (true)
            {
                string input = ReadInput();
                switch (input)
                {
                    case "1": return DifficultyLevel.Easy;
                    case "2": return DifficultyLevel.Medium;
                    case "3": return DifficultyLevel.Hard;
                    default:
                        SetCursorPosition(0, 11);
                        SetForegroundColor("red");
                        _console.WriteLine("Invalid choice! Enter 1, 2, or 3.");
                        ResetColor();
                        SetCursorPosition(0, 9);
                        _console.Write("Enter choice (1-3): ");
                        break;
                }
            }
        }
    }
}