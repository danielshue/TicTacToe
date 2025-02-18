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
        /// <summary>
        /// Initializes a new instance of the <see cref="TicTacToeConsoleUI"/> class.
        /// </summary>
        /// <param name="board">The Tic-Tac-Toe board.</param>
        public TicTacToeConsoleUI(ITicTacToeBoard board)
        {
            CurrentRow = 0;
            CurrentCol = 0;
            Board = board;
        }

        /// <inheritdoc />
        public ITicTacToeBoard Board { get; set; }

        /// <inheritdoc />
        public void Clear()
        {
            Console.Clear();
        }

        /// <inheritdoc />
        public void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }

        /// <inheritdoc />
        public void SetBackgroundColor(string color)
        {
            switch (color.ToLower())
            {
                case "black":
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case "blue":
                    Console.BackgroundColor = ConsoleColor.Blue;
                    break;
                case "cyan":
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    break;
                case "darkblue":
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;
                case "darkcyan":
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    break;
                case "darkgray":
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    break;
                case "darkgreen":
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                case "darkmagenta":
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    break;
                case "darkred":
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                case "darkyellow":
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
                case "gray":
                    Console.BackgroundColor = ConsoleColor.Gray;
                    break;
                case "green":
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;
                case "magenta":
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    break;
                case "red":
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case "white":
                    Console.BackgroundColor = ConsoleColor.White;
                    break;
                case "yellow":
                    Console.BackgroundColor = ConsoleColor.Yellow;
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
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case "blue":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "cyan":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case "darkblue":
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case "darkcyan":
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case "darkgray":
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case "darkgreen":
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case "darkmagenta":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case "darkred":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case "darkyellow":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case "gray":
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case "green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "magenta":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case "red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "white":
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "yellow":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                default:
                    throw new ArgumentException("Invalid color");
            }
        }

        /// <inheritdoc />
        public void ResetColor()
        {
            Console.ResetColor();
        }

        /// <inheritdoc />
        public int WindowWidth => Console.WindowWidth;

        /// <inheritdoc />
        public int CurrentRow { get; set; }

        /// <inheritdoc />
        public int CurrentCol { get; set; }

        /// <inheritdoc />
        public Score Score { get; set; }

        /// <inheritdoc />
        public void DisplayScore()
        {
            Clear();

            int screenWidth = WindowWidth;
            int padding = (screenWidth - Score.ToString().Length) / 2;

            SetBackgroundColor(ConsoleColor.White.ToString());
            SetForegroundColor(ConsoleColor.Black.ToString());
            SetCursorPosition(0, 0);
            Console.WriteLine(new string(' ', screenWidth));
            SetCursorPosition(padding, 0);
            Console.Write(Score.ToString());
            ResetColor();
            Console.WriteLine(""); // Add an empty line after the score
        }

        /// <inheritdoc />
        public void PlayerMove(Player currentPlayer)
        {
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

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
        public void DisplayGameBoard(int currentRow, int currentCol)
        {
            DisplayBoard(currentRow, currentCol);
        }

        /// <inheritdoc />
        public void DisplayGameBoard()
        {
            Display();
            Console.WriteLine("\n\n");
        }

        /// <inheritdoc />
        public void DisplayPlayerWin(Player player)
        {
            Console.WriteLine($"{player.Name} wins!");
        }

        /// <inheritdoc />
        public void DisplayDraw()
        {
            Console.WriteLine("It's a draw!");
        }

        /// <inheritdoc />
        public bool PromptPlayAgain()
        {
            Console.Write("Do you want to play again? (y/n) ");

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
        public string ReadInput()
        {
            return Console.ReadLine();
        }

        /// <inheritdoc />
        public string GetPlayersName()
        {
            ShowCursor();

            Console.Write("Enter your name: ");

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

                    Console.Write(Board.BoardArray[i, j].ToString());
                    ResetColor();

                    if (j < ITicTacToeBoard.BoardSize - 1) Console.Write("|");
                }

                Console.WriteLine("");

                if (i < ITicTacToeBoard.BoardSize - 1) Console.WriteLine("-----");
            }
        }

        /// <inheritdoc />
        public void Display()
        {
            DisplayBoard(-1, -1);
        }

        /// <summary>
        /// Shows the cursor in the console.
        /// </summary>
        /// <remarks>
        /// This method sets the <see cref="Console.CursorVisible"/> property to true, making the 
        /// cursor visible in the console. It is typically used when user input is required, such as 
        /// when prompting the player for their name or asking if they want to play again.
        /// </remarks>
        private void ShowCursor()
        {
            Console.CursorVisible = true;
        }

        /// <summary>
        /// Hides the cursor in the console.
        /// </summary>
        /// <remarks>
        /// This method sets the <see cref="Console.CursorVisible"/> property to false, making the cursor 
        /// invisible in the console. It is typically used to improve the visual appearance of the console 
        /// during game play, where the cursor might otherwise be distracting.
        /// </remarks>
        private void HideCursor()
        {
            Console.CursorVisible = false;
        }

    }
}