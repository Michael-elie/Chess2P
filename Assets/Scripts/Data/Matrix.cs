﻿using System;
using System.Collections.Generic;
using System.Linq;

using Managers;
using Enums;
using Unity.VisualScripting;

namespace Data
{
    public static class Matrix
    {
        public const int BoardSize = 8;
        public static readonly Piece[,] Grid = new Piece[BoardSize, BoardSize];

        public static void Init()
        {
            string[] pieceOrder = { "Rook", "Knight", "Bishop", "Queen", "King", "Bishop", "Knight", "Rook" };

            for (int column = 0; column < Matrix.BoardSize; column++)
            {
                Grid[column, 0] = Piece.Create("Light" + pieceOrder[column], new Coordinates(column, 0));
                Grid[column, 1] = Piece.Create("LightPawn", new Coordinates(column, 1));
                Grid[column, 7] = Piece.Create("Dark" + pieceOrder[column], new Coordinates(column, 7));
                Grid[column, 6] = Piece.Create("DarkPawn", new Coordinates(column, 6));
            }
        }

        /// <summary>
        /// Request a unique Cell from the original Grid array by supplying it's name
        /// </summary>
        /// <param name="cellName">Cell's GameObject name</param>
        /// <returns></returns>
        public static Piece GetPiece(string cellName)
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
        public static Piece GetPiece(int column, int row)
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
        public static Piece GetPiece(Piece[,] grid, int column, int row)
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
        public static List<Piece> GetAllPieces()
        {
            List<Piece> allPieces = new();
        
            for (int column = 0; column < BoardSize; column++)
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    allPieces.Add(Grid[column, row]);
                }
            }

            return allPieces;
        }
    
        /// <summary>
        /// Return all Cells objects from the "snapshot" grid as a continous list.<br/>
        /// <b>Notice:</b> Doesn't filter empty Cells
        /// </summary>
        /// <returns></returns>
        public static List<Piece> GetAllPieces(Piece[,] grid)
        {
            List<Piece> allPieces = new();
        
            for (int column = 0; column < BoardSize; column++)
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    allPieces.Add(Grid[column, row]);
                }
            }

            return allPieces;
        }

        /// <summary>
        /// Request all Cells from the original Grid and filters in Pieces from a specified "side".
        /// </summary>
        /// <returns></returns>
        public static List<Piece> GetPiecesFromSide(Side side)
        {
            List<Piece> pieces = new();
        
            for (int column = 0; column < BoardSize; column++)
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    Piece piece = Grid[column, row];
                    if (piece != null && piece.Side == side)
                        pieces.Add(piece);
                }
            }

            return pieces;
        }
    
        /// <summary>
        /// Request all Cells from the specified "snapshot" grid and filter in Pieces from a specified "side".
        /// </summary>
        /// <returns></returns>
        public static List<Piece> GetPiecesFromSide(Piece[,] grid, Side side)
        {
            List<Piece> pieces = new();
        
            for (int column = 0; column < BoardSize; column++)
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    Piece piece = grid[column, row];
                    if (piece != null && piece.Side == side)
                        pieces.Add(piece);
                }
            }

            return pieces;
        }

        /// <summary>
        /// Utitlity method to request only the King from the supplied "side" inside the original Grid 
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static Piece GetKing(Side side)
        {
            Piece piece;
        
            for (int column = 0; column < BoardSize; column++)
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    piece = Grid[column, row];
                    if (piece != null && piece.IsTheKing && piece.Side == side)
                        return piece;
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
        public static Piece GetKing(Piece[,] grid, Side side)
        {
            List<Piece> piece = GetPiecesFromSide(grid, side);

            try
            {
                return piece.First(cell => cell is not null && cell.IsTheKing);
            }
            catch (SystemException exception)
            {
                UnityEngine.Debug.LogError($"Error: Unable to get the {side.ToString()} King. A piece may have bypassed all safeguard and took the King out");
                throw;
            }
        }

        public static List<Piece> GetMoves(Piece piece)
        {
            return piece.AvailableMoves(piece.Coordinates);
        }

        public static Piece[,] GetCurrentGridSnapshot() // Deep Copy
        {
            Piece[,] snapshot = new Piece[BoardSize, BoardSize];

            for (int column = 0; column < BoardSize; column++)
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    snapshot[column, row] = (Piece) Grid[column, row].Clone();
                }
            }

            return snapshot;
        }

        public static Piece[,] DuplicateSnapshot(Piece[,] snapshot)
        {
            Piece[,] duplicate = new Piece[BoardSize, BoardSize];

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    duplicate[row, col] = (Piece) snapshot[row, col].Clone();
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
                    GameManager.GetPieceBehaviour(Grid[column, row]).Cell.IsTargetable(!Grid[column, row].IsEmpty);
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
                    rowCells += GameManager.GetPieceBehaviour(GetPiece(column, row)).Name + " ";
                }
                
                debug += rowCells + "\n";
            }
            UnityEngine.Debug.Log(debug);
        }

        #endregion
    }
}
