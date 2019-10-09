using System;
using System.Drawing;

namespace Mazes
{
    public class HexGrid : Grid
    {
        public HexGrid(int rows, int cols, int seed) : base(rows, cols, seed)
        {

        }

        protected override void PrepareGrid()
        {
            Size = Rows * Columns;
            Cells = new HexCell[Rows][];

            for (int i = 0; i < Rows; i++)
            {
                Cells[i] = new HexCell[Columns];
                for (int j = 0; j < Columns; j++)
                    Cells[i][j] = new HexCell(i, j);
            }
        }

        protected override void ConfigureCells()
        {
            foreach (HexCell cell in EachCell())
            {
                int row = cell.Row;
                int col = cell.Column;
                int northDiagonal, southDiagonal;

                if (col % 2 == 0)
                {
                    northDiagonal = row - 1;
                    southDiagonal = row;
                }
                else
                {
                    northDiagonal = row;
                    southDiagonal = row + 1;
                }

                cell.NorthWest = this[northDiagonal, col - 1] as HexCell;
                cell.North = this[row - 1, col] as HexCell;
                cell.NorthEast = this[northDiagonal, col + 1] as HexCell;
                cell.SouthWest = this[southDiagonal, col - 1] as HexCell;
                cell.South = this[row + 1, col] as HexCell;
                cell.SouthEast = this[southDiagonal, col + 1] as HexCell;
            }
        }

        public override Bitmap ToPNG(int cellSize = 100)
        {
            float aSize = cellSize / 2.0f;
            float bSize = (float)(cellSize * Math.Sqrt(3) / 2.0f);    
            float height = bSize * 2;
            //includeBackgrounds = true;
            int imgWidth = (int)(3 * aSize * Columns + aSize + 0.5f);
            int imgHeight = (int)(height * Rows + bSize + 0.5f);

            Brush background = Brushes.White;
            float penWidth = cellSize * 0.1f;
            Pen wall = new Pen(Brushes.Black, penWidth);

            Bitmap mazeImg = new Bitmap(imgWidth + 1, imgHeight + 1);

            using (var graphics = Graphics.FromImage(mazeImg))
            {
                graphics.FillRectangle(background, 0, 0, imgWidth, imgHeight);

                foreach (HexCell cell in EachCell())
                {
                    float cx = cellSize + 3 * cell.Column * aSize;
                    float cy = bSize + cell.Row * height;
                    if (cell.Column % 2 != 0)
                        cy += bSize;

                    int xFW = (int)(cx - cellSize);
                    int xNW = (int)(cx - aSize);
                    int xNE = (int)(cx + aSize);
                    int xFE = (int)(cx + cellSize);

                    int yN = (int)(cy - bSize);
                    int yM = (int)cy;
                    int yS = (int)(cy + bSize);

                    if (includeBackgrounds)
                    {
                        Color color = BackgroundColor(cell);

                        PointF point1 = new PointF(xFW, yM);
                        PointF point2 = new PointF(xNW, yN);
                        PointF point3 = new PointF(xNE, yN);
                        PointF point4 = new PointF(xFE, yM);
                        PointF point5 = new PointF(xNE, yS);
                        PointF point6 = new PointF(xNW, yS);

                        PointF[] points = new PointF[] { point1, point2, point3, point4, point5, point6 };

                        //if (cell == Start)
                        //    color = CellStartEndColor(cell);
                        //else if (cell == End)
                        //    color = CellStartEndColor(cell);

                        Brush brush = new SolidBrush(color);
                        graphics.DrawPolygon(wall, points);
                    }

                    if (cell.SouthWest == null)
                        graphics.DrawLine(wall, xFW, yM, xNW, yS);
                    if (cell.NorthWest == null)
                        graphics.DrawLine(wall, xFW, yM, xNW, yN);
                    if (cell.North == null)
                        graphics.DrawLine(wall, xNW, yN, xNE, yN);

                    if (!cell.IsLinked(cell.NorthEast))
                        graphics.DrawLine(wall, xNE, yN, xFE, yM);
                    if (!cell.IsLinked(cell.SouthEast))
                        graphics.DrawLine(wall, xFE, yM, xNE, yS);
                    if (!cell.IsLinked(cell.South))
                        graphics.DrawLine(wall, xNE, yS, xNW, yS);
                }
            }

            return mazeImg;
        }
    }
}
