namespace TicTacToe
{
    /// <summary>
    /// Interface for initializing the Tic Tac Toe game.
    /// </summary>
    public interface IGameInitializer
    {
        /// <summary>
        /// Initializes the game by creating the players and setting up the initial score.
        /// </summary>
        /// <param name="userInterface">The user interface to interact with the player.</param>
        /// <returns>A tuple containing player1, player2, and the initial score.</returns>
        (Player player1, Player player2, Score score) InitializeGame(ITickTacToeUI userInterface);
    }
}