using System.Collections.Generic;
using System.Linq;

namespace Mazes.Algorithms
{
    public class Kruskals
    {
        private class State
        {
            public List<KeyValuePair<Cell, Cell>> Neighbors { get; private set; }
            Dictionary<Cell, int> setForCell;
            Dictionary<int, List<Cell>> cellsInSet;

            public State(Grid grid)
            {
                Neighbors = new List<KeyValuePair<Cell, Cell>>();
                setForCell = new Dictionary<Cell, int>();
                cellsInSet = new Dictionary<int, List<Cell>>();

                foreach (Cell cell in grid.EachCell())
                {
                    int set = setForCell.Count;
                    setForCell[cell] = set;
                    cellsInSet[set] = new List<Cell>() { cell };
                    
                    if (cell.South != null)
                    {
                        Neighbors.Add(new KeyValuePair<Cell, Cell>(cell, cell.South));
                    }
                    if (cell.East != null)
                    {
                        Neighbors.Add(new KeyValuePair<Cell, Cell>(cell, cell.East));
                    }
                }
            }

            public bool CanMerge(Cell left, Cell right)
            {
                return setForCell[left] != setForCell[right];
            }

            public void Merge(Cell left, Cell right)
            {
                left.Link(right);

                int winner = setForCell[left];
                int loser = setForCell[right];
                List<Cell> losers = new List<Cell>();

                if (cellsInSet[loser] != null)
                    losers = cellsInSet[loser];
                else
                    losers.Add(right);

                foreach (Cell cell in losers)
                {
                    cellsInSet[winner].Add(cell);
                    setForCell[cell] = winner;
                }

                cellsInSet.Remove(loser);
            }
        }

        public static Grid CreateMaze(Grid grid)
        {
            State state = new State(grid);
            List<KeyValuePair<Cell, Cell>> neighbors = state.Neighbors.OrderBy(e => grid.Random.Next(0, state.Neighbors.Count)).ToList();

            while (neighbors.Any())
            {
                Cell left = neighbors[neighbors.Count - 1].Key;
                Cell right = neighbors[neighbors.Count - 1].Value;
                neighbors.Remove(neighbors[neighbors.Count - 1]);

                if (state.CanMerge(left, right))
                {
                    state.Merge(left, right);
                }
            }

            return grid;
        }
    }
}
