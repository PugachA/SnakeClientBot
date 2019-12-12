using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnakeClient.Models
{
    public class Size : BindableBase
    {
        private int height;
        private int width;

        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                RaisePropertyChanged(nameof(Height));
            }
        }

        public int Width
        {
            get { return width; }
            set
            {
                width = value;
                RaisePropertyChanged(nameof(Width));
            }
        }
    }
}
