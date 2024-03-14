using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public class Queen : Piece
    {
        public Queen(Cell cell, GameObject prefab, Transform root, Side side) : base(cell, prefab, root, side) {}

        public override int HeuristicScore => 10;
        
        public override List<Cell> AvailableMoves()
        {
            List<Cell> availableMoves = new List<Cell>();
            int currentColumn = this.Cell.Coordinates.Columns;
            int currentRow = this.Cell.Coordinates.Row;
            
            GetAlignedCells(availableMoves, currentColumn, currentRow);
            GetDiagonalCells(availableMoves, currentColumn, currentRow);

            return availableMoves;
        }
        
        private void GetDiagonalCells(List<Cell> availableMoves, int currentColumn, int currentRow)
        {
            for (int column = currentColumn + 1, row = currentRow + 1; row < Matrix.BoardSize && column < Matrix.BoardSize; row++, column++) // Upward-right
            {
                Cell cell = Matrix.GetCell(column, row);
                if (!ValidateCell(availableMoves, cell))
                    break;
            }
            
            for (int column = currentColumn - 1, row = currentRow + 1; row < Matrix.BoardSize && column >= 0; row++, column--) // Upward-left
            {
                Cell cell = Matrix.GetCell(column, row);
                if (!ValidateCell(availableMoves, cell))
                    break;
            }

            for (int column = currentColumn + 1, row = currentRow - 1; row >= 0 && column < Matrix.BoardSize; row--, column++) // Downward-right
            {
                Cell cell = Matrix.GetCell(column, row);
                if (!ValidateCell(availableMoves, cell))
                    break;
            }

            for (int column = currentColumn - 1, row = currentRow - 1; row >= 0 && column >= 0; row--, column--) // Downward-left
            {
                Cell cell = Matrix.GetCell(column, row);
                if (!ValidateCell(availableMoves, cell))
                    break;
            }
        }
        
        private void GetAlignedCells(List<Cell> availableMoves, int currentColumn, int currentRow)
        {
            for (int i = currentRow + 1; i < Matrix.BoardSize; i++) // Upward
            {
                Cell upwardCell = Matrix.GetCell(currentColumn, i);
                if (!ValidateCell(availableMoves, upwardCell))
                    break;
            }
            
            for (int i = currentRow - 1; i >= 0; i--) // Downward
            {
                Cell downwardCell = Matrix.GetCell(currentColumn, i);
                if (!ValidateCell(availableMoves, downwardCell))
                    break;
            }
            
            for (int i = currentColumn + 1; i < Matrix.BoardSize; i++) // Rightward
            {
                Cell rightwardCell = Matrix.GetCell(i, currentRow);
                if (!ValidateCell(availableMoves, rightwardCell))
                    break;
            }

            for (int i = currentColumn - 1; i >= 0; i--) // Leftward
            {
                Cell leftwardCell = Matrix.GetCell(i, currentRow);
                if (!ValidateCell(availableMoves, leftwardCell))
                    break;
            }
        }
    }
}
