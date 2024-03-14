using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

using MonoBehaviours;

namespace Pieces
{
    public abstract class Piece
    {
        public static Dictionary<string, GameObject> Prefabs;

        public Cell Cell;
        public PieceBehaviour Behaviour { get; private set; }
        public Side Side { get; private set; }
        public bool HasMoved { get; set; }
        public abstract int HeuristicScore { get; }
        
        public bool IsTheKing => this.GetType() == typeof(King);
        public bool IsNotTheKing => this.GetType() != typeof(King);
        
        protected Piece(Cell cell, GameObject prefab, Transform root, Side side)
        {
            Cell = cell;
            Quaternion rotation = side == Side.Light ? Quaternion.identity : Quaternion.Euler(0, -180, 0);
            GameObject piece = Object.Instantiate(prefab, cell.Coordinates.World, rotation, root);
            Behaviour = piece.GetComponent<PieceBehaviour>();
            Side = side;
            HasMoved = false;
        }

        public static Piece Create(string prefabName, Cell originCell, Transform root)
        {
            return prefabName switch
            {
                "LightPawn"   => new Pawn   (originCell, Prefabs["LightPawn"], root, Side.Light),
                "LightRook"   => new Rook   (originCell, Prefabs["LightRook"], root, Side.Light),
                "LightKnight" => new Knight (originCell, Prefabs["LightKnight"], root, Side.Light),
                "LightBishop" => new Bishop (originCell, Prefabs["LightBishop"], root, Side.Light),
                "LightQueen"  => new Queen  (originCell, Prefabs["LightQueen"], root, Side.Light),
                "LightKing"   => new King   (originCell, Prefabs["LightKing"], root, Side.Light),
                "DarkPawn"    => new Pawn   (originCell, Prefabs["DarkPawn"], root, Side.Dark),
                "DarkRook"    => new Rook   (originCell, Prefabs["DarkRook"], root, Side.Dark),
                "DarkKnight"  => new Knight (originCell, Prefabs["DarkKnight"], root, Side.Dark),
                "DarkBishop"  => new Bishop (originCell, Prefabs["DarkBishop"], root, Side.Dark),
                "DarkQueen"   => new Queen  (originCell, Prefabs["DarkQueen"], root, Side.Dark),
                "DarkKing"    => new King   (originCell, Prefabs["DarkKing"], root, Side.Dark),
                
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
    }
}
