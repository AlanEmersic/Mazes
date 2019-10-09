﻿using System.Collections.Generic;
using System.Linq;

namespace Mazes.Algorithms
{
    public class GrowingTree
    {
        public static G CreateMaze<G, T>(G grid) where G : Grid where T : Cell
        {
            T start = grid.RandomCell() as T;
            List<T> active = new List<T>() { start };

            while (active.Any())
            {
                T cell = grid.Random.Next(0, 2) == 0 ? active[active.Count - 1] : active[grid.Random.Next(0, active.Count)]; //recursive backtracker/prims 50/50 split
                List<T> availableNeighbors = cell.Neighbors.Where(e => e.Links().Count == 0).Cast<T>().ToList();

                if (availableNeighbors.Any())
                {
                    T neighbor = availableNeighbors[grid.Random.Next(0, availableNeighbors.Count)];
                    cell.Link(neighbor);
                    active.Add(neighbor);
                }
                else
                    active.Remove(cell);
            }
            return grid;
        }
    }
}
