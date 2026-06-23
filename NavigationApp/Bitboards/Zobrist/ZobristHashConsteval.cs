using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationApp.Bitboards.Zobrist
{    
    struct PRNG
    {
        public const ulong Seed = 0x98f107;
        public const ulong Multiplier = 0x71abc9;
        public const ulong Summand = 0xff1b3f;
    }

    public struct ZobristHashConsteval
    {
        public static ulong NextRandom(ulong previous)
        {
            return PRNG.Multiplier * previous + PRNG.Summand;
        }

        public static ulong[,,] CalcConstants() {
            ulong[,,] constants = new ulong[64, 2, 64];

            ulong previous = PRNG.Seed;

            for (ulong square = 0; square < 64; square++)
            {
                for (ulong side = 0; side < 2; side++)
                {
                    for (ulong type = 0; type < 6; type++)
                    {
                        previous = NextRandom(previous);
                        constants[square, side, type] = previous;
                    }
                }
            }

            return constants;
        }


        public static ulong[,,] Constants = CalcConstants();
        public static ulong BlackMove = NextRandom(Constants[63, 1, 5]);
        public static ulong WhiteLongCastling = NextRandom(BlackMove);
        public static ulong WhiteShortCastling = NextRandom(WhiteLongCastling);
        public static ulong BlackLongCastling = NextRandom(WhiteShortCastling);
        public static ulong BlackShortCastling = NextRandom(BlackLongCastling);
    }
}
