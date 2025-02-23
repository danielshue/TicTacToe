using Microsoft.UI.Xaml;

namespace TicTacToe.Maui.WinUI;

/// <summary>
/// Program class that serves as the entry point for Windows platform.
/// </summary>
public class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        WinRT.ComWrappersSupport.InitializeComWrappers();
        Microsoft.UI.Xaml.Application.Start(p => new App());
    }
}
