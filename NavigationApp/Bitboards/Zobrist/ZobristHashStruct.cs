using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NavigationApp.Bitboards.ForBitboards;

namespace NavigationApp.Bitboards.Zobrist
{
    public struct ZobristHash
    {
        public ulong Hash;

        public ZobristHash(Pieces pieces, bool BlackMove, bool WhiteLongCastling, bool WhiteShortCastling, bool BlackLongCastling, bool BlackShortCastling)
        {
            Hash = 0;

            if (BlackMove) InvMove();
            if (WhiteLongCastling) InvWhiteLongCastling();
            if (WhiteShortCastling) InvWhiteShortCastling();
            if (BlackLongCastling) InvBlackLongCastling();
            if (BlackShortCastling) InvBlackShortCastling();

            byte side;
            for (byte square = 0; square < 64; square++)
            {
                if (BitboardOperations.Get_bit(pieces.SideBitboards[Color.White], square)) side = Color.White;
                else if (BitboardOperations.Get_bit(pieces.SideBitboards[Color.Black], square)) side = Color.Black;
                else continue;

                for (byte type = 0; type < 6; type++)
                {
                    if (BitboardOperations.Get_bit(pieces.PiecesBitboards[side, type], square))
                    {
                        InvPiece(square, type, side);
                        break;
                    }
                }
            }
        }

        public static bool operator== (ZobristHash left, ZobristHash right)
        {
            return left.Hash == right.Hash;
        }

        public static bool operator!= (ZobristHash left, ZobristHash right)
        {
            return !(left.Hash == right.Hash);
        }

        public static bool operator< (ZobristHash left, ZobristHash right)
        {
            return left.Hash < right.Hash;
        }

        public static bool operator> (ZobristHash left, ZobristHash right)
        {
            return left.Hash > right.Hash;
        }

        public void InvPiece(byte square, byte type, byte side)
            => Hash ^= ZobristHashConsteval.Constants[square, side, type];

        public void InvMove()
            => Hash ^= ZobristHashConsteval.BlackMove;

        public void InvWhiteLongCastling()
            => Hash ^= ZobristHashConsteval.WhiteLongCastling;

        public void InvWhiteShortCastling()
            => Hash ^= ZobristHashConsteval.WhiteShortCastling;

        public void InvBlackLongCastling()
            => Hash ^= ZobristHashConsteval.BlackLongCastling;

        public void InvBlackShortCastling()
            => Hash ^= ZobristHashConsteval.BlackShortCastling;
    }
}
