namespace TicTacToe
{
    /// <summary>
    /// Represents a player in the Tic Tac Toe game.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Gets or sets the number of wins for this player.
        /// </summary>
        public int NumberOfWins { get; set; }

        /// <summary>
        /// Gets or sets the symbol (X or O) used by this player.
        /// </summary>
        public char Symbol { get; set; }

        /// <summary>
        /// Gets the name of this player.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="symbol">The symbol (X or O) used by this player.</param>
        /// <param name="name">The name of this player.</param>
        /// <exception cref="System.ArgumentException">Thrown when the symbol is not 'X' or 'O'.</exception>
        public Player(char symbol, string name)
        {
            if (symbol != 'X' && symbol != 'O')
                throw new System.ArgumentException("Symbol must be either 'X' or 'O'", nameof(symbol));

            Symbol = symbol;
            Name = name;
            NumberOfWins = 0;
        }
    }
}