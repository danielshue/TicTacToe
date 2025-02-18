namespace TicTacToe
{
    /// <summary>
    /// Represents a player in the Tic Tac Toe game.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Gets the symbol representing the player (e.g., 'X' or 'O').
        /// </summary>
        public char Symbol { get; }

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the number of wins for the player.
        /// </summary>
        public int NumberOfWins { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class with the specified symbol and name.
        /// </summary>
        /// <param name="symbol">The symbol representing the player.</param>
        /// <param name="name">The name of the player.</param>
        public Player(char symbol, string name)
        {
            Symbol = symbol;
            Name = name;
            NumberOfWins = 0;
        }
    }
}