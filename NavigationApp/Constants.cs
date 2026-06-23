using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationApp
{
    //Скучные и однотипные Константы, но с enum это в 10раз неудобней
    ///       ^
    ///       |
    ///       (я хз, нужна ли тут запятая, поэтому "и")
    public struct PieceType
    {
        public const byte Pawn = 0;
        public const byte Knight = 1;
        public const byte Bishop = 2;
        public const byte Rook = 3;
        public const byte Queen = 4;
        public const byte King = 5;
    }

    public struct Color
    {
        public const byte White = 0;
        public const byte Black = 1;
    }

    public struct CommonConstant
    {
        public const byte None = 255;
    }

    public struct Direction
    {
        public const sbyte North = 0;
        public const sbyte South = 1;
        public const sbyte West = 2;
        public const sbyte East = 3;

        public const sbyte NorthWest = 4;
        public const sbyte NorthEast = 5;
        public const sbyte SouthWest = 6;
        public const sbyte SouthEast = 7;
    }

    public struct MyMath
    {
        public const double sqrt_2 = 1.4142135623730950488016887242097;
    }
}
