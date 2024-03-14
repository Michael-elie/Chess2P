using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public class King : Piece
    {
        public King(Cell cell, GameObject prefab, Transform root, Side side) : base(cell, prefab, root, side) {}
        public override int HeuristicScore => 20;

        public override List<Cell> AvailableMoves()
        {
            List<Cell> availableMoves = new List<Cell>();
            int currentColumn = this.Cell.Coordinates.Columns;
            int currentRow = this.Cell.Coordinates.Row;

            int[] rowOffsets = { -1, -1, -1,  0, 0,  1, 1, 1 };
            int[] colOffsets = { -1,  0,  1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < Matrix.BoardSize; i++)
            {
                int row = currentRow + rowOffsets[i];
                int column = currentColumn + colOffsets[i];

                Cell cell = Matrix.GetCell(column, row);
                ValidateCell(availableMoves, cell);
            }

            return availableMoves;
        }

        protected override bool ValidateCell(List<Cell> availableMoves, Cell cell)
        {
            if (cell is null) return false; // King will check out-of-board cells, skipping them

            if (!cell.IsOccupied || (cell.IsOccupied && cell.Occupant.Side != Side && cell.Occupant.IsNotTheKing))
            {
                availableMoves.Add(cell);
                return true;
            }

            return false;
        }
    }
}
