using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bitboard = System.UInt64;
using NavigationApp.Bitboards.ForBitboards;

namespace NavigationApp.Bitboards
{
    public struct LegalMoves
    {
        public static Dictionary<byte, Func<Pieces, byte, byte, Bitboard>> PsLegalCaptures = new Dictionary<byte, Func<Pieces, byte, byte, Bitboard>>
        {
            [PieceType.Pawn] = (pieces, square, side) => PsLegalMoveMask.Gen_PawnCaptures(pieces, square, side),
            [PieceType.Knight] = (pieces, square, side) => PsLegalMoveMask.Gen_KnightCaptures(pieces, square, side),
            [PieceType.Bishop] = (pieces, square, side) => PsLegalMoveMask.Gen_BishopCaptures(pieces, square, side),
            [PieceType.Rook] = (pieces, square, side) => PsLegalMoveMask.Gen_RookCaptures(pieces, square, side),
            [PieceType.Queen] = (pieces, square, side) => PsLegalMoveMask.Gen_QueenCaptures(pieces, square, side),
            [PieceType.King] = (pieces, square, side) => PsLegalMoveMask.Gen_KingCaptures(pieces, square, side)
        };

        public static Dictionary<byte, Func<Pieces, byte, byte, Bitboard>> PsLegalMoves = new Dictionary<byte, Func<Pieces, byte, byte, Bitboard>>
        {
            [PieceType.Pawn] = (pieces, square, side) => PsLegalMoveMask.Gen_PawnMoves(pieces, square, side),
            [PieceType.Knight] = (pieces, square, side) => PsLegalMoveMask.Gen_KnightMoves(pieces, square, side),
            [PieceType.Bishop] = (pieces, square, side) => PsLegalMoveMask.Gen_BishopMoves(pieces, square, side),
            [PieceType.Rook] = (pieces, square, side) => PsLegalMoveMask.Gen_RookMoves(pieces, square, side),
            [PieceType.Queen] = (pieces, square, side) => PsLegalMoveMask.Gen_QueenMoves(pieces, square, side),
            [PieceType.King] = (pieces, square, side) => PsLegalMoveMask.Gen_KingMoves(pieces, square, side)
        };

        public static Bitboard Gen_LegalCaptures(Pieces pieces, byte type, byte square, byte side)
        {
            Bitboard MaskCaptures = PsLegalCaptures[type](pieces, square, side), resultMask = 0;
            BitboardOperations.Set_0(ref pieces.PiecesBitboards[side, type], square);
            
            if (type == PieceType.King)
            {
                while (MaskCaptures != 0)
                {
                    byte squareCapt = BitboardOperations.Bsf(MaskCaptures);
                    Bitboard bb_Capt = 1UL << squareCapt;

                    BitboardOperations.Set_1(ref pieces.PiecesBitboards[side, type], squareCapt);
                    
                    byte not_side = Pieces.Inverse(side);                    
                    byte defenderType = 0;
                    for (byte i = 0; i < 6; i++)
                    {
                        if (BitboardOperations.Get_bit(pieces.PiecesBitboards[not_side, i], squareCapt))
                        {
                            pieces.PiecesBitboards[not_side, i] &= ~bb_Capt; // удаляем защитника
                            defenderType = i;
                            break;
                        }
                    }
                    pieces.UpdateBitboards();

                    if (!In_Danger(pieces, squareCapt, side))
                    {
                        resultMask |= bb_Capt;
                    }

                    BitboardOperations.Set_0(ref pieces.PiecesBitboards[side, type], squareCapt); // "возвращем" ход обратно
                    pieces.PiecesBitboards[not_side, defenderType] |= bb_Capt; // возвращаем защитника
                    pieces.UpdateBitboards();                    

                    MaskCaptures &= MaskCaptures - 1;
                }

                BitboardOperations.Set_1(ref pieces.PiecesBitboards[side, type], square);
                pieces.UpdateBitboards();

                return resultMask;
            }

            byte KingSquare = BitboardOperations.Bsf(pieces.PiecesBitboards[side, PieceType.King]);

            while (MaskCaptures != 0)
            {
                byte squareCapt = BitboardOperations.Bsf(MaskCaptures);
                Bitboard bb_Capt = 1UL << squareCapt;

                BitboardOperations.Set_1(ref pieces.PiecesBitboards[side, type], squareCapt);
                
                byte not_side = Pieces.Inverse(side);                
                byte defenderType = 0;
                for (byte i = 0; i < 6; i++)
                {
                    if (BitboardOperations.Get_bit(pieces.PiecesBitboards[not_side, i], squareCapt))
                    {
                        pieces.PiecesBitboards[not_side, i] &= ~bb_Capt;
                        defenderType = i;
                    }
                }
                pieces.UpdateBitboards();

                if (!In_Danger(pieces, KingSquare, side))
                {
                    resultMask |= bb_Capt;
                }

                BitboardOperations.Set_0(ref pieces.PiecesBitboards[side, type], squareCapt); // "возвращем" ход обратно
                pieces.PiecesBitboards[not_side, defenderType] |= bb_Capt; // возвращаем защитника
                pieces.UpdateBitboards();                

                MaskCaptures &= MaskCaptures - 1;
            }

            BitboardOperations.Set_1(ref pieces.PiecesBitboards[side, type], square);
            pieces.UpdateBitboards();

            return resultMask;
        }

