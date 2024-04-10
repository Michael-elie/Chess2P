using System.Collections.Generic;
using Enums;

namespace Data.Pieces
{
    public class Knight : Piece
    {
        public Knight(Side side, Coordinates coords) : base(side, coords) {}
        
        public Knight(Knight copy) : base(copy) {}
        
        public override float Heuristic
        {
            get
            {
                float baseValue = 3.20f;
                return baseValue;
            }
        }

        public override List<Coordinates> AvailableMoves()
        {
            List<Coordinates> availableMoves = new ();
            int currentColumn = this.Coordinates.Column;
            int currentRow = this.Coordinates.Row;

            int[] columnsOffsets = { 1, 2,  2,  1, -1, -2, -2, -1 };
            int[] rowOffsets     = { 2, 1, -1, -2, -2, -1,  1,  2 };

            for (int i = 0; i < Matrix.BoardSize; i++)
            {
                int column = currentColumn + columnsOffsets[i];
                int row = currentRow + rowOffsets[i];
                
                availableMoves.Add(new Coordinates(column, row));
            }

            ValidateMoves(availableMoves);
            return availableMoves;
        }
    }
}
