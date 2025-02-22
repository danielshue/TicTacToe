namespace TicTacToe
{
    /// <summary>
    /// Entry point for the Console version of Tic Tac Toe.
    /// </summary>
    /// <remarks>
    /// This class serves as the bootstrap for the console interface implementation:
    /// 
    /// Initialization Flow:
    /// 1. Set up console environment (window size, colors)
    /// 2. Create TicTacToeConsoleUI instance
    /// 3. Initialize game board and players
    /// 4. Configure game settings through console interface
    /// 5. Start main game loop
    /// 
    /// Console Features:
    /// - Uses ITickTacToeConsoleUI for platform-specific operations
    /// - Manages console window properties
    /// - Handles console-specific initialization
    /// 
    /// Design Features:
    /// - Clean separation of console setup from game logic
    /// - Proper interface implementation selection
    /// - Environment-specific configuration
    /// </remarks>
    class Program
    {
        /// <summary>
        /// The main entry point for the Console Tic Tac Toe application.
        /// </summary>
        /// <remarks>
        /// Initializes the console environment, creates the console UI implementation,
        /// and starts the game with proper console-specific settings.
        /// </remarks>
        /// <param name="args">The command-line arguments.</param>
        static void Main(string[] args)
        {
            ITicTacToeBoard board = new TickTacToeBoard();
            ITickTacToeUI output = new TicTacToeConsoleUI(board);
            Game game = new Game(output);
            game.StartGame();
        }
    }
}