using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bitboard = System.UInt64;

using NavigationApp.Bitboards.ForBitboards;

namespace NavigationApp.Bitboards
{
    public struct PsLegalMoveMask
    {        
        //Кони (тут не будет отсылки на "Джентельмены удачи" с фразой которого читатель естественно знаком, ибо, опять же: не то время, не то место)
        public static Bitboard Gen_KnightCaptures(Pieces pieces, byte square, byte side)
        {
            return KnightMasks.Masks[square] & pieces.SideBitboards[Pieces.Inverse(side)];
        }

        public static Bitboard Gen_KnightMoves(Pieces pieces, byte square, byte side)
        {
            return KnightMasks.Masks[square] & pieces.Empty;
        }

        //Короли ничем не владеют. Они только царствуют
        public static Bitboard Gen_KingCaptures(Pieces pieces, byte square, byte side)
        {
            return KingMasks.Masks[square] & pieces.SideBitboards[Pieces.Inverse(side)];
        }

        public static Bitboard Gen_KingMoves(Pieces pieces, byte square, byte side)
        {
            return KingMasks.Masks[square] & pieces.Empty;
        }

        //Считаем луч в заданную сторону для рассчёта скользящих фигур
        private static Bitboard Calc_RayCaptures(Pieces pieces, byte square, byte side, sbyte direction, bool bsr)
        {
            Bitboard blockers = SliderMasks.Masks[square, direction] & pieces.All;
            if (blockers == 0) return 0;
            byte blocking_sq = (bsr) ? BitboardOperations.Bsr(blockers) : BitboardOperations.Bsf(blockers);
            Bitboard moves = 0;
            BitboardOperations.Set_1(ref moves, blocking_sq);

            return moves & pieces.InvSideBitboards[side];
        }

        private static Bitboard Calc_RayMoves(Pieces pieces, byte square, byte side, sbyte direction, bool bsr)
        {
            Bitboard blockers = SliderMasks.Masks[square, direction] & pieces.All;
            if (blockers == 0)
            {
                return SliderMasks.Masks[square, direction];
            }

            byte blocking_sq = (bsr) ? BitboardOperations.Bsr(blockers) : BitboardOperations.Bsf(blockers);
            Bitboard moves = SliderMasks.Masks[square, direction] ^ SliderMasks.Masks[blocking_sq, direction];

            if (BitboardOperations.Get_bit(pieces.SideBitboards[side], blocking_sq)) BitboardOperations.Set_0(ref moves, blocking_sq);
            else BitboardOperations.Set_1(ref moves, blocking_sq);

            return moves & pieces.Empty;
        }
        
        //Ладьи(потому что сначала ладьи, а потом слоны)
        public static Bitboard Gen_RookCaptures(Pieces pieces, byte square, byte side)
        {
            return Calc_RayCaptures(pieces, square, side, Direction.North, false) |
                   Calc_RayCaptures(pieces, square, side, Direction.South, true)  |
                   Calc_RayCaptures(pieces, square, side, Direction.West, true)   |
                   Calc_RayCaptures(pieces, square, side, Direction.East, false);
        }

        public static Bitboard Gen_RookMoves(Pieces pieces, byte square, byte side)
        {
            return Calc_RayMoves(pieces, square, side, Direction.North, false) |
                   Calc_RayMoves(pieces, square, side, Direction.South, true)  |
                   Calc_RayMoves(pieces, square, side, Direction.West, true)   |
                   Calc_RayMoves(pieces, square, side, Direction.East, false);
        }

        //Слоны, офицеры, эпископы, шуты, сумашедшие(у всех существительное, а у румын прилагательное), бегуны, копья, гонцы, стрельцы, охотники
        public static Bitboard Gen_BishopCaptures(Pieces pieces, byte square, byte side)
        {
            return Calc_RayCaptures(pieces, square, side, Direction.NorthWest, false) |
                   Calc_RayCaptures(pieces, square, side, Direction.NorthEast, false) |
                   Calc_RayCaptures(pieces, square, side, Direction.SouthWest, true)  |
                   Calc_RayCaptures(pieces, square, side, Direction.SouthEast, true);
        }

        public static Bitboard Gen_BishopMoves(Pieces pieces, byte square, byte side)
        {
            return Calc_RayMoves(pieces, square, side, Direction.NorthWest, false) |
                   Calc_RayMoves(pieces, square, side, Direction.NorthEast, false) |
                   Calc_RayMoves(pieces, square, side, Direction.SouthWest, true)  |
                   Calc_RayMoves(pieces, square, side, Direction.SouthEast, true);
        }

        //Королева
        public static Bitboard Gen_QueenCaptures(Pieces pieces, byte square, byte side)
        {
            //Код несложный, так что прямым текстом, без обобщающих функций
            return Calc_RayCaptures(pieces, square, side, Direction.North, false) |
                   Calc_RayCaptures(pieces, square, side, Direction.South, true)  |
                   Calc_RayCaptures(pieces, square, side, Direction.West, true)   |
                   Calc_RayCaptures(pieces, square, side, Direction.East, false)  |

                   Calc_RayCaptures(pieces, square, side, Direction.NorthWest, false) |
                   Calc_RayCaptures(pieces, square, side, Direction.NorthEast, false) |
                   Calc_RayCaptures(pieces, square, side, Direction.SouthWest, true)  |
                   Calc_RayCaptures(pieces, square, side, Direction.SouthEast, true);
        }

        public static Bitboard Gen_QueenMoves(Pieces pieces, byte square, byte side)
        {
            return Calc_RayMoves(pieces, square, side, Direction.North, false) |
                   Calc_RayMoves(pieces, square, side, Direction.South, true)  |
                   Calc_RayMoves(pieces, square, side, Direction.West, true)   |
                   Calc_RayMoves(pieces, square, side, Direction.East, false)  |

                   Calc_RayMoves(pieces, square, side, Direction.NorthWest, false) |
                   Calc_RayMoves(pieces, square, side, Direction.NorthEast, false) |
                   Calc_RayMoves(pieces, square, side, Direction.SouthWest, true)  |
                   Calc_RayMoves(pieces, square, side, Direction.SouthEast, true);
        }

        //Пешки (тут хотелось бы вставить отсылку на Горьково, но её не будет, ибо время не то)
        public static Bitboard Gen_PawnCaptures(Pieces pieces, byte square, byte side)
        {
            Bitboard pawn = 1UL << square;
            if (side == Color.White)
            {
                return ((pawn << 9) & BitboardColumns.Inv_A | (pawn << 7) & BitboardColumns.Inv_H) & pieces.SideBitboards[Pieces.Inverse(side)];
            }

            return ((pawn >> 9) & BitboardColumns.Inv_H | (pawn >> 7) & BitboardColumns.Inv_A) & pieces.SideBitboards[Pieces.Inverse(side)];
        }

        public static Bitboard Gen_PawnMoves(Pieces pieces, byte square, byte side)
        {
            Bitboard pawn = 1UL << square, default_move;

            if (side == Color.White)
            {
                default_move = (pawn << 8) & pieces.Empty;
                if (default_move != 0)
                {
                    return (default_move | (pawn & BitboardRows.Row_1) << 16) & pieces.Empty;
                }

                return 0;
            }

            default_move = (pawn >> 8) & pieces.Empty;
            if (default_move != 0)
            {
                return (default_move | (pawn & BitboardRows.Row_6) >> 16) & pieces.Empty;
            }

            return 0;
        }        
    }
}
