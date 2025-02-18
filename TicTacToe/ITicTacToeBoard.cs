namespace TicTacToe
{
    /// <summary>
    /// Interface for a Tic-Tac-Toe BoardArray.
    /// </summary>
    public interface ITicTacToeBoard
    {
        /// <summary>
        /// Gets the size of the board.
        /// </summary>
        public const int BoardSize = 3;

        /// <summary>
        /// Creates a clone of the current BoardArray.
        /// </summary>
        /// <returns>A new ITicTacToeBoard object that is a clone of the current BoardArray.</returns>
        ITicTacToeBoard Clone();

        /// <summary>
        /// Gets or sets the board array.
        /// </summary>

        char[,] BoardArray { get; set; }


        /// <summary>
        /// Clears the board.
        /// </summary>
        void ClearBoard();

        /// <summary>
        /// Places a symbol on the BoardArray at the specified position.
        /// </summary>
        /// <param name="row">The row to place the symbol.</param>
        /// <param name="col">The column to place the symbol.</param>
        /// <param name="symbol">The symbol to place.</param>
        /// <returns>True if the symbol was placed successfully, false otherwise.</returns>
        bool PlaceSymbol(int row, int col, char symbol);

        /// <summary>
        /// Checks if the specified symbol has won the game.
        /// </summary>
        /// <param name="symbol">The symbol to check for a win.</param>
        /// <returns>True if the symbol has won, false otherwise.</returns>
        bool CheckForWin(char symbol);

        /// <summary>
        /// Checks if the BoardArray is full.
        /// </summary>
        /// <returns>True if the BoardArray is full, false otherwise.</returns>
        bool IsBoardFull();

        /// <summary>
        /// Checks if a cell is empty.
        /// </summary>
        /// <param name="row">The row of the cell to check.</param>
        /// <param name="col">The column of the cell to check.</param>
        /// <returns>True if the cell is empty, false otherwise.</returns>
        bool IsCellEmpty(int row, int col);

        /// <summary>
        /// Counts the number of empty cells on the board.
        /// </summary>
        /// <returns>The number of empty cells.</returns>
        int CountEmptyCells();
    }
}