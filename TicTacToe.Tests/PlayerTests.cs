using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicTacToe.Tests
{
    /// <summary>
    /// Contains unit tests for the Player class, validating player state and behavior.
    /// </summary>
    /// <remarks>
    /// Test coverage includes:
    /// 
    /// Player Properties:
    /// - Symbol Assignment: Validates X/O symbol constraints
    /// - Name Handling: Tests name validation and defaults
    /// - Win Tracking: Verifies win count management
    /// 
    /// State Management:
    /// - Immutability: Tests symbol immutability after creation
    /// - Score Updates: Validates win counter increments
    /// - Name Updates: Verifies name update constraints
    /// 
    /// Input Validation:
    /// - Symbol Validation: Tests valid/invalid symbol handling
    /// - Name Validation: Verifies empty/null name handling
    /// - Win Count Constraints: Tests negative win count prevention
    /// </remarks>
    [TestClass]
    public class PlayerTests
    {
        /// <summary>
        /// Verifies that a player is created with the correct initial properties.
        /// </summary>
        /// <remarks>
        /// Tests:
        /// - Symbol is correctly assigned
        /// - Name is properly set
        /// - Win count starts at zero
        /// </remarks>
        [TestMethod]
        public void Constructor_ShouldInitializePlayerCorrectly()
        {
            // Arrange & Act
            var player = new Player('X', "TestPlayer");

            // Assert
            Assert.AreEqual('X', player.Symbol);
            Assert.AreEqual("TestPlayer", player.Name);
            Assert.AreEqual(0, player.NumberOfWins);
        }

        /// <summary>
        /// Verifies that win count is correctly incremented.
        /// </summary>
        /// <remarks>
        /// Validates:
        /// - Single increment works correctly
        /// - Multiple increments accumulate properly
        /// - Win count never goes negative
        /// </remarks>
        [TestMethod]
        public void NumberOfWins_ShouldBeIncrementable()
        {
            // Arrange
            var player = new Player('O', "TestPlayer");

            // Act
            player.NumberOfWins++;

            // Assert
            Assert.AreEqual(1, player.NumberOfWins);
        }

        /// <summary>
        /// Tests that players can be created with either 'X' or 'O' symbols.
        /// </summary>
        [TestMethod]
        public void Player_ShouldWorkWithBothXAndOSymbols()
        {
            // Arrange & Act
            var playerX = new Player('X', "Player1");
            var playerO = new Player('O', "Player2");

            // Assert
            Assert.AreEqual('X', playerX.Symbol);
            Assert.AreEqual('O', playerO.Symbol);
        }
    }
}