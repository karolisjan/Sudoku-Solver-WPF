using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver_WPF
{
    public class SudokuSolver
    {
        public int[,] sudoku;
        public int[] rows;
        public int[] columns;
        public int goBackFlag;
        public int nextNum = 1;
        public int c = 0;
        public int r = 0;
        public int iteration;

        public int[,] solveSudoku(int[,] sudokuProblem, out int[,] sudokuAnswer, out int iterations)
        {
            sudoku = (int[,])sudokuProblem.Clone();

            c = 0;
            r = 0;

            while (true)
            {
                if (c > 8)
                {
                    c = 0;
                    r = r + 1;
                    if (r > 8)
                    {
                        iterations = iteration;
                        sudokuAnswer = sudoku;
                        return sudokuAnswer;
                    }
                }
                //else if (c < -1)
                //{
                //    c = 8;
                //    r = r - 1;
                //}

                if (nextNum > 9)
                {
                    goBackFlag = 1;
                    if (sudokuProblem[r, c] == 0)
                    {
                        sudoku[r, c] = 0;
                    }
                    if (c == 0)
                    {
                        c = 9;
                        r = r - 1;
                    }
                    c = c - 1;
                    nextNum = sudoku[r, c] + 1;
                }

                if (sudokuProblem[r, c] == 0)
                {
                    for (int i = nextNum; i <= 9; i++)
                    {
                        sudoku[r, c] = i;
                        int row = r;
                        int column = c;
                        if (checkIfValid(sudoku, row, column, i))
                        {
                            goBackFlag = 0;
                            sudoku[r, c] = i;
                            c = c + 1;
                            nextNum = 1;
                            break;
                        }
                        else if (i == 9)
                        {
                            goBackFlag = 1;
                            sudoku[r, c] = 0;
                            if (c == 0)
                            {
                                c = 9;
                                r = r - 1;
                            }
                            c = c - 1;
                            nextNum = sudoku[r, c] + 1;
                            break;
                        }
                    }
                }
                else
                {
                    if (goBackFlag == 1)
                    {
                        if (c == 0)
                        {
                            c = 9;
                            r = r - 1;
                        }
                        c = c - 1;
                        nextNum = sudoku[r, c] + 1;
                    }
                    else
                    {
                        c = c + 1;
                    }
                }
                iteration = iteration + 1;
            }

            sudokuAnswer = sudoku;
            return sudokuAnswer;
        }

        public bool checkIfValid(int[,] sudokuTemp, int row, int column, int number)
        {
            sudoku = (int[,])sudokuTemp.Clone();

            sudoku[row, column] = 0;

            if (row >= 0 && row <= 2 && column >= 0 && column <= 2)
            {
                rows = new int[] { 0, 1, 2 };
                columns = new int[] { 0, 1, 2 };
            }
            else if (row >= 0 && row <= 2 && column >= 3 && column <= 5)
            {
                rows = new int[] { 0, 1, 2 };
                columns = new int[] { 3, 4, 5 };
            }
            else if (row >= 0 && row <= 2 && column >= 6 && column <= 8)
            {
                rows = new int[] { 0, 1, 2 };
                columns = new int[] { 6, 7, 8 };
            }
            else if (row >= 3 && row <= 5 && column >= 0 && column <= 2)
            {
                rows = new int[] { 3, 4, 5 };
                columns = new int[] { 0, 1, 2 };
            }
            else if (row >= 3 && row <= 5 && column >= 3 && column <= 5)
            {
                rows = new int[] { 3, 4, 5 };
                columns = new int[] { 3, 4, 5 };
            }
            else if (row >= 3 && row <= 5 && column >= 6 && column <= 8)
            {
                rows = new int[] { 3, 4, 5 };
                columns = new int[] { 6, 7, 8 };
            }
            else if (row >= 6 && row <= 8 && column >= 0 && column <= 2)
            {
                rows = new int[] { 6, 7, 8 };
                columns = new int[] { 0, 1, 2 };
            }
            else if (row >= 6 && row <= 8 && column >= 3 && column <= 5)
            {
                rows = new int[] { 6, 7, 8 };
                columns = new int[] { 3, 4, 5 };
            }
            else if (row >= 6 && row <= 8 && column >= 6 && column <= 8)
            {
                rows = new int[] { 6, 7, 8 };
                columns = new int[] { 6, 7, 8 };
            }

            int p = 0;
            int elements = 1;

            while (p <= 8)
            {
                if (number == sudoku[row, p] || number == sudoku[p, column])
                {
                    return false;
                }
                else if (p == 8)
                {
                    foreach (int r in rows)
                    {
                        foreach (int c in columns)
                        {
                            if (number == sudoku[r, c])
                            {
                                return false;
                            }
                            else if (elements == 9)
                            {
                                return true;
                            }
                            elements = elements + 1;
                        }
                    }
                }
                else
                {
                    p = p + 1;
                }
            }

            return false;
        }
    }
}
