namespace TicTacToe.Tests
{
    /// <summary>
    /// Contains unit tests for the <see cref="ScoreManager"/> class to verify
    /// score tracking and display functionality.
    /// </summary>
    [TestClass]
    public class ScoreManagerTests
    {
        private ScoreManager _scoreManager;
        private Player _player1;
        private Player _player2;
        private Score _score;

        /// <summary>
        /// Initializes test environment before each test.
        /// Creates two players, a score object, and a score manager instance.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _player1 = new Player('X', "Player1");
            _player2 = new Player('O', "Player2");
            _score = new Score(_player1, _player2);
            _scoreManager = new ScoreManager(_score);
        }

        /// <summary>
        /// Tests that Player 1's win count is correctly incremented
        /// when they win a game.
        /// </summary>
        [TestMethod]
        public void UpdateScore_ShouldIncrementPlayer1Wins_WhenPlayer1Wins()
        {
            // Arrange
            int initialWins = _player1.NumberOfWins;

            // Act
            _scoreManager.UpdateScore(_player1);

            // Assert
            Assert.AreEqual(initialWins + 1, _player1.NumberOfWins);
            Assert.AreEqual(0, _player2.NumberOfWins);
        }

        /// <summary>
        /// Tests that Player 2's win count is correctly incremented
        /// when they win a game.
        /// </summary>
        [TestMethod]
        public void UpdateScore_ShouldIncrementPlayer2Wins_WhenPlayer2Wins()
        {
            // Arrange
            int initialWins = _player2.NumberOfWins;

            // Act
            _scoreManager.UpdateScore(_player2);

            // Assert
            Assert.AreEqual(initialWins + 1, _player2.NumberOfWins);
            Assert.AreEqual(0, _player1.NumberOfWins);
        }

        /// <summary>
        /// Tests that the score string is formatted correctly when
        /// displaying the current game state.
        /// </summary>
        [TestMethod]
        public void GetScoreString_ShouldReturnCorrectFormat()
        {
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