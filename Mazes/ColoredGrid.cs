using System;
using System.Drawing;
using System.Linq;

namespace Mazes
{
    public class ColoredGrid : Grid
    {
        int maximum;
        public Distances distances { get; set; }
        public Color color { get; set; } = Color.Yellow;
        public bool IsIntensityOff { get; set; }

        public ColoredGrid(int rows, int columns, int seed) : base(rows, columns, seed)
        {
            includeBackgrounds = true;
        }

        public override Color BackgroundColor(Cell cell)
        {
            maximum = distances.Values.Max();

            if (distances != null && distances.ContainsKey(cell))
            {
                int distance = distances[cell];
                float intensity = ((float)maximum - (float)distance) / (float)maximum;

                int dark = Convert.ToInt32(255 * intensity);
                int bright = 128 + Convert.ToInt32(127 * intensity);

                //int r = Convert.ToInt32(color.R * (distance / (float)maximum));
                //int g = Convert.ToInt32(color.G * (distance / (float)maximum));
                //int b = Convert.ToInt32(color.B * (distance / (float)maximum));
                ////Console.WriteLine($"r:{r},g:{g},b:{b}");                

                if (IsIntensityOff)
                    return color;
                else
                    return Color.FromArgb(bright, dark, bright);
            }
            else
            {
                return Color.White;
            }
        }
    }
}
