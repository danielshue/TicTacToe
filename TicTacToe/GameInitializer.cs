namespace TicTacToe
{
    /// <summary>
    /// Initializes the Tic Tac Toe game by setting up the players and the initial score.
    /// </summary>
    public class GameInitializer : IGameInitializer
    {
        /// <inheritdoc />
        public (Player player1, Player player2, Score score) InitializeGame(ITickTacToeUI userInterface)
        {
            var player1 = new Player('X', userInterface.GetPlayersName());
            var player2 = new Player('O', "Computer");
            var score = new Score(player1, player2);
            return (player1, player2, score);
        }
    }
}
