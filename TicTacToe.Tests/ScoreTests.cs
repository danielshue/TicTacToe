using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicTacToe.Tests
{
    /// <summary>
    /// Contains unit tests for the Score class, validating score tracking and player management.
    /// </summary>
    /// <remarks>
    /// Test coverage includes:
    /// 
    /// Score State Management:
    /// - Player property initialization
    /// - Draw counter functionality
    /// - Current player tracking
    /// 
    /// Player Switching:
    /// - Alternating between players
    /// - Edge case handling
    /// 
    /// String Representation:
    /// - ToString implementation
    /// - Custom player name formatting
    /// </remarks>
    [TestClass]
    public class ScoreTests
    {
        private Player _player1;
        private Player _player2;
        private Score _score;

        [TestInitialize]
        public void Setup()
        {
            _player1 = new Player('X', "Player1");
            _player2 = new Player('O', "Player2");
            _score = new Score(_player1, _player2);
        }

        /// <summary>
        /// Tests that the score is properly initialized with the provided players.
        /// </summary>
        [TestMethod]
        public void Score_ShouldInitializeWithCorrectPlayers()
        {
            // Assert
            Assert.AreEqual(_player1, _score.Player1);
            Assert.AreEqual(_player2, _score.Player2);
            Assert.AreEqual(_player1, _score.CurrentPlayer);
            Assert.AreEqual(0, _score.Draws);
        }

        /// <summary>
        /// Tests that the default constructor initializes with zero draws.
        /// </summary>
        [TestMethod]
        public void Score_DefaultConstructor_ShouldInitializeDrawsToZero()
        {
            // Arrange
            var score = new Score();
            
            // Assert
            Assert.AreEqual(0, score.Draws);
        }

        /// <summary>
        /// Tests that the SwitchPlayer method correctly alternates between players.
        /// </summary>
        [TestMethod]
        public void SwitchPlayer_ShouldAlternateBetweenPlayers()
        {
            // Arrange - CurrentPlayer starts as Player1 from Setup

            // Act - First switch
            _score.SwitchPlayer();
            
            // Assert
            Assert.AreEqual(_player2, _score.CurrentPlayer);
            
            // Act - Second switch
            _score.SwitchPlayer();
            
            // Assert
            Assert.AreEqual(_player1, _score.CurrentPlayer);
        }

        /// <summary>
        /// Tests the GetScoreString method with custom player names.
        /// </summary>
        [TestMethod]
        public void GetScoreString_WithCustomNames_ShouldFormatCorrectly()
        {
            // Arrange
            string customPlayer1 = "CustomPlayer1";
            string customPlayer2 = "CustomPlayer2";
            _player1.NumberOfWins = 3;
            _player2.NumberOfWins = 2;
            _score.Draws = 1;
            
            // Act
            string result = _score.GetScoreString(customPlayer1, customPlayer2);
            
            // Assert
            string expected = $"{customPlayer2} wins: 2 | {customPlayer1} wins: 3 | Draws: 1";
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Tests that the ToString method correctly formats the score using player names.
        /// </summary>
        [TestMethod]
        public void ToString_ShouldUsePlayerNames()
        {
            // Arrange
            _player1.NumberOfWins = 1;
            _player2.NumberOfWins = 2;
            _score.Draws = 3;
            
            // Act
            string result = _score.ToString();
            
            // Assert
            string expected = $"{_player2.Name} wins: 2 | {_player1.Name} wins: 1 | Draws: 3";
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Tests that the Draws property can be incremented properly.
        /// </summary>
        [TestMethod]
        public void Draws_CanBeIncremented()
        {
            // Arrange
            int initialDraws = _score.Draws;
            
            // Act
            _score.Draws++;
            
            // Assert
            Assert.AreEqual(initialDraws + 1, _score.Draws);
        }

        /// <summary>
        /// Tests that the SwitchPlayer method works correctly with null CurrentPlayer.
        /// </summary>
        [TestMethod]
        public void SwitchPlayer_WithNullCurrentPlayer_ShouldSetToPlayer1()
        {
            // Arrange
            var score = new Score(_player1, _player2);
            score.CurrentPlayer = null;
            
            // Act
            score.SwitchPlayer();
            
            // Assert
            Assert.AreEqual(_player1, score.CurrentPlayer);
        }
    }
}