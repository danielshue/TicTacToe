using Microsoft.UI.Xaml;

namespace TicTacToe.Maui.WinUI;

public partial class App : MauiWinUIApplication
{
    /// <summary>
    /// Initializes the singleton application object.
    /// </summary>
    public App()
    {
        this.InitializeComponent();
        // Add logging for unhandled exceptions
        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            // Log the exception (you can replace this with your logging framework)
            Console.WriteLine($"Unhandled exception: {e.ExceptionObject}");
        };
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

