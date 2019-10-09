using System.Collections.Generic;
using System.Linq;

namespace Mazes
{
    public class Cell
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        public Cell East { get; set; }
        public Cell West { get; set; }
        public Cell North { get; set; }
        public Cell South { get; set; }

        Dictionary<Cell, bool> links;

        public Cell(int row, int col)
        {
            Row = row;
            Column = col;
            links = new Dictionary<Cell, bool>();
        }

        public Cell Link(Cell cell, bool bidirectional = true)
        {
            links[cell] = true;

            if (bidirectional)
                cell.Link(this, false);

            return this;
        }

        public Cell Unlink(Cell cell, bool bidirectional = true)
        {
            links.Remove(cell);

            if (bidirectional)
                cell.Unlink(this, false);

            return this;
        }

        public List<Cell> Links() => links.Keys.ToList();

        public bool IsLinked(Cell cell)
        {
            if (cell == null)
                return false;

            return links.ContainsKey(cell);
        }

        public virtual List<Cell> Neighbors
        {
            get
            {
                List<Cell> list = new List<Cell>();

                if (North != null) list.Add(North);
                if (South != null) list.Add(South);
                if (East != null) list.Add(East);
                if (West != null) list.Add(West);

                return list;
            }
        }

        public Distances Distances()
        {
            Distances distances = new Distances(this);
            List<Cell> frontier = new List<Cell>() { this };

            while (frontier.Any())
            {
                List<Cell> newFrontier = new List<Cell>();

                foreach (Cell cell in frontier)
                {
                    foreach (Cell linked in cell.Links())
                    {
                        if (distances.ContainsKey(linked))
                            continue;

                        distances.Add(linked, distances[cell] + 1);
                        newFrontier.Add(linked);
                    }
                }

                frontier = newFrontier;
            }

            return distances;
        }
    }
}
