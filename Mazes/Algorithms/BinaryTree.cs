using System.Collections.Generic;

namespace Mazes.Algorithms
{
    public class BinaryTree
    {
        public static Grid CreateMaze(Grid grid)
        {
            foreach (Cell cell in grid.EachCell())
            {
                List<Cell> neighbors = new List<Cell>();

                if (cell.North != null)
                    neighbors.Add(cell.North);
                if (cell.East != null)
                    neighbors.Add(cell.East);

                int index = grid.Random.Next(0, neighbors.Count);

                if (index < neighbors.Count)
                {
                    Cell neighbor = neighbors[index];
                    cell.Link(neighbor);
                }
            }

            return grid;
        }
    }
}
