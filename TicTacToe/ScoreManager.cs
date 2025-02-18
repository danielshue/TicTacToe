namespace TicTacToe
{
    /// <summary>
    /// Manages the score of the Tic Tac Toe game, including updating player wins and providing a string representation of the score.
    /// </summary>
    public class ScoreManager : IScoreManager
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

        /// <inheritdoc />
        public void UpdateScore(Player player) {
            if (player == _score.Player1) {
                _score.Player1.NumberOfWins++;
            } else {
                _score.Player2.NumberOfWins++;
            }
        }

        /// <inheritdoc />
        public string GetScoreString() => _score.ToString();
    }
}
