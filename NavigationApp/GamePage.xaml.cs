using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Newtonsoft.Json.Linq;

using Windows.UI.Xaml.Shapes;

using NavigationApp.FrontendClass;
using NavigationApp.Bitboards;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace NavigationApp
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        public GamePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
                //NewGame((gameInfo)e.Parameter);
                //GameInfo game = (gameInfo)e.Parameter;
                NewGame("");
        }

        public void NewGame(string jsonObj)
        {
            Position position = new Position("4r1k1/p4pp1/2n2n1B/2b5/N3Q3/P2q1N2/1r4PP/R4R1K", 255, false, false, false, false, 0); 
            ChessBoard chessBoard = new ChessBoard(CanvasBoard, position);
            ///chessBoard.GetNamesCells("a3").Fill = new SolidColorBrush(Windows.UI.Colors.Red);
            ///4r1k1/p4pp1/2n2n1B/2b5/N6Q/P2q1N2/1r4PP/R4R1K
            ///4r1k1/p4pp1/7B/2bpn3/N1rQk3/P1nq1N2/6PP/R4R1K
        }
    }
}
