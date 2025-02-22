namespace TicTacToe
{
    /// <summary>
    /// Defines the core user interface contract for the Tic Tac Toe game across different platforms.
    /// </summary>
    /// <remarks>
    /// This interface serves as the primary abstraction layer between the game logic and UI implementations.
    /// It enables the Game class to remain platform-agnostic while supporting both console and graphical interfaces.
    /// 
    /// Key responsibilities are organized into categories:
    /// 1. State Management:
    ///    - Track current board position (CurrentRow, CurrentCol)
    ///    - Maintain game score
    ///    - Handle game board state
    /// 
    /// 2. Display Operations:
    ///    - Show game board and current state
    ///    - Display scores and game results
    ///    - Present game messages
    /// 
    /// 3. Player Interaction:
    ///    - Handle player moves
    ///    - Collect player information
    ///    - Process game continuation decisions
    /// 
    /// 4. Game Configuration:
    ///    - Set difficulty levels
    ///    - Initialize game parameters
    /// 
    /// The interface is designed to be:
    /// - Platform Independent: No assumptions about the UI technology
    /// - Complete: Contains all necessary methods for game operation
    /// - Cohesive: Methods relate to UI operations only
    /// - Minimal: No unnecessary methods that might not apply to all platforms
    /// </remarks>
    public interface ITickTacToeUI
    {
        /// <summary>
        /// Gets or sets the current row position.
        /// </summary>
        int CurrentRow { get; set; }

        /// <summary>
        /// Gets or sets the current column position.
        /// </summary>
        int CurrentCol { get; set; }

        /// <summary>
        /// Gets or sets the score of the game.
        /// </summary>
        IScore Score { get; set; }

        /// <summary>
        /// Gets or sets the game board.
        /// </summary>
        ITicTacToeBoard Board { get; set; }

        /// <summary>
        /// Clears the output.
        /// </summary>
        void Clear();

        /// <summary>
        /// Displays the current score.
        /// </summary>
        void DisplayScore();

        /// <summary>
        /// Handles the player's move.
        /// </summary>
        /// <param name="currentPlayer">The current player.</param>
        void PlayerMove(Player currentPlayer);

        /// <summary>
        /// Displays the game board with the current position highlighted.
        /// </summary>
        /// <param name="currentRow">The row of the current position.</param>
        /// <param name="currentCol">The column of the current position.</param>
        void DisplayGameBoard(int currentRow, int currentCol);

        /// <summary>
        /// Displays the game board without highlighting any position.
        /// </summary>
        void DisplayGameBoard();

        /// <summary>
        /// Displays a win message for the specified player.
        /// </summary>
        /// <param name="player">The winning player.</param>
        void DisplayPlayerWin(Player player);

        /// <summary>
        /// Displays a draw message.
        /// </summary>
        void DisplayDraw();

        /// <summary>
        /// Prompts the user to play again.
        /// </summary>
        /// <returns>True if the user wants to play again, false otherwise.</returns>
        bool PromptPlayAgain();

        /// <summary>
        /// Reads input from the user.
        /// </summary>
        /// <returns>The user's input as a string.</returns>
        string ReadInput();

        /// <summary>
        /// Gets the player's name from user input.
        /// </summary>
        /// <returns>The player's name.</returns>
        string GetPlayersName();

        /// <summary>
        /// Handles a draw game situation.
        /// </summary>
        void HandleDraw();

        /// <summary>
        /// Displays the board with optional position highlighting.
        /// </summary>
        /// <param name="currentRow">The row to highlight.</param>
        /// <param name="currentCol">The column to highlight.</param>
        void DisplayBoard(int currentRow, int currentCol);

        /// <summary>
        /// Displays the board without highlighting.
        /// </summary>
        void Display();

        /// <summary>
        /// Prompts the user to select a difficulty level for the game.
        /// </summary>
        /// <returns>The selected difficulty level.</returns>
        DifficultyLevel PromptDifficultyLevel();
    }
}