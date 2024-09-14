using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            Console.WriteLine($"Debug -> Unmatched - {matchesCount}");
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

            Console.WriteLine($"Debug -> Straght of five - {matchesCount}");
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

    public class CrossShape : MatchRule
    {
        public CrossShape(ConsoleColor matrixColor) : base(matrixColor)
        {
        }

        public override int FindMatches(Grid grid)
        {
            if (grid.Matrix.GetLength(0) % 2 == 0 || grid.Matrix.GetLength(1) % 2 == 0)
            {
                return 0;
            }

            int iCenter = grid.Matrix.GetLength(0) / 2;
            int jCenter = grid.Matrix.GetLength(1) / 2;

            for (int step = 0;; step++)
            {
                int rightSide = jCenter + step;
                int leftSide = jCenter - step;
                int topSide = iCenter + step;
                int bottomSide = iCenter - step;

                if (topSide >= grid.Matrix.GetLength(0) && rightSide >= grid.Matrix.GetLength(1))
                {
                    break;
                }

                if (topSide < grid.Matrix.GetLength(0) && grid.Matrix[topSide, jCenter].value == 0 || grid.Matrix[topSide, jCenter].color != matrixColor)
                {
                    return 0;
                }

                if (bottomSide >= 0 && grid.Matrix[bottomSide, jCenter].value == 0 || grid.Matrix[bottomSide, jCenter].color != matrixColor)
                {
                    return 0;
                }

                if (rightSide < grid.Matrix.GetLength(0) && grid.Matrix[iCenter, rightSide].value == 0 || grid.Matrix[iCenter, rightSide].color != matrixColor)
                {
                    return 0;
                }

                if (leftSide >= 0 && grid.Matrix[iCenter, leftSide].value == 0 || grid.Matrix[iCenter, leftSide].color != matrixColor)
                {
                    return 0;
                }
            }

            FillColorCross(grid, iCenter, jCenter);

            Console.WriteLine($"Debug -> CrossShape - 1");
            return 1;
        }

        private void FillColorCross(Grid grid, int iCenter, int jCenter)
        {
            grid.Matrix[iCenter, jCenter].color = requiredColor;

            for (int step = 0; ; step++)
            {
                int rightSide = jCenter + step;
                int leftSide = jCenter - step;
                int topSide = iCenter + step;
                int bottomSide = iCenter - step;

                if (topSide >= grid.Matrix.GetLength(0) && rightSide >= grid.Matrix.GetLength(1))
                {
                    break;
                }

                if (topSide < grid.Matrix.GetLength(0))
                {
                    grid.Matrix[topSide, jCenter].color = requiredColor;
                }

                if (bottomSide >= 0)
                {
                    grid.Matrix[bottomSide, jCenter].color = requiredColor;
                }

                if (rightSide < grid.Matrix.GetLength(0))
                {
                    grid.Matrix[iCenter, rightSide].color = requiredColor;
                }

                if (leftSide >= 0)
                {
                    grid.Matrix[iCenter, leftSide].color = requiredColor;
                }
            }
        }
    }

    public class TShape : MatchRule
    {
        public TShape(ConsoleColor matrixColor) : base(matrixColor)
        {
        }

        public override int FindMatches(Grid grid)
        {
            if (grid.Matrix.GetLength(0) % 2 == 0 || grid.Matrix.GetLength(1) % 2 == 0)
            {
                return 0;
            }

            int iCenter = grid.Matrix.GetLength(0) / 2;
            int jCenter = grid.Matrix.GetLength(1) / 2;

            bool isHorizontalOut = false;
            bool isVerticalOut = false;

            for (int step = 0; ; step++)
            {
                int rightSide = jCenter + step;
                int leftSide = jCenter - step;
                int topSide = iCenter + step;
                int bottomSide = iCenter - step;

                if (topSide >= grid.Matrix.GetLength(0) && rightSide >= grid.Matrix.GetLength(1) ||
                    isHorizontalOut && isVerticalOut)
                {
                    break;
                }

                if (topSide < grid.Matrix.GetLength(0) && grid.Matrix[topSide, jCenter].value == 0 || grid.Matrix[topSide, jCenter].color != matrixColor ||
                    bottomSide >= 0 && grid.Matrix[bottomSide, jCenter].value == 0 || grid.Matrix[bottomSide, jCenter].color != matrixColor)
                {
                    isVerticalOut = true;
                }

                if (rightSide < grid.Matrix.GetLength(0) && grid.Matrix[iCenter, rightSide].value == 0 || grid.Matrix[iCenter, rightSide].color != matrixColor ||
                    leftSide >= 0 && grid.Matrix[iCenter, leftSide].value == 0 || grid.Matrix[iCenter, leftSide].color != matrixColor)
                {
                    isHorizontalOut = true;
                }
            }

            if (!isVerticalOut && IsHorizontalSidesIsFilled(grid, out bool isBottom))
            {
                TShapeSide shapeSide = isBottom ? TShapeSide.Buttom : TShapeSide.Top;
                FillTShapeColor(grid, shapeSide);

                Console.WriteLine($"Debug -> TShape - 1");
                return 1;
            }

            else if (!isHorizontalOut && IsVerticalSidesIsFilled(grid, out bool isRight))
            {
                TShapeSide shapeSide = isRight ? TShapeSide.Right : TShapeSide.Left;
                FillTShapeColor(grid, shapeSide);

                Console.WriteLine($"Debug -> TShape - 1");
                return 1;
            }

            return 0;
        }

        private bool IsVerticalSidesIsFilled(Grid grid, out bool isRight)
        {
            int rightSide = 1;
            int leftSide = 1;

            int horizontalSize = grid.Matrix.GetLength(1) - 1;

            for (int verticalIndex = 0; verticalIndex < grid.Matrix.GetLength(0); verticalIndex++)
            {
                if (rightSide == 0 && leftSide == 0)
                {
                    isRight = false;
                    return false;
                }

                rightSide *= grid.Matrix[verticalIndex, horizontalSize].value * (grid.Matrix[verticalIndex, horizontalSize].color == matrixColor ? 1 : 0);
                leftSide *= grid.Matrix[verticalIndex, 0].value * (grid.Matrix[verticalIndex, 0].color == matrixColor ? 1 : 0);
            }

            isRight = rightSide == 1;
            return true;
        }

        private bool IsHorizontalSidesIsFilled(Grid grid, out bool isDown)
        {
            int topSide = 1;
            int bottomSide = 1;

            int verticalSize = grid.Matrix.GetLength(0) - 1;

            for (int horizontalIndex = 0; horizontalIndex < grid.Matrix.GetLength(1); horizontalIndex++)
            {
                if (topSide == 0 && bottomSide == 0)
                {
                    isDown = false;
                    return false;
                }

                topSide *= grid.Matrix[0, horizontalIndex].value * (grid.Matrix[0, horizontalIndex].color == matrixColor ? 1 : 0);
                bottomSide *= grid.Matrix[verticalSize, horizontalIndex].value * (grid.Matrix[verticalSize, horizontalIndex].color == matrixColor ? 1 : 0);
            }

            isDown = bottomSide == 1;
            return true;
        }

        private void FillTShapeColor(Grid grid, TShapeSide shapeSide)
        {
            FillSideColor(grid, shapeSide);

            if (shapeSide == TShapeSide.Top || shapeSide == TShapeSide.Buttom)
            {
                FillMiddleVerticalColor(grid);
            }

            else
            {
                FillMiddleHorizontalColor(grid);
            }
        }

        private void FillMiddleVerticalColor(Grid grid)
        {
            int jCenter = grid.Matrix.GetLength(1) / 2;

            for (int i = 0; i < grid.Matrix.GetLength(0); i++)
            {
                grid.Matrix[i, jCenter].color = requiredColor;
            }
        }

        private void FillMiddleHorizontalColor(Grid grid)
        {
            int iCenter = grid.Matrix.GetLength(0) / 2;

            for (int j = 0; j < grid.Matrix.GetLength(1); j++)
            {
                grid.Matrix[iCenter, j].color = requiredColor;
            }
        }

        private void FillSideColor(Grid grid, TShapeSide shapeSide)
        {
            if(shapeSide == TShapeSide.Top || shapeSide == TShapeSide.Buttom)
            {
                int i = shapeSide == TShapeSide.Top ? 0 : grid.Matrix.GetLength(0) - 1;

                for (int j = 0; j < grid.Matrix.GetLength(1); j++)
                {
                    grid.Matrix[i, j].color = requiredColor;
                }
            }

            else
            {
                int j = shapeSide == TShapeSide.Left ? 0 : grid.Matrix.GetLength(1) - 1;

                for (int i = 0; i < grid.Matrix.GetLength(0); i++)
                {
                    grid.Matrix[i, j].color = requiredColor;
                }
            }
        }

        private enum TShapeSide
        {
            Top,
            Buttom,
            Right,
            Left,
        }
    }
}
