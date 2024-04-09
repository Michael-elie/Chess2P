using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Data.Pieces
{
    public class King : Piece
    {
        public King(Side side, Coordinates coords) : base(side, coords) {}

        public King(King copy) : base(copy) {}

        public override float HeuristicScore
        {
            get
            {
                float baseValue = Mathf.Infinity;
                return baseValue;
            }
        }

        public override List<Coordinates> AvailableMoves(Coordinates coordinates)
        {
            List<Coordinates> availableMoves = new ();
            int currentColumn = coordinates.Column;
            int currentRow = coordinates.Row;

            int[] rowOffsets = { -1, -1, -1,  0, 0,  1, 1, 1 };
            int[] colOffsets = { -1,  0,  1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < Matrix.BoardSize; i++)
            {
                int row = currentRow + rowOffsets[i];
                int column = currentColumn + colOffsets[i];

                Piece piece = Matrix.GetPiece(column, row);
                ValidateCell(availableMoves, new Coordinates(column, row));
            }

            return availableMoves;
        }

        protected override bool ValidateCell(ICollection<Coordinates> availableMoves, Coordinates coordsToCheck)
        {
            if ((coordsToCheck.Column < 0 || coordsToCheck.Row < 0) || (coordsToCheck.Column > 7 || coordsToCheck.Row > 7)) {
                return false;
            }
            
            Piece piece = Matrix.GetPiece(coordsToCheck);
            
            if (piece is not null) // If a piece exist at the provided coords...
            {
                if (piece.Side != Side && piece.IsNotTheKing) availableMoves.Add(coordsToCheck); // ...add the coords to valid moves if it is an opponent's piece.
                return false; // Tell the check system to stop here and don't check behind any piece found anwyay.
            }
            
            availableMoves.Add(coordsToCheck); // No piece are in the coords so these coords are a valid move
            return false; // King to have line of sight.
        }
    }
}
