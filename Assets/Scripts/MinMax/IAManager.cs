using Data;
using Enums;
using Managers;
using UnityEngine;

public class IAManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [ContextMenu("Think")]
    public void Think()
    {
        
        Node firstNode = new Node(Side.Light, Matrix.GetCurrentGridSnapshot(),new Coordinates(-1,-1),new Coordinates(-1,-1));
        Node bestChild = null;
         
        foreach (Node child in firstNode.GetChilds()) // first Getchilds for get the cell destination
        {
           float score = MinMax(firstNode, 1, false);
            if (bestChild == null || score > bestChild.GetHeuristicValue()) {
                bestChild = child;
            }
            else if (score == bestChild.GetHeuristicValue() && Random.Range(0,1f) == 0)
            {
                bestChild = child;
            }
            
            _gameManager.PerformMovement(bestChild.InitialPosition,bestChild.Direction);
            
            
        }
         
      
        MinMax(firstNode, 2, true);
    }

    private float MinMax(Node node, int depth, bool maximizingPlayer)
    {
        if (depth == 0 || node.IsTerminal() ) {
            node.GetHeuristicValue();
        }

        float EvalValue;
        if (maximizingPlayer)
        { 
            EvalValue = int.MaxValue;
            foreach (Node child in node.GetChilds())
            {
                EvalValue = Mathf.Max(EvalValue, MinMax(child, depth - 1, false));
            }
        }
        else // minimizingPlayer
        {
            EvalValue = int.MinValue;
            foreach (Node child in node.GetChilds())
            {
                EvalValue =  Mathf.Min(EvalValue, MinMax(child, depth - 1, true));
            }
        }
        return EvalValue;
    }
}

