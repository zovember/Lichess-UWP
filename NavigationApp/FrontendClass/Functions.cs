using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

using System.Windows.Input;

using Windows.Graphics.Display;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;

namespace NavigationApp
{   
    public class Functions
    {
        public static string GetNameCell(uint i, uint j)
        {
            char letter = (char)('a' + j);
            return letter + (8 - i).ToString();
        }
    }
}
