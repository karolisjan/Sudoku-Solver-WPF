using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace SudokuSolver_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker bcLoad = new BackgroundWorker();
        SudokuSolver puzzle = new SudokuSolver();

        int[,] sudokuProblem;
        int[,] answer;
        int steps;
        int i;
        int j;
        double ts;

        public MainWindow()
        {
            InitializeComponent();

            bcLoad.DoWork += new DoWorkEventHandler(bcLoad_DoWork);
            bcLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bcLoad_RunWorkerCompleted);
        }

        private void solveBtn_Click(object sender, RoutedEventArgs e)
        {
            reportBox.Text = "Running...";

            sudokuProblem = new int[9, 9];

            i = 0;
            j = 0;

            foreach (Control txtBox in sudokuGrid.Children)
            {
                if (txtBox.GetType() == typeof(TextBox))
                {
                    try
                    {
                        if (((TextBox)txtBox).Text == string.Empty)
                        {
                            sudokuProblem[j, i] = 0;
                        }
                        else
                        {
                            sudokuProblem[j, i] = int.Parse(((TextBox)txtBox).Text);
                        }
                    }
                    catch (Exception)
                    {
                        if (i == 0 && j == 0)
                        {
                            goto Run;
                        }
                        else
                        {
                            reportBox.Text = "Puzzle cannot be solved";
                            return;
                        }                     
                    }

                    i++;

                    if (i > 8)
                    {
                        i = 0;
                        j++;                   
                    }

                    if (j > 8 && i > 8)
                    {
                        goto Run;
                    }
                    else if (j > 8)
                    {
                        j = 0;
                    }
                }
            }

        Run:
            try
            {
                solveBtn.IsEnabled = false;
                bcLoad.RunWorkerAsync();
            }
            catch (Exception)
            {
                solveBtn.IsEnabled = true;
                reportBox.Text = "Puzzle cannot be solved";
            }                      
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            answer = new int[9, 9];
            steps = 0;

            foreach (Control txtBox in sudokuGrid.Children)
            {
                if (txtBox.GetType() == typeof(TextBox))
                {
                    try
                    {
                        ((TextBox)txtBox).Text = string.Empty;
                    }
                    catch (Exception)
                    {
                        reportBox.Text = "Something went wrong";
                        return;
                    }
                }
            }

            reportBox.Text = "Enter the puzzle and click \"Solve\"";
        }

        private void bcLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            int[,] sudokuCopy = sudokuProblem;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (sudokuProblem[i, j] != 0)
                    {
                        if (!puzzle.checkIfValid(sudokuProblem, i, j, sudokuProblem[i, j])
                            || sudokuProblem[i, j] < 0
                            || sudokuProblem[i, j] > 9
                            || sudokuProblem[i, j] % 1 != 0)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }

            sudokuProblem = sudokuCopy;

            Run();
        }

        private void bcLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                solveBtn.IsEnabled = true;
                reportBox.Text = "Puzzle cannot be solved";
            }
            else
            {
                i = j = 0;

                foreach (Control txtBox in sudokuGrid.Children)
                {
                    if (txtBox.GetType() == typeof(TextBox))
                    {
                        try
                        {
                            ((TextBox)txtBox).Text = answer[j, i].ToString();
                        }
                        catch (Exception)
                        {
                            ((TextBox)txtBox).Text = "x";
                        }

                        i++;

                        if (i > 8)
                        {
                            i = 0;
                            j++;
                        }
                        else if (j > 8)
                        {
                            j = 0;
                        }
                    }
                }

                solveBtn.IsEnabled = true;

                reportBox.Text = String.Format("Solved in {0} steps and {1} seconds", steps, ts); ;
            }
        }

        private void Run()
        {
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            puzzle.solveSudoku(sudokuProblem, out answer, out steps);
            stopWatch.Stop();

            ts = stopWatch.ElapsedMilliseconds;

            ts = ts / 1000;         
        }
    }
}
