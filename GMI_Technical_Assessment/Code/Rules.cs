using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMI_Technical_Assessment.Code
{
    public abstract class MatchRule
    {
        protected ConsoleColor requiredColor;
        protected ConsoleColor matrixColor;

        public ConsoleColor RequiredColor { get { return requiredColor; }  set { requiredColor = value; } }
        public ConsoleColor MatrixColor { get { return matrixColor; }  set { matrixColor = value; } }

        public MatchRule(ConsoleColor matrixColor)
        {
            this.matrixColor = matrixColor;
        }

        public MatchRule(ConsoleColor matrixColor, ConsoleColor requiredColor)
        {
            this.matrixColor = matrixColor;
            this.requiredColor = requiredColor;
        }

        public abstract int FindMatches(Grid grid);
    }

    public class UnmatchedRule : MatchRule
    {
        public UnmatchedRule(ConsoleColor matrixColor) : base(matrixColor)
        {
        }

        public UnmatchedRule(ConsoleColor matrixColor, ConsoleColor requiredColor) : base(matrixColor, requiredColor)
        {
        }

        public override int FindMatches(Grid grid)
        {
            int matchesCount = 0;

            for (int i = 0; i < grid.Matrix.GetLength(0); i++)
            {
                for(int j = 0; j < grid.Matrix.GetLength(1); j++)
                {
                    if (grid.Matrix[i,j].value != 1 || grid.Matrix[i,j].color != matrixColor)
                        continue;

                    grid.Matrix[i,j].color = requiredColor;
                    matchesCount++;
                }
            }

            return matchesCount;
        }
    }

    public class StraightLineOfFive : MatchRule
    {
        private const int requiredLength = 5;

        public StraightLineOfFive(ConsoleColor matrixColor, ConsoleColor requiredColor) : base(matrixColor, requiredColor)
        {
        }

        public StraightLineOfFive(ConsoleColor matrixColor) : base(matrixColor)
        {
        }

        public override int FindMatches(Grid grid)
        {
            int matchesCount = 0;

            matchesCount += FindHorizontalMatch(grid);
            matchesCount += FindVerticalMatch(grid);

            return matchesCount;
        }

        private int FindHorizontalMatch(Grid grid)
        {
            int matchesCount = 0;

            if (grid.Matrix.GetLength(1) < 5)
                return 0;

            for (int i = 0; i < grid.Matrix.GetLength(0); i++)
            {
                int sum = 0;

                for (int j = 0; j < grid.Matrix.GetLength(1); j++)
                {
                    if (requiredLength - sum + j > grid.Matrix.GetLength(1))
                    {
                        break;
                    }

                    else if (grid.Matrix[i,j].value != 1 || grid.Matrix[i,j].color != matrixColor)
                    {
                        sum = 0;
                        continue;
                    }

                    else
                    {
                        sum++;
                    }

                    if (sum >= requiredLength)
                    {
                        FillColorHorizontaly(grid, i, j - (requiredLength - 1));
                        matchesCount++;
                        sum = 0;
                    }
                }
            }

            return matchesCount;
        }

        private void FillColorHorizontaly(Grid grid, int iFrom, int jFrom)
        {
            for(int j = jFrom; j < jFrom + requiredLength; j++)
            {
                grid.Matrix[iFrom,j].color = requiredColor;
            }
        }

        private int FindVerticalMatch(Grid grid)
        {
            int matchesCount = 0;

            if (grid.Matrix.GetLength(0) < 5)
                return 0;

            for (int j = 0; j < grid.Matrix.GetLength(1); j++)
            {
                int sum = 0;

                for (int i = 0; i < grid.Matrix.GetLength(0); i++)
                {
                    if (requiredLength - sum + i > grid.Matrix.GetLength(0))
                    {
                        break;
                    }

                    else if (grid.Matrix[i,j].value != 1 || grid.Matrix[i,j].color != matrixColor)
                    {
                        sum = 0;
                        continue;
                    }

                    else
                    {
                        sum++;
                    }

                    if (sum >= requiredLength)
                    {
                        FillColorVertical(grid, i - (requiredLength - 1), j);
                        matchesCount++;
                        sum = 0;
                    }
                }
            }

            return matchesCount;
        }

        private void FillColorVertical(Grid grid, int iFrom, int jFrom)
        {
            for (int i = iFrom; i < iFrom + requiredLength; i++)
            {
                grid.Matrix[i,jFrom].color = requiredColor;
            }
        }
    }
}
