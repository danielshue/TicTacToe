using System.Windows.Input;

namespace TicTacToe.Maui;

public partial class MainPage : ContentPage, ITickTacToeUI
{
    private readonly Button[,] _buttons;
    private readonly SemaphoreSlim _moveSemaphore = new(0, 1);
    private int _currentRow;
    private int _currentCol;
    private ITicTacToeBoard _board;
    private IScore? _score;
    private Game? _game;
    private CancellationTokenSource? _gameCts;

    public int CurrentRow 
    { 
        get => _currentRow;
        set => _currentRow = value;
    }

    public int CurrentCol 
    { 
        get => _currentCol;
        set => _currentCol = value;
    }

    public IScore Score
    {
        get => _score ?? throw new InvalidOperationException("Score not initialized");
        set
        {
            _score = value;
            DisplayScore();
        }
    }

    public ITicTacToeBoard Board
    {
        get => _board ?? throw new InvalidOperationException("Board not initialized");
        set => _board = value;
    }

    public MainPage()
    {
        InitializeComponent();
        _buttons = new Button[ITicTacToeBoard.BoardSize, ITicTacToeBoard.BoardSize];
        _board = new TickTacToeBoard();
        InitializeGameBoard();
        MainThread.BeginInvokeOnMainThread(InitializeGameAsync);
    }

    public char PromptPlayerChoice()
    {
        return GetPlayersSymbol();
    }

    private async void InitializeGameAsync()
    {
        try
        {
            Console.WriteLine("[DEBUG] Initializing game async...");
            _gameCts?.Cancel();
            _gameCts = new CancellationTokenSource();

            var playerName = await GetPlayersNameAsync();
            var playerSymbol = GetPlayersSymbol();
            var computerSymbol = playerSymbol == 'X' ? 'O' : 'X';
            var player1 = new Player(playerSymbol, playerName);
            var player2 = new Player(computerSymbol, "Computer");
            Score = new Score(player1, player2);

            var difficulty = await PromptDifficultyLevelAsync();
            _game = new Game(this, difficulty);

            Console.WriteLine("[DEBUG] Starting game on background thread...");
            // Start game on background thread
            await Task.Run(_game.StartGame, _gameCts.Token);
        }
        catch (OperationCanceledException)
        {
            // Game was cancelled, ignore
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to start game: {ex.Message}");
            await DisplayAlert("Error", $"Failed to start game: {ex.Message}", "OK");
        }
    }

    private void InitializeGameBoard()
    {
        for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
        {
            for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
            {
                var button = new Button
                {
                    FontSize = 48,
                    FontAttributes = FontAttributes.Bold,
                    BackgroundColor = Colors.LightGray
                };
                
                var row = i;
                var col = j;
                button.Clicked += (s, e) =>
                {
                    if (Score?.CurrentPlayer == null) return;
                    
                    CurrentRow = row;
                    CurrentCol = col;
                    
                    if (Board.PlaceSymbol(CurrentRow, CurrentCol, Score.CurrentPlayer.Symbol))
                    {
                        DisplayGameBoard(); // Update the entire board display
                        _moveSemaphore.Release();
                    }
                };

                _buttons[i, j] = button;
                GameBoard.Add(button, j, i);
            }
        }
    }

