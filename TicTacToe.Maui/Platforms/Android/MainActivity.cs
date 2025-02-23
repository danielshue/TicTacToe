using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace TicTacToe.Maui;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        // Add logging for unhandled exceptions
        AndroidEnvironment.UnhandledExceptionRaiser += (sender, e) =>
        {
            // Log the exception (you can replace this with your logging framework)
            Console.WriteLine($"Unhandled exception: {e.Exception}");
            e.Handled = true;
        };
    }
}
