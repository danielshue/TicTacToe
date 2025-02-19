using System;

namespace TicTacToe
{

    /// <summary>
    /// Handles the user interface aspects of the Tic-Tac-Toe game.
    /// </summary>
    /// <remarks>
    /// The TicTacToeConsoleUI class focuses on displaying the game board, handling user input, and 
    /// managing the display of game-related information such as the score and game results. This separation 
    /// of concerns aligns with the single responsibility principle.
    /// 
    /// Key responsibilities of the TicTacToeConsoleUI class:
    /// 1. Display Logic:
    ///     - Displays the game board with or without highlighting the current position.
    ///     - Displays the current score.
    ///     - Displays messages indicating the game result (win or draw).
    /// 2. User Input Handling:
    ///     - Handles player moves by capturing keyboard input.
    ///     - Prompts the user to play again and reads the input.
    ///     - Reads the player's name.
    /// 3. UI Management:
    ///     - Clears the console.
    ///     - Sets the cursor position.
    ///     - Sets the background and foreground colors.
    ///     - Resets the console colors.
    ///     - Shows and hides the cursor.
    /// 
    /// The class does not include any game logic or state management, which is appropriate. 
    /// Game logic and state management should be handled by separate classes, 
    /// such as <see cref="TickTacToeBoard"/> and Game, to maintain a clear separation of concerns.
    /// </remarks>
    public class TicTacToeConsoleUI : ITickTacToeUI
    {
        private readonly IConsole _console;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicTacToeConsoleUI"/> class.
        /// </summary>
        /// <param name="board">The Tic-Tac-Toe board.</param>
        public TicTacToeConsoleUI(ITicTacToeBoard board, IConsole console = null)
        {
            CurrentRow = 0;
            CurrentCol = 0;
            Board = board;
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
            switch (color.ToLower())
            {
                case "black":
                    _console.BackgroundColor = ConsoleColor.Black;
                    break;
                case "blue":
                    _console.BackgroundColor = ConsoleColor.Blue;
                    break;
                case "cyan":
                    _console.BackgroundColor = ConsoleColor.Cyan;
                    break;
                case "darkblue":
                    _console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;
                case "darkcyan":
                    _console.BackgroundColor = ConsoleColor.DarkCyan;
                    break;
                case "darkgray":
                    _console.BackgroundColor = ConsoleColor.DarkGray;
                    break;
                case "darkgreen":
                    _console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                case "darkmagenta":
                    _console.BackgroundColor = ConsoleColor.DarkMagenta;
                    break;
                case "darkred":
                    _console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                case "darkyellow":
                    _console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
                case "gray":
                    _console.BackgroundColor = ConsoleColor.Gray;
                    break;
                case "green":
                    _console.BackgroundColor = ConsoleColor.Green;
                    break;
                case "magenta":
                    _console.BackgroundColor = ConsoleColor.Magenta;
                    break;
                case "red":
                    _console.BackgroundColor = ConsoleColor.Red;
                    break;
                case "white":
                    _console.BackgroundColor = ConsoleColor.White;
                    break;
                case "yellow":
                    _console.BackgroundColor = ConsoleColor.Yellow;
                    break;
                default:
                    throw new ArgumentException("Invalid color");
            }
        }

        /// <inheritdoc />
        public void SetForegroundColor(string color)
        {
            switch (color.ToLower())
            {
                case "black":
                    _console.ForegroundColor = ConsoleColor.Black;
                    break;
                case "blue":
                    _console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "cyan":
                    _console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case "darkblue":
                    _console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case "darkcyan":
                    _console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case "darkgray":
                    _console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case "darkgreen":
                    _console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case "darkmagenta":
                    _console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case "darkred":
                    _console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case "darkyellow":
                    _console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case "gray":
                    _console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case "green":
                    _console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "magenta":
                    _console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case "red":
                    _console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "white":
                    _console.ForegroundColor = ConsoleColor.White;
                    break;
                case "yellow":
                    _console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                default:
                    throw new ArgumentException("Invalid color");
            }
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

            int screenWidth = WindowWidth;
            int padding = (screenWidth - Score.ToString().Length) / 2;

            SetBackgroundColor(ConsoleColor.White.ToString());
            SetForegroundColor(ConsoleColor.Black.ToString());
            SetCursorPosition(0, 0);
            _console.WriteLine(new string(' ', screenWidth));
            SetCursorPosition(padding, 0);
            _console.Write(Score.ToString());
            ResetColor();
            _console.WriteLine(""); // Add an empty line after the score
        }

