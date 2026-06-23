using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Graphics.Display;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;

namespace NavigationApp.FrontendClass.ForChessBoard
{
    public class Cell
    {
        public Rectangle Rect;
        public ImagePiece _ImagePiece = null;
        public byte _Color { get; }
        public uint X { get; }
        public uint Y { get; }
        //public byte Position { get; }
 

        public Cell(uint size, uint x, uint y, byte position, byte color, Brush colorBrush)
        {
            Rect = new Rectangle
            {
                Width = size,
                Height = size,
                Fill = colorBrush
            };

            X = x;
            Y = y;
            //Position = position;
            _Color = color;
        }

        public Cell() { }
    }
}
