namespace TicTacToe.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        // Add logging for unhandled exceptions
        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            // Log the exception (you can replace this with your logging framework)
            Console.WriteLine($"Unhandled exception: {e.ExceptionObject}");
        };
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}