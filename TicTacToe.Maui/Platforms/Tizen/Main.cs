using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace TicTacToe.Maui;

class Program : MauiApplication
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    static void Main(string[] args)
    {
        var app = new Program();
        try
        {
            app.Run(args);
        }
        catch (Exception ex)
        {
            // Log the exception (you can replace this with your logging framework)
            Console.WriteLine($"Exception during app run: {ex}");
        }
    }
}
