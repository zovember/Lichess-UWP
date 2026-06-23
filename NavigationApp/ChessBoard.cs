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

namespace NavigationApp
{
    class Cell
    {
        public Rectangle rect;
        public uint X;
        public uint Y;


        public Cell(string name, uint Size, uint x, uint y, SolidColorBrush Color)
        {
            rect = new Rectangle
            {
                Name = name,
                Width = Size,
                Height = Size,
                Fill = Color
            };

            X = x;
            Y = y;
        }

        public Cell() { }
    }


    class Piece
    {
        public Image SvgImg;
        private uint X { get; set; }
        private uint Y { get; set; }
        private bool alive { get; set; }
        private uint Type { get; }
        private string Location { get; set; }


        public Piece(uint Size, uint x, uint y, Uri uri, uint type, string location)
        {
            SvgImg = new Image
            {
                Width = Size,
                Height = Size,
                Source = new SvgImageSource(uri)
            };

            SvgImg.Tapped += SvgImg_Tapped;

            X = x;
            Y = y;
            alive = true;
            Type = type;
            Location = location;
        }

        public Piece() { }

        private void SvgImg_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (Type == 3)
            {
                SvgImg.Height = 100;
                SvgImg.Width = 100;
            }
        }
    }


    public class ChessBoard
    {
        private static readonly uint displayWidth = DisplayInformation.GetForCurrentView().ScreenWidthInRawPixels;
        //private static uint displayHeight = DisplayInformation.GetForCurrentView().ScreenHeightInRawPixels;
        private static readonly uint Y = displayWidth / 8;
        private static readonly uint Size = displayWidth / 16;

        private Dictionary<string, Rectangle> NamesCells = new Dictionary<string, Rectangle>();
        public Rectangle GetNamesCells(string TKey) { return NamesCells[TKey]; }

        public SolidColorBrush Color1 = new SolidColorBrush(Windows.UI.Colors.White);
        public SolidColorBrush Color2 = new SolidColorBrush(Windows.UI.Colors.LightBlue);

        public Cell[,] Board = new Cell[8, 8];

        uint Type = 0;
        Uri bBishop = new Uri("ms-appx:///Assets/Piece/cardinal/bB.svg");
        Uri wBishop = new Uri("ms-appx:///Assets/Piece/cardinal/wB.svg");

        Uri bKing = new Uri("ms-appx:///Assets/Piece/cardinal/bK.svg");
        Uri wKing = new Uri("ms-appx:///Assets/Piece/cardinal/wK.svg");

        Uri bKnight = new Uri("ms-appx:///Assets/Piece/cardinal/bN.svg");
        Uri wKnight = new Uri("ms-appx:///Assets/Piece/cardinal/wN.svg");

        Uri bPawn = new Uri("ms-appx:///Assets/Piece/cardinal/bP.svg");
        Uri wPawn = new Uri("ms-appx:///Assets/Piece/cardinal/wP.svg");

        Uri bQueen = new Uri("ms-appx:///Assets/Piece/cardinal/bQ.svg");
        Uri wQueen = new Uri("ms-appx:///Assets/Piece/cardinal/wQ.svg");

        Uri bRook = new Uri("ms-appx:///Assets/Piece/cardinal/bR.svg");
        Uri wRook = new Uri("ms-appx:///Assets/Piece/cardinal/wR.svg");


        public async void NewBoard(Canvas canvas)
        {
            uint X = 0;
            uint _Y = Y;

            for (uint i = 0; i < 8; i++)
            {
                for (uint j = 0; j < 8; j++)
                {
                    Uri uri = null;

                    if (i == 1) { uri = bPawn; Pawn pawn = new Pawn() }
                    else if (i == 6) { uri = wPawn; Type = 10; }

                    else if (i == 0)
                    {
                        if (j == 0 || j == 7) { uri = bRook; Type = 6; }
                        else if (j == 1 || j == 6) { uri = bKnight; Type = 3; }
                        else if (j == 2 || j == 5) { uri = bBishop; Type = 1; }
                        else if (j == 3) { uri = bKing; Type = 2; }
                        else { uri = bQueen; Type = 5; }
                    }

                    else if (i == 7)
                    {
                        if (j == 0 || j == 7) { uri = wRook; Type = 12; }
                        else if (j == 1 || j == 6) { uri = wKnight; Type = 9; }
                        else if (j == 2 || j == 5) { uri = wBishop; Type = 7; }
                        else if (j == 3) { uri = wKing; Type = 8; }
                        else { uri = wQueen; Type = 11; }
                    }

                    string nameCell = Functions.GetNameCell(i, j);
                    Cell cell = new Cell(nameCell, Size, X, _Y, (i + j) % 2 == 0 ? Color1 : Color2);
                    Board[i, j] = cell;
                    NamesCells.Add(nameCell, cell.rect);

                    Canvas.SetLeft(cell.rect, X);
                    Canvas.SetTop(cell.rect, _Y);
                    canvas.Children.Add(cell.rect);

                    if (uri != null)
                    {
                        Piece piece;
                        piece = new Piece(Size, X, _Y, uri, Type, nameCell);

                        Canvas.SetZIndex(piece.SvgImg, 0);
                        Canvas.SetLeft(piece.SvgImg, X);
                        Canvas.SetTop(piece.SvgImg, _Y);
                        canvas.Children.Add(piece.SvgImg);
                    }

                    X += Size;
                }

                X = 0;
                _Y += Size;
            }
        }

        public void Move(string begin, string end)
        {

        }        
    }
}
