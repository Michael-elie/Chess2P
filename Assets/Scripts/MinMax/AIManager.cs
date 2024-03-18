using System;
using Data;
using Managers;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace MinMax
{
   public class AIManager : MonoBehaviour
   {
      [ContextMenu("Think")]
      public void Think()
      {
         Node firstNode = new Node(GameManager.CurrentPlayerTurn, GameManager.CurrentPlayerTurn,
            Matrix.GetCurrentGridSnapshot());
         
         foreach (Node child in firstNode.GetChilds()) // first getchilds for get the cell destination
         {
            MinMax(firstNode, 1, false);
            //child._matrix = 
         }
         
         MinMax(firstNode, 2, true);
      }

      private int MinMax(Node node, int depth, bool maximizingPlayer)
      {
         if (depth == 0 || node.IsTerminal() )
         {
            node.GetHeuristicValue();
         }

         int EvalValue;
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
}
