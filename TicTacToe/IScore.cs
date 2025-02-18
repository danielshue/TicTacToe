namespace TicTacToe
{
    /// <summary>
    /// Interface for managing game score and player state.
    /// </summary>
    public interface IScore
    {
        /// <summary>
        /// Gets or sets the number of draws.
        /// </summary>
        int Draws { get; set; }

        /// <summary>
        /// Gets the first player.
        /// </summary>
        Player Player1 { get; }

        /// <summary>
        /// Gets the second player.
        /// </summary>
        Player Player2 { get; }

        /// <summary>
        /// Gets or sets the current player.
        /// </summary>
        Player CurrentPlayer { get; set; }

        /// <summary>
        /// Switches the current player.
        /// </summary>
        void SwitchPlayer();

        /// <summary>
        /// Gets a string representation of the score.
        /// </summary>
        /// <returns>A formatted string containing the current score.</returns>
        string ToString();
    }
}