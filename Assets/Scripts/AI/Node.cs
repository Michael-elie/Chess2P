using System;
using System.Collections.Generic;

using Data;
using Enums;

namespace AI
{
    public class Node
    {
        public int Depth { get; private set; }
        public Node Parent { get; private set; }
        public List<Node> Children { get; private set; }
        public float HeuristicScore { get; private set; }

        public bool IsRoot => Turn == Side.Empty;
        public bool IsTerminal;

        public Piece[,] Grid { get; private set; }
        public Side Turn { get; private set; }
        private Side OpponentTurn => Turn == Side.Light ? Side.Dark : Side.Light;

        public Coordinates Piece { get; private set; }
        public Coordinates Destination { get; private set; }
        
        public Node(Node parent, Side turn, Piece[,] grid = null)
        {
            Turn = turn;
            Parent = parent;
            Children = GenerateChildren();
            HeuristicScore = EvaluateHeuristics();
            Grid = Matrix.DuplicateSnapshot(grid);
        }

        private float EvaluateHeuristics()
        {
            return 0f;
        }

        private List<Node> GenerateChildren()
        {
            List<Node> children = new ();
            
            List<Piece> sidePieces = Matrix.GetAllPieces(Grid, Turn);

            foreach (Piece piece in sidePieces)
            {
                foreach (Coordinates moves in piece.AvailableMoves())
                {
                    Node child = new Node(this, IsRoot ? Side.Light : OpponentTurn, Grid);
                    VirtualPerform(child);
                }
            }
        }
        
        /// <summary>
        /// Take a node, perform the move it's represent and update it's Grid for future reference
        /// </summary>
        /// <param name="node">The Node object to perform the move</param>
        /// <exception cref="ArgumentException"></exception>
        public static void VirtualPerform(Node node)
        {
            Piece origin = node.Grid[node.Piece.Column, node.Piece.Row];
            Piece destination = node.Grid[node.Destination.Column, node.Destination.Row];
            
            if (origin == null || origin.Side != node.Turn)
                throw new ArgumentException("Unexpected origin while Perfom(): origin can't be empty or from the opponent side");
            if (destination is not null && destination.Equals(origin))
                throw new ArgumentException("Unexpected destination while Perform(): destination can't be equals to origin.");
            if (destination is not null && destination.Side == origin.Side)
                throw new ArgumentException("Unexpected destination while Perform(): destination can't be an allied piece.");
            
            node.Grid[node.Destination.Column, node.Destination.Row] = origin;
            node.Grid[node.Destination.Column, node.Destination.Row].Coordinates = node.Destination;
            node.Grid[node.Piece.Column, node.Piece.Row] = null;
        }
    }
}
