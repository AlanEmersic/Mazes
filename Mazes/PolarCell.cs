using System.Collections.Generic;
using System.Linq;

namespace Mazes
{
    public class PolarCell : Cell
    {
        public PolarCell CW { get; set; }
        public PolarCell CCW { get; set; }
        public PolarCell Inward { get; set; }
        public List<PolarCell> Outward { get; private set; }

        public PolarCell(int row, int col) : base(row, col)
        {
            Outward = new List<PolarCell>();
        }

        public override List<Cell> Neighbors
        {
            get
            {
                List<PolarCell> list = new List<PolarCell>();
                
                if (CW != null) list.Add(CW);
                if (CCW != null) list.Add(CCW);
                if (Inward != null) list.Add(Inward);

                for (int i = 0; i < Outward.Count; i++)
                    list.Add(Outward[i]);

                return list.Cast<Cell>().ToList();
            }
        }
    }
}
