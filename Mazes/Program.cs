using System;
using System.Drawing;
using System.Diagnostics;
using Mazes.Algorithms;

namespace Mazes
{
    class Program
    {
        static Algorithm algorithm;
        static MazeType mazeType;

        enum Algorithm
        {
            AldousBroder, BinaryTree, HuntAndKill, RecursiveBacktracker, Sidewinder,
            Wilsons, Prims, TruePrims, Kruskals, GrowingTree, RecursiveDivision, Ellers, Houstons
        }

        enum MazeType
        {
            Grid, Polar, Hex, Triangle
        }

        static void Main()
        {
            //ColoredGrid(50);
            NormalGrid(10);
            //PolarGrid(5);
            //HexGrid(5);
            //TriangleGrid(10, 15);
            //RandomMaze(10);
        }

        static void ColoredGrid(int size)
        {
            Stopwatch stopwatch = new Stopwatch();
            Random random = new Random();
            int seed = random.Next(int.MinValue, int.MaxValue);

            ColoredGrid grid = new ColoredGrid(size, size, seed)
            {
                color = Color.Coral,
                IsIntensityOff = true
            };

            algorithm = (Algorithm)random.Next(Enum.GetNames(typeof(Algorithm)).Length);

            stopwatch.Start();

            ColoredGrid maze = (ColoredGrid)RandomAlgorithm<Grid, Cell>(grid, algorithm);
            Cell start = maze.GetCell(0, 0);

            //longest path in maze            
            Distances distances = start.Distances();
            var newStart = distances.Maximum();

            var newDistances = newStart.Key.Distances();
            maze.Start = newStart.Key;
            var goal = newDistances.Maximum();
            maze.End = goal.Key;
            maze.distances = newDistances.PathTo(goal.Key);

            Bitmap img = maze.ToPNG(50);
            string name = "Maze.png";
            img.Save(name);

            Process process = new Process();
            process.StartInfo.FileName = name;
            process.Start();
            process.Close();
            stopwatch.Stop();

            Console.WriteLine(algorithm);
            Console.WriteLine(stopwatch.ElapsedMilliseconds / (float)1000 + " seconds");

        }

        static void RandomMaze(int size)
        {
            Random random = new Random();
            int seed = random.Next(int.MinValue, int.MaxValue);
            algorithm = (Algorithm)random.Next(Enum.GetNames(typeof(Algorithm)).Length);
            Console.WriteLine(algorithm);
            Grid grid = RandomMazeType<Grid>(size, seed);

            if (algorithm != Algorithm.BinaryTree && algorithm != Algorithm.Ellers && algorithm != Algorithm.Kruskals && algorithm != Algorithm.RecursiveDivision && algorithm != Algorithm.Sidewinder)
                switch (mazeType)
                {
                    case MazeType.Grid:
                        grid = RandomAlgorithm<Grid, Cell>(grid, algorithm);
                        break;
                    case MazeType.Polar:
                        grid = RandomAlgorithm<PolarGrid, PolarCell>(grid, algorithm);
                        break;
                    case MazeType.Hex:
                        grid = RandomAlgorithm<HexGrid, HexCell>(grid, algorithm);
                        break;
                    case MazeType.Triangle:
                        grid = RandomAlgorithm<TriangleGrid, TriangleCell>(grid, algorithm);
                        break;
                }
            else
            {
                grid = new Grid(size, size, seed);
                grid = RandomAlgorithm<Grid, Cell>(grid, algorithm);
            }

            Bitmap img = grid.ToPNG(50);
            string name = "Maze.png";
            img.Save(name);

            Process process = new Process();
            process.StartInfo.FileName = name;
            process.Start();
            process.Close();
        }

