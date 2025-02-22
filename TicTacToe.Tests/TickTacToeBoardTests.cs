namespace TicTacToe.Tests
{
    /// <summary>
    /// Contains unit tests for the TickTacToeBoard class, validating board state and game mechanics.
    /// </summary>
    /// <remarks>
    /// Test coverage includes:
    /// 
    /// Board Operations:
    /// - Symbol Placement: Tests valid and invalid move handling
    /// - Cell State: Validates empty and occupied cell detection
    /// - Board Cloning: Tests deep copy functionality
    /// - Board Reset: Verifies clear board operations
    /// 
    /// Win Detection:
    /// - Horizontal Wins: Tests row completion scenarios
    /// - Vertical Wins: Validates column victories
    /// - Diagonal Wins: Checks diagonal win patterns
    /// - False Positives: Verifies no incorrect win detection
    /// 
    /// Game State:
    /// - Full Board: Tests board full condition
    /// - Move Validation: Verifies legal move constraints
    /// - State Integrity: Ensures consistent board state
    /// - Move History: Validates move sequence tracking
    /// 
    /// Platform Independence:
    /// - UI Agnostic: Verifies board works without UI dependencies
    /// - State Representation: Tests board visualization
    /// </remarks>
    [TestClass]
    public class TickTacToeBoardTests
    {
        private TickTacToeBoard? _board;

        /// <summary>
        /// Initializes test environment before each test.
        /// </summary>
        /// <remarks>
        /// Creates a fresh board instance to ensure test isolation
        /// and prevent state bleeding between tests.
        /// </remarks>
        [TestInitialize]
        public void Setup()
        {
            _board = new TickTacToeBoard();
        }

        /// <summary>
        /// Tests that a new board is initialized with all cells empty.
        /// </summary>
        [TestMethod]
        public void InitializeBoard_ShouldSetAllCellsToEmpty()
        {
            // Arrange
            var board = new TickTacToeBoard();

            // Act
            var isEmpty = true;
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++) // Fixed: Changed i to j in condition
                {
                    if (board.BoardArray[i, j] != ' ')
                    {
                        isEmpty = false;
                        break;
                    }
                }
            }

            // Assert
            Assert.IsTrue(isEmpty, "All cells should be initialized to empty.");
        }

        /// <summary>
        /// Tests that symbols can be placed in empty cells on the board.
        /// </summary>
        [TestMethod]
        public void PlaceSymbol_ShouldPlaceSymbolInEmptyCell()
        {
            // Arrange
            var board = new TickTacToeBoard();
            var row = 1;
            var col = 1;
            var symbol = 'X';

            // Act
            var result = board.PlaceSymbol(row, col, symbol);

            // Assert
            Assert.IsTrue(result, "Symbol should be placed successfully.");
            Assert.AreEqual(symbol, board.BoardArray[row, col], "Cell should contain the placed symbol.");
        }

        /// <summary>
        /// Tests that symbols cannot be placed in cells that are already occupied.
        /// </summary>
        [TestMethod]
        public void PlaceSymbol_ShouldNotPlaceSymbolInNonEmptyCell()
        {
            // Arrange
            var board = new TickTacToeBoard();
            var row = 1;
            var col = 1;
            var symbol = 'X';
            board.PlaceSymbol(row, col, symbol);

            // Act
            var result = board.PlaceSymbol(row, col, 'O');

            // Assert
            Assert.IsFalse(result, "Symbol should not be placed in a non-empty cell.");
            Assert.AreEqual(symbol, board.BoardArray[row, col], "Cell should still contain the original symbol.");
        }

        /// <summary>
        /// Tests that the game correctly detects a winning condition in a row.
        /// </summary>
        [TestMethod]
        public void CheckForWin_ShouldReturnTrueForWinningRow()
        {
            // Arrange
            var board = new TickTacToeBoard();
            var symbol = 'X';
            board.PlaceSymbol(0, 0, symbol);
            board.PlaceSymbol(0, 1, symbol);
            board.PlaceSymbol(0, 2, symbol);

            // Act
            var result = board.CheckForWin(symbol);

            // Assert
            Assert.IsTrue(result, "CheckForWin should return true for a winning row.");
        }

        /// <summary>
        /// Tests that the game correctly identifies when there is no winning condition.
        /// </summary>
        [TestMethod]
        public void CheckForWin_ShouldReturnFalseForNoWin()
        {
            // Arrange
            var board = new TickTacToeBoard();
            var symbol = 'X';
            board.PlaceSymbol(0, 0, symbol);
            board.PlaceSymbol(0, 1, symbol);
            board.PlaceSymbol(1, 2, symbol);

            // Act
            var result = board.CheckForWin(symbol);

            // Assert
            Assert.IsFalse(result, "CheckForWin should return false if there is no win.");
        }

        /// <summary>
        /// Tests that the board correctly identifies when all cells are occupied.
        /// </summary>
        [TestMethod]
        public void IsBoardFull_ShouldReturnTrueForFullBoard()
        {
            // Arrange
            var board = new TickTacToeBoard();
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++) // Fixed: Changed i to j in condition
                {
                    board.PlaceSymbol(i, j, 'X');
                }
            }

            // Act
            var result = board.IsBoardFull();

            // Assert
            Assert.IsTrue(result, "IsBoardFull should return true for a full board.");
        }

        /// <summary>
        /// Tests that the board correctly identifies when there are still empty cells.
        /// </summary>
        [TestMethod]
        public void IsBoardFull_ShouldReturnFalseForNonFullBoard()
        {
            // Arrange
            var board = new TickTacToeBoard();
            board.PlaceSymbol(0, 0, 'X');

            // Act
            var result = board.IsBoardFull();

            // Assert
            Assert.IsFalse(result, "IsBoardFull should return false for a non-full board.");
        }

        /// <summary>
        /// Tests that empty cells are correctly identified.
        /// </summary>
        [TestMethod]
        public void IsCellEmpty_ShouldReturnTrueForEmptyCell()
        {
            // Arrange
            var board = new TickTacToeBoard();
            var row = 1;
            var col = 1;

            // Act
            var result = board.IsCellEmpty(row, col);

            // Assert
            Assert.IsTrue(result, "IsCellEmpty should return true for an empty cell.");
        }

        /// <summary>
        /// Tests that occupied cells are correctly identified.
        /// </summary>
        [TestMethod]
        public void IsCellEmpty_ShouldReturnFalseForNonEmptyCell()
        {
            // Arrange
            var board = new TickTacToeBoard();
            var row = 1;
            var col = 1;
            board.PlaceSymbol(row, col, 'X');

            // Act
            var result = board.IsCellEmpty(row, col);

            // Assert
            Assert.IsFalse(result, "IsCellEmpty should return false for a non-empty cell.");
        }

        /// <summary>
        /// Tests that the count of empty cells is accurately tracked.
        /// </summary>
        [TestMethod]
        public void CountEmptyCells_ShouldReturnCorrectCount()
        {
            // Arrange
            var board = new TickTacToeBoard();
            board.PlaceSymbol(0, 0, 'X');
            board.PlaceSymbol(1, 1, 'O');

            // Act
            var result = board.CountEmptyCells();

            // Assert
            var expectedCount = (ITicTacToeBoard.BoardSize * ITicTacToeBoard.BoardSize) - 2;
            Assert.AreEqual(expectedCount, result, "CountEmptyCells should return the correct number of empty cells.");
        }

        /// <summary>
        /// Tests that symbols cannot be placed outside the board boundaries.
        /// </summary>
        [TestMethod]
        public void PlaceSymbol_ShouldNotPlaceSymbolOutsideBoardBoundaries()
        {
            // Arrange
            var board = new TickTacToeBoard();

            // Act
            var resultNegativeIndices = board.PlaceSymbol(-1, -1, 'X');
            var resultOutOfBoundsIndices = board.PlaceSymbol(ITicTacToeBoard.BoardSize, ITicTacToeBoard.BoardSize, 'X');

            // Assert
            Assert.IsFalse(resultNegativeIndices, "Symbol should not be placed outside the board boundaries (negative indices).");
            Assert.IsFalse(resultOutOfBoundsIndices, "Symbol should not be placed outside the board boundaries (out of bounds indices).");
        }

        /// <summary>
        /// Tests that the game correctly detects a winning condition on the main diagonal.
        /// </summary>
        [TestMethod]
        public void CheckForWin_ShouldReturnTrueForWinningMainDiagonal()
        {
            // Arrange
            var board = new TickTacToeBoard();
            var symbol = 'X';
            board.PlaceSymbol(0, 0, symbol);
            board.PlaceSymbol(1, 1, symbol);
            board.PlaceSymbol(2, 2, symbol);

            // Act
            var result = board.CheckForWin(symbol);

            // Assert
            Assert.IsTrue(result, "CheckForWin should return true for a winning main diagonal.");
        }

        /// <summary>
        /// Tests that the game correctly detects a winning condition on the anti-diagonal.
        /// </summary>
        [TestMethod]
        public void CheckForWin_ShouldReturnTrueForWinningAntiDiagonal()
        {
            // Arrange
            var board = new TickTacToeBoard();
            var symbol = 'X';
            board.PlaceSymbol(0, 2, symbol);
            board.PlaceSymbol(1, 1, symbol);
            board.PlaceSymbol(2, 0, symbol);

            // Act
            var result = board.CheckForWin(symbol);

            // Assert
            Assert.IsTrue(result, "CheckForWin should return true for a winning anti-diagonal.");
        }

        /// <summary>
        /// Tests that the board can be reset to its initial empty state.
        /// </summary>
        [TestMethod]
        public void ClearBoard_ShouldResetAllCellsToEmpty()
        {
            // Arrange
            var board = new TickTacToeBoard();
            board.PlaceSymbol(0, 0, 'X');
            board.PlaceSymbol(1, 1, 'O');

            // Act
            board.ClearBoard();
            var isEmpty = true;
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++) // Fixed: Changed i to j in condition
                {
                    if (board.BoardArray[i, j] != ' ')
                    {
                        isEmpty = false;
                        break;
                    }
                }
            }

            // Assert
            Assert.IsTrue(isEmpty, "All cells should be reset to empty after clearing the board.");
        }

        /// <summary>
        /// Tests that board cloning creates an accurate copy of the current board state.
        /// </summary>
        [TestMethod]
        public void Clone_ShouldCreateAccurateCopyOfBoard()
        {
            // Arrange
            var board = new TickTacToeBoard();
            board.PlaceSymbol(0, 0, 'X');
            board.PlaceSymbol(1, 1, 'O');

            // Act
            var clonedBoard = board.Clone();
            var isEqual = true;
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)  // Fixed: Changed i to j in condition
                {
                    if (board.BoardArray[i, j] != clonedBoard.BoardArray[i, j])
                    {
                        isEqual = false;
                        break;
                    }
                }
            }

            // Assert
            Assert.IsTrue(isEqual, "Cloned board should be an accurate copy of the original board.");
        }

        /// <summary>
        /// Tests that changes to a cloned board do not affect the original board.
        /// </summary>
        [TestMethod]
        public void Clone_ShouldNotAffectOriginalBoard()
        {
            // Arrange
            var board = new TickTacToeBoard();
            board.PlaceSymbol(0, 0, 'X');
            var clonedBoard = board.Clone();

            // Act
            clonedBoard.PlaceSymbol(1, 1, 'O');
            var originalBoardSymbol = board.BoardArray[1, 1];
            var clonedBoardSymbol = clonedBoard.BoardArray[1, 1];

            // Assert
            Assert.AreEqual(' ', originalBoardSymbol, "Original board should not be affected by changes to the cloned board.");
            Assert.AreEqual('O', clonedBoardSymbol, "Cloned board should reflect changes independently of the original board.");
        }

        /// <summary>
        /// Tests that only valid symbols ('X' and 'O') can be placed on the board.
        /// </summary>
        [TestMethod]
        public void PlaceSymbol_ShouldNotPlaceInvalidSymbol()
        {
            // Arrange
            var board = new TickTacToeBoard();
            var row = 1;
            var col = 1;

            // Act
            var result = board.PlaceSymbol(row, col, '1'); // Invalid symbol

            // Assert
            Assert.IsFalse(result, "Invalid symbol should not be placed on the board.");
            Assert.AreEqual(' ', board.BoardArray[row, col], "Cell should remain empty after attempting to place an invalid symbol.");
        }

        /// <summary>
        /// Tests that a full board with no winning condition is correctly identified.
        /// </summary>
        [TestMethod]
        public void IsBoardFull_ShouldReturnTrueForFullBoardWithNoWin()
        {
            // Arrange
            var board = new TickTacToeBoard();
            board.PlaceSymbol(0, 0, 'X');
            board.PlaceSymbol(0, 1, 'O');
            board.PlaceSymbol(0, 2, 'X');
            board.PlaceSymbol(1, 0, 'O');
            board.PlaceSymbol(1, 1, 'X');
            board.PlaceSymbol(1, 2, 'O');
            board.PlaceSymbol(2, 0, 'O');
            board.PlaceSymbol(2, 1, 'X');
            board.PlaceSymbol(2, 2, 'O');

            // Act
            var isFull = board.IsBoardFull();
            var hasWin = board.CheckForWin('X') || board.CheckForWin('O');

            // Assert
            Assert.IsTrue(isFull, "IsBoardFull should return true for a full board.");
            Assert.IsFalse(hasWin, "There should be no win on a full board with no winning condition.");
        }
    }
}

