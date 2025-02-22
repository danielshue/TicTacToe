using System;

namespace TicTacToe
{
    /// <summary>
    /// Defines console-specific user interface operations for the Tic Tac Toe game.
    /// </summary>
    /// <remarks>
    /// This interface extends ITickTacToeUI to provide console-specific display and input capabilities.
    /// It encapsulates all console-related operations that aren't relevant to other UI platforms,
    /// following the Interface Segregation Principle.
    /// 
    /// Key Features:
    /// - Console Window Management: Controls window dimensions and cursor positioning
    /// - Visual Styling: Manages console colors for enhanced visual feedback
    /// - Platform Specific: Contains methods that only make sense in a console context
    /// 
    /// This separation ensures that:
    /// - Windows Forms implementations aren't forced to implement console-specific methods
    /// - Console UI can fully utilize platform-specific features
    /// - Future UI implementations can choose the appropriate interface level
    /// </remarks>
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
