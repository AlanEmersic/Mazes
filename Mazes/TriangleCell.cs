using System.Collections.Generic;
using System.Linq;

namespace Mazes
{
    public class TriangleCell : Cell
    {
        public TriangleCell(int row, int col) : base(row, col)
        {

        }

        public bool IsUpright() => (Row + Column) % 2 == 0;

        public override List<Cell> Neighbors
        {
            get
            {
                List<TriangleCell> list = new List<TriangleCell>();

                if (West != null) list.Add(West as TriangleCell);
                if (East != null) list.Add(East as TriangleCell);
                if (!IsUpright() && North != null) list.Add(North as TriangleCell);
                if (IsUpright() && South != null) list.Add(South as TriangleCell);

                return list.Cast<Cell>().ToList();
            }
        }
    }
}
