using System.Windows.Forms;
using System.Threading.Tasks;

namespace TicTacToe.WinForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            var form = new Form1();
            
            // Initialize game components before showing form
            var playerName = form.GetPlayersName(); // Prompt once
            var player1 = new Player('X', playerName);
            var player2 = new Player('O', "Computer");
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