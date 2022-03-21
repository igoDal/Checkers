using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Checkers
{
    public partial class MainWindow : Window
    {
        private CheckersType[,] boardArray;
        private bool playerOneTurn;
        private bool fieldToMove;
        private List<Button> buttonList;
        private Button previewsButton;
        private int row, column, previewsRow, previewsColumn;
        private int playerOneCheckersCount, playerTwoCheckersCount;
        private Brush playerOneColor;
        private Brush playerTwoColor;

        public MainWindow()
        {
            InitializeComponent();
            playerOneColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            playerTwoColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            NewGame();
        }

        private void NewGame()
        {
            buttonList = Board.Children.Cast<Button>().ToList();
            boardArray = new CheckersType[8, 8];

            for (int checkerRow = 0; checkerRow < 8; checkerRow++)
            {
                if (checkerRow == 0 || checkerRow == 2 || checkerRow == 6)
                {
                    for (int col = 0; col < 7; col += 2)
                    {
                        if (checkerRow == 0 || checkerRow == 2)
                        {
                            boardArray[checkerRow, col] = CheckersType.PlayerTwoChecker;
                        }
                        else
                        {
                            boardArray[checkerRow, col] = CheckersType.PlayerOneChecker;
                        }
                    }
                }

                if (checkerRow == 1 || checkerRow == 5 || checkerRow == 7)
                {

                    for (int col = 1; col < 8; col += 2)
                    {
                        if (checkerRow == 5 || checkerRow == 7)
                        {
                            boardArray[checkerRow, col] = CheckersType.PlayerOneChecker;
                        }
                        else
                        {
                            boardArray[checkerRow, col] = CheckersType.PlayerTwoChecker;
                        }
                    }
                }
            }

            playerOneTurn = true;
            fieldToMove = false;
            row = -1;
            column = 0;
            previewsRow = 0;
            previewsColumn = 0;

            playerOneCheckersCount = 12;
            playerTwoCheckersCount = 12;

            int counter = 0;

            buttonList.ForEach(button =>
                {
                    if (counter < 12)
                    {
                        button.Content = "o";
                        button.Foreground = playerTwoColor;
                        counter++;
                    }
                    else if (counter >= 20 && counter < 32)
                    {
                        button.Content = "x";
                        button.Foreground = playerOneColor;
                        counter++;
                    }
                    else
                    {
                        button.Content = string.Empty;
                        counter++;
                    }
                }
            );
        }

        private void UpdateBoard()
        {
            buttonList.ForEach(button => {

                int currentRow = Grid.GetRow(button);
                int currentColumn = Grid.GetColumn(button);

                if (boardArray[currentRow, currentColumn] == CheckersType.PlayerOneChecker)
                {
                    button.Content = "x";
                    button.Foreground = playerOneColor;
                }
                else if (boardArray[currentRow, currentColumn] == CheckersType.PlayerTwoChecker)
                {
                    button.Content = "o";
                    button.Foreground = playerTwoColor;
                }
                else
                {
                    button.Content = "";
                }
            });
        }

        private void EndTurn()
        {

            fieldToMove = !fieldToMove;
            playerOneTurn = !playerOneTurn;

        }

        private void InvalidMove()
        {
            fieldToMove = false;
        }

        private bool GameOver()
        {
            if (playerOneCheckersCount == 0 || playerTwoCheckersCount == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (GameOver())
            {
                MessageBox.Show(playerOneCheckersCount > 0 ? "X won" : "O won", "Game over");
            }

            var button = (Button)sender;

            column = Grid.GetColumn(button);
            row = Grid.GetRow(button);

            if (playerOneTurn)
            {
                if (fieldToMove)
                {
                    previewsRow = Grid.GetRow(previewsButton);
                    previewsColumn = Grid.GetColumn(previewsButton);
                    if (boardArray[previewsRow, previewsColumn] == CheckersType.PlayerOneChecker)
                    {

                        if (boardArray[row, column] == CheckersType.Free && (row - previewsRow == -1) && (column - previewsColumn == -1 || column - previewsColumn == 1))
                        {

                            boardArray[row, column] = CheckersType.PlayerOneChecker;
                            boardArray[previewsRow, previewsColumn] = CheckersType.Free;
                            button.Content = "x";
                            button.Foreground = playerOneColor;
                            previewsButton.Content = "";

                            EndTurn();
                        }
                        else if (boardArray[row, column] == CheckersType.Free && (row - previewsRow == -2) && (column - previewsColumn == -2))
                        {
                            if (boardArray[row + 1, column + 1] == CheckersType.PlayerTwoChecker)
                            {
                                playerTwoCheckersCount--;

                                boardArray[row + 1, column + 1] = CheckersType.Free;

                                boardArray[row, column] = CheckersType.PlayerOneChecker;
                                boardArray[previewsRow, previewsColumn] = CheckersType.Free;

                                UpdateBoard();
                                EndTurn();
                            }

                        }
                        else if (boardArray[row, column] == CheckersType.Free && (row - previewsRow == -2) && (column - previewsColumn == 2))
                        {

                            if (boardArray[row + 1, column - 1] == CheckersType.PlayerTwoChecker)
                            {
                                playerTwoCheckersCount--;
                                
                                boardArray[row + 1, column - 1] = CheckersType.Free;

                                boardArray[row, column] = CheckersType.PlayerOneChecker;
                                boardArray[previewsRow, previewsColumn] = CheckersType.Free;

                                UpdateBoard();
                                EndTurn();

                            }
                        }
                        else
                        {
                            InvalidMove();
                        }
                    }
                }
                else
                {

                    if (boardArray[row, column] == CheckersType.PlayerOneChecker && row != 0)
                    {
                        previewsButton = button;
                        fieldToMove = true;
                    }
                }
            }
            else
            {
                if (fieldToMove)
                {
                    previewsRow = Grid.GetRow(previewsButton);
                    previewsColumn = Grid.GetColumn(previewsButton);
                    if (boardArray[previewsRow, previewsColumn] == CheckersType.PlayerTwoChecker)
                    {
                        if (boardArray[row, column] == CheckersType.Free && (row - previewsRow == 1) && (column - previewsColumn == -1 || column - previewsColumn == 1))
                        {
                            boardArray[row, column] = CheckersType.PlayerTwoChecker;

                            boardArray[previewsRow, previewsColumn] = CheckersType.Free;

                            button.Content = "o";
                            button.Foreground = playerTwoColor;
                            previewsButton.Content = "";

                            EndTurn();
                        }
                        else if (boardArray[row, column] == CheckersType.Free && (row - previewsRow == 2) && column - previewsColumn == -2)
                        {
                            if (boardArray[row - 1, column + 1] == CheckersType.PlayerOneChecker)
                            {
                                playerOneCheckersCount--;

                                boardArray[row - 1, column + 1] = CheckersType.Free;

                                boardArray[row, column] = CheckersType.PlayerTwoChecker;
                                boardArray[previewsRow, previewsColumn] = CheckersType.Free;

                                UpdateBoard();
                                EndTurn();
                            }
                        }
                        else if (boardArray[row, column] == CheckersType.Free && (row - previewsRow == 2) && column - previewsColumn == 2)
                        {
                            playerOneCheckersCount--;

                            boardArray[row - 1, column - 1] = CheckersType.Free;

                            boardArray[row, column] = CheckersType.PlayerTwoChecker;
                            boardArray[previewsRow, previewsColumn] = CheckersType.Free;

                            UpdateBoard();
                            EndTurn();

                        }
                        else
                        {
                            InvalidMove();
                        }
                    }
                }
                else
                {

                    if (boardArray[row, column] == CheckersType.PlayerTwoChecker && row != 7)
                    {
                        previewsButton = button;
                        fieldToMove = true;
                    }
                }
            }
        }
    }
}