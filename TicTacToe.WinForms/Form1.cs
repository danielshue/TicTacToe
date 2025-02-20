using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;

namespace TicTacToe.WinForms
{
    public partial class Form1 : Form, ITickTacToeUI
    {
        private readonly Button[,] _buttons;
        private readonly int _buttonSize = 140; // Increased button size further for an even larger window
        private readonly int _padding = 30; // Increased padding further for an even larger window
        private int _currentRow;
        private int _currentCol;
        private IScore? _score;
        private ITicTacToeBoard _board;
        private readonly AutoResetEvent _moveCompleted = new AutoResetEvent(false);

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentRow 
        { 
            get => _currentRow;
            set => _currentRow = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentCol 
        { 
            get => _currentCol;
            set => _currentCol = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IScore Score 
        { 
            get => _score ?? throw new InvalidOperationException("Score not initialized");
            set => _score = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ITicTacToeBoard Board 
        { 
            get => _board ?? throw new InvalidOperationException("Board not initialized");
            set => _board = value;
        }

        public Form1()
        {
            InitializeComponent();
            
            // Set form properties
            Text = "Tic Tac Toe";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            
            _buttons = new Button[ITicTacToeBoard.BoardSize, ITicTacToeBoard.BoardSize];
            _board = new TickTacToeBoard();
            InitializeGameBoard();
        }

        private void InitializeGameBoard()
        {
            SuspendLayout();
            
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    var button = new Button
                    {
                        Size = new Size(_buttonSize, _buttonSize),
                        Location = new Point(j * (_buttonSize + _padding) + _padding, i * (_buttonSize + _padding) + _padding),
                        Font = new Font("Segoe UI", 48, FontStyle.Bold),
                        Tag = (i, j),
                        TabIndex = i * ITicTacToeBoard.BoardSize + j,
                        FlatStyle = FlatStyle.Flat
                    };
                    button.Click += Button_Click!;
                    _buttons[i, j] = button;
                    Controls.Add(button);
                }
            }

            // Set form size to fit the game board plus padding
            int totalWidth = ITicTacToeBoard.BoardSize * (_buttonSize + _padding) + _padding;
            int totalHeight = ITicTacToeBoard.BoardSize * (_buttonSize + _padding) + _padding;
            ClientSize = new Size(totalWidth, totalHeight); // Window size recalculated based on updated button size and padding
            
            ResumeLayout(false);
            PerformLayout();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            CenterToScreen();
            Activate();
            DisplayScore();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is ValueTuple<int, int> position)
            {
                CurrentRow = position.Item1;
                CurrentCol = position.Item2;
                if (Board.PlaceSymbol(CurrentRow, CurrentCol, Score.CurrentPlayer.Symbol))
                {
                    button.Text = Score.CurrentPlayer.Symbol.ToString();
                    button.ForeColor = Score.CurrentPlayer.Symbol == 'X' ? Color.Red : Color.Blue;
                    button.Enabled = false;
                    button.Cursor = Cursors.No;
                    RefreshBoard();
                    _moveCompleted.Set();
                }
            }
        }

        public void Clear()
        {
            if (InvokeRequired)
            {
                Invoke(Clear);
                return;
            }

            foreach (var button in _buttons)
            {
                button.Text = "";
                button.Enabled = true;
            }
        }

        public void Display()
        {
            DisplayBoard(-1, -1);
        }

