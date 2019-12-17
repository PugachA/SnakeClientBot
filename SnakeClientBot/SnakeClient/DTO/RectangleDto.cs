using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeClient.DTO
{
    public class RectangleDto
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public RectangleDto TransformForView(int rectangleSize, int margin)
        {
            this.X = ParseCoordinate(this.X, rectangleSize, margin);
            this.Y = ParseCoordinate(this.Y, rectangleSize, margin);
            this.Width = ParseCoordinate(this.Width, rectangleSize, margin);
            this.Height = ParseCoordinate(this.Height, rectangleSize, margin);

            return this;
        }

        private int ParseCoordinate(int coordinate, int rectangleSize, int margin) => coordinate * (rectangleSize + margin);
    }
}
