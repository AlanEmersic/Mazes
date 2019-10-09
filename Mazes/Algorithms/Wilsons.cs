using System.Collections.Generic;
using System.Linq;

namespace Mazes.Algorithms
{
    public class Wilsons
    {
        public static G CreateMaze<G, T>(G grid) where G : Grid where T : Cell
        {
            List<T> unvisted = new List<T>();

            foreach (T cell in grid.EachCell())
                unvisted.Add(cell);

            T first = unvisted[grid.Random.Next(0, unvisted.Count)];
            unvisted.Remove(first);

            while (unvisted.Any())
            {
                T cell = unvisted[grid.Random.Next(0, unvisted.Count)];
                List<T> path = new List<T> { cell };

                while (unvisted.Contains(cell))
                {
                    cell = cell.Neighbors[grid.Random.Next(0, cell.Neighbors.Count)] as T;
                    int position = path.IndexOf(cell);

                    if (position >= 0)
                    {
                        path = path.Take(position + 1).ToList();
                    }
                    else
                    {
                        path.Add(cell);
                    }
                }

                for (int index = 0; index < path.Count - 1; index++)
                {
                    path[index].Link(path[index + 1]);
                    unvisted.Remove(path[index]);
                }
            }

            return grid;
        }
    }
}