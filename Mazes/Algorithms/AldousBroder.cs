namespace Mazes.Algorithms
{
    public class AldousBroder
    {
        public static G CreateMaze<G, T>(G grid) where G : Grid where T : Cell
        {
            T cell = grid.RandomCell() as T;
            int unvisited = grid.Size - 1;

            while (unvisited > 0)
            {
                T neighbor = cell.Neighbors[grid.Random.Next(0, cell.Neighbors.Count)] as T;
                
                if (neighbor.Links().Count == 0)
                {
                    cell.Link(neighbor);
                    unvisited--;
                }
                cell = neighbor;
            }

            return grid;
        }
    }
}