        public static Bitboard Gen_LegalMoves(Pieces pieces, byte type, byte square, byte side)
        {
            Bitboard MaskMoves = PsLegalMoves[type](pieces, square, side), resultMask = 0;
            BitboardOperations.Set_0(ref pieces.PiecesBitboards[side, type], square);

            if (type == PieceType.King)
            {
                while (MaskMoves != 0)
                {
                    byte squareMove = BitboardOperations.Bsf(MaskMoves);
                    Bitboard bb_Move = 1UL << squareMove;

                    BitboardOperations.Set_1(ref pieces.PiecesBitboards[side, type], squareMove);

                    byte not_side = Pieces.Inverse(side);
                    
                    if (!In_Danger(pieces, squareMove, side))
                    {
                        resultMask |= bb_Move;
                    }

                    BitboardOperations.Set_0(ref pieces.PiecesBitboards[side, type], squareMove); // "возвращем" ход обратно
                    pieces.UpdateBitboards();                    

                    MaskMoves &= MaskMoves - 1;
                }

                BitboardOperations.Set_1(ref pieces.PiecesBitboards[side, type], square);
                pieces.UpdateBitboards();

                return resultMask;
            }

            byte KingSquare = BitboardOperations.Bsf(pieces.PiecesBitboards[side, PieceType.King]);

            while (MaskMoves != 0)
            {
                byte squareMove = BitboardOperations.Bsf(MaskMoves);
                Bitboard bb_Move = 1UL << squareMove;

                BitboardOperations.Set_1(ref pieces.PiecesBitboards[side, type], squareMove);

                if (!In_Danger(pieces, KingSquare, side))
                {
                    resultMask |= bb_Move;
                }

                BitboardOperations.Set_0(ref pieces.PiecesBitboards[side, type], squareMove); // "возвращем" ход обратно
                pieces.UpdateBitboards();

                MaskMoves &= MaskMoves - 1;
            }

            BitboardOperations.Set_1(ref pieces.PiecesBitboards[side, type], square);
            pieces.UpdateBitboards();

            return resultMask;
        }

        public static Bitboard Gen_DangerMask(Pieces pieces, byte square, byte side)
        {
            byte not_side = Pieces.Inverse(side);
            return
                PsLegalMoveMask.Gen_PawnCaptures(pieces, square, side)   & pieces.PiecesBitboards[not_side, PieceType.Pawn]   |
                PsLegalMoveMask.Gen_KnightCaptures(pieces, square, side) & pieces.PiecesBitboards[not_side, PieceType.Knight] |
                PsLegalMoveMask.Gen_BishopCaptures(pieces, square, side) & pieces.PiecesBitboards[not_side, PieceType.Bishop] |
                PsLegalMoveMask.Gen_RookCaptures(pieces, square, side)   & pieces.PiecesBitboards[not_side, PieceType.Rook]   |
                PsLegalMoveMask.Gen_QueenCaptures(pieces, square, side)  & pieces.PiecesBitboards[not_side, PieceType.Queen]  |
                PsLegalMoveMask.Gen_KingCaptures(pieces, square, side)   & pieces.PiecesBitboards[not_side, PieceType.King];
        }

        // Проверка: находится ли клетка под ударом
        public static bool In_Danger(Pieces pieces, byte square, byte side)
        {
            byte not_side = Pieces.Inverse(side);

            if ((PsLegalMoveMask.Gen_PawnCaptures(pieces, square, side) & pieces.PiecesBitboards[not_side, PieceType.Pawn]) != 0) return true;
            if ((PsLegalMoveMask.Gen_KnightCaptures(pieces, square, side) & pieces.PiecesBitboards[not_side, PieceType.Knight]) != 0) return true;
            if ((PsLegalMoveMask.Gen_BishopCaptures(pieces, square, side) & pieces.PiecesBitboards[not_side, PieceType.Bishop]) != 0) return true;
            if ((PsLegalMoveMask.Gen_RookCaptures(pieces, square, side) & pieces.PiecesBitboards[not_side, PieceType.Rook]) != 0) return true;
            if ((PsLegalMoveMask.Gen_QueenCaptures(pieces, square, side) & pieces.PiecesBitboards[not_side, PieceType.Queen]) != 0) return true;
            if ((PsLegalMoveMask.Gen_KingCaptures(pieces, square, side) & pieces.PiecesBitboards[not_side, PieceType.King]) != 0) return true;

            return false;
        }
    }
}
