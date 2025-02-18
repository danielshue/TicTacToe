using System;

namespace TicTacToe
{
    /// <summary>
    /// Interface for _userInterface operations.
    /// </summary>
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
        Score Score { get; set; }

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
        /// Displays a message indicating the specified player has won.
        /// </summary>
        /// <param name="player">The player who won.</param>
        void DisplayPlayerWin(Player player);

        /// <summary>
        /// Displays a message indicating the game is a draw.
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
        /// <returns>The input from the user.</returns>
        string ReadInput();

        /// <summary>
        /// Sets the background color.
        /// </summary>
        /// <param name="color">The color to set.</param>
        void SetBackgroundColor(string color);

        /// <summary>
        /// Sets the foreground color.
        /// </summary>
        /// <param name="color">The color to set.</param>
        void SetForegroundColor(string color);

        /// <summary>
        /// Resets the color to the default.
        /// </summary>
        void ResetColor();

        /// <summary>
        /// Gets the player's name.
        /// </summary>
        /// <returns>The player's name.</returns>
        string GetPlayersName();

        /// <summary>
        /// Handles the logic for a draw game.
        /// </summary>
        void HandleDraw();

        /// <summary>
        /// Displays the BoardArray with the current position highlighted.
        /// </summary>
        /// <param name="currentRow">The row of the current position.</param>
        /// <param name="currentCol">The column of the current position.</param>
        void DisplayBoard(int currentRow, int currentCol);

        /// <summary>
        /// Displays the BoardArray without highlighting any position.
        /// </summary>
        void Display();

    }
}