        public void DisplayBoard(int currentRow, int currentCol)
        {
            if (InvokeRequired)
            {
                Invoke(() => DisplayBoard(currentRow, currentCol));
                return;
            }

            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    _buttons[i, j].Text = Board.BoardArray[i, j].ToString();
                    _buttons[i, j].BackColor = (i == currentRow && j == currentCol) ? Color.LightGray : SystemColors.Control;
                }
            }
        }

        public void DisplayDraw()
        {
            if (InvokeRequired)
            {
                Invoke(DisplayDraw);
                return;
            }
            MessageBox.Show("It's a draw!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void DisplayGameBoard()
        {
            Display();
        }

        public void DisplayGameBoard(int currentRow, int currentCol)
        {
            DisplayBoard(currentRow, currentCol);
        }

        public void DisplayPlayerWin(Player player)
        {
            if (InvokeRequired)
            {
                Invoke(() => DisplayPlayerWin(player));
                return;
            }
            MessageBox.Show($"{player.Name} wins!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void DisplayScore()
        {
            if (InvokeRequired)
            {
                Invoke(DisplayScore);
                return;
            }
            Text = Score?.ToString() ?? "Tic Tac Toe";
        }

        public string GetPlayersName()
        {
            if (InvokeRequired)
            {
                return (string)Invoke(() => GetPlayersName());
            }

            // Use lock to prevent multiple dialogs
            lock (_moveCompleted)
            {
                string name = "Player";
                using var dialog = new Form
                {
                    Width = 500,
                    Height = 250,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = "Welcome to Tic-Tac-Toe",
                    StartPosition = FormStartPosition.CenterScreen,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    TopMost = true,
                    ShowInTaskbar = false,
                    KeyPreview = true,
                    Padding = new Padding(20)
                };

                var tableLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    RowCount = 3,
                    Padding = new Padding(20),
                    RowStyles = { 
                        new RowStyle(SizeType.Absolute, 40),
                        new RowStyle(SizeType.Absolute, 40),
                        new RowStyle(SizeType.Absolute, 60)
                    }
                };

                var label = new Label
                {
                    Text = "Enter your name:",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 14F, FontStyle.Regular),
                    Dock = DockStyle.Fill
                };

                var textBox = new TextBox
                {
                    Text = "Player",
                    Font = new Font("Segoe UI", 14F, FontStyle.Regular),
                    Height = 35,
                    Dock = DockStyle.Fill
                };
                textBox.SelectAll();

                var buttonPanel = new Panel
                {
                    Dock = DockStyle.Fill,
                    Height = 50
                };

                var buttonOk = new Button
                {
                    Text = "&Start Game",
                    Height = 45,
                    Width = 200,
                    Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                    DialogResult = DialogResult.OK,
                    Anchor = AnchorStyles.None
                };
                buttonOk.Location = new Point((buttonPanel.Width - buttonOk.Width) / 2, 0);

                buttonPanel.Controls.Add(buttonOk);
                tableLayout.Controls.AddRange(new Control[] { label, textBox, buttonPanel });
                dialog.Controls.Add(tableLayout);
                dialog.AcceptButton = buttonOk;
                dialog.CancelButton = buttonOk;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    name = string.IsNullOrWhiteSpace(textBox.Text) ? "Player" : textBox.Text;
                }
                return name;
            }
        }

        public void HandleDraw()
        {
            Clear();
            DisplayScore();
            DisplayGameBoard();
            DisplayDraw();
            if (Score != null) Score.Draws++;
        }

        public void PlayerMove(Player currentPlayer)
        {
            if (InvokeRequired)
            {
                Invoke(() => PlayerMove(currentPlayer));
                return;
            }

            // Reset the move event to ensure waiting for new input
            _moveCompleted.Reset();

            // Enable buttons only for empty cells
            for (int i = 0; i < ITicTacToeBoard.BoardSize; i++)
            {
                for (int j = 0; j < ITicTacToeBoard.BoardSize; j++)
                {
                    var button = _buttons[i, j];
                    bool isEmpty = Board.IsCellEmpty(i, j);
                    button.Enabled = isEmpty;
                    button.Cursor = isEmpty ? Cursors.Hand : Cursors.No;
                    button.BackColor = isEmpty ? SystemColors.Control : Color.WhiteSmoke;
                }
            }

            // Set focus to the first available button
            _buttons.Cast<Button>().FirstOrDefault(b => b.Enabled)?.Focus();
        }

        public string ReadInput()
        {
            throw new NotImplementedException("ReadInput is not used in Windows Forms implementation");
        }

        public bool PromptPlayAgain()
        {
            if (InvokeRequired)
            {
                return (bool)Invoke(PromptPlayAgain);
            }

            var result = MessageBox.Show(this, 
                "Would you like to play again?", 
                "Game Over", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
            {
                Clear();
                return true;
            }
            return false;
        }

        public DifficultyLevel PromptDifficultyLevel()
        {
            if (InvokeRequired)
            {
                return (DifficultyLevel)Invoke(() => PromptDifficultyLevel());
            }

            // Use lock to prevent multiple dialogs
            lock (_moveCompleted)
            {
                using var dialog = new Form
                {
                    Width = 500,
                    Height = 350,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = "Game Difficulty",
                    StartPosition = FormStartPosition.CenterScreen,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    TopMost = true,
                    ShowInTaskbar = false,
                    KeyPreview = true,
                    Padding = new Padding(20)
                };

                var tableLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    RowCount = 5,
                    Padding = new Padding(20),
                    RowStyles = {
                        new RowStyle(SizeType.Absolute, 50),  // Label
                        new RowStyle(SizeType.Absolute, 50),  // Easy
                        new RowStyle(SizeType.Absolute, 50),  // Medium
                        new RowStyle(SizeType.Absolute, 50),  // Hard
                        new RowStyle(SizeType.Absolute, 60)   // Button
                    }
                };

                var label = new Label
                {
                    Text = "Select difficulty level:",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 14F, FontStyle.Regular),
                    Dock = DockStyle.Fill
                };

                var radioEasy = new RadioButton 
                { 
                    Text = "&Easy (E)", 
                    Checked = true,
                    Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                    AutoSize = true,
                    Dock = DockStyle.Fill
                };
                
                var radioMedium = new RadioButton 
                { 
                    Text = "&Medium (M)",
                    Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                    AutoSize = true,
                    Dock = DockStyle.Fill
                };
                
                var radioHard = new RadioButton 
                { 
                    Text = "&Hard (H)",
                    Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                    AutoSize = true,
                    Dock = DockStyle.Fill
                };

                var buttonPanel = new Panel
                {
                    Dock = DockStyle.Fill,
                    Height = 50
                };

                var buttonOk = new Button
                {
                    Text = "Start Game",
                    Height = 45,
                    Width = 200,
                    Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                    DialogResult = DialogResult.OK,
                    Anchor = AnchorStyles.None
                };
                buttonOk.Location = new Point((buttonPanel.Width - buttonOk.Width) / 2, 0);

                buttonPanel.Controls.Add(buttonOk);
                tableLayout.Controls.AddRange(new Control[] { label, radioEasy, radioMedium, radioHard, buttonPanel });
                dialog.Controls.Add(tableLayout);
                dialog.AcceptButton = buttonOk;
                dialog.CancelButton = buttonOk;

                // Add keyboard shortcuts
                dialog.KeyDown += (s, e) =>
                {
                    switch (e.KeyCode)
                    {
                        case Keys.E:
                            radioEasy.Checked = true;
                            e.Handled = true;
                            break;
                        case Keys.M:
                            radioMedium.Checked = true;
                            e.Handled = true;
                            break;
                        case Keys.H:
                            radioHard.Checked = true;
                            e.Handled = true;
                            break;
                    }
                };

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    if (radioHard.Checked) return DifficultyLevel.Hard;
                    if (radioMedium.Checked) return DifficultyLevel.Medium;
                }
            }
            return DifficultyLevel.Easy;
        }

        private void RefreshBoard()
        {
            if (InvokeRequired)
            {
                Invoke(RefreshBoard);
                return;
            }
            
            DisplayScore();
            DisplayGameBoard(CurrentRow, CurrentCol);
            
            // Ensure proper focus and activation
            BringToFront();
            Activate();
        }

        // Added method to wait for player's move completion
        public void WaitForMoveCompletion() => _moveCompleted.WaitOne();

        // These methods are part of ITickTacToeUI but not used in Windows Forms
        public void ResetColor() { }
        public void SetBackgroundColor(string color) { }
        public void SetForegroundColor(string color) { }
        public void SetCursorPosition(int left, int top) { }
    }
}
