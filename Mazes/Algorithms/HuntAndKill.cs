using System.Collections.Generic;
using System.Linq;

namespace Mazes.Algorithms
{
    public class HuntAndKill
    {
        public static G CreateMaze<G, T>(G grid) where G : Grid where T : Cell
        {
            T current = grid.RandomCell() as T;

            while (current != null)
            {
                List<T> unvistedNeighbors = current.Neighbors.Where(e => e.Links().Count == 0).Cast<T>().ToList();

                if (unvistedNeighbors.Any())
                {
                    T neighbor = unvistedNeighbors[grid.Random.Next(0, unvistedNeighbors.Count)];
                    current.Link(neighbor);
                    current = neighbor;
                }
                else
                {
                    current = null;

                    foreach (T cell in grid.EachCell())
                    {
                        List<T> vistedNeighbors = cell.Neighbors.Where(e => e.Links().Any()).Cast<T>().ToList();

                        if (cell.Links().Count == 0 && vistedNeighbors.Any())
                        {
                            current = cell;
                            T neighbor = vistedNeighbors[grid.Random.Next(0, vistedNeighbors.Count)];
                            current.Link(neighbor);
                            break;
                        }
                    }
                }
            }
            return grid;
        }
    }
}
