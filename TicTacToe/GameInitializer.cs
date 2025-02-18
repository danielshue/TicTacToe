namespace TicTacToe
{
    /// <summary>
    /// Initializes the Tic Tac Toe game by setting up the players and the initial score.
    /// </summary>
    public class GameInitializer
    {
        /// <summary>
        /// Initializes the game by creating the players and setting up the initial score.
        /// </summary>
        /// <param name="userInterface">The user interface to interact with the player.</param>
        /// <returns>A tuple containing player1, player2, and the initial score.</returns>
        public (Player player1, Player player2, Score score) InitializeGame(ITickTacToeUI userInterface)
        {
            string playerName = userInterface.GetPlayersName();
            var player1 = new Player('X', playerName);
            var player2 = new Player('O', "Computer");
            var score = new Score(player1, player2);
            return (player1, player2, score);
        }
    }
}
