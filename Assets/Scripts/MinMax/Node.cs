using System.Collections.Generic;

public class Node
{

    private Side _owner;
    private Side _turn;
    private Cell[,] _matrix;

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

            // heuristicValue += cell.Occupant.Side == Owner ? cell.Occupant.HeuristicScore : -cell.Occupant.HeuristicScore;
            
            if (cell.Occupant.Side == _owner)
                heuristicValue += cell.Occupant.HeuristicScore;
            else
                heuristicValue -= cell.Occupant.HeuristicScore;
        }

        return heuristicValue;
    }


    public void IsTerminal()
    {
        throw new System.NotImplementedException();
    }

    public List<Node> GetChilds()
    {
        throw new System.NotImplementedException();
    }
}
