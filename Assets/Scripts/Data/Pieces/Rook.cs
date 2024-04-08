using System.Collections.Generic;
using Enums;

namespace Data.Pieces
{
    public class Rook : Piece
    {
        public Rook(Side side, Coordinates coords) : base(side, coords) {}
        
        public Rook(Rook copy) : base(copy) {}
        
        public override float HeuristicScore
        {
            get
            {
                float baseValue = 5.10f;
                return baseValue;
            }
        }

        public override List<Coordinates> AvailableMoves(Coordinates coordinates)
        {
            List<Coordinates> availableMoves = new ();
            int currentColumn = coordinates.Column;
            int currentRow = coordinates.Row;

            for (int row = currentRow + 1; row < Matrix.BoardSize; row++) // Upward
            {
                Piece upwardCell = Matrix.GetPiece(currentColumn, row);
                if (!ValidateCell(availableMoves, upwardCell.Coordinates))
                    break;
            }
            
            for (int row = currentRow - 1; row >= 0; row--) // Downward
            {
                Piece downwardCell = Matrix.GetPiece(currentColumn, row);
                if (!ValidateCell(availableMoves, downwardCell.Coordinates))
                    break;
            }
            
            for (int column = currentColumn + 1; column < Matrix.BoardSize; column++) // Rightward
            {
                Piece rightwardCell = Matrix.GetPiece(column, currentRow);
                if (!ValidateCell(availableMoves, rightwardCell.Coordinates))
                    break;
            }

            for (int column = currentColumn - 1; column >= 0; column--) // Leftward
            {
                Piece leftwardCell = Matrix.GetPiece(column, currentRow);
                if (!ValidateCell(availableMoves, leftwardCell.Coordinates))
                    break;
            }

            return availableMoves;
        }
    }
}
