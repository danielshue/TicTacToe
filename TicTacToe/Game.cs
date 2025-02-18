using System;

namespace TicTacToe
{
    /// <summary>
    /// Manages the Tic Tac Toe game, including game initialization, player turns, and game end conditions.
    /// </summary>
    /// <remarks>
    /// The Game class appears to have a proper level of abstraction for managing the overall flow and logic 
    /// of the Tic-Tac-Toe game. It focuses on game initialization, player turns, and game end conditions, 
    /// while delegating display and user input responsibilities to the ITickTacToeUI interface. This separation 
    /// of concerns aligns with the single responsibility principle.
    /// 
    /// Here are the key responsibilities of the Game class:
    /// 
    /// 1. Game Initialization:
    ///     - Initializes players and the game board.
    ///     - Sets up the game loop and handles game restarts.
    /// 2. Game Loop Management:
    ///     - Manages the main game loop, alternating turns between players.
    ///     - Calls methods to handle player moves and checks for game end conditions.
    /// 3. Player Moves:
    ///     - Handles player moves and checks for win or draw conditions.
    ///     - Switches the current player after each move.
    /// 4. Computer Moves:
    ///     - Simulates computer moves by cloning the board and trying to make a winning move or a random move.
    /// 
    /// The class does not include any display or user interface logic, which is appropriate.
    /// Display logic and user input handling are delegated to the ITickTacToeUI interface, maintaining a clear 
    /// separation of concerns.
    /// 
    /// Overall, the Game class has a proper level of abstraction for managing the overall flow and logic 
    /// of the Tic-Tac-Toe game. It encapsulates the game initialization, player turns, and game end conditions, 
    /// and does not include any unrelated responsibilities.
    /// </remarks>
    /// <seealso cref="ComputerPlayer"/>
    /// <seealso cref="GameInitializer"/>
    /// <seealso cref="ScoreManager"/>
    /// <seealso cref="ITickTacToeUI"/>
    /// <seealso cref="TicTacToeConsoleUI"/>
    /// <seealso cref="ITicTacToeBoard"/>
    /// <seealso cref="TickTacToeBoard"/>
    public class Game
    {
        private readonly ITickTacToeUI _userInterface;
        private readonly ComputerPlayer _computerPlayer;
        private readonly GameInitializer _gameInitializer;
        private readonly ScoreManager _scoreManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="userInterface">The user interface for the game.</param>
        public Game(ITickTacToeUI userInterface)
        {
            _userInterface = userInterface;
            _computerPlayer = new ComputerPlayer(userInterface.Board);
            _gameInitializer = new GameInitializer();
            var (player1, player2, score) = _gameInitializer.InitializeGame(userInterface);
            _userInterface.Score = score;
            _scoreManager = new ScoreManager(score);
        }

        /// <summary>
        /// Starts the game loop, allowing the game to be played and restarted.
        /// </summary>
        public void StartGame()
        {
            while (true)
            {
                PlayGame();

                bool playAgain = _userInterface.PromptPlayAgain();

                if (playAgain)
                {
                    _userInterface.Board.ClearBoard();
                    _userInterface.Score.CurrentPlayer = _userInterface.Score.Player1;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Manages the main game loop, alternating turns between players.
        /// </summary>
        private void PlayGame()
        {
            while (true)
            {
                if (PerformMove(_userInterface.Score.CurrentPlayer))
                {
                    return;
                }

                _userInterface.Score.SwitchPlayer();
            }
        }

        /// <summary>
        /// Handles a player's move and checks for win or draw conditions.
        /// </summary>
        /// <param name="player">The player making the move.</param>
        /// <returns>True if the game has ended, false otherwise.</returns>
        private bool PerformMove(Player player)
        {
            _userInterface.DisplayScore();
            _userInterface.DisplayGameBoard();

            if (player == _userInterface.Score.Player1)
            {
                _userInterface.PlayerMove(player);
            }
            else
            {
                _computerPlayer.MakeMove(player.Symbol);
            }

            if (_userInterface.Board.CheckForWin(player.Symbol))
            {
                _userInterface.DisplayScore();
                _userInterface.DisplayGameBoard();
                _userInterface.DisplayPlayerWin(player);
                _scoreManager.UpdateScore(player);
                return true;
            }
            if (_userInterface.Board.IsBoardFull())
            {
                _userInterface.HandleDraw();
                return true;
            }

            if (player == _userInterface.Score.Player1 && _userInterface.Board.CountEmptyCells() == 1)
            {
                _userInterface.HandleDraw();
                return true;
            }

            return false;
        }
    }
}