using System.Collections.Generic;
using Enums;

namespace Data.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(Side side, Coordinates coords) : base(side, coords) {}
        
        public Pawn(Pawn copy) : base(copy) {}

        public override float HeuristicScore
        {
            get
            {
                float baseValue = 1.0f;
                return baseValue;
            }
        }

        public override List<Coordinates> AvailableMoves(Coordinates coordinates)
        {
            List<Coordinates> moves = new ();
            int currentColumn = coordinates.Column;
            int currentRow = coordinates.Row;
            int offset = (Side == Side.Light) ? 1 : -1;
            
            Piece forward, forwardLeft, forwardRight, forwardPush;

           
            forward = Matrix.GetPiece(currentColumn, currentRow + offset);
            forwardLeft = Matrix.GetPiece(currentColumn - 1, currentRow + offset);
            forwardRight = Matrix.GetPiece(currentColumn + 1, currentRow + offset);
            forwardPush = Matrix.GetPiece(currentColumn, currentRow + offset * 2);
            
        
            
            if (forwardLeft is not null && forwardLeft.Side != Side)
                moves.Add(forwardLeft.Coordinates);

            if (forwardRight is not null && forwardRight.Side != Side)
                moves.Add(forwardRight.Coordinates);

            if (forward is null)
                moves.Add(new Coordinates(currentColumn, currentRow + offset));
            else
                return moves;
            
            if (forwardPush is null && !HasMoved)
                moves.Add(new Coordinates(currentColumn, currentRow + offset * 2));
            
            return moves;
        }
    }
}