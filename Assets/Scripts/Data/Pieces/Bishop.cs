using System.Collections.Generic;
using Enums;

namespace Data.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Side side, Coordinates coords) : base(side, coords) {}

        public Bishop(Bishop copy) : base(copy) {}

        public override float HeuristicScore
        {
            get
            {
                float baseValue = 3.30f;
                return baseValue;
            }
        }

        public override List<Coordinates> AvailableMoves(Coordinates coordinates)
        {
            List<Coordinates> availableMoves = new ();
            int currentColumn = coordinates.Column;
            int currentRow = coordinates.Row;
            
            for (int column = currentColumn + 1, row = currentRow + 1; row < Matrix.BoardSize && column < Matrix.BoardSize; row++, column++) // Upward-right
            {
                if (!ValidateCell(availableMoves, coordinates))
                    break;
            }
            
            for (int column = currentColumn - 1, row = currentRow + 1; row < Matrix.BoardSize && column >= 0; row++, column--) // Upward-left
            {
                if (!ValidateCell(availableMoves, coordinates))
                    break;
            }

            for (int column = currentColumn + 1, row = currentRow - 1; row >= 0 && column < Matrix.BoardSize; row--, column++) // Downward-right
            {
                if (!ValidateCell(availableMoves, coordinates))
                    break;
            }

            for (int column = currentColumn - 1, row = currentRow - 1; row >= 0 && column >= 0; row--, column--) // Downward-left
            {
                if (!ValidateCell(availableMoves, coordinates))
                    break;
            }

            return availableMoves;
        }
    }
}
