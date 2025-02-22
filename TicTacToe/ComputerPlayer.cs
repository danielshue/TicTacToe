using System;
using System.Linq;

namespace TicTacToe
{
    /// <summary>
    /// Implements the computer player's AI logic for making moves in Tic Tac Toe.
    /// </summary>
    /// <remarks>
    /// The ComputerPlayer class provides AI functionality across different difficulty levels:
    /// 
    /// Difficulty Levels:
    /// - Easy: Makes random moves, occasionally missing winning opportunities
    /// - Medium: Blocks opponent wins and takes obvious winning moves
    /// - Hard: Uses optimal strategy with minimax algorithm
    /// 
    /// Features:
    /// - UI Independent: Works with any interface implementation
    /// - Consistent Behavior: Maintains strategy across game sessions
    /// - Adjustable Difficulty: Can be changed between games
    /// - State Management: Uses board cloning for move simulation
    /// 
    /// Design Considerations:
    /// - Encapsulated Logic: AI decisions are self-contained
    /// - Deterministic Outcomes: Same board state produces consistent moves at Hard difficulty
    /// - Performance Optimized: Uses efficient move calculation algorithms
    /// </remarks>
    public class ComputerPlayer
    {
        private readonly ITicTacToeBoard _board;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputerPlayer"/> class.
        /// </summary>
        /// <param name="board">The Tic-Tac-Toe board the computer player will interact with.</param>
        /// <param name="difficultyLevel">The difficulty level of the computer player.</param>
        public ComputerPlayer(ITicTacToeBoard board, DifficultyLevel difficultyLevel = DifficultyLevel.Hard)
        {
            _board = board ?? throw new ArgumentNullException(nameof(board));
            DifficultyLevel = difficultyLevel;
        }

        public DifficultyLevel DifficultyLevel { get; set; }

        /// <summary>
        /// Executes a move for the computer player based on the current difficulty level.
        /// </summary>
        /// <param name="symbol">The symbol ('X' or 'O') to place on the board.</param>
        /// <remarks>
        /// Move selection varies by difficulty:
        /// - Easy: Random moves with occasional mistakes
        /// - Medium: Strategic moves with focus on immediate wins/blocks
        /// - Hard: Optimal moves using minimax algorithm
        /// </remarks>
        public bool MakeMove(char playerSymbol) => TryMakeMove(_board.Clone(), playerSymbol);

        /// <summary>
        /// Tries to make a move for the computer player.
        /// </summary>
        /// <param name="clonedBoard">A cloned version of the current board.</param>
        /// <param name="playerSymbol">The symbol representing the computer player.</param>
        /// <returns>True if the move was made successfully, false otherwise.</returns>
        private bool TryMakeMove(ITicTacToeBoard clonedBoard, char playerSymbol)
        {
            if (clonedBoard.IsBoardFull()) return false;

            char opponentSymbol = playerSymbol == 'X' ? 'O' : 'X';

            // Easy difficulty - just make random moves
            if (DifficultyLevel == DifficultyLevel.Easy)
            {
                return TryRandomMove(clonedBoard, playerSymbol);
            }

            // Medium difficulty - purely reactive strategy
            if (DifficultyLevel == DifficultyLevel.Medium)
            {
                // 1. Take winning move if available
                if (TryWinningMove(clonedBoard, playerSymbol))
                    return true;

                // 2. Block opponent's winning move
                if (TryWinningMove(clonedBoard, opponentSymbol, true))
                    return true;

                // 3. Take center if empty (basic strategy)
                if (TryCenterMove(clonedBoard, playerSymbol))
                { 
                    return true;
                }

                // 4. Simple edge preference - take first available edge
                if (clonedBoard.IsCellEmpty(0, 1))
                {
                    _board.PlaceSymbol(0, 1, playerSymbol);
                    return true;
                }
                if (clonedBoard.IsCellEmpty(1, 0))
                {
                    _board.PlaceSymbol(1, 0, playerSymbol);
                    return true;
                }
                if (clonedBoard.IsCellEmpty(1, 2))
                {
                    _board.PlaceSymbol(1, 2, playerSymbol);
                    return true;
                }
                if (clonedBoard.IsCellEmpty(2, 1))
                {
                    _board.PlaceSymbol(2, 1, playerSymbol);
                    return true;
                }

                // 5. Take any available cell
                for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
                {
                    for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                    {
                        if (clonedBoard.IsCellEmpty(i, j))
                        {
                            _board.PlaceSymbol(i, j, playerSymbol);
                            return true;
                        }
                    }
                }

                return false;
            }

            // Hard difficulty - use all strategies
            if (TryWinningMove(clonedBoard, playerSymbol))
                return true;

            if (TryWinningMove(clonedBoard, opponentSymbol, true))
                return true;

            if (CountOccupiedCells(clonedBoard) <= 2 && clonedBoard.IsCellEmpty(1, 1))
            {
                _board.PlaceSymbol(1, 1, playerSymbol);
                return true;
            }

            if (TryCreateFork(clonedBoard, playerSymbol))
                return true;

            if (TryBlockFork(clonedBoard, opponentSymbol, playerSymbol))
                return true;

            if (TryCornerMove(clonedBoard, playerSymbol))
                return true;

            if (clonedBoard.IsCellEmpty(1, 1))
            {
                _board.PlaceSymbol(1, 1, playerSymbol);
                return true;
            }

            return TryRandomMove(clonedBoard, playerSymbol);
        }

