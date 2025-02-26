using System.Windows.Forms;
using System.Threading.Tasks;

namespace TicTacToe.WinForms
{
    /// <summary>
    /// Entry point for the Windows Forms version of Tic Tac Toe.
    /// </summary>
    /// <remarks>
    /// This class orchestrates the initialization sequence for the Windows Forms UI:
    /// 
    /// Startup Flow:
    /// 1. Initialize UI components and show main form
    /// 2. Gather initial player information through dialogs
    /// 3. Set up game state with player names and score
    /// 4. Configure game difficulty
    /// 5. Start game loop on background thread
    /// 
    /// Threading Model:
    /// - UI runs on main thread
    /// - Game logic runs on background thread
    /// - Coordination through WaitForHumanMove delegate
    /// 
    /// Design Features:
    /// - Clean separation between UI and game logic
    /// - Non-blocking game initialization
    /// - Proper thread synchronization
    /// </remarks>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the Windows Forms Tic Tac Toe application.
        /// </summary>
        /// <remarks>
        /// Initializes the Windows Forms application, creates the main form,
        /// sets up the game state, and starts the game loop on a background thread
        /// to maintain UI responsiveness.
        /// </remarks>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            var form = new Form1();
            
            // Initialize game components before showing form
            var playerName = form.GetPlayersName(); // Prompt once
            var playerSymbol = form.GetPlayersSymbol();
            var computerSymbol = playerSymbol == 'X' ? 'O' : 'X';
            var player1 = new Player(playerSymbol, playerName);
            var player2 = new Player(computerSymbol, "Computer");
            form.Score = new Score(player1, player2);
            
            // Show form
            form.Show();
            
            // Get difficulty and start game
            var difficulty = form.PromptDifficultyLevel();
            var game = new Game(form, difficulty);
            // Assign the delegate so that the game waits for the human move
            game.WaitForHumanMove = form.WaitForMoveCompletion;
            Task.Run(game.StartGame);
            
            // Run application
            Application.Run(form);
        }
    }
}