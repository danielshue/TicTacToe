using ObjCRuntime;
using UIKit;

namespace TicTacToe.Maui;

public class Program
{
    // This is the main entry point of the application.
    static void Main(string[] args)
    {
        // Add logging for unhandled exceptions
        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            // Log the exception (you can replace this with your logging framework)
            Console.WriteLine($"Unhandled exception: {e.ExceptionObject}");
        };

        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}
