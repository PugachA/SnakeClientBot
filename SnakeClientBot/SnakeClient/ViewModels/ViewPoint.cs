using SnakeClient.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnakeClient.ViewModels
{
    public class ViewPoint
    {

        public ViewPoint(int x, int y, int rectangleSize, int margin)
        {
            X = x;
            Y = y;
            RectangleSize = rectangleSize;
            Margin = margin;
        }

        public ViewPoint(PointDto point, int rectangleSize, int margin)
        {
            this.RectangleSize = rectangleSize;
            this.Margin = margin;

            this.X = ParseCoordinate(point.X);
            this.Y = ParseCoordinate(point.Y);
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int RectangleSize { get; private set; }
        public int Margin { get; private set; }
        public string Description { get; set; }
        private int ParseCoordinate(int coordinate) => coordinate * (RectangleSize + Margin);
    }
}
