using System;
using UnityEngine;

namespace Data
{
    public struct Coordinates: IEquatable<Coordinates>
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Vector3 World => new Vector3(Column, 0.001f, Row);
    
        public Coordinates(int column, int row)
        {
            Row = row;
            Column = column;
        }

        public bool Equals(Coordinates other)
        {
            return Row == other.Row && Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            return obj is Coordinates other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }
    }
}
