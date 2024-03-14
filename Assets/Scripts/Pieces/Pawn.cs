using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public class Pawn : Piece
    {
        public Pawn(Cell cell, GameObject prefab, Transform root, Side side) : base(cell, prefab, root, side) {}
        
        public override int HeuristicScore => 1;
        
        public override List<Cell> AvailableMoves()
        {
            List<Cell> moves = new List<Cell>();
            int currentColumn = this.Cell.Coordinates.Columns;
            int currentRow = this.Cell.Coordinates.Row;
            int offset = (Side == Side.Light) ? 1 : -1;
            
            Cell forward = Matrix.GetCell(currentColumn, currentRow + offset);
            Cell forwardLeft = Matrix.GetCell(currentColumn - 1, currentRow + offset);
            Cell forwardRight = Matrix.GetCell(currentColumn + 1, currentRow + offset);
            Cell forwardPush = Matrix.GetCell(currentColumn, currentRow + offset * 2);
            
            if (forwardLeft is { IsOccupied: true } && forwardLeft.Occupant.Side != Side)
                moves.Add(forwardLeft);

            if (forwardRight is { IsOccupied: true } && forwardRight.Occupant.Side != Side)
                moves.Add(forwardRight);

            if (forward is { IsOccupied: false })
                moves.Add(forward);
            else
                return moves;
            
            if (forwardPush is { IsOccupied: false } && !HasMoved)
                moves.Add(forwardPush);
            
            return moves;
        }
    }
}
