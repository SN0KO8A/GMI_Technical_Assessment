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
        static void Main(string[] args)
        {
            const ConsoleColor DEFAULT_DIGIT_COLOR = ConsoleColor.White;
            
            Grid grid = GridLoader.LoadFromFile("test.txt");
            grid.SetColor(DEFAULT_DIGIT_COLOR);

            MatchFormations multicolorFormation = new MatchFormations
                (
                    "Multicolor",
                    ConsoleColor.Blue,

                    new MatchRule[]
                    {
                        new StraightLineOfFive(DEFAULT_DIGIT_COLOR),
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
                unmatchedFormation,
            });

            if (grid != null)
            {
                grid.DisplayMatrix();
                gridAnalyzer.Analyze(grid);
                Console.WriteLine("");
                grid.DisplayMatrix();
                Console.WriteLine("");
                gridAnalyzer.DisplayResult();
            }
        }
    }
}
