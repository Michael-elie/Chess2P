﻿using UnityEngine;

namespace Pieces
{
    public class Queen : Piece
    {
        public Queen(Cell cell, GameObject prefab, Transform root, Side side) : base(cell, prefab, root, side) {}
        
        public override void Move()
        {
            throw new System.NotImplementedException();
        }
    }
}