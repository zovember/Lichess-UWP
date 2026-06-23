using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NavigationApp.Bitboards.ForBitboards;
using NavigationApp.Bitboards.Zobrist;

namespace NavigationApp.Bitboards
{
    public struct Move
    {
        public byte From { get; }
        public byte To { get; }

        public byte AttackerType { get; }
        public byte AttackerSide { get; }

        public byte DefenderType { get; }
        public byte DefenderSide { get; }

        public byte _Flag { get; }


        public Move(byte from, byte to, byte attacker_type, byte attacker_side, byte defender_type, byte defender_side, byte flag = Flag.Default)
        {
            From = from;
            To = to;

            AttackerType = attacker_type;
            AttackerSide = attacker_side;

            DefenderType = defender_type;
            DefenderSide = defender_side;

            _Flag = flag;
        } 

        public static bool operator== (Move left, Move right) // Сюда не заглядывай, тут всё примитивно 
        {
            if (left.From != right.From) return false;
            if (left.To != right.To) return false;
            if (left.AttackerType != right.AttackerType) return false;
            if (left.AttackerSide != right.AttackerSide) return false;
            if (left.DefenderType != right.DefenderType) return false;
            if (left.DefenderSide != right.DefenderSide) return false;
            if (left._Flag != right._Flag) return false;

            return true;
        }

        public static bool operator!= (Move left, Move right) // Аналогично 
        {
            if (left.From != right.From) return true;
            if (left.To != right.To) return true;
            if (left.AttackerType != right.AttackerType) return true;
            if (left.AttackerSide != right.AttackerSide) return true;
            if (left.DefenderType != right.DefenderType) return true;
            if (left.DefenderSide != right.DefenderSide) return true;
            if (left._Flag != right._Flag) return true;

            return false;
        } 

        public struct Flag // Загляни 
        {
            public const byte Default = 0; // обычный ход или взятие

            public const byte PawnLongMove = 1; // тут понятно
            public const byte EnPassantCapture = 2; // взятие на проходе

            public const byte WhiteLongCastling = 3; // длинная рокировка
            public const byte WhiteShortCastling = 4;
            public const byte BlackLongCastling = 5;
            public const byte BlackShortCastling = 6;

            public const byte PromoteToKnight = 7; // из пешки в коня
            public const byte PromoteToBishop = 8;
            public const byte PromoteToRook = 9;
            public const byte PromoteToQueen = 10;
        } 
    }
}
