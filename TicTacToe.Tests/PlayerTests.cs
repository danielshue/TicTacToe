using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicTacToe.Tests
{
    /// <summary>
    /// Contains unit tests for the <see cref="Player"/> class to verify
    /// player creation and state management.
    /// </summary>
    [TestClass]
    public class PlayerTests
    {
        /// <summary>
        /// Tests that a player is correctly initialized with the specified
        /// symbol, name, and zero wins.
        /// </summary>
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
        /// Tests that the number of wins can be incremented for a player.
        /// </summary>
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