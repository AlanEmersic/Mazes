using System;
using System.Drawing;

namespace Mazes
{
    public class PolarGrid : Grid
    {
        public PolarGrid(int rows, int seed) : base(rows, 1, seed)
        {

        }

        public override Cell this[int row, int col]
        {
            get
            {
                if (row < 0 || row >= Rows || col < 0) return null;
                //if (col < 0 || col >= Cells[row].Length) return null;
                //Console.WriteLine($"col:{col} mod:{col % Cells[row].Length}");                
                return Cells[row][col % Cells[row].Length] as PolarCell;
            }
        }

        protected override void PrepareGrid()
        {
            Cells = new PolarCell[Rows][];
            float rowHeight = 1.0f / Rows;
            Cells[0] = new PolarCell[1] { new PolarCell(0, 0) };
            Size = 1;

            for (int row = 1; row < Rows; row++)
            {
                float radius = (float)row / Rows;
                float circumference = (float)(2 * Math.PI * radius);
                int previousCount = Cells[row - 1].Length;
                float estimatedCellWidth = circumference / previousCount;
                float ratio = (float)Math.Round(estimatedCellWidth / rowHeight);
                int cells = (int)(previousCount * ratio);

                Cells[row] = new PolarCell[cells];
                for (int col = 0; col < cells; col++)
                {
                    Cells[row][col] = new PolarCell(row, col);
                    Size++;
                }
            }
        }

        protected override void ConfigureCells()
        {
            foreach (PolarCell cell in EachCell())
            {
                int row = cell.Row;
                int col = cell.Column;

                if (row > 0)
                {
                    cell.CW = this[row, col + 1] as PolarCell;
                    cell.CCW = this[row, col - 1] as PolarCell;

                    int ratio = Cells[row].Length / Cells[row - 1].Length;
                    PolarCell parent = Cells[row - 1][col / ratio] as PolarCell;
                    parent.Outward.Add(cell);
                    cell.Inward = parent;
                }
            }
        }

        public override Cell RandomCell()
        {
            int row = Random.Next(Rows);
            int col = Random.Next(Cells[row].Length);

            return Cells[row][col] as PolarCell;
        }

        public override Bitmap ToPNG(int cellSize = 100)
        {
            int imgSize = 2 * Rows * cellSize;
            Brush background = Brushes.White;
            float penWidth = cellSize * 0.1f;
            Pen wall = new Pen(Brushes.Black, penWidth);

            Bitmap mazeImg = new Bitmap(imgSize + 1, imgSize + 1);
            int center = imgSize / 2;

            using (var graphics = Graphics.FromImage(mazeImg))
            {
                graphics.FillRectangle(background, 0, 0, imgSize, imgSize);

                #region BG
                //if (includeBackgrounds)
                //{
                //    foreach (var cell in EachCell())
                //    {
                //        int x1 = cell.Column * cellSize;
                //        int y1 = cell.Row * cellSize;
                //        int x2 = (cell.Column + 1) * cellSize;
                //        int y2 = (cell.Row + 1) * cellSize;

                //        Color color = BackgroundColor(cell);

                //        if (cell == Start)
                //            color = CellStartEndColor(cell);
                //        else if (cell == End)
                //            color = CellStartEndColor(cell);

                //        Brush brush = new SolidBrush(color);
                //        graphics.FillRectangle(brush, x1, y1, x2 - x1, y2 - y1);
                //    }
                //}
                #endregion

                foreach (PolarCell cell in EachCell())
                {
                    if (cell.Row == 0) continue;

                    float theta = (float)(2 * Math.PI / Cells[cell.Row].Length);
                    float innerRadius = cell.Row * cellSize;
                    float outerRadius = (cell.Row + 1) * cellSize;
                    float thetaCCW = cell.Column * theta;
                    float thetaCW = (cell.Column + 1) * theta;

                    int ax = center + (int)(innerRadius * Math.Cos(thetaCCW));
                    int ay = center + (int)(innerRadius * Math.Sin(thetaCCW));
                    int bx = center + (int)(outerRadius * Math.Cos(thetaCCW));
                    int by = center + (int)(outerRadius * Math.Sin(thetaCCW));
                    int cx = center + (int)(innerRadius * Math.Cos(thetaCW));
                    int cy = center + (int)(innerRadius * Math.Sin(thetaCW));
                    int dx = center + (int)(outerRadius * Math.Cos(thetaCW));
                    int dy = center + (int)(outerRadius * Math.Sin(thetaCW));

                    if (!cell.IsLinked(cell.Inward))
                        graphics.DrawLine(wall, ax, ay, cx, cy);
                    if (!cell.IsLinked(cell.CW))
                        graphics.DrawLine(wall, cx, cy, dx, dy);
                }

                graphics.DrawEllipse(wall, center - Rows * cellSize, center - Rows * cellSize, Rows * 2 * cellSize, Rows * 2 * cellSize);
            }

            return mazeImg;
        }
    }
}
