using System;
using Pieces;

namespace Data
{
    public class Cell: IEquatable<Cell>
    {
        public Coordinates Coordinates { get; }
        public Piece Occupant { get; set; }

        public bool IsOccupied => Occupant != null;

        public Cell(int column, int row) {
            Coordinates = new Coordinates(column, row);
        }

        public Cell(Cell copy) {
            this.Coordinates = new Coordinates(copy.Coordinates.Column, copy.Coordinates.Row);
        }

        //TODO: NUllReferenceException here. Unknow reason yet. Stack Trace is unchanged and calls comes from HandleCheck() and may implies Matrix.GetCurrentGridSnapshot()
        public bool Equals(Cell other) 
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true; // If commented: Equals won't return true if the same reference is shared, it will only test equivalence
            return Coordinates.Equals(other.Coordinates) && Occupant.Equals(other.Occupant);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Cell)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Coordinates);
        }
    }
}
