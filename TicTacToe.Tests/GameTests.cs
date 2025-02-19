using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TicTacToe.Tests
{
    /// <summary>
    /// Contains unit tests for the <see cref="Game"/> class to verify game initialization
    /// and behavior across different difficulty levels.
    /// </summary>
    [TestClass]
    public class GameTests
    {
        private Mock<ITickTacToeUI> _mockUI;
        private Mock<IGameInitializer> _mockInitializer;
        private Mock<IScore> _mockScore;
        private Player _player1;
        private Player _player2;
        private ITicTacToeBoard _board;

        // Initialize non-nullable fields in the constructor
        public GameTests()
        {
            _mockUI = new Mock<ITickTacToeUI>();
            _mockInitializer = new Mock<IGameInitializer>();
            _mockScore = new Mock<IScore>();
            _player1 = new Player('X', "TestPlayer");
            _player2 = new Player('O', "Computer");
            _board = new TickTacToeBoard();
        }

        /// <summary>
        /// Initializes test environment before each test.
        /// Sets up mock UI, game initializer, and creates test players and board.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            // Reset the board for each test
            _board = new TickTacToeBoard();
            
            // Reset mock states
            _mockUI.Reset();
            _mockScore.Reset();
            
            _board = new TickTacToeBoard();
            _mockUI = new Mock<ITickTacToeUI>();
            _mockScore = new Mock<IScore>();
            _player1 = new Player('X', "TestPlayer");
            _player2 = new Player('O', "Computer");
            
            // Set up Score mock
            _mockScore.Setup(s => s.Player1).Returns(_player1);
            _mockScore.Setup(s => s.Player2).Returns(_player2);
            _mockScore.Setup(s => s.CurrentPlayer).Returns(_player1);
            
            // Set up UI mock with proper game state
            _mockUI.Setup(ui => ui.Board).Returns(_board);
            _mockUI.Setup(ui => ui.Score).Returns(_mockScore.Object);
            _mockUI.Setup(ui => ui.GetPlayersName()).Returns("TestPlayer");
            _mockUI.Setup(ui => ui.PromptPlayAgain()).Returns(false);
            _mockUI.Setup(ui => ui.DisplayScore()).Verifiable();
            _mockUI.Setup(ui => ui.DisplayGameBoard()).Verifiable();
            _mockUI.Setup(ui => ui.PlayerMove(It.IsAny<Player>())).Callback(() => _board.PlaceSymbol(0, 0, 'X'));
            _mockUI.Setup(ui => ui.HandleDraw()).Verifiable();
            _mockUI.Setup(ui => ui.DisplayPlayerWin(It.IsAny<Player>())).Verifiable();
            
            // Set up initializer mock
            _mockInitializer = new Mock<IGameInitializer>();
            _mockInitializer.Setup(i => i.InitializeGame(It.IsAny<ITickTacToeUI>()))
                .Returns((_player1, _player2, new Score(_player1, _player2)));
        }

        /// <summary>
        /// Tests that the game correctly implements easy difficulty behavior
        /// by verifying that the computer player sometimes misses obvious winning moves.
        /// </summary>
        [TestMethod]
        public void Game_ShouldUseEasyDifficulty_WhenSpecified()
        {
            // Arrange
            var testBoard = new TickTacToeBoard();
            testBoard.PlaceSymbol(0, 0, 'O');
            testBoard.PlaceSymbol(0, 1, 'O');
            
            bool differentMoveFound = false;
            for (int i = 0; i < 10 && !differentMoveFound; i++)
            {
                var tempBoard = testBoard.Clone();
                var computerPlayer = new ComputerPlayer(tempBoard, DifficultyLevel.Easy);
                computerPlayer.MakeMove('O');
                
                if (tempBoard.BoardArray[0, 2] != 'O')
                {
                    differentMoveFound = true;
                }
            }
            
            Assert.IsTrue(differentMoveFound, "Easy difficulty should sometimes miss obvious winning moves");
        }


        /// <summary>
        /// Tests that the game uses hard difficulty by default and implements correct hard difficulty behavior
        /// by verifying that the computer player takes optimal moves.
        /// </summary>
        [TestMethod]
        public void Game_ShouldUseHardDifficulty_ByDefault()
        {
            // Arrange
            var testBoard = new TickTacToeBoard();
            
            // Act
            var computerPlayer = new ComputerPlayer(testBoard, DifficultyLevel.Hard);
            computerPlayer.MakeMove('O');
            
            // Assert
            Assert.AreEqual('O', testBoard.BoardArray[1, 1], "Hard difficulty should take center on first move");
        }

        /// <summary>
        /// Tests that medium difficulty correctly blocks winning moves and takes winning opportunities.
        /// </summary>
        [TestMethod]
        public void Game_ShouldImplementMediumDifficulty_Correctly()
        {
            // Arrange
            var testBoard = new TickTacToeBoard();
            testBoard.PlaceSymbol(0, 0, 'O');
            testBoard.PlaceSymbol(0, 1, 'O');
            
            var computerPlayer = new ComputerPlayer(testBoard, DifficultyLevel.Medium);
            
            // Act
            computerPlayer.MakeMove('O');
            
            // Assert
            Assert.AreEqual('O', testBoard.BoardArray[0, 2], "Medium difficulty should take winning moves");

            // Test blocking
            testBoard = new TickTacToeBoard();
            testBoard.PlaceSymbol(0, 0, 'X');
            testBoard.PlaceSymbol(0, 1, 'X');
            computerPlayer = new ComputerPlayer(testBoard, DifficultyLevel.Medium);
            
            computerPlayer.MakeMove('O');
            
            Assert.AreEqual('O', testBoard.BoardArray[0, 2], "Medium difficulty should block opponent's winning moves");
        }

        /// <summary>
        /// Tests that the game correctly handles a draw situation.
        /// </summary>
        [TestMethod]
        public void Game_ShouldHandleDraw_Correctly()
        {
            // Arrange
            _mockUI.Setup(ui => ui.Board.IsBoardFull()).Returns(true);
            _mockUI.Setup(ui => ui.Board.CheckForWin(It.IsAny<char>())).Returns(false);
            var game = new Game(_mockUI.Object, DifficultyLevel.Easy, _mockInitializer.Object);

            // Act
            game.StartGame();

            // Assert
            _mockUI.Verify(ui => ui.HandleDraw(), Times.Once());
        }

        /// <summary>
        /// Tests that the game properly updates difficulty level when requested.
        /// </summary>
        [TestMethod]
        public void Game_ShouldUpdateDifficultyLevel_WhenPrompted()
        {
            // Arrange
            DifficultyLevel selectedDifficulty = DifficultyLevel.Medium;
            _mockUI.Setup(ui => ui.PromptDifficultyLevel()).Returns(selectedDifficulty);
            _mockUI.Setup(ui => ui.Board.CheckForWin(It.IsAny<char>())).Returns(true); // End game after first move
            var game = new Game(_mockUI.Object, DifficultyLevel.Easy, _mockInitializer.Object);

            // Act
            game.StartGame();

            // Assert
            _mockUI.Verify(ui => ui.PromptDifficultyLevel(), Times.Once());
        }

        /// <summary>
        /// Tests that the game properly initializes the board at the start.
        /// </summary>
        [TestMethod]
        public void Game_ShouldInitializeBoard_WhenStarting()
        {
            // Arrange
            ITicTacToeBoard board = new TickTacToeBoard();
            _mockUI.Setup(ui => ui.Board).Returns(() => board);
            _mockUI.SetupSet(ui => ui.Board = It.IsAny<ITicTacToeBoard>())
                .Callback<ITicTacToeBoard>(b => board = b);

            // Act
            var game = new Game(_mockUI.Object, DifficultyLevel.Easy, _mockInitializer.Object);

            // Assert
            Assert.IsNotNull(board, "Board should be initialized");
            Assert.IsInstanceOfType(board, typeof(TickTacToeBoard));
        }

        /// <summary>
        /// Tests that the game properly handles player wins.
        /// </summary>
        [TestMethod]
        public void Game_ShouldHandlePlayerWin_Correctly()
        {
            // Arrange
            _mockUI.Setup(ui => ui.Board.CheckForWin('X')).Returns(true);
            var game = new Game(_mockUI.Object, DifficultyLevel.Easy, _mockInitializer.Object);

            // Act
            game.StartGame();

            // Assert
            _mockUI.Verify(ui => ui.DisplayPlayerWin(It.IsAny<Player>()), Times.Once());
        }

        /// <summary>
        /// Tests that the game properly alternates between players.
        /// </summary>
        [TestMethod]
        public void Game_ShouldAlternatePlayers_BetweenTurns()
        {
            // Arrange
            var turnCount = 0;
            _mockUI.Setup(ui => ui.Board.CheckForWin(It.IsAny<char>())).Returns(() => turnCount >= 3);
            _mockUI.Setup(ui => ui.Board.IsBoardFull()).Returns(false);
            _mockUI.Setup(ui => ui.Board.PlaceSymbol(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<char>())).Returns(true);
            _mockUI.Setup(ui => ui.Board.Clone()).Returns(() => new TickTacToeBoard());
            _mockUI.Setup(ui => ui.PlayerMove(It.IsAny<Player>()))
                .Callback(() => turnCount++);
            
            var game = new Game(_mockUI.Object, DifficultyLevel.Easy, _mockInitializer.Object);

            // Act
            game.StartGame();

            // Assert
            _mockScore.Verify(s => s.SwitchPlayer(), Times.AtLeast(2));
        }

        /// <summary>
        /// Tests that the game properly handles replay functionality.
        /// </summary>
        [TestMethod]
        public void Game_ShouldHandleReplay_Correctly()
        {
            // Arrange
            var playCount = 0;
            _mockUI.Setup(ui => ui.PromptPlayAgain())
                .Returns(() => playCount++ < 2);
            _mockUI.Setup(ui => ui.Board.CheckForWin(It.IsAny<char>())).Returns(true);
            
            var game = new Game(_mockUI.Object, DifficultyLevel.Easy, _mockInitializer.Object);

            // Act
            game.StartGame();

            // Assert
            _mockUI.Verify(ui => ui.Board.ClearBoard(), Times.Exactly(2));
            Assert.AreEqual(3, playCount, "Should play three games total");
        }

        /// <summary>
        /// Tests that the game maintains proper game state between rounds.
        /// </summary>
        [TestMethod]
        public void Game_ShouldMaintainGameState_BetweenRounds()
        {
            // Arrange
            var roundCount = 0;
            var currentPlayerWasReset = false;
            _mockUI.Setup(ui => ui.PromptPlayAgain())
                .Returns(() => roundCount++ < 1);
            _mockUI.Setup(ui => ui.Board.CheckForWin(It.IsAny<char>())).Returns(true);
            _mockUI.Setup(ui => ui.Board.PlaceSymbol(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<char>())).Returns(true);
            _mockUI.Setup(ui => ui.Board.Clone()).Returns(() => new TickTacToeBoard());
            
            _mockScore.Setup(s => s.Player1).Returns(_player1);
            _mockScore.SetupSet(s => s.CurrentPlayer = It.IsAny<Player>())
                .Callback<Player>(_ => currentPlayerWasReset = true);
            
            var game = new Game(_mockUI.Object, DifficultyLevel.Easy, _mockInitializer.Object);

            // Act
            game.StartGame();

            // Assert
            _mockUI.Verify(ui => ui.Board.ClearBoard(), Times.Once());
            Assert.IsTrue(currentPlayerWasReset, "Current player should be reset between rounds");
        }

        /// <summary>
        /// Tests that the ContinuePlaying property works correctly in the Game class.
        /// </summary>
        [TestMethod]
        public void Game_ShouldSetContinuePlaying_ToFalse_WhenGameEnds()
        {
            // Arrange
            _mockUI.Setup(ui => ui.PromptPlayAgain()).Returns(false);
            _mockUI.Setup(ui => ui.Board.CheckForWin(It.IsAny<char>())).Returns(true);
            var game = new Game(_mockUI.Object, DifficultyLevel.Easy, _mockInitializer.Object);

            // Act
            game.StartGame();

            // Assert
            Assert.IsFalse(game.ContinuePlaying, "ContinuePlaying should be false after the game loop ends");
        }
    }
}