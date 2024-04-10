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

        public override List<Coordinates> AvailableMoves()
        {
            List<Coordinates> availableMoves = new ();
            int currentColumn = this.Coordinates.Column;
            int currentRow = this.Coordinates.Row;

            for (int row = currentRow + 1; row < Matrix.BoardSize; row++) // Upward
                availableMoves.Add(new Coordinates(currentColumn, currentRow));
            
            for (int row = currentRow - 1; row >= 0; row--) // Downward
                availableMoves.Add(new Coordinates(currentColumn, currentRow));
            
            for (int column = currentColumn + 1; column < Matrix.BoardSize; column++) // Rightward
                availableMoves.Add(new Coordinates(currentColumn, currentRow));

            for (int column = currentColumn - 1; column >= 0; column--) // Leftward
                availableMoves.Add(new Coordinates(currentColumn, currentRow));

            ValidateMoves(availableMoves);
            return availableMoves;
        }
    }
}
