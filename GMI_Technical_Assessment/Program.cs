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
            Grid grid = GridLoader.LoadFromFile("test.txt");

            if (grid != null)
            {
                grid.DisplayMatrix();
            }
        }
    }
}
