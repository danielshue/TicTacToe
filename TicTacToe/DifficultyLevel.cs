namespace TicTacToe
{
    /// <summary>
    /// Represents the difficulty level of the computer player.
    /// </summary>
    public enum DifficultyLevel
    {
        /// <summary>
        /// Easy difficulty - makes random moves.
        /// </summary>
        Easy,

        /// <summary>
        /// Medium difficulty - blocks opponent wins and takes winning moves.
        /// </summary>
        Medium,

        /// <summary>
        /// Hard difficulty - uses advanced strategies like forks and optimal moves.
        /// </summary>
        Hard
    }
}
