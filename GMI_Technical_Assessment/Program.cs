using GMI_Technical_Assessment.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMI_Technical_Assessment
{
    internal class Program
    {
        private const ConsoleColor DEFAULT_DIGIT_COLOR = ConsoleColor.White;

        static void Main(string[] args)
        {
            TestRun();
            RandomRun();
        }

        private static GridAnalyzer GetCurrentGridAnalyzer()
        {
            MatchFormations multicolorFormation = new MatchFormations
                (
                    "Multicolor",
                    ConsoleColor.Blue,

                    new MatchRule[]
                    {
                        new CrossShape(DEFAULT_DIGIT_COLOR),
                        new TShape(DEFAULT_DIGIT_COLOR),
                        new ShapeRule(
                            DEFAULT_DIGIT_COLOR,
                            "Straight Line of 5",
                            new int[,]
                            {
                                { 1, 1, 1, 1, 1},
                            }),
                    }
                );

            MatchFormations propellerFormation = new MatchFormations
                (
                    "Propeller",
                    ConsoleColor.Red,

                    new MatchRule[]
                    {
                        new ShapeRule(
                            DEFAULT_DIGIT_COLOR,
                            "Double Layer",
                            new int[,]
                            {
                                { 1, 1, 1},
                                { 1, 1, 1},
                            }),
                        new ShapeRule(
                            DEFAULT_DIGIT_COLOR,
                            "2x2 Block",
                            new int[,]
                            {
                                { 1, 1},
                                { 1, 1},
                            }),
                    }
                );

            MatchFormations unmatchedFormation = new MatchFormations
                (
                    "Unmatched",
                    ConsoleColor.Yellow,

                    new MatchRule[]
                    {
                        new UnmatchedRule(DEFAULT_DIGIT_COLOR),
                    }
                );

            GridAnalyzer gridAnalyzer = new GridAnalyzer(new MatchFormations[]
            {
                multicolorFormation,
                propellerFormation,
                unmatchedFormation,
            });

            return gridAnalyzer;
        }

        static void TestRun()
        {
            Grid grid = GridLoader.LoadFromFile("test.txt");
            grid.SetColor(DEFAULT_DIGIT_COLOR);

            GridAnalyzer gridAnalyzer = GetCurrentGridAnalyzer();

            if (grid != null)
            {
                gridAnalyzer.Analyze(grid);
                Console.WriteLine("");
                grid.DisplayMatrix();
                Console.WriteLine("");
                gridAnalyzer.DisplayResult();
            }
        }

        static void RandomRun()
        {
            GridAnalyzer gridAnalyzer = GetCurrentGridAnalyzer();
            int fillPercent = 50;

            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter || keyInfo.Key == ConsoleKey.RightArrow || keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    Console.Clear();

                    if(keyInfo.Key == ConsoleKey.RightArrow)
                    {
                        fillPercent += 5;
                        fillPercent = fillPercent > 100 ? 100 : fillPercent;
                    }

                    if(keyInfo.Key == ConsoleKey.LeftArrow)
                    {
                        fillPercent -= 5;
                        fillPercent = fillPercent < 0 ? 0 : fillPercent;
                    }

                    Grid grid = GridLoader.GetRandomized(17, 17, fillPercent);
                    grid.SetColor(DEFAULT_DIGIT_COLOR);

                    if (grid != null)
                    {
                        gridAnalyzer.Analyze(grid);
                        Console.WriteLine("");
                        grid.DisplayMatrix();
                        Console.WriteLine("");
                        gridAnalyzer.DisplayResult();
                        Console.WriteLine($"Fill percent: {fillPercent}");
                    }
                }

                if(keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }


            }while(true);
        }
    }
}
