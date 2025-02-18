namespace TicTacToe.Tests
{
    [TestClass]
    public sealed class TickTacToeBoardTests
    {
        [TestMethod]
        public void InitializeBoard_ShouldSetAllCellsToEmpty()
        {
            // Arrange
            var board = new TickTacToeBoard();

            // Act
            var isEmpty = true;
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
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

        [TestMethod]
        public void IsBoardFull_ShouldReturnTrueForFullBoard()
        {
            // Arrange
            var board = new TickTacToeBoard();
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    board.PlaceSymbol(i, j, 'X');
                }
            }

            // Act
            var result = board.IsBoardFull();

            // Assert
            Assert.IsTrue(result, "IsBoardFull should return true for a full board.");
        }

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
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
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
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
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

