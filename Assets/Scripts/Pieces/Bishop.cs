﻿using UnityEngine;

namespace Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Cell cell, GameObject prefab, Transform root, Side side) : base(cell, prefab, root, side) {}
        
        public override void Move()
        {
            throw new System.NotImplementedException();
        }
    }
}