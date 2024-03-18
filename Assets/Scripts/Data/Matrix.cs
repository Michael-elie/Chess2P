using System;
using System.Collections.Generic;
using Enums;
using Managers;
using UnityEngine.Assertions;

namespace Data
{
    public static class Matrix
    {
        public const int BoardSize = 8;
        private static readonly Cell[,] Grid = new Cell[BoardSize, BoardSize];

        public static void Init()
        {
            for (int column = 0; column < BoardSize; column++)
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    Grid[column, row] = new Cell(column, row);
                }
            }
        }

        /// <summary>
        /// Request a unique Cell from the original Grid array by supplying it's name
        /// </summary>
        /// <param name="cellName">Cell's GameObject name</param>
        /// <returns></returns>
        public static Cell GetCell(string cellName)
        {
            char columnLetter = cellName[0];
            int row = int.Parse(cellName[1..]) - 1;
            int column = columnLetter - 'A';

            if (column is < 0 or > 7) return null;
            if (row is < 0 or > 7) return null;

            return Grid[column, row];
        }
    
        /// <summary>
        /// Request a unique Cell from the original Grid array tied to the chess board
        /// </summary>
        /// <param name="column">Coordinates component between 0 and 7</param>
        /// <param name="row">Coordinates component between 0 and 7</param>
        /// <returns></returns>
        public static Cell GetCell(int column, int row)
        {
            if (column is < 0 or > 7) return null;
            if (row is < 0 or > 7) return null;
        
            return Grid[column, row];
        }

        /// <summary>
        /// Request a unique Cell from the supplied "snapshot" matrix
        /// </summary>
        /// <param name="grid">The grid to request</param>
        /// <param name="column">Coordinates component between 0 and 7</param>
        /// <param name="row">Coordinates component between 0 and 7</param>
        /// <returns></returns>
        public static Cell GetCell(Cell[,] grid, int column, int row)
        {
            if (column is < 0 or > 7) return null;
            if (row is < 0 or > 7) return null;
        
            return grid[column, row];
        }

        /// <summary>
        /// Return all Cells objects from the original Grid as a continous list.<br/>
        /// <b>Notice:</b> Doesn't filter empty Cells
        /// </summary>
        /// <returns></returns>
        public static List<Cell> GetAllCells()
        {
            List<Cell> allCells = new();
        
            for (int column = 0; column < BoardSize; column++)
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    allCells.Add(Grid[column, row]);
                }
            }

            return allCells;
        }
    
        /// <summary>
        /// Return all Cells objects from the "snapshot" grid as a continous list.<br/>
        /// <b>Notice:</b> Doesn't filter empty Cells
        /// </summary>
        /// <returns></returns>
        public static List<Cell> GetAllCells(Cell[,] grid)
        {
            List<Cell> allCells = new();
        
            for (int column = 0; column < BoardSize; column++)
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    allCells.Add(Grid[column, row]);
                }
            }

            return allCells;
        }

        /// <summary>
        /// Request all Cells from the original Grid and filters in Pieces from a specified "side".
        /// </summary>
        /// <returns></returns>
        public static List<Cell> GetPieceCells(Side side)
        {
            List<Cell> pieceCells = new();
        
            for (int column = 0; column < BoardSize; column++)
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    Cell cell = Grid[column, row];
                    if (cell.IsOccupied && cell.Occupant.Side == side)
                        pieceCells.Add(cell);
                }
            }

            return pieceCells;
        }
    
        /// <summary>
        /// Request all Cells from the specified "snapshot" grid and filter in Pieces from a specified "side".
        /// </summary>
        /// <returns></returns>
        public static List<Cell> GetPieceCells(Cell[,] grid, Side side)
        {
            List<Cell> pieceCells = new();
        
            for (int column = 0; column < BoardSize; column++)
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    Cell cell = grid[column, row];
                    if (cell.IsOccupied && cell.Occupant.Side == side)
                        pieceCells.Add(cell);
                }
            }

            return pieceCells;
        }

        /// <summary>
        /// Utitlity method to request only the King from the supplied "side" inside the original Grid 
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static Cell GetKing(Side side)
        {
            Cell cell;
        
            for (int column = 0; column < BoardSize; column++)
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    cell = Grid[column, row];
                    if (cell.IsOccupied && cell.Occupant.IsTheKing && cell.Occupant.Side == side)
                        return cell;
                }
            }

            throw new NullReferenceException($"Error: Unable to get the {side.ToString()} King. A piece may have bypassed all safeguard and took the King out");
        }

        /// <summary>
        /// Utitlity method to request only the King from the supplied "side" inside a specific "snapshot" grid 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static Cell GetKing(Cell[,] grid, Side side)
        {
            Cell cell;

            if (grid is not { Length: BoardSize * BoardSize }) throw new ArgumentException("Grid snapshot is not valid at that point");
        
            for (int column = 0; column < BoardSize; column++)
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    cell = grid[column, row];
                    if (cell.IsOccupied && cell.Occupant.IsTheKing && cell.Occupant.Side == side)
                        return cell;
                }
            }

            throw new NullReferenceException($"Error: Unable to get the {side.ToString()} King. A piece may have bypassed all safeguard and took the King out");
        }

        public static List<Cell> GetMoves(Cell cell)
        {
            return cell.Occupant.AvailableMoves();
        }

        public static Cell[,] GetCurrentGridSnapshot() // Deep Copy
        {
            Cell[,] snapshot = new Cell[BoardSize, BoardSize];

            for (int column = 0; column < BoardSize; column++)
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    snapshot[column, row] = new Cell(Grid[column, row]);
                    if (!snapshot[column, row].Equals(Grid[column, row])) throw new Exception("Comparison fails: Cell duplication compromised copy from the source.");
                }
            }

            return snapshot;
        }

        public static Cell[,] DuplicateSnapshot(Cell[,] snapshot)
        {
            Cell[,] duplicate = new Cell[BoardSize, BoardSize];

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    duplicate[row, col] = new Cell(snapshot[row, col]);
                }
            }

            return duplicate;
        }
    
        public static void ResetCellsTargetState()
        {
            for (int column = 0; column < BoardSize; column++)
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    GameManager.GetBehaviourCell(Grid[column, row]).IsTargetable(Grid[column, row].IsOccupied);
                }
            }
        }

        #region Debug

        /// <summary>
        /// 
        /// </summary>
        public static void Debug()
        {
            string debug = "Debug Matrix :\n\n";
            for (int column = 0; column < BoardSize; column++)
            {
                string rowCells = "";
                for (int row = 0; row < BoardSize; row++)
                {
                    rowCells += GameManager.GetBehaviourCell(GetCell(column, row)).Name + " ";
                }
                
                debug += rowCells + "\n";
            }
            UnityEngine.Debug.Log(debug);
        }

        #endregion
    }
}
