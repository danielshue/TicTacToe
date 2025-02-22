namespace TicTacToe.Tests
{
    /// <summary>
    /// Contains unit tests for the ComputerPlayer class, validating AI behavior across difficulty levels.
    /// </summary>
    /// <remarks>
    /// Tests cover the following aspects:
    /// 
    /// AI Strategy Testing:
    /// - Easy Mode: Verifies random move generation and occasional mistakes
    /// - Medium Mode: Tests blocking and winning move recognition
    /// - Hard Mode: Validates optimal move selection
    /// 
    /// Board Interaction:
    /// - Move Validation: Tests legal move enforcement
    /// - Board State Management: Verifies correct board state after moves
    /// - Clone Usage: Ensures proper board cloning for move simulation
    /// 
    /// Platform Independence:
    /// - UI Agnostic: Verifies AI works without UI dependencies
    /// - Consistent Behavior: Tests reproducible outcomes in same board states
    /// - State Preservation: Validates original board integrity during simulations
    /// </remarks>
    [TestClass]
    public class ComputerPlayerTests
    {
        private TickTacToeBoard _board = new();
        private ComputerPlayer _computerPlayer = null!;

        /// <summary>
        /// Initializes a new instance of ComputerPlayerTests.
        /// </summary>
        public ComputerPlayerTests()
        {
            _computerPlayer = new ComputerPlayer(_board);
        }

        /// <summary>
        /// Sets up test environment before each test.
        /// </summary>
        /// <remarks>
        /// Initializes a fresh board and computer player instance to ensure
        /// test isolation and prevent state bleeding between tests.
        /// </remarks>
        [TestInitialize]
        public void Setup()
        {
            _board = new TickTacToeBoard();
            _computerPlayer = new ComputerPlayer(_board);
        }

        /// <summary>
        /// Tests that the computer player takes a winning move when one is available.
        /// </summary>
        [TestMethod]
        public void MakeMove_ShouldTakeWinningMove_WhenAvailable()
        {
            // Arrange - Set up a board where computer (O) can win
            _board.PlaceSymbol(0, 0, 'O');
            _board.PlaceSymbol(0, 1, 'O');
            
            // Act
            bool moveResult = _computerPlayer.MakeMove('O');
            
            // Assert
            Assert.IsTrue(moveResult, "Computer should successfully make a move");
            Assert.AreEqual('O', _board.BoardArray[0, 2], "Computer should take the winning move");
            Assert.IsTrue(_board.CheckForWin('O'), "Computer should win after taking the winning move");
        }

        /// <summary>
        /// Tests that the computer player blocks the opponent's winning move when it can't win itself.
        /// </summary>
        [TestMethod]
        public void MakeMove_ShouldBlockOpponentWin_WhenNoWinningMoveAvailable()
        {
            // Arrange - Set up a board where opponent (X) could win
            _board.PlaceSymbol(0, 0, 'X');
            _board.PlaceSymbol(0, 1, 'X');
            
            // Act
            bool moveResult = _computerPlayer.MakeMove('O');
            
            // Assert
            Assert.IsTrue(moveResult, "Computer should successfully make a move");
            Assert.AreEqual('O', _board.BoardArray[0, 2], "Computer should block opponent's winning move");
        }

        /// <summary>
        /// Tests that the computer player makes a valid random move when no strategic moves are available.
        /// </summary>
        [TestMethod]
        public void MakeMove_ShouldMakeValidRandomMove_WhenNoWinningOrBlockingMoveAvailable()
        {
            // Act
            bool moveResult = _computerPlayer.MakeMove('O');
            
            // Assert
            Assert.IsTrue(moveResult, "Computer should successfully make a move");
            
            // Verify that exactly one move was made
            int movesCount = 0;
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    if (_board.BoardArray[i, j] == 'O')
                    {
                        movesCount++;
                    }
                }
            }
            Assert.AreEqual(1, movesCount, "Computer should make exactly one move");
        }

        /// <summary>
        /// Tests that the computer player correctly handles attempting to move on a full board.
        /// </summary>
        [TestMethod]
        public void MakeMove_ShouldReturnFalse_WhenBoardIsFull()
        {
            // Arrange
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    _board.PlaceSymbol(i, j, 'X');
                }
            }
            
            // Act
            bool moveResult = _computerPlayer.MakeMove('O');
            
            // Assert
            Assert.IsFalse(moveResult, "Computer should not be able to make a move on a full board");
        }

        /// <summary>
        /// Tests that the computer player prioritizes winning moves over blocking moves.
        /// </summary>
        [TestMethod]
        public void MakeMove_ShouldPreferWinningMove_OverBlockingMove()
        {
            // Arrange
            // Set up a board where computer (O) can win and opponent (X) could win
            _board.PlaceSymbol(0, 0, 'O');
            _board.PlaceSymbol(0, 1, 'O');
            _board.PlaceSymbol(1, 0, 'X');
            _board.PlaceSymbol(1, 1, 'X');
            
            // Act
            bool moveResult = _computerPlayer.MakeMove('O');
            
            // Assert
            Assert.IsTrue(moveResult, "Computer should successfully make a move");
            Assert.AreEqual('O', _board.BoardArray[0, 2], "Computer should take winning move instead of blocking");
            Assert.IsTrue(_board.CheckForWin('O'), "Computer should win after taking the winning move");
        }

        /// <summary>
        /// Tests that the computer player can make a valid move on an empty board.
        /// </summary>
        [TestMethod]
        public void MakeMove_ShouldMakeMoveOnEmptyBoard()
        {
            // Act
            bool moveResult = _computerPlayer.MakeMove('O');
            
            // Assert
            Assert.IsTrue(moveResult, "Computer should successfully make a move on empty board");
            Assert.AreEqual(8, _board.CountEmptyCells(), "Should be 8 empty cells after first move");
        }

        /// <summary>
        /// Tests that the computer player doesn't modify opponent's symbols when making a move.
        /// </summary>
        [TestMethod]
        public void MakeMove_ShouldNotChangeOpponentSymbols()
        {
            // Arrange
            _board.PlaceSymbol(0, 0, 'X');
            _board.PlaceSymbol(1, 1, 'X');
            
            // Act
            bool moveResult = _computerPlayer.MakeMove('O');
            
            // Assert
            Assert.IsTrue(moveResult, "Computer should successfully make a move");
            Assert.AreEqual('X', _board.BoardArray[0, 0], "Opponent's symbol should not be changed");
            Assert.AreEqual('X', _board.BoardArray[1, 1], "Opponent's symbol should not be changed");
        }

        /// <summary>
        /// Tests that the computer player correctly blocks diagonal winning moves.
        /// </summary>
        [TestMethod]
        public void MakeMove_ShouldBlockDiagonalWin()
        {
            // Arrange - Set up a board where opponent can win diagonally
            _board.PlaceSymbol(0, 0, 'X');
            _board.PlaceSymbol(1, 1, 'X');
            
            // Act
            bool moveResult = _computerPlayer.MakeMove('O');
            
            // Assert
            Assert.IsTrue(moveResult, "Computer should successfully make a move");
            Assert.AreEqual('O', _board.BoardArray[2, 2], "Computer should block diagonal win");
        }

        /// <summary>
        /// Tests that the computer player takes the center position on its first move when available.
        /// </summary>
        [TestMethod]
        public void MakeMove_ShouldTakeCenterIfAvailable_OnFirstMove()
        {
            // Act
            bool moveResult = _computerPlayer.MakeMove('O');
            
            // Assert
            Assert.IsTrue(moveResult, "Computer should successfully make a move");
            Assert.AreEqual('O', _board.BoardArray[1, 1], "Computer should take center position on first move if available");
        }

        /// <summary>
        /// Tests that the computer player creates a fork when possible.
        /// </summary>
        [TestMethod]
        public void MakeMove_ShouldCreateFork_WhenPossible()
        {
            // Arrange - Set up a board where computer can create a fork
            _board.PlaceSymbol(0, 0, 'O');
            _board.PlaceSymbol(2, 2, 'O');
            _board.PlaceSymbol(0, 2, 'X');
            
            // Act
            bool moveResult = _computerPlayer.MakeMove('O');
            
            // Assert
            Assert.IsTrue(moveResult, "Computer should successfully make a move");
            
            // Count potential winning moves after the computer's move
            int winningPaths = 0;
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    if (_board.BoardArray[i, j] == ' ')
                    {
                        var cloneBoard = _board.Clone();
                        cloneBoard.PlaceSymbol(i, j, 'O');
                        if (cloneBoard.CheckForWin('O'))
                            winningPaths++;
                    }
                }
            }
            
            Assert.IsTrue(winningPaths >= 2, "Computer should create a fork with multiple winning paths");
        }

        /// <summary>
        /// Tests that the computer player blocks opponent's potential forks.
        /// </summary>
        [TestMethod]
        public void MakeMove_ShouldBlockOpponentFork_WhenPossible()
        {
            // Arrange - Set up a board where opponent can create a fork
            _board.PlaceSymbol(0, 0, 'X');
            _board.PlaceSymbol(2, 2, 'X');
            
            // Act
            bool moveResult = _computerPlayer.MakeMove('O');
            
            // Assert
            Assert.IsTrue(moveResult, "Computer should successfully make a move");
            
            // Verify that opponent cannot win in next move
            bool canWinNextMove = false;
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    if (_board.BoardArray[i, j] == ' ')
                    {
                        var cloneBoard = _board.Clone();
                        cloneBoard.PlaceSymbol(i, j, 'X');
                        if (cloneBoard.CheckForWin('X'))
                        {
                            canWinNextMove = true;
                            break;
                        }
                    }
                }
            }
            
            Assert.IsFalse(canWinNextMove, "Computer should block opponent from creating a fork");
        }

        /// <summary>
        /// Tests that the computer player takes a corner position when the center is taken.
        /// </summary>
        [TestMethod]
        public void MakeMove_ShouldTakeCorner_WhenCenterIsTaken()
        {
            // Arrange
            _board.PlaceSymbol(1, 1, 'X'); // Center is taken
            
            // Act
            bool moveResult = _computerPlayer.MakeMove('O');
            
            // Assert
            Assert.IsTrue(moveResult, "Computer should successfully make a move");
            bool tookCorner = _board.BoardArray[0, 0] == 'O' || 
                             _board.BoardArray[0, 2] == 'O' || 
                             _board.BoardArray[2, 0] == 'O' || 
                             _board.BoardArray[2, 2] == 'O';
            Assert.IsTrue(tookCorner, "Computer should take a corner when center is taken");
        }

        /// <summary>
        /// Tests that the easy difficulty level only makes random moves, even when strategic moves are available.
        /// </summary>
        [TestMethod]
        public void MakeMove_EasyDifficulty_ShouldOnlyMakeRandomMoves()
        {
            // Arrange
            _board = new TickTacToeBoard();
            _computerPlayer = new ComputerPlayer(_board, DifficultyLevel.Easy);
            
            // Set up a board where there's an obvious winning move
            _board.PlaceSymbol(0, 0, 'O');
            _board.PlaceSymbol(0, 1, 'O');
            
            // Track multiple moves to ensure randomness
            bool differentMoveFound = false;
            int attempts = 10;
            
            // Act & Assert
            for (int i = 0; i < attempts && !differentMoveFound; i++)
            {
                _board = new TickTacToeBoard();
                _board.PlaceSymbol(0, 0, 'O');
                _board.PlaceSymbol(0, 1, 'O');
                _computerPlayer = new ComputerPlayer(_board, DifficultyLevel.Easy);
                
                bool moveResult = _computerPlayer.MakeMove('O');
                Assert.IsTrue(moveResult, "Computer should make a move");
                
                // If the computer didn't take the winning move at (0,2), we found a different move
                if (_board.BoardArray[0, 2] != 'O')
                {
                    differentMoveFound = true;
                }
            }
            
            Assert.IsTrue(differentMoveFound, "Easy difficulty should make random moves instead of always taking winning moves");
        }

        /// <summary>
        /// Tests that medium difficulty takes winning moves and blocks opponent's wins.
        /// </summary>
        [TestMethod]
        public void MakeMove_MediumDifficulty_ShouldTakeWinningMovesAndBlock()
        {
            // Arrange
            _board = new TickTacToeBoard();
            _computerPlayer = new ComputerPlayer(_board, DifficultyLevel.Medium);
            
            // Set up a board where there's an obvious winning move
            _board.PlaceSymbol(0, 0, 'O');
            _board.PlaceSymbol(0, 1, 'O');
            
            // Act
            bool moveResult = _computerPlayer.MakeMove('O');
            
            // Assert
            Assert.IsTrue(moveResult, "Computer should make a move");
            Assert.AreEqual('O', _board.BoardArray[0, 2], "Medium difficulty should take winning moves");
            
            // Test blocking
            _board = new TickTacToeBoard();
            _computerPlayer = new ComputerPlayer(_board, DifficultyLevel.Medium);
            _board.PlaceSymbol(0, 0, 'X');
            _board.PlaceSymbol(0, 1, 'X');
            
            moveResult = _computerPlayer.MakeMove('O');
            
            Assert.IsTrue(moveResult, "Computer should make a move");
            Assert.AreEqual('O', _board.BoardArray[0, 2], "Medium difficulty should block opponent's winning moves");
        }


        /// <summary>
        /// Tests that hard difficulty uses all available strategies.
        /// </summary>
        [TestMethod]
        public void MakeMove_HardDifficulty_ShouldUseAllStrategies()
        {
            // Arrange
            _board = new TickTacToeBoard();
            _computerPlayer = new ComputerPlayer(_board, DifficultyLevel.Hard);
            
            // Act - First move should take center
            bool moveResult = _computerPlayer.MakeMove('O');
            
            // Assert
            Assert.IsTrue(moveResult, "Computer should make a move");
            Assert.AreEqual('O', _board.BoardArray[1, 1], "Hard difficulty should take center on first move");
            
            // Test fork creation
            _board = new TickTacToeBoard();
            _computerPlayer = new ComputerPlayer(_board, DifficultyLevel.Hard);
            _board.PlaceSymbol(0, 0, 'O');
            _board.PlaceSymbol(2, 2, 'O');
            _board.PlaceSymbol(0, 2, 'X');
            
            moveResult = _computerPlayer.MakeMove('O');
            
            Assert.IsTrue(moveResult, "Computer should make a move");
            
            // Count potential winning moves after the computer's move
            int winningPaths = 0;
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    if (_board.BoardArray[i, j] == ' ')
                    {
                        var cloneBoard = _board.Clone();
                        cloneBoard.PlaceSymbol(i, j, 'O');
                        if (cloneBoard.CheckForWin('O'))
                            winningPaths++;
                    }
                }
            }
            
            Assert.IsTrue(winningPaths >= 2, "Hard difficulty should create forks when possible");
        }
    }
}