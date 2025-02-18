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
        private ComputerPlayer _computerPlayer;
        private readonly IGameInitializer _gameInitializer;
        private readonly IScoreManager _scoreManager;
        private DifficultyLevel _difficultyLevel;

        public bool ContinuePlaying {get;set;}

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="userInterface">The user interface for the game.</param>
        /// <param name="difficultyLevel">The initial difficulty level for the game.</param>
        /// <param name="gameInitializer">The game initializer service.</param>
        public Game(ITickTacToeUI userInterface, DifficultyLevel difficultyLevel = DifficultyLevel.Hard, IGameInitializer gameInitializer = null)
        {
            ContinuePlaying = true;
            _userInterface = userInterface ?? throw new ArgumentNullException(nameof(userInterface));
            _difficultyLevel = difficultyLevel;
            _gameInitializer = gameInitializer ?? new GameInitializer();
            
            // Ensure we have a board
            if (_userInterface.Board == null)
            {
                _userInterface.Board = new TickTacToeBoard();
            }

            // Initialize game state
            var (player1, player2, score) = _gameInitializer.InitializeGame(_userInterface);
            _userInterface.Score = score ?? throw new InvalidOperationException("Score cannot be null after initialization");
            _scoreManager = new ScoreManager(score);
            _computerPlayer = new ComputerPlayer(_userInterface.Board, _difficultyLevel);
        }

        /// <summary>
        /// Prompts the user to select a difficulty level and updates the computer player.
        /// </summary>
        private void UpdateDifficultyLevel()
        {
            _difficultyLevel = _userInterface.PromptDifficultyLevel();
            _computerPlayer = new ComputerPlayer(_userInterface.Board, _difficultyLevel);
        }

        /// <summary>
        /// Starts the game loop, allowing the game to be played and restarted.
        /// </summary>
        public void StartGame()
        {
            do
            {
                UpdateDifficultyLevel();
                PlayGame();
                if (!_userInterface.PromptPlayAgain())
                {
                    ContinuePlaying = false;
                }
                else
                {
                    _userInterface.Board.ClearBoard();
                    if (_userInterface.Score != null)
                    {
                        _userInterface.Score.CurrentPlayer = _userInterface.Score.Player1;
                    }
                }
            } while (ContinuePlaying);
        }

        /// <summary>
        /// Manages the main game loop, alternating turns between players.
        /// </summary>
        private void PlayGame()
        {
            if (_userInterface.Score?.CurrentPlayer == null)
                throw new InvalidOperationException("Game state is not properly initialized");

            while (!PerformMove(_userInterface.Score.CurrentPlayer))
            {
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
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            _userInterface.DisplayScore();
            _userInterface.DisplayGameBoard();

            if (player == _userInterface.Score?.Player1)
                _userInterface.PlayerMove(player);
            else
                _computerPlayer.MakeMove(player.Symbol);

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

            return false;
        }
    }
}