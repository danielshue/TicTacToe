using System;

namespace TicTacToe
{
    /// <summary>
    /// Interface for console-specific UI operations for Tic Tac Toe.
    /// </summary>
    public interface ITickTacToeConsoleUI : ITickTacToeUI
    {
        /// <summary>
        /// Gets the width of the console window.
        /// </summary>
        int WindowWidth { get; }

        /// <summary>
        /// Sets the cursor position in the console window.
        /// </summary>
        /// <param name="left">The left position.</param>
        /// <param name="top">The top position.</param>
        void SetCursorPosition(int left, int top);

        /// <summary>
        /// Sets the background color of the console.
        /// </summary>
        /// <param name="color">The color name.</param>
        void SetBackgroundColor(string color);

        /// <summary>
        /// Sets the foreground color of the console.
        /// </summary>
        /// <param name="color">The color name.</param>
        void SetForegroundColor(string color);

        /// <summary>
        /// Resets the console colors to their defaults.
        /// </summary>
        void ResetColor();
    }
}
