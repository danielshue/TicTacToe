using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TicTacToe.Tests
{
    /// <summary>
    /// Contains unit tests for the <see cref="GameInitializer"/> class to verify
    /// proper initialization of game components including players and score.
    /// </summary>
    [TestClass]
    public class GameInitializerTests
    {
        private GameInitializer _gameInitializer;
        private Mock<ITickTacToeUI> _mockUI;

        /// <summary>
        /// Initializes test environment before each test.
        /// Sets up the game initializer and mocks the UI with a test player name.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _gameInitializer = new GameInitializer();
            _mockUI = new Mock<ITickTacToeUI>();
            _mockUI.Setup(ui => ui.GetPlayersName()).Returns("TestPlayer");
        }

        /// <summary>
        /// Tests that Player 1 is created with the correct symbol 'X' and
        /// the name provided through the UI.
        /// </summary>
        [TestMethod]
        public void InitializeGame_ShouldCreatePlayer1WithCorrectSymbolAndName()
        {
            // Act
            var (player1, _, _) = _gameInitializer.InitializeGame(_mockUI.Object);

            // Assert
            Assert.AreEqual('X', player1.Symbol);
            Assert.AreEqual("TestPlayer", player1.Name);
        }

        /// <summary>
        /// Tests that Player 2 is created as a computer player with
        /// the correct symbol 'O' and name "Computer".
        /// </summary>
        [TestMethod]
        public void InitializeGame_ShouldCreatePlayer2AsComputer()
        {
            // Act
            var (_, player2, _) = _gameInitializer.InitializeGame(_mockUI.Object);

            // Assert
            Assert.AreEqual('O', player2.Symbol);
            Assert.AreEqual("Computer", player2.Name);
        }

        /// <summary>
        /// Tests that the score is initialized with both players correctly
        /// and starts with zero draws and Player 1 as current player.
        /// </summary>
        [TestMethod]
        public void InitializeGame_ShouldCreateScoreWithBothPlayers()
        {
            // Act
            var (player1, player2, score) = _gameInitializer.InitializeGame(_mockUI.Object);

            // Assert
            Assert.AreEqual(player1, score.Player1);
            Assert.AreEqual(player2, score.Player2);
            Assert.AreEqual(0, score.Draws);
            Assert.AreEqual(player1, score.CurrentPlayer);
        }

        /// <summary>
        /// Tests that the UI's GetPlayersName method is called exactly once
        /// during game initialization.
        /// </summary>
        [TestMethod]
        public void InitializeGame_ShouldCallGetPlayersNameOnce()
        {
            // Act
            _gameInitializer.InitializeGame(_mockUI.Object);

            // Assert
            _mockUI.Verify(ui => ui.GetPlayersName(), Times.Once);
        }

        /// <summary>
        /// Tests that both players start with zero wins when initialized.
        /// </summary>
        [TestMethod]
        public void InitializeGame_ShouldCreatePlayersWithZeroWins()
        {
            // Act
            var (player1, player2, _) = _gameInitializer.InitializeGame(_mockUI.Object);

            // Assert
            Assert.AreEqual(0, player1.NumberOfWins);
            Assert.AreEqual(0, player2.NumberOfWins);
        }
    }
}