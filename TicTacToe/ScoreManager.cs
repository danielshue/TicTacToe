namespace TicTacToe
{
    /// <summary>
    /// Manages the score of the Tic Tac Toe game, including updating player wins and providing a string representation of the score.
    /// </summary>
    public class ScoreManager
    {
        private readonly Score _score;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreManager"/> class with the specified score.
        /// </summary>
        /// <param name="score">The score object to manage.</param>
        public ScoreManager(Score score)
        {
            _score = score;
        }

        /// <summary>
        /// Updates the score for the specified player by incrementing their number of wins.
        /// </summary>
        /// <param name="player">The player whose score should be updated.</param>
        public void UpdateScore(Player player)
        {
            if (player == _score.Player1)
            {
                _score.Player1.NumberOfWins++;
            }
            else
            {
                _score.Player2.NumberOfWins++;
            }
        }

        /// <summary>
        /// Returns a string representation of the current score.
        /// </summary>
        /// <returns>A string representing the current score.</returns>
        public string GetScoreString()
        {
            return _score.ToString();
        }
    }

}
