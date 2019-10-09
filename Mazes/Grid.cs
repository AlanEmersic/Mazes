using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Mazes
{
    public class Grid
    {
        public int Rows { get; protected set; }
        public int Columns { get; protected set; }
        public int Size { get; protected set; }
        public Cell[][] Cells { get; protected set; }
        public Cell Start { get; set; }
        public Cell End { get; set; }
        public Distances Distances { get; set; }
        public Random Random { get; protected set; }
        protected bool includeBackgrounds = false;

        public Grid(int rows, int cols, int seed)
        {
            Rows = rows;
            Columns = cols;
            Random = new Random(seed);
            
            PrepareGrid();
            ConfigureCells();
        }

        public virtual Cell this[int row, int col]
        {
            get
            {
                if (row < 0 || row >= Rows) return null;
                if (col < 0 || col >= Columns) return null;

                return Cells[row][col];
            }
        }

        protected virtual void PrepareGrid()
        {
            Size = Rows * Columns;
            Cells = new Cell[Rows][];
            
            for (int i = 0; i < Rows; i++)
            {
                Cells[i] = new Cell[Columns];
                for (int j = 0; j < Columns; j++)
                    Cells[i][j] = new Cell(i, j);
            }
        }

        protected virtual void ConfigureCells()
        {            
            foreach (Cell[] cellRow in Cells)
            {
                foreach (Cell cell in cellRow)
                {
                    int row = cell.Row;
                    int col = cell.Column;

                    cell.North = this[row - 1, col];
                    cell.South = this[row + 1, col];
                    cell.West = this[row, col - 1];
                    cell.East = this[row, col + 1];
                }
            }
        }

        public virtual Cell RandomCell()
        {
            int row = Random.Next(Rows);
            int col = Random.Next(Columns);
            
            return Cells[row][col];
        }

        public IEnumerable<Cell[]> EachRow()
        {
            for (int i = 0; i < Rows; i++)
                yield return Cells[i];
        }

        public IEnumerable<Cell> EachCell()
        {
            foreach (Cell[] cellRow in Cells)
                foreach (Cell cell in cellRow)
                    yield return cell;
        }

        public Cell GetCell(int row, int column) => Cells[row][column];

        public virtual Color BackgroundColor(Cell cell) => Color.White;

        public Color CellStartEndColor(Cell cell) => cell == Start ? Color.Blue : Color.Red;

        public virtual Bitmap ToPNG(int cellSize = 100)
        {
            int imgWidth = cellSize * Columns;
            int imgHeight = cellSize * Rows;

            Brush background = Brushes.White;
            float penWidth = cellSize * 0.1f;
            Pen wall = new Pen(Brushes.Black, penWidth);

            Bitmap mazeImg = new Bitmap(imgWidth + 1, imgHeight + 1);

            using (var graphics = Graphics.FromImage(mazeImg))
            {
                graphics.FillRectangle(background, 0, 0, imgWidth, imgHeight);

                if (includeBackgrounds)
                {
                    foreach (var cell in EachCell())
                    {
                        int x1 = cell.Column * cellSize;
                        int y1 = cell.Row * cellSize;
                        int x2 = (cell.Column + 1) * cellSize;
                        int y2 = (cell.Row + 1) * cellSize;

                        Color color = BackgroundColor(cell);

                        if (cell == Start)
                            color = CellStartEndColor(cell);
                        else if (cell == End)
                            color = CellStartEndColor(cell);

                        Brush brush = new SolidBrush(color);
                        graphics.FillRectangle(brush, x1, y1, x2 - x1, y2 - y1);
                    }
                }

                foreach (Cell cell in EachCell())
                {
                    int x1 = cell.Column * cellSize;
                    int y1 = cell.Row * cellSize;
                    int x2 = (cell.Column + 1) * cellSize;
                    int y2 = (cell.Row + 1) * cellSize;

                    if (cell.North == null)
                        graphics.DrawLine(wall, x1, y1, x2, y1);
                    if (cell.West == null)
                        graphics.DrawLine(wall, x1, y1, x1, y2);

                    if (!cell.IsLinked(cell.East))
                        graphics.DrawLine(wall, x2, y1, x2, y2);
                    if (!cell.IsLinked(cell.South))
                        graphics.DrawLine(wall, x1, y2, x2, y2);
                }
            }

            return mazeImg;
        }

        public List<Cell> DeadEnds()
        {
            List<Cell> list = new List<Cell>();

            foreach (Cell cell in EachCell())
                if (cell.Links().Count == 1)
                    list.Add(cell);

            return list;
        }

        public void Braid(int p = 100)
        {
            List<Cell> deadends = DeadEnds().OrderBy(x => Random.Next()).ToList();
            foreach (Cell cell in deadends)
            {
                if (cell.Links().Count != 1 || Random.Next(0, p) > p)
                    continue;

                List<Cell> neighbors = cell.Neighbors.FindAll((Cell n) => !cell.IsLinked(n)).ToList();
                List<Cell> best = neighbors.Where(n => n.Links().Count == 1).ToList();

                if (!best.Any())
                    best = neighbors;

                Cell neighbor = best[Random.Next(0, best.Count)];
                cell.Link(neighbor);
            }
        }
    }
}
