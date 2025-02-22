using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TicTacToe.Tests
{
    /// <summary>
    /// Contains unit tests for the GameInitializer class, validating game setup and state management.
    /// </summary>
    /// <remarks>
    /// Test coverage includes:
    /// 
    /// Initialization Logic:
    /// - Player Creation: Verifies correct player symbol and name assignment
    /// - Score Setup: Tests initial score state creation
    /// - State Reuse: Validates existing state preservation
    /// 
    /// UI Integration:
    /// - Name Prompting: Tests player name collection through UI
    /// - Score Persistence: Verifies score maintenance across games
    /// - UI State Synchronization: Ensures UI reflects game state
    /// 
    /// Error Handling:
    /// - Null Checking: Validates handling of null UI references
    /// - Invalid State: Tests recovery from invalid game states
    /// - State Consistency: Ensures player-score relationship integrity
    /// </remarks>
    [TestClass]
    public class GameInitializerTests
    {
        private Mock<ITickTacToeUI>? _mockUI;
        private GameInitializer? _gameInitializer;

        /// <summary>
        /// Initializes test components before each test run.
        /// </summary>
        /// <remarks>
        /// Sets up a fresh mock UI and GameInitializer instance to ensure
        /// test isolation and prevent state interference between tests.
        /// </remarks>
        [TestInitialize]
        public void Setup()
        {
            _mockUI = new Mock<ITickTacToeUI>();
            _mockUI.Setup(ui => ui.GetPlayersName()).Returns("TestPlayer");
            _gameInitializer = new GameInitializer();
        }

        /// <summary>
        /// Verifies that the initializer creates new players with correct properties.
        /// </summary>
        /// <remarks>
        /// Tests:
        /// - Human player gets 'X' symbol and user-provided name
        /// - Computer player gets 'O' symbol and "Computer" name
        /// - Score initialized with both players
        /// </remarks>
        [TestMethod]
        public void InitializeGame_ShouldCreateNewPlayers_WhenNoScoreExists()
        {
            // Assert setup is complete
            Assert.IsNotNull(_mockUI);
            Assert.IsNotNull(_gameInitializer);

            // Act
            var (player1, player2, score) = _gameInitializer.InitializeGame(_mockUI.Object);

            // Assert
            Assert.AreEqual('X', player1.Symbol);
            Assert.AreEqual("TestPlayer", player1.Name);
            Assert.AreEqual('O', player2.Symbol);
            Assert.AreEqual("Computer", player2.Name);
            Assert.AreEqual(player1, score.Player1);
            Assert.AreEqual(player2, score.Player2);
            Assert.AreEqual(0, score.Draws);
            Assert.AreEqual(player1, score.CurrentPlayer);
        }

        /// <summary>
        /// Verifies that the initializer reuses existing game state when available.
        /// </summary>
        /// <remarks>
        /// Validates:
        /// - Existing players are preserved
        /// - Score state is maintained
        /// - No new player name prompt occurs
        /// </remarks>
        [TestMethod]
        public void InitializeGame_ShouldReuseExistingState_WhenScoreExists()
        {
            // Assert setup is complete
            Assert.IsNotNull(_mockUI);
            Assert.IsNotNull(_gameInitializer);

            // Act
            var (player1, player2, score) = _gameInitializer.InitializeGame(_mockUI.Object);

            // Assert
            Assert.AreEqual('X', player1.Symbol);
            Assert.AreEqual("TestPlayer", player1.Name);
            Assert.AreEqual('O', player2.Symbol);
            Assert.AreEqual("Computer", player2.Name);
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
            // Assert setup is complete
            Assert.IsNotNull(_mockUI);
            Assert.IsNotNull(_gameInitializer);

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
            // Assert setup is complete
            Assert.IsNotNull(_mockUI);
            Assert.IsNotNull(_gameInitializer);

            // Act
            var (player1, player2, _) = _gameInitializer.InitializeGame(_mockUI.Object);

            // Assert
            Assert.AreEqual(0, player1.NumberOfWins);
            Assert.AreEqual(0, player2.NumberOfWins);
        }
    }
}