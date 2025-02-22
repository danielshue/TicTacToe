namespace TicTacToe
{
    /// <summary>
    /// Manages the Tic Tac Toe game logic by initializing game state, handling player turns, and determining game outcomes.
    /// </summary>
    /// <remarks>
    /// The Game class is UI-agnostic and delegates all user interaction to implementations of the ITickTacToeUI interface,
    /// which can be tailored for Windows Forms or Console applications. It utilizes the WaitForHumanMove delegate to pause
    /// execution until a human player completes their move, enabling seamless integration with various UI frameworks.
    /// Core responsibilities include initializing players and the board, alternating turns between the human and computer
    /// players, and checking for win/draw conditions to update the game state.
    /// </remarks>
    public class Game
    {
        // ...existing code...
