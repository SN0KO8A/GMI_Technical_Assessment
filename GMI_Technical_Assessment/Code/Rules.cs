﻿using System;
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

        public ConsoleColor RequiredColor { get { return requiredColor; } set { requiredColor = value; } }
        public ConsoleColor MatrixColor { get { return matrixColor; } set { matrixColor = value; } }

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
                for (int j = 0; j < grid.Matrix.GetLength(1); j++)
                {
                    if (grid.Matrix[i, j].value != 1 || grid.Matrix[i, j].color != matrixColor)
                        continue;

                    grid.Matrix[i, j].color = requiredColor;
                    matchesCount++;
                }
            }

            Console.WriteLine($"Debug -> Unmatched - {matchesCount}");
            return matchesCount;
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
            if (shapeSide == TShapeSide.Top || shapeSide == TShapeSide.Buttom)
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

    public class ShapeRule : MatchRule
    {
        private string name = "test";
        private int[,] pattern;

        public ShapeRule(ConsoleColor matrixColor, string name, int[,] pattern) : base(matrixColor)
        {
            this.pattern = pattern;
            this.name = name;
        }

        public override int FindMatches(Grid grid)
        {
            int matches = 0;

            bool isPatternCube = pattern.GetLength(0) == pattern.GetLength(1);
            bool hasPatternZero = HasPatternZero();

            if (pattern.GetLength(0) <= grid.Matrix.GetLength(0) && pattern.GetLength(1) <= grid.Matrix.GetLength(1))
                matches += FindHorizontalMatches(grid, hasPatternZero);

            if (pattern.GetLength(0) <= grid.Matrix.GetLength(1) && pattern.GetLength(1) <= grid.Matrix.GetLength(0))
                matches += FindVerticalMatches(grid, hasPatternZero);

            Console.WriteLine($"Debug -> {name} - {matches}");

            return matches;
        }

        private bool HasPatternZero()
        {
            for (int i = 0; i < pattern.GetLength(0); i++)
            {
                for (int j = 0; j < pattern.GetLength(1); j++)
                {
                    if (pattern[i,j] == 0)
                        return true;
                }
            }

            return false;
        }

        private int FindHorizontalMatches(Grid grid, bool flipCheck)
        {
            int matches = 0;

            for (int i = 0; i < grid.Matrix.GetLength(0); i++)
            {
                if (i + pattern.GetLength(0) > grid.Matrix.GetLength(0))
                    break;

                for (int j = 0; j < grid.Matrix.GetLength(1); j++)
                {
                    if (j + pattern.GetLength(1) > grid.Matrix.GetLength(1))
                    {
                        break;
                    }

                    if (IsHorizontalPatternApplied(grid, i, j, flipCheck, out int flipIndex))
                    {
                        FillHorizontalShape(grid, i, j, flipIndex);
                        matches++;

                        j += pattern.GetLength(1) - 1;
                    }
                }
            }

            return matches;
        }

        private int FindVerticalMatches(Grid grid, bool flipCheck)
        {
            int matches = 0;

            for (int i = 0; i < grid.Matrix.GetLength(0); i++)
            {
                if (i + pattern.GetLength(0) > grid.Matrix.GetLength(1))
                    break;

                for (int j = 0; j < grid.Matrix.GetLength(1); j++)
                {
                    if (j + pattern.GetLength(1) > grid.Matrix.GetLength(0))
                    {
                        break;
                    }

                    if (IsVerticalPatternApplied(grid, i, j, flipCheck, out int flipIndex))
                    {
                        FillVerticalShape(grid, i, j, flipIndex);
                        matches++;

                        j += pattern.GetLength(0) - 1;
                    }
                }
            }

            return matches;
        }

        private bool IsHorizontalPatternApplied(Grid grid, int iStart, int jStart, bool flipCheck, out int flipIndex)
        {
            int needFlips = flipCheck ? 4 : 1;

            for (flipIndex = 0; flipIndex < needFlips; flipIndex++)
            {
                bool isPatternAppliable = true;
                int[,] flipedPattern = GetFlipedPattern(flipIndex);

                for (int iPattern = 0, iGrid = iStart; iPattern < flipedPattern.GetLength(0); iPattern++, iGrid++)
                {
                    isPatternAppliable = true;

                    for (int jPattern = 0, jGrid = jStart; jPattern < flipedPattern.GetLength(1); jPattern++, jGrid++)
                    {
                        bool isCellSame = grid.Matrix[iGrid, jGrid].value == flipedPattern[iPattern, jPattern];

                        if (!isCellSame || grid.Matrix[iGrid, jGrid].color != matrixColor)
                        {
                            isPatternAppliable = false;
                            break;
                        }
                    }

                    if (!isPatternAppliable)
                        break;
                }

                if(isPatternAppliable)
                    return true;
            }

            return false;
        }

        private bool IsVerticalPatternApplied(Grid grid, int iStart, int jStart, bool flipCheck, out int flipIndex)
        {
            int needFlips = flipCheck ? 4 : 1;

            for (flipIndex = 0; flipIndex < needFlips; flipIndex++)
            {
                bool isPatternAppliable = true;
                int[,] flipedPattern = GetFlipedPattern(flipIndex);

                for (int jPattern = 0, jGrid = jStart; jPattern < flipedPattern.GetLength(1); jPattern++, jGrid++)
                {
                    isPatternAppliable = true;

                    for (int iPattern = 0, iGrid = iStart; iPattern < flipedPattern.GetLength(0); iPattern++, iGrid++)
                    {
                        bool isCellSame = grid.Matrix[jGrid, iGrid].value == flipedPattern[iPattern, jPattern];

                        if (!isCellSame || grid.Matrix[jGrid, iGrid].color != matrixColor)
                        {
                            isPatternAppliable = false;
                            break;
                        }
                    }

                    if (!isPatternAppliable)
                        break;
                }

                if (isPatternAppliable)
                    return true;
            }

            return false;
        }

        private void FillHorizontalShape(Grid grid, int iStart, int jStart, int flipIndex)
        {
            int[,] flipedPattern = GetFlipedPattern(flipIndex);

            for (int i = 0; i < pattern.GetLength(0); i++)
            {
                for (int j = 0; j < pattern.GetLength(1); j++)
                {
                    if (grid.Matrix[iStart + i, jStart + j].value == 1)
                    {
                        grid.Matrix[iStart + i, jStart + j].color = requiredColor;
                    }
                }
            }
        }

        private void FillVerticalShape(Grid grid, int iStart, int jStart, int flipIndex)
        {
            int[,] flipedPattern = GetFlipedPattern(flipIndex);

            for (int j = 0; j < flipedPattern.GetLength(1); j++)
            {
                for (int i = 0; i < flipedPattern.GetLength(0); i++)
                {
                    if (grid.Matrix[jStart + j, iStart + i].value == 1)
                    {
                        grid.Matrix[jStart + j, iStart + i].color = requiredColor;
                    }
                }
            }
        }

        private int[,] GetFlipedPattern(int flips)
        {
            int[,] flipedPatern = new int[pattern.GetLength(0), pattern.GetLength(1)];

            int iFactor;
            int jFactor;

            switch (flips)
            {
                case 1:
                    iFactor = -1;
                    jFactor = 1;
                    break;
                case 2:
                    iFactor = -1;
                    jFactor = -1;
                    break;
                case 3:
                    iFactor = 1;
                    jFactor = -1;
                    break;
                default:
                case 0:
                    iFactor = 1;
                    jFactor = 1;
                    break;
            }

            if (iFactor == 1 && jFactor == 1)
                return pattern;

            for (int i = 0; i < pattern.GetLength(0); i++)
            {
                for (int j = 0; j < pattern.GetLength(1); j++)
                {
                    int iFliped = iFactor == -1 ? pattern.GetLength(0) - i - 1 : i;
                    int jFliped = jFactor == -1 ? pattern.GetLength(1) - j - 1 : j;

                    flipedPatern[i, j] = pattern[iFliped, jFliped];
                }
            }

            return flipedPatern;
        }
    }
}
