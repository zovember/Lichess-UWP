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
using Windows.UI.Xaml.Input;


namespace NavigationApp.FrontendClass.ForChessBoard
{
    public class CircleMove
    {
        public Ellipse Circle { get; }
        public uint X { get; }
        public uint Y { get; }
        public byte IndexCell { get; }

        public delegate void Tapped(CircleMove circle);
        public event Tapped CircleMove_Tapped;


        public CircleMove(uint X, uint Y, double Size, byte CellIndex)
        {
            Circle = new Ellipse
            {
                Fill = Settings.HIGHLIGHT_Color,
                Width = Size,
                Height = Size
            };

            Circle.Tapped += Circle_Tapped;

            this.X = X;
            this.Y = Y;
            this.IndexCell = CellIndex;
        }

        public CircleMove() { }

        private void Circle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CircleMove_Tapped?.Invoke(this);
        }
    }
}
