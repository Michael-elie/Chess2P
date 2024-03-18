using System;
using System.Collections.Generic;
using Data;
using Enums;
using Managers;

public class Node
{

    private Side _owner ;
    private Side _turn;
    public Cell[,] _matrix;

    public Node(Side owner, Side turn, Cell[,] matrix)
    {
        _owner = owner;
        _turn = turn;
        _matrix = matrix;
    }

    public int GetHeuristicValue()
    {
        int heuristicValue = 0;
        foreach (Cell cell in _matrix)
        {
            // There is no cell
            if (cell == null) continue;
            // There is not piece on the cell
            if (cell.Occupant == null) continue;
            
            heuristicValue += cell.Occupant.Side == _owner ? cell.Occupant.HeuristicScore : -cell.Occupant.HeuristicScore; 
            //ou 
           /* if (cell.Occupant.Side == _owner)
                heuristicValue += cell.Occupant.HeuristicScore;
            else
                heuristicValue -= cell.Occupant.HeuristicScore;*/
        }

        return heuristicValue;
    }


    public bool IsTerminal()
    {
        return GetChilds().Count == 0;
    }

    public List<Node> GetChilds()
    {
        List<Node> nodeList = new List<Node>();
        
        foreach (Cell cell in _matrix)
        {
            // There is no cell
            if (cell == null) continue;
            // There is not piece on the cell
            if (cell.Occupant == null) continue;
            
            // 1) Get the Available moves 
            foreach (Cell move in cell.Occupant.AvailableMoves()) // For each moves this piece can achieve
            {
                Cell[,] newMatrix =  Matrix.DuplicateSnapshot(_matrix);  // 2) Duplicate the matrix  
                GameManager.VirtualResolve(newMatrix, cell, move); // 3) Play Virtual Moves
                
                Side nexturn = _turn == Side.Light ? Side.Dark : Side.Light; 
                Node childNode = new Node(_owner, nexturn, newMatrix); // 4) create new node
                nodeList.Add(childNode); // 5) Add node in the Child List 
            }
        }
        return nodeList;
    }
}
