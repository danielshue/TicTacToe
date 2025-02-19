using System;
using System.Runtime.InteropServices;

namespace TicTacToe
{
    /// <summary>
    /// Concrete implementation of <see cref="IConsole"/> that wraps the System.Console functionality.
    /// </summary>
    /// <remarks>
    /// This class provides a bridge between the application's console abstraction and the actual
    /// System.Console operations. It implements all methods defined in IConsole by delegating
    /// to the corresponding System.Console methods.
    /// </remarks>
    public class SystemConsole : IConsole
    {
        /// <inheritdoc />
        public void Clear() => Console.Clear();

        /// <inheritdoc />
        public void SetCursorPosition(int left, int top) => Console.SetCursorPosition(left, top);

        /// <inheritdoc />
        public string ReadLine() => Console.ReadLine();

        /// <inheritdoc />
        public ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);

        /// <inheritdoc />
        public void Write(string value) => Console.Write(value);

        /// <inheritdoc />
        public void WriteLine(string value = "") => Console.WriteLine(value);

        /// <inheritdoc />
        public bool CursorVisible
        {
            get => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? Console.CursorVisible : true;
            set
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Console.CursorVisible = value;
                }
            }
        }

        /// <inheritdoc />
        public int WindowWidth => Console.WindowWidth;

        /// <inheritdoc />
        public ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        /// <inheritdoc />
        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        /// <inheritdoc />
        public void ResetColor() => Console.ResetColor();
    }
}