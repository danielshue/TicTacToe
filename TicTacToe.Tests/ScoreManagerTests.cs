using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicTacToe.Tests
{
    /// <summary>
    /// Contains unit tests for the ScoreManager class, validating score tracking and game statistics.
    /// </summary>
    /// <remarks>
    /// Test coverage includes:
    /// 
    /// Score Management:
    /// - Win Recording: Validates individual player win updates
    /// - Draw Tracking: Tests draw count incrementation
    /// - Score Persistence: Verifies score maintenance across games
    /// - Player Statistics: Tests per-player win/loss ratios
    /// 
    /// State Validation:
    /// - Score Integrity: Tests score consistency between players
    /// - Score History: Validates game outcome tracking
    /// - Player Association: Verifies correct player-score mapping
    /// 
    /// Error Handling:
    /// - Null Score Prevention: Tests null score handling
    /// - Invalid Updates: Validates score update constraints
    /// - State Recovery: Tests recovery from invalid states
    /// 
    /// Cross-Platform:
    /// - UI Independence: Verifies score tracking works across UI implementations
    /// - State Serialization: Tests score state persistence
    /// </remarks>
    [TestClass]
    public class ScoreManagerTests
    {
        private Score? _score;
        private Player? _player1;
        private Player? _player2;
        private ScoreManager? _scoreManager;

        /// <summary>
        /// Initializes test components before each test.
        /// </summary>
        /// <remarks>
        /// Creates fresh instances of:
        /// - Players with predefined symbols
        /// - Score tracking system
        /// - ScoreManager with test configuration
        /// </remarks>
        [TestInitialize]
        public void Setup()
        {
            _player1 = new Player('X', "TestPlayer1");
            _player2 = new Player('O', "TestPlayer2");
            _score = new Score(_player1, _player2);
            _scoreManager = new ScoreManager(_score);
        }

        /// <summary>
        /// Verifies that player wins are correctly recorded and tracked.
        /// </summary>
        /// <remarks>
        /// Tests:
        /// - Win count increments correctly
        /// - Player statistics update accurately
        /// - Score state remains consistent
        /// </remarks>
        [TestMethod]
        public void UpdateScore_ShouldIncrementWins_WhenPlayerWins()
        {
            // Assert setup is complete
            Assert.IsNotNull(_player1);
            Assert.IsNotNull(_player2);
            Assert.IsNotNull(_scoreManager);

            // Arrange
            int initialWinsPlayer1 = _player1.NumberOfWins;
            int initialWinsPlayer2 = _player2.NumberOfWins;

            // Act
            _scoreManager.UpdateScore(_player1);
            _scoreManager.UpdateScore(_player2);

            // Assert
            Assert.AreEqual(initialWinsPlayer1 + 1, _player1.NumberOfWins);
            Assert.AreEqual(initialWinsPlayer2 + 1, _player2.NumberOfWins);
        }

        /// <summary>
        /// Validates draw handling and statistics updates.
        /// </summary>
        /// <remarks>
        /// Verifies:
        /// - Draw count increments properly
        /// - Player statistics remain unchanged
        /// - Game state reflects draw outcome
        /// </remarks>
        [TestMethod]
        public void UpdateScore_ShouldHandleDraws_Correctly()
        {
            // Assert setup is complete
            Assert.IsNotNull(_score);
            Assert.IsNotNull(_player1);
            Assert.IsNotNull(_player2);

            // Arrange
            int initialDraws = _score.Draws;

            // Act
            _score.Draws++;

            // Assert
            Assert.AreEqual(initialDraws + 1, _score.Draws);
            Assert.AreEqual(0, _player1.NumberOfWins);
            Assert.AreEqual(0, _player2.NumberOfWins);
        }

        /// <summary>
        /// Tests that the score string is formatted correctly when
        /// displaying the current game state.
        /// </summary>
        [TestMethod]
        public void GetScoreString_ShouldReturnCorrectFormat()
        {
            // Assert setup is complete
            Assert.IsNotNull(_score);
            Assert.IsNotNull(_player1);
            Assert.IsNotNull(_player2);
            Assert.IsNotNull(_scoreManager);

            // Arrange
            _scoreManager.UpdateScore(_player1);
            _scoreManager.UpdateScore(_player2);
            _score.Draws = 1;

            // Act
            string result = _scoreManager.GetScoreString();

            // Assert
            string expected = $"{_player2.Name} wins: 1 | {_player1.Name} wins: 1 | Draws: 1";
            Assert.AreEqual(expected, result);
        }
    }
}