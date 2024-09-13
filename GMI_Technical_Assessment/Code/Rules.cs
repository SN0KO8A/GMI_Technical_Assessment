using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMI_Technical_Assessment.Code
{
    public abstract class MatchRule
    {
        protected string name;
        protected int matchesCount;
        protected ConsoleColor requiredColor;
        protected ConsoleColor defaultColor;

        public MatchRule(ConsoleColor defaultColor)
        {
            this.defaultColor = defaultColor;
        }

        public virtual void FindMatches(Grid grid) 
        { 
            matchesCount = 0; 
        }

        public virtual void DisplayResult()
        {
            Console.ForegroundColor = requiredColor;
            Console.Write(name);
            Console.ResetColor();

            Console.Write($" Count: {matchesCount}\n");
        }
    }

    public class UnmatchedRule : MatchRule
    {
        public UnmatchedRule(ConsoleColor defaultColor) : base(defaultColor)
        {
            name = "Unmatched";
            matchesCount = 0;
            requiredColor = ConsoleColor.Yellow;
        }

        public override void FindMatches(Grid grid)
        {
            base.FindMatches(grid);

            for (int i = 0; i < grid.Matrix.Length; i++)
            {
                for(int j = 0; j < grid.Matrix[i].Length; j++)
                {
                    if (grid.Matrix[i][j].value != 1 || grid.Matrix[i][j].color != defaultColor)
                        continue;

                    grid.Matrix[i][j].color = requiredColor;
                    matchesCount++;
                }
            }
        }
    }

    public class StraightLineOfFive : MatchRule
    {
        private const int requiredLength = 5;

        public StraightLineOfFive(ConsoleColor defaultColor) : base(defaultColor)
        {
            name = "Straight line of 5";
            matchesCount = 0;
            requiredColor = ConsoleColor.Blue;
        }

        public override void FindMatches(Grid grid)
        {
            base.FindMatches(grid);

            FindHorizontalMatch(grid);
            FindVerticalMatch(grid);
        }

        private void FindHorizontalMatch(Grid grid)
        {
            if (grid.Matrix[0].Length < 5)
                return;

            for (int i = 0; i < grid.Matrix.Length; i++)
            {
                int sum = 0;

                for (int j = 0; j < grid.Matrix[i].Length; j++)
                {
                    if (requiredLength - sum + j > grid.Matrix[i].Length)
                    {
                        break;
                    }

                    else if (grid.Matrix[i][j].value != 1 || grid.Matrix[i][j].color != defaultColor)
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
        }

        private void FillColorHorizontaly(Grid grid, int iFrom, int jFrom)
        {
            for(int j = jFrom; j < jFrom + requiredLength; j++)
            {
                grid.Matrix[iFrom][j].color = requiredColor;
            }
        }

        private void FindVerticalMatch(Grid grid)
        {
            if (grid.Matrix.Length < 5)
                return;

            for (int j = 0; j < grid.Matrix[0].Length; j++)
            {
                int sum = 0;

                for (int i = 0; i < grid.Matrix.Length; i++)
                {
                    if (requiredLength - sum + i > grid.Matrix.Length)
                    {
                        break;
                    }

                    else if (grid.Matrix[i][j].value != 1 || grid.Matrix[i][j].color != defaultColor)
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
        }

        private void FillColorVertical(Grid grid, int iFrom, int jFrom)
        {
            for (int i = iFrom; i < iFrom + requiredLength; i++)
            {
                grid.Matrix[i][jFrom].color = requiredColor;
            }
        }
    }
}
