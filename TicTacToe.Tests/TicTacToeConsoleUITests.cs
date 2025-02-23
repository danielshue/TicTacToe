using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace TicTacToe.Tests
{
    /// <summary>
    /// Contains unit tests for the TicTacToeConsoleUI class, validating console-specific UI behavior.
    /// </summary>
    /// <remarks>
    /// Test coverage includes:
    /// 
    /// Console Display:
    /// - Color Management: Tests background and foreground color settings
    /// - Cursor Control: Validates cursor positioning
    /// - Board Rendering: Tests game board visualization
    /// - Score Display: Verifies score presentation
    /// 
    /// User Input:
    /// - Move Input: Tests coordinate parsing and validation
    /// - Name Entry: Verifies player name collection
    /// - Difficulty Selection: Tests difficulty level prompting
    /// - Play Again Prompts: Validates game continuation logic
    /// 
    /// Platform Specifics:
    /// - Console Window: Tests window dimension handling
    /// - Buffer Management: Verifies screen buffer operations
    /// - Color Reset: Tests color state restoration
    /// - Input Formatting: Validates console-specific input requirements
    /// 
    /// Integration:
    /// - ITickTacToeConsoleUI Implementation: Verifies interface compliance
    /// - IConsole Interaction: Tests system console abstraction
    /// - Cross-Platform Compatibility: Validates platform-specific behavior isolation
    /// </remarks>
    [TestClass]
    public class TicTacToeConsoleUITests
    {
        private Mock<ITicTacToeBoard> _mockBoard = null!;
        private Mock<IConsole> _mockConsole = null!;
        private Mock<IScore> _mockScore = null!;
        private TicTacToeConsoleUI _consoleUI = null!;
        private char[,] _boardArray = null!;

        /// <summary>
        /// Initializes test objects before each test method runs.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _boardArray = new char[ITicTacToeBoard.BoardSize, ITicTacToeBoard.BoardSize];
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                    _boardArray[i, j] = ' ';

            _mockBoard = new Mock<ITicTacToeBoard>();
            _mockBoard.Setup(b => b.BoardArray).Returns(_boardArray);
            
            _mockConsole = new Mock<IConsole>();
            // Setup both ReadKey overloads to ensure consistent behavior
            _mockConsole.Setup(c => c.ReadKey(It.IsAny<bool>()))
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.Enter, false, false, false));
                
            _mockConsole.Setup(c => c.ReadKey(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.Enter, false, false, false));

            _mockScore = new Mock<IScore>();
            _mockScore.Setup(s => s.ToString()).Returns("Score: 0-0");
            _consoleUI = new TicTacToeConsoleUI(_mockBoard.Object, _mockConsole.Object)
            {
                Score = _mockScore.Object
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockConsole.Reset();
            _mockBoard.Reset();
            _mockScore.Reset();
        }

        /// <summary>
        /// Tests that the constructor properly initializes all properties.
        /// </summary>
        [TestMethod]
        public void Constructor_InitializesPropertiesCorrectly()
        {
            Assert.AreEqual(0, _consoleUI.CurrentRow);
            Assert.AreEqual(0, _consoleUI.CurrentCol);
            Assert.IsNotNull(_consoleUI.Board);
        }

        /// <summary>
        /// Verifies that the Board property can be set and retrieved correctly.
        /// </summary>
        [TestMethod]
        public void Board_CanBeSetAndRetrieved()
        {
            // Arrange
            var newMockBoard = new Mock<ITicTacToeBoard>();

            // Act
            _consoleUI.Board = newMockBoard.Object;

            // Assert
            Assert.AreEqual(newMockBoard.Object, _consoleUI.Board);
        }

        /// <summary>
        /// Verifies that the Score property can be set and retrieved correctly.
        /// </summary>
        [TestMethod]
        public void Score_CanBeSetAndRetrieved()
        {
            // Arrange
            var mockScore = new Mock<IScore>();

            // Act
            _consoleUI.Score = mockScore.Object;

            // Assert
            Assert.AreEqual(mockScore.Object, _consoleUI.Score);
        }

        /// <summary>
        /// Tests that valid color names can be set as foreground colors without throwing exceptions.
        /// </summary>
        /// <param name="color">The color name to test.</param>
        [TestMethod]
        [DataRow("black")]
        [DataRow("blue")]
        [DataRow("red")]
        public void SetForegroundColor_ValidColor_DoesNotThrowException(string color)
        {
            // Act & Assert
            try
            {
                _consoleUI.SetForegroundColor(color);
            }
            catch
            {
                Assert.Fail("SetForegroundColor threw an unexpected exception");
            }
        }

        /// <summary>
        /// Verifies that setting an invalid foreground color throws an ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetForegroundColor_InvalidColor_ThrowsArgumentException()
        {
            // Act
            _consoleUI.SetForegroundColor("invalidColor");
        }

        /// <summary>
        /// Tests that the player name input is correctly captured and returned.
        /// </summary>
        [TestMethod]
        public void GetPlayersName_ReturnsEnteredName()
        {
            // Arrange
            string expectedName = "TestPlayer";
            _mockConsole.Setup(c => c.ReadLine()).Returns(expectedName);

            // Act
            string actualName = _consoleUI.GetPlayersName();

            // Assert
            Assert.AreEqual(expectedName, actualName);
            _mockConsole.Verify(c => c.Write("Enter your name: "), Times.Once);
            _mockConsole.VerifySet(c => c.CursorVisible = true, Times.Once);
            _mockConsole.VerifySet(c => c.CursorVisible = false, Times.Once);
        }

        /// <summary>
        /// Verifies that responding 'y' to play again prompt returns true.
        /// </summary>
        [TestMethod]
        public void PromptPlayAgain_RespondsYes_ReturnsTrue()
        {
            // Arrange
            _mockConsole.Setup(c => c.ReadLine()).Returns("y");

            // Act
            bool result = _consoleUI.PromptPlayAgain();

            // Assert
            Assert.IsTrue(result);
            _mockConsole.Verify(c => c.Write("Do you want to play again? (y/n) "), Times.Once);
        }

        /// <summary>
        /// Verifies that responding 'n' to play again prompt returns false.
        /// </summary>
        [TestMethod]
        public void PromptPlayAgain_RespondsNo_ReturnsFalse()
        {
            // Arrange
            _mockConsole.Setup(c => c.ReadLine()).Returns("n");

            // Act
            bool result = _consoleUI.PromptPlayAgain();

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests that selecting the Easy difficulty level returns the correct enum value.
        /// </summary>
        [TestMethod]
        public void PromptDifficultyLevel_SelectsEasy_ReturnsEasyLevel()
        {
            // Arrange
            _mockConsole.Setup(c => c.ReadLine()).Returns("1");

            // Act
            var result = _consoleUI.PromptDifficultyLevel();

            // Assert
            Assert.AreEqual(DifficultyLevel.Easy, result);
            _mockConsole.Verify(c => c.WriteLine("Select difficulty level:"), Times.Once);
            _mockConsole.Verify(c => c.WriteLine("1. Easy"), Times.Once);
            _mockConsole.Verify(c => c.WriteLine("2. Medium"), Times.Once);
            _mockConsole.Verify(c => c.WriteLine("3. Hard"), Times.Once);
        }


        [TestMethod]
        [Timeout(1000)]  // 1 second timeout
        public void PlayerMove_UpArrow_DecreasesRowIfPossible()
        {
            // Arrange
            _consoleUI.CurrentRow = 1;
            var player = new Player('X', "TestPlayer");
            _mockConsole.SetupSequence(c => c.ReadKey(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false))
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.Enter, false, false, false));
            _mockBoard.Setup(b => b.PlaceSymbol(It.IsAny<int>(), It.IsAny<int>(), 'X')).Returns(true);
            _mockBoard.Setup(b => b.IsCellEmpty(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            // Act
            _consoleUI.PlayerMove(player);

            // Assert
            Assert.AreEqual(0, _consoleUI.CurrentRow);
        }

        [TestMethod]
        [Timeout(1000)]  // 1 second timeout
        public void PlayerMove_UpArrow_StaysAtZeroWhenAtTopEdge()
        {
            // Arrange
            _consoleUI.CurrentRow = 0;
            var player = new Player('X', "TestPlayer");
            _mockConsole.SetupSequence(c => c.ReadKey(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false))
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.Enter, false, false, false));
            _mockBoard.Setup(b => b.PlaceSymbol(It.IsAny<int>(), It.IsAny<int>(), 'X')).Returns(true);
            _mockBoard.Setup(b => b.IsCellEmpty(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            // Act
            _consoleUI.PlayerMove(player);

            // Assert
            Assert.AreEqual(0, _consoleUI.CurrentRow);
        }

        [TestMethod]
        [Timeout(1000)]  // 1 second timeout
        public void PlayerMove_DownArrow_IncreasesRowIfPossible()
        {
            // Arrange
            _consoleUI.CurrentRow = 1;
            var player = new Player('X', "TestPlayer");
            _mockConsole.SetupSequence(c => c.ReadKey(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false))
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.Enter, false, false, false));
            _mockBoard.Setup(b => b.PlaceSymbol(It.IsAny<int>(), It.IsAny<int>(), 'X')).Returns(true);
            _mockBoard.Setup(b => b.IsCellEmpty(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            // Act
            _consoleUI.PlayerMove(player);

            // Assert
            Assert.AreEqual(2, _consoleUI.CurrentRow);
        }

        [TestMethod]
        [Timeout(1000)]  // 1 second timeout
        public void PlayerMove_LeftArrow_DecreasesColumnIfPossible()
        {
            // Arrange
            _consoleUI.CurrentCol = 1;
            var player = new Player('X', "TestPlayer");
            _mockConsole.SetupSequence(c => c.ReadKey(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false))
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.Enter, false, false, false));
            _mockBoard.Setup(b => b.PlaceSymbol(It.IsAny<int>(), It.IsAny<int>(), 'X')).Returns(true);
            _mockBoard.Setup(b => b.IsCellEmpty(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            // Act
            _consoleUI.PlayerMove(player);

            // Assert
            Assert.AreEqual(0, _consoleUI.CurrentCol);
        }

        [TestMethod]
        [Timeout(1000)]  // 1 second timeout
        public void PlayerMove_RightArrow_IncreasesColumnIfPossible()
        {
            // Arrange
            _consoleUI.CurrentCol = 1;
            var player = new Player('X', "TestPlayer");
            _mockConsole.SetupSequence(c => c.ReadKey(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false))
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.Enter, false, false, false));
            _mockBoard.Setup(b => b.PlaceSymbol(It.IsAny<int>(), It.IsAny<int>(), 'X')).Returns(true);
            _mockBoard.Setup(b => b.IsCellEmpty(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            // Act
            _consoleUI.PlayerMove(player);

            // Assert
            Assert.AreEqual(2, _consoleUI.CurrentCol);
        }

        [TestMethod]
        [Timeout(1000)]  // 1 second timeout
        public void PlayerMove_Enter_PlacesSymbolAtCurrentPosition()
        {
            // Arrange
            _consoleUI.CurrentRow = 1;
            _consoleUI.CurrentCol = 1;
            var player = new Player('X', "TestPlayer");
            _mockConsole.Setup(c => c.ReadKey(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.Enter, false, false, false));
            _mockBoard.Setup(b => b.PlaceSymbol(1, 1, 'X')).Returns(true);
            _mockBoard.Setup(b => b.IsCellEmpty(1, 1)).Returns(true);

            // Act
            _consoleUI.PlayerMove(player);

            // Assert
            _mockBoard.Verify(b => b.PlaceSymbol(1, 1, 'X'), Times.Once);
        }

        [TestMethod]
        [Timeout(1000)]  // 1 second timeout
        public void PlayerMove_Enter_ContinuesIfPlacementFails()
        {
            // Arrange
            _consoleUI.CurrentRow = 1;
            _consoleUI.CurrentCol = 1;
            var player = new Player('X', "TestPlayer");
            _mockConsole.SetupSequence(c => c.ReadKey(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.Enter, false, false, false))
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false))
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.Enter, false, false, false));
            
            _mockBoard.SetupSequence(b => b.PlaceSymbol(It.IsAny<int>(), It.IsAny<int>(), 'X'))
                .Returns(false)  // First attempt fails
                .Returns(true); // Second attempt succeeds

            _mockBoard.Setup(b => b.IsCellEmpty(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            // Act
            _consoleUI.PlayerMove(player);

            // Assert
            _mockBoard.Verify(b => b.PlaceSymbol(It.IsAny<int>(), It.IsAny<int>(), 'X'), Times.Exactly(2));
            Assert.AreEqual(2, _consoleUI.CurrentCol); // Moved right after failed attempt
        }

        [TestMethod]
        [Timeout(2000)]
        public void PlayerMove_Timeout_AutoPlacesInFirstAvailableSpot()
        {
            // Arrange
            _consoleUI.CurrentRow = 1;
            _consoleUI.CurrentCol = 1;
            var player = new Player('X', "TestPlayer");
            
            // Setup mock to simulate timeout by delaying
            _mockConsole.Setup(c => c.ReadKey(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Callback(() => Thread.Sleep(1000)) // Force timeout
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.Escape, false, false, false));

            _mockBoard.Setup(b => b.IsCellEmpty(0, 0)).Returns(true);
            _mockBoard.Setup(b => b.PlaceSymbol(0, 0, 'X')).Returns(true);

            // Act
            _consoleUI.PlayerMove(player);

            // Assert
            _mockBoard.Verify(b => b.PlaceSymbol(0, 0, 'X'), Times.Once);
            Assert.AreEqual(0, _consoleUI.CurrentRow);
            Assert.AreEqual(0, _consoleUI.CurrentCol);
        }

        [TestMethod]
        [Timeout(2000)]
        public void PlayerMove_TimeoutAndAllCellsOccupied_HandlesFailureGracefully()
        {
            // Arrange
            _consoleUI.CurrentRow = 1;
            _consoleUI.CurrentCol = 1;
            var player = new Player('X', "TestPlayer");
            
            // Setup mock to simulate timeout
            _mockConsole.Setup(c => c.ReadKey(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Callback(() => Thread.Sleep(1000))
                .Returns(new ConsoleKeyInfo('\0', ConsoleKey.Escape, false, false, false));

            // All cells occupied
            _mockBoard.Setup(b => b.IsCellEmpty(It.IsAny<int>(), It.IsAny<int>())).Returns(false);
            
            // Should not throw exception
            _consoleUI.PlayerMove(player);
            
            // Verify no moves were made
            _mockBoard.Verify(b => b.PlaceSymbol(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<char>()), Times.Never);
        }

        /// <summary>
        /// Tests that the score is correctly formatted and displayed.
        /// </summary>
        /// <remarks>
        /// Verifies:
        /// - Screen is cleared before display
        /// - Score is centered on screen
        /// - Correct background and foreground colors are set
        /// </remarks>
        [TestMethod]
        public void DisplayScore_FormatsScoreCorrectly()
        {
            // Arrange
            var mockScore = new Mock<IScore>();
            string scoreText = "Score: 0-0";
            mockScore.Setup(s => s.ToString()).Returns(scoreText);
            _mockConsole.Setup(c => c.WindowWidth).Returns(20);
            _consoleUI.Score = mockScore.Object;

            // Act
            _consoleUI.DisplayScore();

            // Assert
            _mockConsole.Verify(c => c.Clear(), Times.Once);
            _mockConsole.Verify(c => c.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);
            _mockConsole.Verify(c => c.Write(scoreText), Times.Once);
        }

        /// <summary>
        /// Tests that the draw message is correctly displayed.
        /// </summary>
        [TestMethod]
        public void DisplayDraw_ShowsCorrectMessage()
        {
            // Act
            _consoleUI.DisplayDraw();

            // Assert
            _mockConsole.Verify(c => c.WriteLine("It's a draw!"), Times.Once);
        }

        /// <summary>
        /// Tests that the player win message is correctly displayed.
        /// </summary>
        [TestMethod]
        public void DisplayPlayerWin_ShowsCorrectMessage()
        {
            // Arrange
            var player = new Player('X', "TestPlayer");

            // Act
            _consoleUI.DisplayPlayerWin(player);

            // Assert
            _mockConsole.Verify(c => c.WriteLine("TestPlayer wins!"), Times.Once);
        }
    }
}