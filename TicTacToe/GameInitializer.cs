namespace TicTacToe
{
    /// <summary>
    /// Manages the initialization of the Tic Tac Toe game state and players.
    /// </summary>
    /// <remarks>
    /// The GameInitializer class handles the creation and setup of core game components:
    /// 
    /// - Player Creation: Sets up human and computer players with appropriate symbols
    /// - Score Management: Initializes or reuses existing score tracking
    /// - State Persistence: Maintains game state between rounds
    /// 
    /// Key Features:
    /// - Stateless Design: Each initialization is independent
    /// - UI Abstraction: Works with any ITickTacToeUI implementation
    /// - Smart Initialization: Avoids duplicate player prompts by reusing existing state
    /// </remarks>
    public class GameInitializer : IGameInitializer
    {
        /// <summary>
        /// Creates or retrieves the game's initial state including players and score.
        /// </summary>
        /// <param name="userInterface">The UI implementation handling player interaction.</param>
        /// <returns>A tuple containing both players and the game score.</returns>
        /// <remarks>
        /// If a score already exists in the UI, it reuses the existing players and score
        /// to prevent unnecessary reprompting. Otherwise, it creates new instances by
        /// gathering the human player's name through the UI.
        /// </remarks>
        public (Player player1, Player player2, Score score) InitializeGame(ITickTacToeUI userInterface)
        {
            if (userInterface.Score != null)
            {
                return (userInterface.Score.Player1, userInterface.Score.Player2, (Score)userInterface.Score);
            }

            char playerSymbol = userInterface.GetPlayersSymbol();
            char computerSymbol = playerSymbol == 'X' ? 'O' : 'X';
            
            var humanPlayer = new Player(playerSymbol, userInterface.GetPlayersName());
            var computerPlayer = new Player(computerSymbol, "Computer");
            
            // If player chose O, computer (X) goes first
            var score = new Score(
                playerSymbol == 'X' ? humanPlayer : computerPlayer,
                playerSymbol == 'X' ? computerPlayer : humanPlayer
            );
            
            return (score.Player1, score.Player2, score);
        }
    }
}
