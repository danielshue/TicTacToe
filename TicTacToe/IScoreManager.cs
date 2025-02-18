namespace TicTacToe
{
    /// <summary>
    /// Interface for managing the score of the Tic Tac Toe game.
    /// </summary>
    public interface IScoreManager
    {
        /// <summary>
        /// Updates the score for the specified player by incrementing their number of wins.
        /// </summary>
        /// <param name="player">The player whose score should be updated.</param>
        void UpdateScore(Player player);

        /// <summary>
        /// Returns a string representation of the current score.
        /// </summary>
        /// <returns>A string representing the current score.</returns>
        string GetScoreString();
    }
}