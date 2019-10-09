using System.Collections.Generic;
using System.Linq;

namespace Mazes.Algorithms
{
    public class RecursiveBacktracker
    {
        public static G CreateMaze<G, T>(G grid) where G : Grid where T : Cell
        {
            T start = grid.RandomCell() as T;
            Stack<T> stack = new Stack<T>();
            stack.Push(start);

            while (stack.Any())
            {
                T current = stack.Peek();
                List<T> neighbors = current.Neighbors.Where(n => n.Links().Count == 0).Cast<T>().ToList();

                if (neighbors.Count == 0)
                {
                    stack.Pop();
                }
                else
                {
                    T neighbor = neighbors[grid.Random.Next(neighbors.Count)];
                    current.Link(neighbor);
                    stack.Push(neighbor);
                }
            }
            return grid;
        }
    }
}
