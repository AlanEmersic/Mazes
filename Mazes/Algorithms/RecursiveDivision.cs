namespace Mazes.Algorithms
{
    public class RecursiveDivision
    {
        public static Grid CreateMaze(Grid grid)
        {
            foreach (var cell in grid.EachCell())
                cell.Neighbors.ForEach(n => cell.Link(n, false));

            Divide(0, 0, grid.Rows, grid.Columns, grid);

            return grid;
        }

        static void Divide(int row, int column, int height, int width, Grid grid)
        {
            if (height <= 1 || width <= 1)  //  || height < 5 && width < 5 && Random.Range(0, 4) == 0 for rooms            
                return;
            if (height > width)
                DivideHorizontally(row, column, height, width, grid);
            else
                DivideVertically(row, column, height, width, grid);
        }

        static void DivideHorizontally(int row, int column, int height, int width, Grid grid)
        {
            int divideSouthOf = grid.Random.Next(0, height - 1);
            int passageAt = grid.Random.Next(0, width);

            for (int x = 0; x < width; x++)
            {
                if (passageAt == x)
                    continue;

                Cell cell = grid.Cells[row + divideSouthOf][column + x];
                cell.Unlink(cell.South);
            }

            Divide(row, column, divideSouthOf + 1, width, grid);
            Divide(row + divideSouthOf + 1, column, height - divideSouthOf - 1, width, grid);
        }

        static void DivideVertically(int row, int column, int height, int width, Grid grid)
        {
            int divideEastOf = grid.Random.Next(0, width - 1);
            int passageAt = grid.Random.Next(0, height);

            for (int y = 0; y < height; y++)
            {
                if (passageAt == y)
                    continue;

                Cell cell = grid.Cells[row + y][column + divideEastOf];
                cell.Unlink(cell.East);
            }

            Divide(row, column, height, divideEastOf + 1, grid);
            Divide(row, column + divideEastOf + 1, height, width - divideEastOf - 1, grid);
        }
    }
}
