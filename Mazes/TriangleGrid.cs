using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazes
{
    public class TriangleGrid : Grid
    {
        public TriangleGrid(int rows, int cols, int seed) : base(rows, cols, seed)
        {

        }

        protected override void PrepareGrid()
        {
            Size = Rows * Columns;
            Cells = new TriangleCell[Rows][];

            for (int i = 0; i < Rows; i++)
            {
                Cells[i] = new TriangleCell[Columns];
                for (int j = 0; j < Columns; j++)
                    Cells[i][j] = new TriangleCell(i, j);
            }
        }

        protected override void ConfigureCells()
        {
            foreach (TriangleCell cell in EachCell())
            {
                int row = cell.Row;
                int col = cell.Column;

                cell.West = this[row, col - 1];
                cell.East = this[row, col + 1];

                if (cell.IsUpright())
                    cell.South = this[row + 1, col];
                else
                    cell.North = this[row - 1, col];
            }
        }

        public override Bitmap ToPNG(int cellSize = 100)
        {
            float height = (float)(cellSize * Math.Sqrt(3) / 2.0f);
            float halfWidth = cellSize / 2.0f;
            float halfHeight = height / 2.0f;

            int imgWidth = (int)(cellSize * (Columns + 1) / 2.0f);
            int imgHeight = (int)(height * Rows);

            Brush background = Brushes.White;
            float penWidth = cellSize * 0.1f;
            Pen wall = new Pen(Brushes.Black, penWidth);

            Bitmap mazeImg = new Bitmap(imgWidth + 1, imgHeight + 1);

            using (var graphics = Graphics.FromImage(mazeImg))
            {
                graphics.FillRectangle(background, 0, 0, imgWidth, imgHeight);

                foreach (TriangleCell cell in EachCell())
                {
                    float cx = halfWidth + cell.Column * halfWidth;
                    float cy = halfHeight + cell.Row * height;

                    int westX = (int)(cx - halfWidth);
                    int midX = (int)cx;
                    int eastX = (int)(cx + halfWidth);

                    int apexY, baseY;

                    if (cell.IsUpright())
                    {
                        apexY = (int)(cy - halfHeight);
                        baseY = (int)(cy + halfHeight);
                    }
                    else
                    {
                        apexY = (int)(cy + halfHeight);
                        baseY = (int)(cy - halfHeight);
                    }

                    if (includeBackgrounds)
                    {
                        Color color = BackgroundColor(cell);

                        if (cell == Start)
                            color = CellStartEndColor(cell);
                        else if (cell == End)
                            color = CellStartEndColor(cell);

                        Brush brush = new SolidBrush(color);
                        //graphics.FillRectangle(brush, x1, y1, x2 - x1, y2 - y1);
                    }

                    if (cell.West == null)
                        graphics.DrawLine(wall, westX, baseY, midX, apexY);

                    if (!cell.IsLinked(cell.East))
                        graphics.DrawLine(wall, eastX, baseY, midX, apexY);

                    bool noSouth = cell.IsUpright() && cell.South == null;
                    bool notLinked = !cell.IsUpright() && !cell.IsLinked(cell.North);

                    if (noSouth || notLinked)
                        graphics.DrawLine(wall, eastX, baseY, westX, baseY);
                }
            }

            return mazeImg;
        }
    }
}
