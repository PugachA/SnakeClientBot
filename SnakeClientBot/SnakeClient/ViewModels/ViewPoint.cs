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

        public int X { get; set; }
        public int Y { get; set; }
        public int RectangleSize { get; set; }
        public int Margin { get; set; }
    }
}
