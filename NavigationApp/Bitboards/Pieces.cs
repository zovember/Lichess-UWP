using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bitboard = System.UInt64;
using NavigationApp.Bitboards.ForBitboards;
using PType = NavigationApp.PieceType;

namespace NavigationApp.Bitboards
{
    public class Pieces
    {
        public Bitboard[,] PiecesBitboards = new Bitboard[2, 6];

        public Bitboard[] SideBitboards = new Bitboard[2]; // все белые и чёрные фигуры
        public Bitboard[] InvSideBitboards = new Bitboard[2];

        public Bitboard All { get; set; }
        public Bitboard Empty { get; set; }

        public Pieces(string fen) //rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR
        {
            byte x = 0;
            byte y = 7;

            foreach (char sym in fen)
            {
                if (sym == '/')
                {
                    x = 0;
                    y--;
                }

                else if (char.IsDigit(sym))
                {
                    x += (byte)(sym - '0');
                }

                else
                {
                    byte index = (byte)(y * 8 + x);
                    switch (sym)
                    {
                        case 'P': BitboardOperations.Set_1(ref PiecesBitboards[Color.White, PType.Pawn], index); break;
                        case 'N': BitboardOperations.Set_1(ref PiecesBitboards[Color.White, PType.Knight], index); break;
                        case 'B': BitboardOperations.Set_1(ref PiecesBitboards[Color.White, PType.Bishop], index); break;
                        case 'R': BitboardOperations.Set_1(ref PiecesBitboards[Color.White, PType.Rook], index); break;
                        case 'Q': BitboardOperations.Set_1(ref PiecesBitboards[Color.White, PType.Queen], index); break;
                        case 'K': BitboardOperations.Set_1(ref PiecesBitboards[Color.White, PType.King], index); break;

                        case 'p': BitboardOperations.Set_1(ref PiecesBitboards[Color.Black, PType.Pawn], index); break;
                        case 'n': BitboardOperations.Set_1(ref PiecesBitboards[Color.Black, PType.Knight], index); break;
                        case 'b': BitboardOperations.Set_1(ref PiecesBitboards[Color.Black, PType.Bishop], index); break;
                        case 'r': BitboardOperations.Set_1(ref PiecesBitboards[Color.Black, PType.Rook], index); break;
                        case 'q': BitboardOperations.Set_1(ref PiecesBitboards[Color.Black, PType.Queen], index); break;
                        case 'k': BitboardOperations.Set_1(ref PiecesBitboards[Color.Black, PType.King], index); break;
                    }

                    UpdateBitboards();
                    x++;
                }
            }
        }

        public static bool operator== (Pieces Left, Pieces Right)
        {
            for (var i = 0; i < 6; i++)
            {
                if (Left.PiecesBitboards[Color.White, i] != Right.PiecesBitboards[Color.White, i]) return false;
                if (Left.PiecesBitboards[Color.Black, i] != Right.PiecesBitboards[Color.Black, i]) return false;
            }
            return true;
        }

        public static bool operator!= (Pieces Left, Pieces Right)
        {
            for (var i = 0; i < 6; i++)
            {
                if (Left.PiecesBitboards[Color.White, i] != Right.PiecesBitboards[Color.White, i]) return true;
                if (Left.PiecesBitboards[Color.Black, i] != Right.PiecesBitboards[Color.Black, i]) return true;
            }
            return false;
        }

        public static byte Inverse(byte _byte) => (byte)(1 - _byte);

        public void UpdateBitboards()
        {
            SideBitboards[Color.White] = PiecesBitboards[Color.White, PType.Pawn] |
                                 PiecesBitboards[Color.White, PType.Knight] |
                                 PiecesBitboards[Color.White, PType.Bishop] |
                                 PiecesBitboards[Color.White, PType.Rook] |
                                 PiecesBitboards[Color.White, PType.Queen] |
                                 PiecesBitboards[Color.White, PType.King];

            SideBitboards[Color.Black] = PiecesBitboards[Color.Black, PType.Pawn] |
                                 PiecesBitboards[Color.Black, PType.Knight] |
                                 PiecesBitboards[Color.Black, PType.Bishop] |
                                 PiecesBitboards[Color.Black, PType.Rook] |
                                 PiecesBitboards[Color.Black, PType.Queen] |
                                 PiecesBitboards[Color.Black, PType.King];

            InvSideBitboards[Color.White] = ~SideBitboards[Color.White];
            InvSideBitboards[Color.Black] = ~SideBitboards[Color.Black];

            All = SideBitboards[Color.White] | SideBitboards[Color.Black];
            Empty = ~All;
        }
    }
}
