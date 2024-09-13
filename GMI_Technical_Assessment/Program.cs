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

            GridAnalyzer gridAnalyzer = new GridAnalyzer(new MatchRule[]
            {
                new StraightLineOfFive(DEFAULT_DIGIT_COLOR),
                new UnmatchedRule(DEFAULT_DIGIT_COLOR),
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
