using Android.App;
using Android.Runtime;

namespace TicTacToe.Maui;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override void OnCreate()
    {
        base.OnCreate();
        // Add logging for unhandled exceptions
        AndroidEnvironment.UnhandledExceptionRaiser += (sender, e) =>
        {
            // Log the exception (you can replace this with your logging framework)
            Console.WriteLine($"Unhandled exception: {e.Exception}");
            e.Handled = true;
        };
    }
}
