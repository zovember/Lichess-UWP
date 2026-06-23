using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NavigationApp.Bitboards.Zobrist;
using NavigationApp.Bitboards.Zobrist.RepetitionHistory;
using NavigationApp.Bitboards;

using BOperation = NavigationApp.Bitboards.ForBitboards.BitboardOperations;
using _ = NavigationApp.CommonConstant; /// Вопросы, предложения, замечения?
//    ^
//    |
// на заметку: 255 означает отсутствие

namespace NavigationApp.Bitboards
{
    public class Position
    {
        public Pieces _Pieces;
        public byte _EnPassant; // взятие на проходе

        // Рокировки
        public bool _WhiteLongCastling;
        public bool _WhiteShortCastling;
        public bool _BlackLongCastling;
        public bool _BlackShortCastling;

        public bool WhiteCastling_Happened;
        public bool BlackCastling_Happened;

        public float MoveCtr;
        public ZobristHash _Hash;
        public float FiftyMovesCtr;
        public RepetitionHistory _RepetitionHistory = new RepetitionHistory();


        public Position(string fen, byte en_passant, bool w_l_castling, bool w_s_castling, bool b_l_castling, bool b_s_castling, float move_ctr)
        {
            _Pieces = new Pieces(fen);
            _EnPassant = en_passant;

            _WhiteLongCastling = w_l_castling;
            _WhiteShortCastling = w_s_castling;
            _BlackLongCastling = b_l_castling;
            _BlackShortCastling = b_s_castling;
            
            WhiteCastling_Happened = false;
            BlackCastling_Happened = false;

            MoveCtr = move_ctr;   
            _Hash = new ZobristHash(_Pieces, (move_ctr - Math.Floor(move_ctr)) > 1e-4, w_l_castling, w_s_castling, b_l_castling, b_s_castling);
            FiftyMovesCtr = 0;
            _RepetitionHistory.AddPosition(_Hash);
        }

        public void Add_Piece(byte square, byte type, byte side)
        {
            if (!BOperation.Get_bit(_Pieces.PiecesBitboards[side, type], square))
            {
                BOperation.Set_1(ref _Pieces.PiecesBitboards[side, type], square);
                _Hash.InvPiece(square, type, side);
            }
        }

        public void Remove_Piece(byte square, byte type, byte side)
        {
            if (BOperation.Get_bit(_Pieces.PiecesBitboards[side, type], square))
            {
                BOperation.Set_0(ref _Pieces.PiecesBitboards[side, type], square);
                _Hash.InvPiece(square, type, side);
            }
        }

        public void Change_EnPassant(byte value) => _EnPassant = value;

        public void Remove_WhiteLongCastling()
        {
            if (_WhiteLongCastling)
            {
                _WhiteLongCastling = false;
                _Hash.InvWhiteLongCastling();
            }
        }

        public void Remove_WhiteShortCastling()
        {
            if (_WhiteShortCastling)
            {
                _WhiteShortCastling = false;
                _Hash.InvWhiteShortCastling();
            }
        }

        public void Remove_BlackLongCastling()
        {
            if (_BlackLongCastling)
            {
                _BlackLongCastling = false;
                _Hash.InvBlackLongCastling();
            }
        }

        public void Remove_BlackShortCastling()
        {
            if (_BlackShortCastling)
            {
                _BlackShortCastling = false;
                _Hash.InvBlackShortCastling();
            }
        }

        public void Update_MoveCtr()
        {
            MoveCtr += 0.5f;
            _Hash.InvMove();
        }

        public void Update_FiftyMovesCtr(bool break_event)
        {
            if (break_event) FiftyMovesCtr = 0;
            else FiftyMovesCtr += 0.5f;
        }

        public void MakeMove(Move move)
        {
            Remove_Piece(move.From, move.AttackerType, move.AttackerSide);
            Add_Piece(move.To, move.AttackerType, move.AttackerSide);

            if (move.DefenderType != _.None) Remove_Piece(move.To, move.DefenderType, move.DefenderSide);

            switch (move._Flag)
            {
                case Move.Flag.Default:
                    break;

                case Move.Flag.PawnLongMove:
                    Change_EnPassant( (byte)((move.From + move.To) / 2) );
                    break;

                case Move.Flag.EnPassantCapture:
                    if (move.AttackerSide == Color.White) Remove_Piece((byte)(move.To - 8), PieceType.Pawn, Color.Black);
                    else Remove_Piece( (byte)(move.To + 8), PieceType.Pawn, Color.White);
                    break;

                // В рокировках не трогаем короля, потому что ход идёт от него, а для ходящего действия прописаны в самом начале функции 
                case Move.Flag.WhiteLongCastling:  
                    Remove_Piece(0, PieceType.Rook, Color.White);
                    Add_Piece(3, PieceType.Rook, Color.White);
                    WhiteCastling_Happened = true;
                    break;

                case Move.Flag.WhiteShortCastling:
                    Remove_Piece(7, PieceType.Rook, Color.White);
                    Add_Piece(5, PieceType.Rook, Color.White);
                    WhiteCastling_Happened = true;
                    break;

                case Move.Flag.BlackLongCastling:
                    Remove_Piece(56, PieceType.Rook, Color.Black);
                    Add_Piece(59, PieceType.Rook, Color.Black);
                    BlackCastling_Happened = true;
                    break;

                case Move.Flag.BlackShortCastling:
                    Remove_Piece(63, PieceType.Rook, Color.Black);
                    Add_Piece(61, PieceType.Rook, Color.Black);
                    BlackCastling_Happened = true;
                    break;


                case Move.Flag.PromoteToKnight:
                    Remove_Piece(move.To, PieceType.Pawn, move.AttackerSide);
                    Add_Piece(move.To, PieceType.Knight, move.AttackerSide);
                    break;

                case Move.Flag.PromoteToBishop:
                    Remove_Piece(move.To, PieceType.Pawn, move.AttackerSide);
                    Add_Piece(move.To, PieceType.Bishop, move.AttackerSide);
                    break;

                case Move.Flag.PromoteToRook:
                    Remove_Piece(move.To, PieceType.Pawn, move.AttackerSide);
                    Add_Piece(move.To, PieceType.Rook, move.AttackerSide);
                    break;

                case Move.Flag.PromoteToQueen:
                    Remove_Piece(move.To, PieceType.Pawn, move.AttackerSide);
                    Add_Piece(move.To, PieceType.Queen, move.AttackerSide);
                    break;
            }

            _Pieces.UpdateBitboards();

            if (move._Flag != Move.Flag.PawnLongMove) Change_EnPassant(_.None); // Так как взятие на проходе может быть только ответным ходом

            switch (move.From)
            {
                case 0:
                    Remove_WhiteLongCastling();
                    break;
                case 4:
                    Remove_WhiteLongCastling();
                    Remove_WhiteShortCastling();
                    break;
                case 7:
                    Remove_WhiteShortCastling();
                    break;
                case 56:
                    Remove_BlackLongCastling();
                    break;
                case 60:
                    Remove_BlackLongCastling();
                    Remove_BlackShortCastling();
                    break;
                case 63:
                    Remove_BlackShortCastling();
                    break;
            }

            Update_MoveCtr();
            Update_FiftyMovesCtr(move.AttackerType == PieceType.Pawn || move.DefenderType != _.None);
        }
    }
}
