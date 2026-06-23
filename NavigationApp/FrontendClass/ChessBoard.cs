using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using NavigationApp.Bitboards.ForBitboards;
using NavigationApp.Bitboards;

using NavigationApp.FrontendClass.ForChessBoard;

using Bitboard = System.UInt64;
using _ = NavigationApp.CommonConstant; /// Вопросы, предложения, замечения?
//    ^
//    |
// на заметку: 255 означает отсутствие

/* ZIndex
 * 0 - доска
 * 1 - фигуры и всё выделение 
 */

namespace NavigationApp.FrontendClass
{
    public class ChessBoard
    {
        public Cell[] Board = new Cell[64]; // 64 клетки, в двумерном массиве нет смысла
        public Canvas _Canvas; // Храним квас, чтобы в дальнейшем взаимодействовать с ним(P.S. Grid для слабаков)

        public Position _Position; // Битборды

        private static readonly uint StartY = (uint)Settings.DisplayWidth / 8; // Точка левого верхнего угла самой левой верхней клетки по вертикали, посчитанная сверху вниз (справедливо только для вертикальных экранов)
        private static readonly uint Size = (uint)Settings.DisplayWidth / 16; // Размер клетки

        private List<CircleMove> PossibleMovesList = new List<CircleMove>(); // Храним кружочки
        private List<Path> PossibleCapturesList = new List<Path>(); // Храним фиговины
        private byte IndexChooseCell = _.None;


        public ChessBoard(Canvas canvas, Position position)
        {
            _Position = position;
            uint X = 0;
            uint _Y = StartY;

            for (int i = 7; i >= 0; i--)
            {
                for (uint j = 0; j < 8; j++)
                {
                    byte pos = (byte)(i * 8 + j);
                    byte color = ((i + j) % 2 == 0) ? Color.White : Color.Black;
                    SolidColorBrush colorBrush = (color == Color.White) ? Settings.WHITE_Color : Settings.BLACK_Color;

                    Cell cell = new Cell(Size, X, _Y, pos, color, colorBrush);
                    Board[pos] = cell;

                    Canvas.SetLeft(cell.Rect, X);
                    Canvas.SetTop(cell.Rect, _Y);
                    canvas.Children.Add(cell.Rect);

                    if (BitboardOperations.Get_bit(position._Pieces.All, pos)) // Если обнаружена фигура на битборде 
                    {
                        byte Pcolor, Ptype;

                        // Пробиваем по баzе, узнаём всё: цвет, тип, индекс в битборде
                        if (BitboardOperations.Get_bit(position._Pieces.SideBitboards[Color.White], pos))
                        {
                            Pcolor = Color.White;
                            if (BitboardOperations.Get_bit(position._Pieces.PiecesBitboards[Color.White, PieceType.Pawn], pos))Ptype = PieceType.Pawn;
                            else if (BitboardOperations.Get_bit(position._Pieces.PiecesBitboards[Color.White, PieceType.Knight], pos)) Ptype = PieceType.Knight;
                            else if (BitboardOperations.Get_bit(position._Pieces.PiecesBitboards[Color.White, PieceType.Bishop], pos)) Ptype = PieceType.Bishop;
                            else if (BitboardOperations.Get_bit(position._Pieces.PiecesBitboards[Color.White, PieceType.Rook], pos)) Ptype = PieceType.Rook;
                            else if (BitboardOperations.Get_bit(position._Pieces.PiecesBitboards[Color.White, PieceType.Queen], pos)) Ptype = PieceType.Queen;
                            else Ptype = PieceType.King;
                        }
                        else
                        {
                            Pcolor = Color.Black;
                            if (BitboardOperations.Get_bit(position._Pieces.PiecesBitboards[Color.Black, PieceType.Pawn], pos)) Ptype = PieceType.Pawn;
                            else if (BitboardOperations.Get_bit(position._Pieces.PiecesBitboards[Color.Black, PieceType.Knight], pos)) Ptype = PieceType.Knight;
                            else if (BitboardOperations.Get_bit(position._Pieces.PiecesBitboards[Color.Black, PieceType.Bishop], pos)) Ptype = PieceType.Bishop;
                            else if (BitboardOperations.Get_bit(position._Pieces.PiecesBitboards[Color.Black, PieceType.Rook], pos)) Ptype = PieceType.Rook;
                            else if (BitboardOperations.Get_bit(position._Pieces.PiecesBitboards[Color.Black, PieceType.Queen], pos)) Ptype = PieceType.Queen;
                            else Ptype = PieceType.King;
                        }

                        cell._ImagePiece = new ImagePiece(Size, Pcolor, Ptype, pos, cell.Rect);

                        Canvas.SetZIndex(cell._ImagePiece.Img, 1);
                        Canvas.SetLeft(cell._ImagePiece.Img, X);
                        Canvas.SetTop(cell._ImagePiece.Img, _Y);
                        canvas.Children.Add(cell._ImagePiece.Img);

                        cell._ImagePiece.ImagePiece_Tapped += OnSelected; // Подписываем класс картинки на событие тапа хомяка
                    }

                    X += Size;
                }

                X = 0;
                _Y += Size;
            }

            _Canvas = canvas;
        }

