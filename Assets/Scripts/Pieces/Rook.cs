using System.Collections.Generic;
using Data;
using Enums;

namespace Pieces
{
    public class Rook : Piece
    {
        public Rook(Cell cell, Side side) : base(cell, side) {}

        public override int HeuristicScore => 5;
        
        public override List<Cell> AvailableMoves()
        {
            List<Cell> availableMoves = new List<Cell>();
            int currentColumn = this.Cell.Coordinates.Column;
            int currentRow = this.Cell.Coordinates.Row;

            for (int row = currentRow + 1; row < Matrix.BoardSize; row++) // Upward
            {
                Cell upwardCell = Matrix.GetCell(currentColumn, row);
                if (!ValidateCell(availableMoves, upwardCell))
                    break;
            }
            
            for (int row = currentRow - 1; row >= 0; row--) // Downward
            {
                Cell downwardCell = Matrix.GetCell(currentColumn, row);
                if (!ValidateCell(availableMoves, downwardCell))
                    break;
            }
            
            for (int column = currentColumn + 1; column < Matrix.BoardSize; column++) // Rightward
            {
                Cell rightwardCell = Matrix.GetCell(column, currentRow);
                if (!ValidateCell(availableMoves, rightwardCell))
                    break;
            }

            for (int column = currentColumn - 1; column >= 0; column--) // Leftward
            {
                Cell leftwardCell = Matrix.GetCell(column, currentRow);
                if (!ValidateCell(availableMoves, leftwardCell))
                    break;
            }

            return availableMoves;
        }
    }
}
