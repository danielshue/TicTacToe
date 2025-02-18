namespace TicTacToe
{
    /// <summary>
    /// Represents a Tic-Tac-Toe board.
    /// The <see cref="TickTacToeBoard"/> class focuses on managing the state and logic of a Tic-Tac-Toe board. 
    /// It includes functionalities such as initializing the board, placing symbols, checking for wins, 
    /// and determining if the board is full or if a cell is empty. 
    /// </summary>
    /// <remarks>
    /// The <see cref="TickTacToeBoard"/>  class appears to have a proper level of abstraction for managing the 
    /// state and logic of a Tic-Tac-Toe board.It focuses on the core functionalities related to the game board, 
    /// such as initializing the board, placing symbols, checking for wins, and determining if the board is full 
    /// or if a cell is empty.This separation of concerns is appropriate and aligns with the single responsibility 
    /// principle.
    /// 
    /// Here are the key responsibilities of the TickTacToeBoard class:
    /// 
    /// 1.	Initialization:
    ///     �	Initializes the board with empty cells.
    ///     �	Provides constructors for creating a new board or initializing with a specified state.
    ///     
    /// 2.	State Management:
    ///     �	Manages the board array(BoardArray) and its size(BoardSize).
    ///     �	Provides methods to clear the board and clone the board state.
    ///     
    /// 3.	Game Logic:
    ///     �	Places symbols on the board.
    ///     �	Checks for win conditions(rows, columns, diagonals).
    ///     �	Checks if the board is full or if a cell is empty.
    ///     �	Counts the number of empty cells.
    /// 
    /// The class does not include any display or user interface logic, which is appropriate.
    /// Display logic should be handled by a separate class, such as TickTacToeConsoleUI, to maintain a clear 
    /// separation of concerns. Overall, the TickTacToeBoard class has a proper level of abstraction for managing 
    /// the state and logic of a Tic-Tac-Toe board.It encapsulates the core functionalities related to the game board 
    /// and does not include any unrelated responsibilities.
    /// </remarks>
    public class TickTacToeBoard : ITicTacToeBoard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TickTacToeBoard"/> class.
        /// </summary>
        public TickTacToeBoard() => Initialize();

        /// <summary>
        /// Initializes a new instance of the <see cref="TickTacToeBoard"/> class with a specified board state.
        /// </summary>
        /// <param name="board">The board array state to initialize with.</param>
        public TickTacToeBoard(char[,] board) => BoardArray = (char[,])board.Clone();

        /// <inheritdoc />
        public char[,] BoardArray { get; set; }

        /// <summary>
        /// Initializes the board array with empty cells.
        /// </summary>
        private void Initialize()
        {
            BoardArray = new char[ITicTacToeBoard.BoardSize, ITicTacToeBoard.BoardSize];
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    BoardArray[i, j] = ' ';
                }
            }
        }

        /// <inheritdoc />
        public ITicTacToeBoard Clone() => new TickTacToeBoard(BoardArray);

        /// <inheritdoc />
        public bool PlaceSymbol(int row, int col, char playerSymbol)
        {
            // Validate the player symbol
            if (playerSymbol != 'X' && playerSymbol != 'O') return false;

            // Validate the position and check if the cell is empty
            if (row < 0 || row >= ITicTacToeBoard.BoardSize || col < 0 || col >= ITicTacToeBoard.BoardSize || BoardArray[row, col] != ' ') return false;

            BoardArray[row, col] = playerSymbol;
            return true;
        }

        /// <inheritdoc />
        public bool CheckForWin(char playerSymbol)
        {
            // Check rows and columns
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                if ((BoardArray[i, 0] == playerSymbol && BoardArray[i, 1] == playerSymbol && BoardArray[i, 2] == playerSymbol) ||
                    (BoardArray[0, i] == playerSymbol && BoardArray[1, i] == playerSymbol && BoardArray[2, i] == playerSymbol)) return true;
            }

            // Check diagonals
            if ((BoardArray[0, 0] == playerSymbol && BoardArray[1, 1] == playerSymbol && BoardArray[2, 2] == playerSymbol) ||
                (BoardArray[0, 2] == playerSymbol && BoardArray[1, 1] == playerSymbol && BoardArray[2, 0] == playerSymbol)) return true;

            return false;
        }

        /// <inheritdoc />
        public bool IsBoardFull()
        {
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    if (BoardArray[i, j] == ' ') return false;
                }
            }
            return true;
        }

        /// <inheritdoc />
        public bool IsCellEmpty(int row, int col) => BoardArray[row, col] == ' ';

        /// <inheritdoc />
        public int CountEmptyCells()
        {
            int emptyCells = 0;
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    if (IsCellEmpty(i, j)) emptyCells++;
                }
            }
            return emptyCells;
        }

        /// <inheritdoc />
        public void ClearBoard() => Initialize();
    }
}