        // Выделяем клетку, на которой стоит выбранная фигура
        // Отрисовываем возможные взятия и тихие ходы на доске
        public void OnSelected(ImagePiece piece)
        {
            Clear_PossibleCapturesList();
            Clear_PossibleMovesList();

            if (IndexChooseCell != _.None)
            {
                Board[IndexChooseCell].Rect.Fill = (Board[IndexChooseCell]._Color == Color.White) ? Settings.WHITE_Color : Settings.BLACK_Color;
            }
            piece.Location.Fill = Settings.HIGHLIGHT_Color;
            IndexChooseCell = piece.Square;
            Toaster.Show($"Square: {piece.Square}");

            Bitboard Captures = LegalMoves.Gen_LegalCaptures(_Position._Pieces, piece._PieceType, piece.Square, piece._Color);            
            while (Captures != 0)
            {
                Cell cell = Board[BitboardOperations.Bsf(Captures)];
                PossibleCapturesList.Add( Draw_Capture(cell.X, cell.Y, cell.Rect.Width) );

                Captures &= Captures - 1;
            }

            Bitboard Moves = LegalMoves.Gen_LegalMoves(_Position._Pieces, piece._PieceType, piece.Square, piece._Color);
            while (Moves != 0)
            {
                byte CellIndex = BitboardOperations.Bsf(Moves);
                Cell cell = Board[CellIndex];

                double size = cell.Rect.Width / 3;
                CircleMove circleMove = new CircleMove(cell.X, cell.Y, size, CellIndex);
                PossibleMovesList.Add(circleMove);

                Canvas.SetZIndex(circleMove.Circle, 1);
                Canvas.SetLeft(circleMove.Circle, circleMove.X + size);
                Canvas.SetTop(circleMove.Circle, circleMove.Y + size);
                _Canvas.Children.Add(circleMove.Circle);

                circleMove.CircleMove_Tapped += MakeMove;

                Moves &= Moves - 1;
            }
        }

        // 
        public void MakeMove(CircleMove circleMove)
        {
            //Toaster.Show($"{IndexChooseCell}, {circleMove.IndexCell}");
            Cell From = Board[IndexChooseCell], 
                 To = Board[circleMove.IndexCell];

            Move move = new Move
            (
                IndexChooseCell,
                circleMove.IndexCell,
                From._ImagePiece._PieceType,
                From._Color,
                _.None, // Никогда не будет взятия, так как кружочки - тихие ходы
                To._Color
            );
            _Position.MakeMove(move);

            //Удаляем все выделения
            Clear_PossibleCapturesList();
            Clear_PossibleMovesList();
            From.Rect.Fill = (From._Color == Color.White) ? Settings.WHITE_Color : Settings.BLACK_Color;
            IndexChooseCell = _.None;

            To._ImagePiece = new ImagePiece(From._ImagePiece, From.Rect);
            //To._ImagePiece = From._ImagePiece;
            Canvas.SetLeft(To._ImagePiece.Img, To.X);
            Canvas.SetTop(To._ImagePiece.Img, To.Y);
            _Canvas.Children.Add(To._ImagePiece.Img);

            _Canvas.Children.Remove(From._ImagePiece.Img);
            From._ImagePiece.ImagePiece_Tapped -= OnSelected;            
            From._ImagePiece = null;

            To._ImagePiece.Location = To.Rect;
            To._ImagePiece.Square = circleMove.IndexCell;
            //Toaster.Show($"{To.Rect.}");
            To._ImagePiece.ImagePiece_Tapped += OnSelected;
        }

