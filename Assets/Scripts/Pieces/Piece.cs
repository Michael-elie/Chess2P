using System;
using System.Collections.Generic;
using UnityEngine;

using Data;
using Enums;

namespace Pieces
{
    public abstract class Piece : IEquatable<Piece>
    {
        public static Dictionary<string, GameObject> Prefabs;

        public Cell Cell;
        public Side Side { get; }
        public bool HasMoved { get; set; }
        public abstract int HeuristicScore { get; }
        
        public bool IsTheKing => this.GetType() == typeof(King);
        public bool IsNotTheKing => this.GetType() != typeof(King);
        
        protected Piece(Cell cell, Side side)
        {
            Cell = cell;
            Side = side;
            HasMoved = false;
        }

        public static Piece Create(string prefabName, Cell originCell)
        {
            return prefabName switch
            {
                "LightPawn"   => new Pawn   (originCell, Side.Light),
                "LightRook"   => new Rook   (originCell, Side.Light),
                "LightKnight" => new Knight (originCell, Side.Light),
                "LightBishop" => new Bishop (originCell, Side.Light),
                "LightQueen"  => new Queen  (originCell, Side.Light),
                "LightKing"   => new King   (originCell, Side.Light),
                "DarkPawn"    => new Pawn   (originCell, Side.Dark),
                "DarkRook"    => new Rook   (originCell, Side.Dark),
                "DarkKnight"  => new Knight (originCell, Side.Dark),
                "DarkBishop"  => new Bishop (originCell, Side.Dark),
                "DarkQueen"   => new Queen  (originCell, Side.Dark),
                "DarkKing"    => new King   (originCell, Side.Dark),
                
                _ => throw new ArgumentOutOfRangeException(prefabName, "Invalid piece name provided for Creation")
            };
        }
        
        public abstract List<Cell> AvailableMoves();
        
        protected virtual bool ValidateCell(List<Cell> availableMoves, Cell cell)
        {
            if (cell is null) throw new IndexOutOfRangeException($"{GetType().Name} shouldn't try to validate any cell out of board !");

            if (cell.IsOccupied)
            {
                if (cell.Occupant.Side != Side)
                    availableMoves.Add(cell);

                return false;
            }

            availableMoves.Add(cell);
            return true;
        }


        public bool Equals(Piece other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true; // If commented: Only test equivalence
            return Cell.Equals(other.Cell) && Side == other.Side && HasMoved == other.HasMoved;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true; // If commented: Only test equivalence
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Piece)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)Side);
        }
    }
}
