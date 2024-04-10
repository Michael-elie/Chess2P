using System.Collections.Generic;
using Data;
using Enums;
using Managers;
using UnityEngine.Rendering;

namespace MinMax
{
   public class Node
   {
       public Side OpponentTurn => (GameManager.CurrentPlayerTurn == Side.Light ? Side.Dark : Side.Light);
       private Side _owner;
       private Piece[,] _matrix;
       private Coordinates _direction;
       private Coordinates _initialPosition;
   
       public Coordinates Direction
       {
           get => _direction;
           set => _direction = value;
       }
       public Coordinates InitialPosition
       {
           get => _initialPosition;
           set => _initialPosition = value;
       }
       
   
       public Node(Side owner, Piece[,] matrix,Coordinates moveDirection, Coordinates initialPosition)
       {
           _owner = owner;
           _matrix = matrix;
           _direction = moveDirection;
           _initialPosition = initialPosition; 
       }
   
       public float GetHeuristicValue()
       {
           float heuristicValue = 0;
           foreach (Piece piece in _matrix)
           {
               if (piece == null)continue;//There is no piece
               heuristicValue += piece.Side == _owner ? piece.HeuristicScore : -piece.HeuristicScore;
           }
   
           return heuristicValue;
       }
   
     public bool IsTerminal()
       {
           return GetChilds().Count == 0; 
       }
   
     public List<Node> GetChilds() {
         
         List<Node> nodeList = new List<Node>();
         List<Piece> alliedPieces = Matrix.GetAllPieces(_matrix, _owner);

         foreach (Piece piece in alliedPieces)
         {
             if (piece == null) continue; // There is no piece
             // 1) Get the Available moves 
            foreach ( Coordinates move in piece.AvailableMoves() ) // For each moves this piece can achieve
            {
                Piece[,] newmatrix = Matrix.DuplicateSnapshot(_matrix); // 2) Duplicate the matrix  
                Matrix.VirtualPerform(newmatrix, _owner,piece.Coordinates,move);
                
                Node childNode = new Node(OpponentTurn, newmatrix,move,piece.Coordinates); // 4) create new node
                nodeList.Add(childNode); // 5) Add node in the Child List 
            }
             
         }
         return nodeList;
     }
   
   } 
}
