using System;
using System.Collections.Generic;

using Enums;
using Data.Pieces;

namespace Data
{
    public abstract class Piece : IEquatable<Piece>, ICloneable
    {
        public Coordinates Coordinates { get; set; }
        public Side Side { get; set; }
        public bool HasMoved { get; set; }

        public abstract float HeuristicScore { get; }

        public string Type => this.GetType().Name;
        public bool IsTheKing => this.GetType() == typeof(King);
        public bool IsNotTheKing => this.GetType() != typeof(King);
        
        protected Piece(Side side, Coordinates coordinates)
        {
            Coordinates = coordinates;
            Side = side;
            HasMoved = false;
        }

        protected Piece(Piece copy)
        {
            this.Coordinates = copy.Coordinates;
            this.Side = copy.Side;
            this.HasMoved = copy.HasMoved;
        }

        public static Piece Create(string prefabName, Coordinates coordinates)
        {
            return prefabName switch
            {
                "LightPawn"   => new Pawn   (Side.Light, coordinates),
                "LightRook"   => new Rook   (Side.Light, coordinates),
                "LightKnight" => new Knight (Side.Light, coordinates),
                "LightBishop" => new Bishop (Side.Light, coordinates),
                "LightQueen"  => new Queen  (Side.Light, coordinates),
                "LightKing"   => new King   (Side.Light, coordinates),
                "DarkPawn"    => new Pawn   (Side.Dark, coordinates),
                "DarkRook"    => new Rook   (Side.Dark, coordinates),
                "DarkKnight"  => new Knight (Side.Dark, coordinates),
                "DarkBishop"  => new Bishop (Side.Dark, coordinates),
                "DarkQueen"   => new Queen  (Side.Dark, coordinates),
                "DarkKing"    => new King   (Side.Dark, coordinates),
                
                _ => throw new ArgumentOutOfRangeException(prefabName, "Invalid piece name provided for Creation")
            };
        }
        
        public abstract List<Coordinates> AvailableMoves(Coordinates coords);
        
        protected virtual bool ValidateCell(ICollection<Coordinates> availableMoves, Coordinates coordsToCheck)
        {
            Piece piece = Matrix.GetPiece(coordsToCheck);
            
            if (piece is not null) // If a piece exist at the provided coords...
            {
                if (piece.Side != Side) availableMoves.Add(coordsToCheck); // ...add the coords to valid moves if it is an opponent's piece.
                return false; // Tell the check system to stop here and don't check behind any piece found anwyay.
            }
            
            availableMoves.Add(coordsToCheck); // No piece are in the coords so these coords are a valid move
            return true; // Line of sight isn't blocked so the system can continue.
        }

        #region Equality and Copy

        public object Clone()
        {
            return MemberwiseClone();
        }

        public bool Equals(Piece other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Coordinates.Equals(other.Coordinates) && Side == other.Side && HasMoved == other.HasMoved && HeuristicScore.Equals(other.HeuristicScore);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Piece)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(HeuristicScore);
        }

        #endregion
    }
}