    public void Clear()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            foreach (var button in _buttons)
            {
                button.Text = "";
                button.IsEnabled = true;
                button.BackgroundColor = Colors.LightGray;
            }
        });
    }

    public void Display() => DisplayBoard(-1, -1);

    public void DisplayBoard(int currentRow, int currentCol)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    var button = _buttons[i, j];
                    var symbol = Board.BoardArray[i, j];
                    button.Text = symbol.ToString();
                    button.TextColor = symbol == 'X' ? Colors.Red : Colors.Blue;
                    
                    bool isEmpty = Board.IsCellEmpty(i, j);
                    button.IsEnabled = isEmpty;
                    button.BackgroundColor = (i == currentRow && j == currentCol) 
                        ? Colors.Yellow 
                        : isEmpty ? Colors.LightGray : Colors.WhiteSmoke;
                }
            }
        });
    }

    public void DisplayGameBoard() => Display();

    public void DisplayGameBoard(int currentRow, int currentCol) => DisplayBoard(currentRow, currentCol);

    public async void DisplayPlayerWin(Player player)
    {
        await MainThread.InvokeOnMainThreadAsync(() => 
            DisplayAlert("Game Over", $"{player.Name} wins!", "OK"));
    }

    public void DisplayScore()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ScoreLabel.Text = Score?.ToString() ?? "Tic Tac Toe";
        });
    }

    public async void HandleDraw()
    {
        Clear();
        DisplayScore();
        DisplayGameBoard();
        await MainThread.InvokeOnMainThreadAsync(() => 
            DisplayAlert("Game Over", "It's a draw!", "OK"));
        if (Score != null) Score.Draws++;
    }

    public void DisplayDraw()
    {
        HandleDraw();
    }

    public void PlayerMove(Player currentPlayer)
    {
        DisplayGameBoard();  // Display current board state at start of move
        
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Enable buttons only for empty cells and update display
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    var button = _buttons[i, j];
                    bool isEmpty = Board.IsCellEmpty(i, j);
                    button.IsEnabled = isEmpty;
                    button.BackgroundColor = isEmpty ? Colors.LightGray : Colors.WhiteSmoke;
                    
                    // Update button text and color for existing moves
                    var symbol = Board.BoardArray[i, j];
                    if (symbol != ' ')
                    {
                        button.Text = symbol.ToString();
                        button.TextColor = symbol == 'X' ? Colors.Red : Colors.Blue;
                    }
                    else
                    {
                        button.Text = "";
                    }
                }
            }

            DisplayScore();
            StatusLabel.Text = $"{currentPlayer.Name}'s turn ({currentPlayer.Symbol})";
        });

        try
        {
            _moveSemaphore.Wait(_gameCts?.Token ?? CancellationToken.None);
        }
        catch (OperationCanceledException)
        {
            // Game was cancelled, ignore
        }
    }

    public string ReadInput()
    {
        return "";  // Not used in MAUI implementation
    }

    public bool PromptPlayAgain()
    {
        var tcs = new TaskCompletionSource<bool>();
        
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                var result = await DisplayAlert("Play Again?", "Would you like to play another game?", "Yes", "No");
                tcs.SetResult(result);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

        return tcs.Task.GetAwaiter().GetResult();
    }

    public DifficultyLevel PromptDifficultyLevel()
    {
        var tcs = new TaskCompletionSource<DifficultyLevel>();
        
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                var result = await DisplayActionSheet(
                    "Select Difficulty",
                    "Cancel",
                    null,
                    "Easy",
                    "Medium",
                    "Hard"
                );

                var difficulty = result switch
                {
                    "Easy" => DifficultyLevel.Easy,
                    "Medium" => DifficultyLevel.Medium,
                    _ => DifficultyLevel.Hard
                };
                tcs.SetResult(difficulty);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

        return tcs.Task.GetAwaiter().GetResult();
    }

    public string GetPlayersName()
    {
        var tcs = new TaskCompletionSource<string>();
        
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                var name = await DisplayPromptAsync(
                    "Welcome to Tic-Tac-Toe",
                    "Enter your name:",
                    placeholder: "Player"
                );
                tcs.SetResult(string.IsNullOrWhiteSpace(name) ? "Player" : name);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

        return tcs.Task.GetAwaiter().GetResult();
    }

    private async Task<bool> PromptPlayAgainAsync()
    {
        var result = await MainThread.InvokeOnMainThreadAsync(async () =>
            await DisplayAlert("Play Again?", "Would you like to play another game?", "Yes", "No")
        );
        return result;
    }

    private async Task<DifficultyLevel> PromptDifficultyLevelAsync()
    {
        var result = await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var choice = await DisplayActionSheet(
                "Select Difficulty",
                "Cancel",
                null,
                "Easy",
                "Medium",
                "Hard"
            );

            return choice switch
            {
                "Easy" => DifficultyLevel.Easy,
                "Medium" => DifficultyLevel.Medium,
                _ => DifficultyLevel.Hard
            };
        });
        return result;
    }

    private async Task<string> GetPlayersNameAsync()
    {
        Console.WriteLine("[DEBUG] Prompting for player name...");

        return await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var name = await DisplayPromptAsync(
                "Welcome to Tic-Tac-Toe",
                "Enter your name:",
                placeholder: "Player"
            );

            Console.WriteLine($"[DEBUG] Player name received: {name}");

            return string.IsNullOrWhiteSpace(name) ? "Player" : name;
        });
    }

    public async Task<char> GetPlayersSymbolAsync()
    {
        bool result = await DisplayAlert(
            "Choose Your Symbol",
            "Do you want to play as X? (X goes first)",
            "Yes (X)",
            "No (O)"
        );
        return result ? 'X' : 'O';
    }

    public char GetPlayersSymbol()
    {
        // Since we can't block on async calls in sync methods,
        // we'll use GetAwaiter().GetResult() pattern
        return GetPlayersSymbolAsync().GetAwaiter().GetResult();
    }

    private void NewGameButton_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine("[DEBUG] NewGameButton clicked. Initiating new game...");
        Clear();
        Board.ClearBoard();
        MainThread.BeginInvokeOnMainThread(() => InitializeGameAsync());
    }
    // Not needed anymore since we're using TaskCompletionSource
    public void WaitForMoveCompletion() { }
}

