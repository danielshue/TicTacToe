namespace TicTacToe
{
    /// <summary>
    /// Interface for a Tic-Tac-Toe board.
    /// </summary>
    public interface ITicTacToeBoard
    {
        /// <summary>
        /// The size of the board (3x3).
        /// </summary>
        /// <remarks>
        /// This constant defines the dimensions of the Tic-Tac-Toe board.
        /// The board is always square, with this value representing both width and height.
        /// </remarks>
        const int BoardSize = 3;

        /// <summary>
        /// Gets or sets the board array.
        /// </summary>
        char[,] BoardArray { get; set; }

        /// <summary>
        /// Creates a deep copy of the current board.
        /// </summary>
        /// <returns>A new instance of ITicTacToeBoard with the same state.</returns>
        ITicTacToeBoard Clone();

        /// <summary>
        /// Places a symbol on the board at the specified position.
        /// </summary>
        /// <param name="row">The row position.</param>
        /// <param name="col">The column position.</param>
        /// <param name="playerSymbol">The player's symbol (X or O).</param>
        /// <returns>True if the symbol was placed successfully, false otherwise.</returns>
        bool PlaceSymbol(int row, int col, char playerSymbol);

        /// <summary>
        /// Checks if the specified symbol has won.
        /// </summary>
        /// <param name="playerSymbol">The symbol to check for a win.</param>
        /// <returns>True if the symbol has won, false otherwise.</returns>
        bool CheckForWin(char playerSymbol);

        /// <summary>
        /// Checks if the board is full.
        /// </summary>
        /// <returns>True if the board is full, false otherwise.</returns>
        bool IsBoardFull();

        /// <summary>
        /// Checks if a specific cell is empty.
        /// </summary>
        /// <param name="row">The row position.</param>
        /// <param name="col">The column position.</param>
        /// <returns>True if the cell is empty, false otherwise.</returns>
        bool IsCellEmpty(int row, int col);

        /// <summary>
        /// Counts the number of empty cells on the board.
        /// </summary>
        /// <returns>The number of empty cells.</returns>
        int CountEmptyCells();

        /// <summary>
        /// Clears the board, setting all cells to empty.
        /// </summary>
        void ClearBoard();
    }
}