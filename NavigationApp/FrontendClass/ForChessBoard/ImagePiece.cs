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
    public class ImagePiece
    {
        public Image Img;
        public Rectangle Location;
        public byte _PieceType { get; }
        public byte _Color { get; }
        public byte Square;

        public delegate void Tapped(ImagePiece piece);
        public event Tapped ImagePiece_Tapped;


        public ImagePiece(uint size, byte color, byte Type, byte index, Rectangle cell)
        {
            Uri uri = null;
            switch (color)
            {
                case Color.White:
                    switch (Type)
                    {
                        case PieceType.Pawn: uri = new Uri("ms-appx:///Assets/Piece/cardinal/PNG/wP.png"); break;
                        case PieceType.Knight: uri = new Uri("ms-appx:///Assets/Piece/cardinal/PNG/wN.png"); break;
                        case PieceType.Bishop: uri = new Uri("ms-appx:///Assets/Piece/cardinal/PNG/wB.png"); break;
                        case PieceType.Rook: uri = new Uri("ms-appx:///Assets/Piece/cardinal/PNG/wR.png"); break;
                        case PieceType.Queen: uri = new Uri("ms-appx:///Assets/Piece/cardinal/PNG/wQ.png"); break;
                        case PieceType.King: uri = new Uri("ms-appx:///Assets/Piece/cardinal/PNG/wK.png"); break;
                    }
                    break;

                case Color.Black:
                    switch (Type)
                    {
                        case PieceType.Pawn: uri = new Uri("ms-appx:///Assets/Piece/cardinal/PNG/bP.png"); break;
                        case PieceType.Knight: uri = new Uri("ms-appx:///Assets/Piece/cardinal/PNG/bN.png"); break;
                        case PieceType.Bishop: uri = new Uri("ms-appx:///Assets/Piece/cardinal/PNG/bB.png"); break;
                        case PieceType.Rook: uri = new Uri("ms-appx:///Assets/Piece/cardinal/PNG/bR.png"); break;
                        case PieceType.Queen: uri = new Uri("ms-appx:///Assets/Piece/cardinal/PNG/bQ.png"); break;
                        case PieceType.King: uri = new Uri("ms-appx:///Assets/Piece/cardinal/PNG/bK.png"); break;
                    }
                    break;
            }

            Img = new Image
            {
                Width = size,
                Height = size,
                Source = new BitmapImage(uri)
            };

            Img.Tapped += SvgImg_Tapped;

            _Color = color;
            _PieceType = Type;
            Square = index;
            Location = cell;
        }

        public ImagePiece(ImagePiece imagePiece, Rectangle rect)
        {
            Img = new Image
            {
                Width = imagePiece.Img.Width,
                Height = imagePiece.Img.Height,
                Source = imagePiece.Img.Source
            };

            Img.Tapped += SvgImg_Tapped;

            Location = rect;
            _PieceType = imagePiece._PieceType;
            _Color = imagePiece._Color;
            Square = imagePiece.Square;
        }

        public ImagePiece() { }

        private void SvgImg_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ImagePiece_Tapped?.Invoke(this);
        }
    }

    /*public class King : Piece
    {
        public string[] PossibleMoves;

        public King(uint size, uint x, uint y, Uri uri, bool color, Cell loc) : base(size, x, y, uri, color, loc) { }

        public override List<Cell> GetPossibleMoves(Cell[,] Board)
        {
            return new List<Cell>();
        }
    }*/

    /*public class Knight : Piece
    {
        public string[] PossibleMoves;

        public Knight(uint size, uint x, uint y, Uri uri, bool color, Cell loc) : base(size, x, y, uri, color, loc) { }

        public override List<Cell> GetPossibleMoves(Cell[,] Board)
        {
            return new List<Cell>();
        }
    }*/

    /*public class Pawn : Piece
    {
        public string[] PossibleMoves;

        public Pawn(uint size, uint x, uint y, Uri uri, bool color, Cell loc) : base(size, x, y, uri, color, loc) { }

        public override List<Cell> GetPossibleMoves(Cell[,] Board)
        {
            return new List<Cell>();
        }
    }*/

    /*public class Queen : Piece
    {
        public string[] PossibleMoves;

        public Queen(uint size, uint x, uint y, Uri uri, bool color, Cell loc) : base(size, x, y, uri, color, loc) { }

        public override List<Cell> GetPossibleMoves(Cell[,] Board)
        {
            return new List<Cell>();
        }
    }*/

    /*public class Rook : Piece
    {
        public string[] PossibleMoves;

        public Rook(uint size, uint x, uint y, Uri uri, bool color, Cell loc) : base(size, x, y, uri, color, loc) { }

        public override List<Cell> GetPossibleMoves(Cell[,] Board)
        {
            return new List<Cell>();
        }
    }*/
}
