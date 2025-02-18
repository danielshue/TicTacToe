namespace TicTacToe
{
    /// <summary>
    /// The entry point of the Tic Tac Toe application.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The main method, which serves as the entry point for the application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        static void Main(string[] args) {
            ITicTacToeBoard board = new TickTacToeBoard();
            ITickTacToeUI output = new TicTacToeConsoleUI(board);
            Game game = new Game(output);
            game.StartGame();
        }
    }
}