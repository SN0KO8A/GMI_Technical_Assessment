using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMI_Technical_Assessment.Code
{
    internal class Grid
    {
        private GridCell[][] gridMaxtrix;

        public GridCell[][] GridMatrix => gridMaxtrix;

        public Grid(int[][] grid)
        {
            gridMaxtrix = new GridCell[grid.Length][];

            for (int i = 0; i < grid.Length; i++)
            {
                gridMaxtrix[i] = new GridCell[grid[i].Length];
                for (int j = 0; j < grid[i].Length; j++)
                {
                    gridMaxtrix[i][j].value = grid[i][j];
                    gridMaxtrix[i][j].color = ConsoleColor.White;
                }
            }
        }

        public void DisplayMatrix()
        {
            string gridString = string.Empty;

            for (int i = 0; i < gridMaxtrix.Length; i++)
            {
                for (int j = 0; j < gridMaxtrix[i].Length; j++)
                {
                    Console.ForegroundColor = gridMaxtrix[i][j].color;
                    Console.Write(" " + gridMaxtrix[i][j].value);
                    Console.ResetColor();
                }

                Console.Write('\n');
            }
        }
    }

    struct GridCell
    {
        public int value;
        public ConsoleColor color;

        public GridCell(int value = 0, ConsoleColor color = ConsoleColor.White)
        {
            this.value = value;
            this.color = color;
        }
    }
}
