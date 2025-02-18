namespace TicTacToe
{
    /// <summary>
    /// Represents the score and players of the Tic Tac Toe game.
    /// </summary>
    public class Score : IScore
    {
        /// <inheritdoc />
        public virtual int Draws { get; set; }

        /// <inheritdoc />
        public virtual Player Player1 { get; internal set; }

        /// <inheritdoc />
        public virtual Player Player2 { get; internal set; }

        /// <inheritdoc />
        public virtual Player CurrentPlayer { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Score"/> class with default values.
        /// </summary>
        public Score()
        {
            Draws = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Score"/> class with the specified players.
        /// </summary>
        /// <param name="player1">The first player.</param>
        /// <param name="player2">The second player.</param>
        public Score(Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;
            CurrentPlayer = player1;
            Draws = 0;
        }

        /// <summary>
        /// Switches the current player.
        /// </summary>
        public virtual void SwitchPlayer() => CurrentPlayer = (CurrentPlayer == Player1) ? Player2 : Player1;

        /// <summary>
        /// Gets a string representation of the score with custom player names.
        /// </summary>
        /// <param name="player1Name">The name of player 1.</param>
        /// <param name="player2Name">The name of player 2.</param>
        /// <returns>A formatted string with the current score.</returns>
        public virtual string GetScoreString(string player1Name, string player2Name) => 
            $"{player2Name} wins: {Player2.NumberOfWins} | {player1Name} wins: {Player1.NumberOfWins} | Draws: {Draws}";

        /// <summary>
        /// Returns a string representation of the score.
        /// </summary>
        /// <returns>A formatted string with the current score.</returns>
        public override string ToString() => GetScoreString(Player1.Name, Player2.Name);
    }
}