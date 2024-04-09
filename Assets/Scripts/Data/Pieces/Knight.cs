using System.Collections.Generic;
using Enums;

namespace Data.Pieces
{
    public class Knight : Piece
    {
        public Knight(Side side, Coordinates coords) : base(side, coords) {}
        
        public Knight(Knight copy) : base(copy) {}
        
        public override float HeuristicScore
        {
            get
            {
                float baseValue = 3.20f;
                return baseValue;
            }
        }

        public override List<Coordinates> AvailableMoves(Coordinates coordinates)
        {
            List<Coordinates> availableMoves = new ();
            int currentColumn = coordinates.Column;
            int currentRow = coordinates.Row;

            int[] columnsOffsets = { 1, 2,  2,  1, -1, -2, -2, -1 };
            int[] rowOffsets     = { 2, 1, -1, -2, -2, -1,  1,  2 };

            for (int i = 0; i < Matrix.BoardSize; i++)
            {
                int column = currentColumn + columnsOffsets[i];
                int row = currentRow + rowOffsets[i];

                Piece piece = Matrix.GetPiece(column, row);
                
                ValidateCell(availableMoves, coordinates);
            }

            return availableMoves;
        }

        protected override bool ValidateCell(ICollection<Coordinates> availableMoves, Coordinates coordsToCheck)
        {
            if ((coordsToCheck.Column < 0 || coordsToCheck.Row < 0) || (coordsToCheck.Column > 7 || coordsToCheck.Row > 7)) {
                return false;
            }
            
            Piece piece = Matrix.GetPiece(coordsToCheck);

            if (piece == null) {
                availableMoves.Add(coordsToCheck);
                return true;
            }

            if (piece.Side == Side) return false;
            
            availableMoves.Add(coordsToCheck);
            return false;
        }
    }
}
