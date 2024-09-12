using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMI_Technical_Assessment.Code
{
    internal class Grid
    {
        private int[][] gridMaxtrix;

        public int[][] GridMatrix => gridMaxtrix;

        public Grid(int[][] grid)
        {
            this.gridMaxtrix = grid;
        }

        public override string ToString()
        {
            string gridString = string.Empty;

            for (int i = 0; i < gridMaxtrix.Length; i++)
            {
                for (int j = 0; j < gridMaxtrix[i].Length; j++)
                {
                    gridString += " " + gridMaxtrix[i][j].ToString();
                }
                gridString += "\n";
            }

            return gridString;
        }
    }
}
