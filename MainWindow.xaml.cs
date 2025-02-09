using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace amoba
{
    public enum PlayersType
    {
        X, O
    }

	public partial class MainWindow : Window
    {
        public const int BOARD_SIZE = 3;

        private PlayersType currentTurn = PlayersType.X;
        private int scoreOfX = 0;
        private int scoreOfO = 0;

		public MainWindow()
        {
            InitializeComponent();
            Start(true);
        }
		private void NewGame(object sender, RoutedEventArgs e)
		{
			Start(true);
		}

        private void Reset()
        {
			scoreOfO = 0;
			scoreOfX = 0;
			currentTurn = PlayersType.X;

            UpdateScores();
		}

		private void UpdateScores()
		{
			x_score.Text = scoreOfX.ToString();
			o_score.Text = scoreOfO.ToString();

			state.Text = $"{currentTurn} jön!";
		}

		private void GenerateBoard()
		{
			GameBoard.Children.Clear();
			for (int row = 0; row < BOARD_SIZE; row++)
			{
				for (int col = 0; col < BOARD_SIZE; col++)
				{
					Button button = new Button()
					{
						FontSize = 40,
					};
					button.Click += GameButton;

					GameBoard.Children.Add(button);
					Grid.SetRow(button, row);
					Grid.SetColumn(button, col);
				}
			}
		}
		private void Start(bool reset = false)
		{
			if (reset) Reset();

			currentTurn = PlayersType.X;
			UpdateScores();
			GenerateBoard();
		}

		private void GameButton(object sender, RoutedEventArgs e)
        {
			var pressed = sender as Button;
			if (pressed is null) return;

			pressed.IsEnabled = false;
			pressed.Content = currentTurn == PlayersType.X ? "X" : "O";
			pressed.Foreground = currentTurn == PlayersType.X ? Brushes.Blue : Brushes.Red;

			if (CheckWin())
			{
				MessageBox.Show($"{currentTurn} nyert!");
				if (currentTurn == PlayersType.X) scoreOfX++;
				else scoreOfO++;

				Start();
				return;
			}

			if (CheckDraw())
			{
				MessageBox.Show("Döntetlen!");
				Start();
				return;
			}

			currentTurn = currentTurn == PlayersType.X ? PlayersType.O : PlayersType.X;
            UpdateScores();
        }

        private bool CheckWin()
        {
			for (int row = 0; row < BOARD_SIZE; row++)
			{
				if (CheckLine(0, row, 1, 0)) return true;
				if (CheckLine(row, 0, 0, 1)) return true;
			}
			if (CheckLine(0, 0, 1, 1)) return true;
			if (CheckLine(0, BOARD_SIZE - 1, 1, -1)) return true;
			return false;
		}
		private bool CheckLine(int row, int col, int rowInc, int colInc)
		{
			var first = GameBoard.Children[row * BOARD_SIZE + col] as Button;
			if (first is null) return false;
			var firstContent = first.Content;
			if (firstContent is null) return false;

			for (int i = 1; i < BOARD_SIZE; i++)
			{
				var current = GameBoard.Children[(row + i * rowInc) * BOARD_SIZE + (col + i * colInc)] as Button;
				if (current is null) return false;
				var currentContent = current.Content;
				if (currentContent is null) return false;
				if (firstContent != currentContent) return false;
			}
			return true;
		}
		private bool CheckDraw()
		{
			return GameBoard.Children.Cast<Button>().All(button => button.IsEnabled == false);
		}
	}
}