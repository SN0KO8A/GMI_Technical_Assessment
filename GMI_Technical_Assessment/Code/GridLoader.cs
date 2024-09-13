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
            int[,] gridMatrix = new int[rowsLength, columnsLength];


            for (int i = 0; i < rowsLength; i++)
            {
                string[] currentRow = rows[i].Split(' ');

                for (int j = 0; j < columnsLength; j++)
                {
                    if(currentRow[j] == "x")
                    {
                        gridMatrix[i,j] = -1;
                    }
                    else
                    {
                        gridMatrix[i,j] = int.Parse(currentRow[j]);
                    }
                }
            }

            Grid grid = new Grid(gridMatrix);

            return grid;
        }

        public static Grid GetRandomized(int height, int width)
        {
            int[,] gridMatrix = new int[height, width];
            Random random = new Random();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    gridMatrix[i,j] = random.Next(0, 2);
                }
            }

            Grid grid = new Grid(gridMatrix);

            return grid;
        }
    }
}
