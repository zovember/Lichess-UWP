/*
* Операции с битбордами
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bitboard = System.UInt64;

namespace NavigationApp.Bitboards.ForBitboards
{
    public static class BitboardOperations
    {
        private static readonly byte[] BitScanTable = {
            0, 47,  1, 56, 48, 27,  2, 60,
            57, 49, 41, 37, 28, 16,  3, 61,
            54, 58, 35, 52, 50, 42, 21, 44,
            38, 32, 29, 23, 17, 11,  4, 62,
            46, 55, 26, 59, 40, 36, 15, 53,
            34, 51, 20, 43, 31, 22, 10, 45,
            25, 39, 14, 33, 19, 30,  9, 24,
            13, 18,  8, 12,  7,  6,  5, 63
        };

        public static void Set_1(ref Bitboard bb, byte square) // устанавливает значение в 1 по индексу
        {
            bb = bb | (1UL << square);
        }

        public static void Set_0(ref Bitboard bb, byte square) // устанавливает значение в 0 по индексу
        {
            bb = bb & (~(1UL << square));
        }

        public static bool Get_bit(Bitboard bb, byte square) // получение значения бита по индексу(0 - самый правый бит)
        {
            return (bb & (1UL << square)) != 0;
        }

        public static byte Count_1(Bitboard bb) // количество единичных битов
        {
            byte count_1 = 0;
            while (bb > 0)
            {
                bb &= bb - 1;
                count_1++;
            }
            return count_1;
        }

        public static byte Bsf(Bitboard bb) // поиск первого единичного бита (возвращает индекс, как и его приятель-антоним)
        {
            return BitScanTable[((bb ^ (bb - 1)) * 0x03f79d71b4cb0a89) >> 58];
        }

        public static byte Bsr(Bitboard bb) // поиск последнего единичного бита
        {
            bb = bb | (bb >> 1);
            bb = bb | (bb >> 2);
            bb = bb | (bb >> 4);
            bb = bb | (bb >> 8);
            bb = bb | (bb >> 16);
            bb = bb | (bb >> 32);

            return BitScanTable[(bb * 0x03f79d71b4cb0a89) >> 58];
        }
    }

    public static class BitboardRows //хохо
    {
        //Терпи, карлик
        public const Bitboard Row_0 = 0b11111111;
        public const Bitboard Row_1 = 0b11111111_00000000;
        public const Bitboard Row_2 = 0b11111111_0000000000000000;
        public const Bitboard Row_3 = 0b11111111_000000000000000000000000;
        public const Bitboard Row_4 = 0b11111111_00000000000000000000000000000000;
        public const Bitboard Row_5 = 0b11111111_0000000000000000000000000000000000000000;
        public const Bitboard Row_6 = 0b11111111_000000000000000000000000000000000000000000000000;
        public const Bitboard Row_7 = 0b11111111_00000000000000000000000000000000000000000000000000000000;

        public const Bitboard InvRow_0 = 0b11111111111111111111111111111111111111111111111111111111_00000000;
        public const Bitboard InvRow_1 = 0b111111111111111111111111111111111111111111111111_00000000_11111111;
        public const Bitboard InvRow_2 = 0b1111111111111111111111111111111111111111_00000000_1111111111111111;
        public const Bitboard InvRow_3 = 0b11111111111111111111111111111111_00000000_111111111111111111111111;
        public const Bitboard InvRow_4 = 0b111111111111111111111111_00000000_11111111111111111111111111111111;
        public const Bitboard InvRow_5 = 0b1111111111111111_00000000_1111111111111111111111111111111111111111;
        public const Bitboard InvRow_6 = 0b11111111_00000000_111111111111111111111111111111111111111111111111;
        public const Bitboard InvRow_7 = 0b00000000_11111111111111111111111111111111111111111111111111111111;
    }

    public static class BitboardColumns // эти 64-битные числа такие красивые, ммм
    {
        //Терпи карлик 2.0
        public const Bitboard A = 0b0000000100000001000000010000000100000001000000010000000100000001;
        public const Bitboard B = 0b0000001000000010000000100000001000000010000000100000001000000010;
        public const Bitboard C = 0b0000010000000100000001000000010000000100000001000000010000000100;
        public const Bitboard D = 0b0000100000001000000010000000100000001000000010000000100000001000;
        public const Bitboard E = 0b0001000000010000000100000001000000010000000100000001000000010000;
        public const Bitboard F = 0b0010000000100000001000000010000000100000001000000010000000100000;
        public const Bitboard G = 0b0100000001000000010000000100000001000000010000000100000001000000;
        public const Bitboard H = 0b1000000010000000100000001000000010000000100000001000000010000000;

        public const Bitboard Inv_A = 0b1111111011111110111111101111111011111110111111101111111011111110;
        public const Bitboard Inv_B = 0b1111110111111101111111011111110111111101111111011111110111111101;
        public const Bitboard Inv_C = 0b1111101111111011111110111111101111111011111110111111101111111011;
        public const Bitboard Inv_D = 0b1111011111110111111101111111011111110111111101111111011111110111;
        public const Bitboard Inv_E = 0b1110111111101111111011111110111111101111111011111110111111101111;
        public const Bitboard Inv_F = 0b1101111111011111110111111101111111011111110111111101111111011111;
        public const Bitboard Inv_G = 0b1011111110111111101111111011111110111111101111111011111110111111;
        public const Bitboard Inv_H = 0b0111111101111111011111110111111101111111011111110111111101111111;
    }
}
