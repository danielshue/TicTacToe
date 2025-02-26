using System;

namespace TicTacToe
{
    /// <summary>
    /// Manages the Tic Tac Toe game lifecycle and core game mechanics.
    /// </summary>
    /// <remarks>
    /// The Game class provides a UI-agnostic implementation that can work with both console and Windows Forms interfaces
    /// through the ITickTacToeUI abstraction. Key features include:
    /// 
    /// - Platform Independence: Uses a delegate-based approach (WaitForHumanMove) to coordinate with different UI implementations
    /// - Game State Management: Handles player turns, board state, and game progression
    /// - Difficulty Levels: Supports multiple AI difficulty settings that can be changed between games
    /// - Score Tracking: Maintains game statistics through the IScoreManager interface
    /// 
    /// The class follows SOLID principles:
    /// - Single Responsibility: Focuses solely on game flow and rules
    /// - Open/Closed: Extends behavior through interface implementations
    /// - Interface Segregation: Uses specific interfaces for UI, scoring, and game initialization
    /// - Dependency Inversion: Depends on abstractions rather than concrete implementations
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

        /// <summary>
        /// Gets or sets whether the game should continue running.
        /// </summary>
        public bool ContinuePlaying { get; set; }

        /// <summary>
        /// Delegate to coordinate with the UI implementation for waiting on human player moves.
        /// This enables UI-agnostic implementation of turn management.
        /// </summary>
        public Action WaitForHumanMove { get; set; } = () => { };

        /// <summary>
        /// Initializes a new instance of the Game class with the specified UI implementation and settings.
        /// </summary>
        /// <param name="userInterface">The UI implementation (Windows Forms or Console) handling user interaction.</param>
        /// <param name="difficultyLevel">Initial AI difficulty setting. Defaults to Hard if not specified.</param>
        /// <param name="gameInitializer">Optional custom game initializer. A default one is created if not provided.</param>
        /// <exception cref="ArgumentNullException">Thrown when userInterface is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when score initialization fails.</exception>
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

            // Ask for player choice
            AskForPlayerChoice();
        }

        /// <summary>
        /// Updates the game's difficulty level based on user selection and reinitializes the computer player.
        /// </summary>
        /// <remarks>
        /// This method delegates the difficulty selection to the UI implementation and updates the
        /// computer player's behavior accordingly. It's called once at the start of each game session.
        /// </remarks>
        private void UpdateDifficultyLevel()
        {
            _difficultyLevel = _userInterface.PromptDifficultyLevel();
            _computerPlayer = new ComputerPlayer(_userInterface.Board, _difficultyLevel);
        }

        /// <summary>
        /// Initiates the main game loop and handles game progression.
        /// </summary>
        /// <remarks>
        /// This method:
        /// - Prompts for initial difficulty setting
        /// - Manages the game loop until player quits
        /// - Handles game restarts and board resets
        /// - Maintains player turn order across games
        /// </remarks>
        public void StartGame()
        {
            // Call UpdateDifficultyLevel once at the start
            UpdateDifficultyLevel();

            do
            {
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
        /// Controls the progression of a single game until completion.
        /// </summary>
        /// <remarks>
        /// Manages turn alternation between human and computer players until a win or draw occurs.
        /// Uses the WaitForHumanMove delegate to synchronize with the UI during human player turns.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when game state is not properly initialized.</exception>
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
        /// Executes a single player's turn and evaluates the game state afterwards.
        /// </summary>
        /// <param name="player">The player whose turn is being processed.</param>
        /// <returns>True if the game has ended (win or draw), false if the game should continue.</returns>
        /// <remarks>
        /// For human players, this method:
        /// - Updates the display
        /// - Waits for move completion via WaitForHumanMove delegate
        /// - Validates the move result
        /// 
        /// For computer players:
        /// - Executes the AI move based on current difficulty
        /// - Updates the game state
        /// 
        /// In both cases, checks for win/draw conditions and updates the score accordingly.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when player is null.</exception>
        private bool PerformMove(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            _userInterface.DisplayScore();
            _userInterface.DisplayGameBoard();

            if (player == _userInterface.Score?.Player1)
            {
                _userInterface.PlayerMove(player);
                // Wait for the human player's move to complete via delegate
                WaitForHumanMove();
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

            return false;
        }

        /// <summary>
        /// Prompts the user for their choice of X or O and sets the players accordingly.
        /// </summary>
        private void AskForPlayerChoice()
        {
            char choice;
            do
            {
                choice = _userInterface.GetPlayersSymbol();
            } while (choice != 'X' && choice != 'O');

            if (choice == 'X')
            {
                _userInterface.Score.Player1.Symbol = 'X';
                _userInterface.Score.Player2.Symbol = 'O';
            }
            else
            {
                _userInterface.Score.Player1.Symbol = 'O';
                _userInterface.Score.Player2.Symbol = 'X';
            }
        }
    }
}