        /// <inheritdoc />
        public void PlayerMove(Player currentPlayer)
        {
            while (true)
            {
                ConsoleKeyInfo keyInfo = _console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        CurrentRow = (CurrentRow > 0) ? CurrentRow - 1 : CurrentRow;
                        break;
                    case ConsoleKey.DownArrow:
                        CurrentRow = (CurrentRow < 2) ? CurrentRow + 1 : CurrentRow;
                        break;
                    case ConsoleKey.LeftArrow:
                        CurrentCol = (CurrentCol > 0) ? CurrentCol - 1 : CurrentCol;
                        break;
                    case ConsoleKey.RightArrow:
                        CurrentCol = (CurrentCol < 2) ? CurrentCol + 1 : CurrentCol;
                        break;
                    case ConsoleKey.Enter:
                        if (Board.PlaceSymbol(CurrentRow, CurrentCol, currentPlayer.Symbol))
                        {
                            return;
                        }
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                }

                Clear();
                DisplayScore();
                DisplayBoard(CurrentRow, CurrentCol);
            }
        }

        /// <inheritdoc />
        public void DisplayGameBoard(int currentRow, int currentCol) => DisplayBoard(currentRow, currentCol);

        /// <inheritdoc />
        public void DisplayGameBoard()
        {
            Display();
            _console.WriteLine("\n\n");
        }

        /// <inheritdoc />
        public void DisplayPlayerWin(Player player) => _console.WriteLine($"{player.Name} wins!");

        /// <inheritdoc />
        public void DisplayDraw() => _console.WriteLine("It's a draw!");

        /// <inheritdoc />
        public bool PromptPlayAgain()
        {
            _console.Write("Do you want to play again? (y/n) ");

            ShowCursor();

            string playAgain = ReadInput().ToLower();

            if (playAgain == "y")
            {
                HideCursor();
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public string ReadInput() => _console.ReadLine();

        /// <inheritdoc />
        public string GetPlayersName()
        {
            ShowCursor();

            _console.Write("Enter your name: ");

            string playerName = ReadInput();

            HideCursor();

            return playerName;
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
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    if (i == currentRow && j == currentCol)
                    {
                        SetBackgroundColor(ConsoleColor.Gray.ToString());
                        SetForegroundColor(ConsoleColor.Black.ToString());
                    }
                    else if (Board.BoardArray[i, j] == 'X')
                    {
                        SetForegroundColor(ConsoleColor.Red.ToString());
                    }
                    else if (Board.BoardArray[i, j] == 'O')
                    {
                        SetForegroundColor(ConsoleColor.Blue.ToString());
                    }

                    _console.Write(Board.BoardArray[i, j].ToString());
                    
                    ResetColor();

                    if (j < ITicTacToeBoard.BoardSize - 1) _console.Write("|");
                }

                _console.WriteLine("");

                if (i < ITicTacToeBoard.BoardSize - 1) _console.WriteLine("-----");
            }
        }

        /// <inheritdoc />
        public void Display() => DisplayBoard(-1, -1);

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
            _console.WriteLine("Select difficulty level:");
            _console.WriteLine("1. Easy");
            _console.WriteLine("2. Medium");
            _console.WriteLine("3. Hard");
            
            ShowCursor();
            
            while (true)
            {
                string input = ReadInput().Trim();
                switch (input)
                {
                    case "1":
                        HideCursor();
                        return DifficultyLevel.Easy;
                    case "2":
                        HideCursor();
                        return DifficultyLevel.Medium;
                    case "3":
                        HideCursor();
                        return DifficultyLevel.Hard;
                    default:
                        _console.WriteLine("Invalid input. Please enter 1, 2, or 3:");
                        break;
                }
            }
        }
    }
}