        private Path Draw_Capture(uint X, uint Y, double Size) // Нужно переделать 
        {
            Path path = new Path();
            PathGeometry geometry = new PathGeometry();
            PathFigure f1 = new PathFigure(), f2 = new PathFigure(), f3 = new PathFigure(), f4 = new PathFigure();

            f1.StartPoint = new Windows.Foundation.Point(X + Size / 2 + Size / 6, Y + Size);;
            QuadraticBezierSegment curveDownRight = new QuadraticBezierSegment
            {
                Point1 = new Windows.Foundation.Point(X + Size, Y + Size),
                Point2 = new Windows.Foundation.Point(X + Size, Y + Size / 2 + Size / 6)
            };

            f1.Segments.Add(curveDownRight);
            f1.Segments.Add(new LineSegment { Point = new Windows.Foundation.Point(X + Size, Y + Size) });

            f2.StartPoint = new Windows.Foundation.Point(X + Size / 2 - Size / 6, Y + Size);
            QuadraticBezierSegment curveDownLeft = new QuadraticBezierSegment
            {
                Point1 = new Windows.Foundation.Point(X, Y + Size),
                Point2 = new Windows.Foundation.Point(X, Y + Size / 2 + Size / 6)
            };

            f2.Segments.Add(curveDownLeft);
            f2.Segments.Add(new LineSegment { Point = new Windows.Foundation.Point(X, Y + Size) });

            f3.StartPoint = new Windows.Foundation.Point(X, Y + Size / 2 - Size / 6);
            QuadraticBezierSegment curveUpLeft = new QuadraticBezierSegment
            {
                Point1 = new Windows.Foundation.Point(X, Y),
                Point2 = new Windows.Foundation.Point(X + Size / 2 - Size / 6, Y)
            };

            f3.Segments.Add(curveUpLeft);
            f3.Segments.Add(new LineSegment { Point = new Windows.Foundation.Point(X, Y) });

            f4.StartPoint = new Windows.Foundation.Point(X + Size / 2 + Size / 6, Y);
            QuadraticBezierSegment curveUpRight = new QuadraticBezierSegment
            {
                Point1 = new Windows.Foundation.Point(X + Size, Y),
                Point2 = new Windows.Foundation.Point(X + Size, Y + Size / 2 - Size / 6)
            };

            f4.Segments.Add(curveUpRight);
            f4.Segments.Add(new LineSegment { Point = new Windows.Foundation.Point(X + Size, Y) });

            geometry.Figures.Add(f1);
            geometry.Figures.Add(f2);
            geometry.Figures.Add(f3);
            geometry.Figures.Add(f4);

            path.Data = geometry;
            path.Fill = Settings.HIGHLIGHT_Color;

            _Canvas.Children.Add(path);

            return path;
        }

        private void Clear_PossibleCapturesList() // Баzа 
        {
            foreach (Path path in PossibleCapturesList)
            {
                _Canvas.Children.Remove(path);
            }
            PossibleCapturesList.Clear();
        }

        private void Clear_PossibleMovesList() // Баzа 
        {
            foreach (CircleMove circleMove in PossibleMovesList)
            {
                _Canvas.Children.Remove(circleMove.Circle);
            }
            PossibleMovesList.Clear();
        }
    }
}