        static void NormalGrid(int size)
        {
            Random random = new Random();
            int seed = random.Next(int.MinValue, int.MaxValue);
            Grid grid = new Grid(size, size, seed);
            Houstons.CreateMaze<Grid, Cell>(grid);
            Bitmap img = grid.ToPNG(50);
            string name = "Maze.png";
            img.Save(name);

            Process process = new Process();
            process.StartInfo.FileName = name;
            process.Start();
            process.Close();
        }

        static void PolarGrid(int size)
        {
            Random random = new Random();
            int seed = random.Next(int.MinValue, int.MaxValue);
            PolarGrid grid = new PolarGrid(size, seed);
            Houstons.CreateMaze<PolarGrid, PolarCell>(grid);
            Bitmap img = grid.ToPNG(50);
            string name = "Maze.png";
            img.Save(name);

            Process process = new Process();
            process.StartInfo.FileName = name;
            process.Start();
            process.Close();
        }

        static void HexGrid(int size)
        {
            Random random = new Random();
            int seed = random.Next(int.MinValue, int.MaxValue);
            HexGrid grid = new HexGrid(size, size, seed);
            RecursiveBacktracker.CreateMaze<HexGrid, HexCell>(grid);

            Bitmap img = grid.ToPNG(50);
            string name = "Maze.png";
            img.Save(name);

            Process process = new Process();
            process.StartInfo.FileName = name;
            process.Start();
            process.Close();
        }

        static void TriangleGrid(int rows, int cols)
        {
            Random random = new Random();
            int seed = random.Next(int.MinValue, int.MaxValue);
            TriangleGrid grid = new TriangleGrid(rows, cols, seed);
            RecursiveBacktracker.CreateMaze<TriangleGrid, TriangleCell>(grid);

            Bitmap img = grid.ToPNG(50);
            string name = "Maze.png";
            img.Save(name);

            Process process = new Process();
            process.StartInfo.FileName = name;
            process.Start();
            process.Close();
        }

        static G RandomAlgorithm<G, T>(Grid grid, Algorithm algorithm) where G : Grid where T : Cell
        {
            switch (algorithm)
            {
                case Algorithm.AldousBroder: return AldousBroder.CreateMaze<G, T>(grid as G);

                case Algorithm.BinaryTree: return BinaryTree.CreateMaze(grid) as G;

                case Algorithm.HuntAndKill: return HuntAndKill.CreateMaze<G, T>(grid as G);

                case Algorithm.RecursiveBacktracker: return RecursiveBacktracker.CreateMaze<G, T>(grid as G);

                case Algorithm.Sidewinder: return Sidewinder.CreateMaze(grid) as G;

                case Algorithm.Wilsons: return Wilsons.CreateMaze<G, T>(grid as G);

                case Algorithm.Kruskals: return Kruskals.CreateMaze(grid) as G;

                case Algorithm.Prims: return Prims.CreateMaze<G, T>(grid as G);

                case Algorithm.TruePrims: return TruePrims.CreateMaze<G, T>(grid as G);

                case Algorithm.GrowingTree: return GrowingTree.CreateMaze<G, T>(grid as G);

                case Algorithm.RecursiveDivision: return RecursiveDivision.CreateMaze(grid) as G;

                case Algorithm.Ellers: return Ellers.CreateMaze(grid) as G;

                case Algorithm.Houstons: return Houstons.CreateMaze<G, T>(grid as G);
            }
            return null;
        }

        static G RandomMazeType<G>(int size, int seed) where G : Grid
        {
            Random random = new Random(seed);
            mazeType = (MazeType)random.Next(Enum.GetNames(typeof(MazeType)).Length);
            Console.WriteLine(mazeType);
            switch (mazeType)
            {
                case MazeType.Grid: return new Grid(size, size, seed) as G;
                case MazeType.Polar: return new PolarGrid(size, seed) as G;
                case MazeType.Hex: return new HexGrid(size, size, seed) as G;
                case MazeType.Triangle: return new TriangleGrid(size, size + 5, seed) as G;
            }

            return null;
        }
    }
}