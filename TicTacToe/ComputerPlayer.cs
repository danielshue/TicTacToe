using System;

namespace TicTacToe
{
    /// <summary>
    /// Represents a computer player in the Tic-Tac-Toe game.
    /// </summary>
    public class ComputerPlayer
    {
        private readonly ITicTacToeBoard _board;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputerPlayer"/> class.
        /// </summary>
        /// <param name="board">The Tic-Tac-Toe board the computer player will interact with.</param>
        public ComputerPlayer(ITicTacToeBoard board)
        {
            _board = board;
        }

        /// <summary>
        /// Makes a move for the computer player.
        /// </summary>
        /// <param name="playerSymbol">The symbol representing the computer player.</param>
        /// <returns>True if the move was made successfully, false otherwise.</returns>
        public bool MakeMove(char playerSymbol)
        {
            ITicTacToeBoard clonedBoard = _board.Clone();
            return TryMakeMove(clonedBoard, playerSymbol);
        }

        /// <summary>
        /// Tries to make a move for the computer player.
        /// </summary>
        /// <param name="clonedBoard">A cloned version of the current board.</param>
        /// <param name="playerSymbol">The symbol representing the computer player.</param>
        /// <returns>True if the move was made successfully, false otherwise.</returns>
        private bool TryMakeMove(ITicTacToeBoard clonedBoard, char playerSymbol)
        {
            // Try to make a winning move
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    if (clonedBoard.IsCellEmpty(i, j))
                    {
                        // Simulate the move
                        clonedBoard.PlaceSymbol(i, j, playerSymbol);
                        if (clonedBoard.CheckForWin(playerSymbol))
                        {
                            // Apply the move to the actual board
                            _board.PlaceSymbol(i, j, playerSymbol);
                            return true;
                        }
                        clonedBoard.PlaceSymbol(i, j, ' '); // Undo move
                    }
                }
            }

            // Make a random move if no winning move is possible
            Random rand = new();
            int row = -1, col = -1;

            // Find an open spot and set the initial row and col
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
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
                if (row != -1 && col != -1)
                {
                    break;
                }
            }

            // Try to place the symbol in a random spot
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

            // If no move was made, return false
            if (row == -1 || col == -1)
            {
                return false;
            }

            // Apply the move to the actual board
            _board.PlaceSymbol(row, col, playerSymbol);

            return true;
        }
    }
}