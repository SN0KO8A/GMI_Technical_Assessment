using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GMI_Technical_Assessment.Code
{
    internal static class GridLoader
    {
        public static Grid LoadFromFile(string fileName)
        {
            if(!File.Exists(fileName))
            {
                Console.WriteLine($"ERROR - File {fileName} doesn't exist");
                return null;
            }

            string[] rows = File.ReadAllLines(fileName);

            if (rows.Length == 0)
            {
                Console.WriteLine($"ERROR - File {fileName} doesn't have content or broken");
                return null;
            }

            int rowsLength = rows.Length;
            int columnsLength = rows[0].Split(' ').Length;
            int[][] gridMatrix = new int[rowsLength][];


            for (int i = 0; i < gridMatrix.Length; i++)
            {
                string[] currentRow = rows[i].Split(' ');

                gridMatrix[i] = new int[currentRow.Length];

                for (int j = 0; j < currentRow.Length; j++)
                {
                    if(currentRow[j] == "x")
                    {
                        gridMatrix[i][j] = -1;
                    }
                    else
                    {
                        gridMatrix[i][j] = int.Parse(currentRow[j]);
                    }
                }
            }

            Grid grid = new Grid(gridMatrix);

            return grid;
        }
    }
}
