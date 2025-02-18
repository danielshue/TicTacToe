namespace TicTacToe
{
    /// <summary>
    /// Represents the score and players of the Tic Tac Toe game.
    /// </summary>
    public class Score
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Score"/> class.
        /// </summary>
        public Score()
        {
            Draws = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Score"/> class with the specified players.
        /// </summary>
        /// <param name="player1">Player 1.</param>
        /// <param name="player2">Player 2.</param>
        public Score(Player player1, Player player2) : this()
        {
            Player1 = player1;
            Player2 = player2;
            CurrentPlayer = player1;
        }

        /// <summary>
        /// Gets or sets the number of draws.
        /// </summary>
        public int Draws { get; set; }

        /// <summary>
        /// Gets or sets player 1.
        /// </summary>
        public Player Player1 { get; internal set; }

        /// <summary>
        /// Gets or sets player 2.
        /// </summary>
        public Player Player2 { get; internal set; }

        /// <summary>
        /// Gets or sets the current player.
        /// </summary>
        public Player CurrentPlayer { get; set; }

        /// <summary>
        /// Switches the current player to the other player.
        /// </summary>
        public void SwitchPlayer()
        {
            CurrentPlayer = (CurrentPlayer == Player1) ? Player2 : Player1;
        }

        /// <summary>
        /// Returns a string representation of the score.
        /// </summary>
        /// <param name="player1Name">The name of player 1.</param>
        /// <param name="player2Name">The name of player 2.</param>
        /// <returns>A string representing the score.</returns>
        public string GetScoreString(string player1Name, string player2Name)
        {
            return $"{player2Name} wins: {Player2.NumberOfWins} | {player1Name} wins: {Player1.NumberOfWins} | Draws: {Draws}";
        }

        /// <summary>
        /// Returns a string representation of the score.
        /// </summary>
        /// <returns>A string representing the score.</returns>
        public override string ToString()
        {
            return GetScoreString(Player1.Name, Player2.Name);
        }
    }
}