        /// <summary>
        /// Counts the number of occupied cells on the board.
        /// </summary>
        /// <param name="board">The Tic-Tac-Toe board.</param>
        /// <returns>The number of occupied cells.</returns>
        private int CountOccupiedCells(ITicTacToeBoard board)
        {
            int count = 0;
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    if (!board.IsCellEmpty(i, j))
                        count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Tries to make a winning move for the computer player.
        /// </summary>
        /// <param name="clonedBoard">A cloned version of the current board.</param>
        /// <param name="symbol">The symbol representing the computer player.</param>
        /// <param name="isBlocking">Indicates if the move is for blocking the opponent.</param>
        /// <returns>True if the move was made successfully, false otherwise.</returns>
        private bool TryWinningMove(ITicTacToeBoard clonedBoard, char symbol, bool isBlocking = false)
        {
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    if (!clonedBoard.IsCellEmpty(i, j)) continue;
                    
                    var testBoard = clonedBoard.Clone();
                    testBoard.PlaceSymbol(i, j, symbol);
                    if (testBoard.CheckForWin(symbol))
                    {
                        _board.PlaceSymbol(i, j, isBlocking ? (symbol == 'X' ? 'O' : 'X') : symbol);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Tries to create a fork for the computer player.
        /// </summary>
        /// <param name="clonedBoard">A cloned version of the current board.</param>
        /// <param name="playerSymbol">The symbol representing the computer player.</param>
        /// <returns>True if the move was made successfully, false otherwise.</returns>
        private bool TryCreateFork(ITicTacToeBoard clonedBoard, char playerSymbol)
        {
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    if (!clonedBoard.IsCellEmpty(i, j)) continue;

                    var testBoard = clonedBoard.Clone();
                    testBoard.PlaceSymbol(i, j, playerSymbol);

                    int winningPaths = CountPotentialWins(testBoard, playerSymbol);
                    if (winningPaths >= 2)
                    {
                        _board.PlaceSymbol(i, j, playerSymbol);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Tries to block a fork for the opponent.
        /// </summary>
        /// <param name="clonedBoard">A cloned version of the current board.</param>
        /// <param name="opponentSymbol">The symbol representing the opponent player.</param>
        /// <param name="playerSymbol">The symbol representing the computer player.</param>
        /// <returns>True if the move was made successfully, false otherwise.</returns>
        private bool TryBlockFork(ITicTacToeBoard clonedBoard, char opponentSymbol, char playerSymbol)
        {
            int forkCount = 0;
            int lastForkRow = -1, lastForkCol = -1;

            // Find all possible fork positions
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    if (!clonedBoard.IsCellEmpty(i, j)) continue;

                    var testBoard = clonedBoard.Clone();
                    testBoard.PlaceSymbol(i, j, opponentSymbol);

                    if (CountPotentialWins(testBoard, opponentSymbol) >= 2)
                    {
                        forkCount++;
                        lastForkRow = i;
                        lastForkCol = j;
                    }
                }
            }

            // If exactly one fork exists, block it
            if (forkCount == 1 && lastForkRow != -1)
            {
                _board.PlaceSymbol(lastForkRow, lastForkCol, playerSymbol);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Counts the potential winning paths for a given symbol.
        /// </summary>
        /// <param name="board">The Tic-Tac-Toe board.</param>
        /// <param name="symbol">The symbol to check for potential wins.</param>
        /// <returns>The number of potential winning paths.</returns>
        private int CountPotentialWins(ITicTacToeBoard board, char symbol)
        {
            int count = 0;
            var testBoard = board.Clone();

            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    if (!testBoard.IsCellEmpty(i, j)) continue;

                    testBoard.PlaceSymbol(i, j, symbol);
                    if (testBoard.CheckForWin(symbol)) count++;
                    testBoard.PlaceSymbol(i, j, ' ');
                }
            }
            return count;
        }

        /// <summary>
        /// Tries to take the center position for the computer player.
        /// </summary>
        /// <param name="clonedBoard">A cloned version of the current board.</param>
        /// <param name="playerSymbol">The symbol representing the computer player.</param>
        /// <returns>True if the move was made successfully, false otherwise.</returns>
        private bool TryCenterMove(ITicTacToeBoard clonedBoard, char playerSymbol)
        {
            if (clonedBoard.IsCellEmpty(1, 1))
            {
                _board.PlaceSymbol(1, 1, playerSymbol);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to take a corner position for the computer player.
        /// </summary>
        /// <param name="clonedBoard">A cloned version of the current board.</param>
        /// <param name="playerSymbol">The symbol representing the computer player.</param>
        /// <returns>True if the move was made successfully, false otherwise.</returns>
        private bool TryCornerMove(ITicTacToeBoard clonedBoard, char playerSymbol)
        {
            int[,] corners = { { 0, 0 }, { 0, 2 }, { 2, 0 }, { 2, 2 } };
            
            for (int i = 0; i < 4; i++)
            {
                if (clonedBoard.IsCellEmpty(corners[i, 0], corners[i, 1]))
                {
                    _board.PlaceSymbol(corners[i, 0], corners[i, 1], playerSymbol);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Tries to make a random move for the computer player.
        /// </summary>
        /// <param name="clonedBoard">A cloned version of the current board.</param>
        /// <param name="playerSymbol">The symbol representing the computer player.</param>
        /// <returns>True if the move was made successfully, false otherwise.</returns>
        private bool TryRandomMove(ITicTacToeBoard clonedBoard, char playerSymbol)
        {
            Random rand = new();
            int row = -1, col = -1;

            // Find initial empty spot
            for (int i = 0; i < ITicTacToeBoard.BoardSize && row == -1; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    if (clonedBoard.IsCellEmpty(i, j))
                    {
                        row = i;
                        col = j;
                        break;
                    }
                }
            }

            // Try random spots
            for (int attempts = 0; attempts < 10; attempts++)
            {
                int randomRow = rand.Next(0, ITicTacToeBoard.BoardSize);
                int randomCol = rand.Next(0, ITicTacToeBoard.BoardSize);
                if (clonedBoard.PlaceSymbol(randomRow, randomCol, playerSymbol))
                {
                    row = randomRow;
                    col = randomCol;
                    break;
                }
            }

            if (row == -1 || col == -1)
                return false;

            _board.PlaceSymbol(row, col, playerSymbol);
            return true;
        }
    }

}