using System;
using System.Threading;

namespace TicTacToe
{
    /// <summary>
    /// Defines a contract for console operations, allowing for abstraction of console interactions
    /// and easier testing through dependency injection.
    /// </summary>
    /// <remarks>
    /// This interface wraps common console operations to facilitate unit testing of console-based
    /// user interfaces. By abstracting console operations, we can mock console behavior in tests
    /// and verify how the application interacts with the console without actually writing to
    /// or reading from the system console.
    /// </remarks>
    public interface IConsole
    {
        /// <summary>
        /// Clears the console screen.
        /// </summary>
        void Clear();

        /// <summary>
        /// Sets the position of the cursor in the console buffer.
        /// </summary>
        /// <param name="left">The column position from the left edge of the buffer.</param>
        /// <param name="top">The row position from the top edge of the buffer.</param>
        void SetCursorPosition(int left, int top);

        /// <summary>
        /// Reads the next line of characters from the console input stream.
        /// </summary>
        /// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
        string ReadLine();

        /// <summary>
        /// Obtains the next character or function key pressed by the user.
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window.</param>
        /// <returns>Information about the pressed key.</returns>
        ConsoleKeyInfo ReadKey(bool intercept);

        /// <summary>
        /// Obtains the next character or function key pressed by the user.
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window.</param>
        /// <param name="cancellationToken">Optional token to cancel the operation.</param>
        /// <returns>Information about the pressed key.</returns>
        ConsoleKeyInfo ReadKey(bool intercept, CancellationToken cancellationToken = default);

        /// <summary>
        /// Writes the specified string value to the console output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        void Write(string value);

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the console output stream.
        /// </summary>
        /// <param name="value">The value to write. If not provided, only writes the line terminator.</param>
        void WriteLine(string value = "");

        /// <summary>
        /// Gets or sets a value indicating whether the cursor is visible in the console window.
        /// </summary>
        bool CursorVisible { get; set; }

        /// <summary>
        /// Gets the width of the console window measured in character columns.
        /// </summary>
        int WindowWidth { get; }

        /// <summary>
        /// Gets or sets the background color of the console.
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the console.
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Resets the console colors to their defaults.
        /// </summary>
        void ResetColor();
    }
}