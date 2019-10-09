using System;
using System.Collections.Generic;
using System.Linq;

namespace Mazes
{
    public class HexCell : Cell
    {
        public HexCell NorthEast { get; set; }
        public HexCell NorthWest { get; set; }
        public HexCell SouthEast { get; set; }
        public HexCell SouthWest { get; set; }

        public HexCell(int row, int col) : base(row, col)
        {

        }

        public override List<Cell> Neighbors
        {
            get
            {
                List<HexCell> list = new List<HexCell>();

                if (NorthWest != null) list.Add(NorthWest);
                if (North != null) list.Add(North as HexCell);
                if (NorthEast != null) list.Add(NorthEast);

                if (SouthWest != null) list.Add(SouthWest);
                if (South != null) list.Add(South as HexCell);
                if (SouthEast != null) list.Add(SouthEast);

                return list.Cast<Cell>().ToList();
            }
        }
    }
}
