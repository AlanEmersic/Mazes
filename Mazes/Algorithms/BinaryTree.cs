using System;
using System.Collections.Generic;

namespace Mazes.Algorithms
{
    public class BinaryTree
    {
        enum Direction
        {
            NorthWest, NorthEast
        }

        public static Grid CreateMaze(Grid grid)
        {
            Direction direction = (Direction)grid.Random.Next(Enum.GetNames(typeof(Direction)).Length);
            Console.WriteLine(direction);

            foreach (Cell cell in grid.EachCell())
            {
                List<Cell> neighbors = new List<Cell>();

                switch (direction)
                {
                    case Direction.NorthWest:
                        if (cell.North != null)
                            neighbors.Add(cell.North);
                        if (cell.West != null)
                            neighbors.Add(cell.West);
                        break;
                    case Direction.NorthEast:
                        if (cell.North != null)
                            neighbors.Add(cell.North);
                        if (cell.East != null)
                            neighbors.Add(cell.East);
                        break;
                }

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
