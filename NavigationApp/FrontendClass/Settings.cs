using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.Graphics.Display;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;

namespace NavigationApp.FrontendClass
{
    public static class Settings
    {
        /// Узнаём ширину экрана в физ. пикселях посредством умножения количества физ. пикселей в одном лог. пикселе на количество лог. пикселей. Один лог. пиксель - 1/96 дюйма.
        /// Улавливаешь иронию?
        public static readonly double DisplayWidth = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel * Window.Current.Bounds.Width;

        public static readonly SolidColorBrush WHITE_Color = new SolidColorBrush(Windows.UI.Colors.White); // "Белый" (шахматная доска)
        public static readonly SolidColorBrush BLACK_Color = new SolidColorBrush(Windows.UI.Colors.LightBlue); // "Чёрный" (шахматная доска)
        public static readonly SolidColorBrush HIGHLIGHT_Color = new SolidColorBrush(Windows.UI.Colors.DeepSkyBlue); // Любое выделение на доске
    }
}
