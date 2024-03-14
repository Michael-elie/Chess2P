using Managers;
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
         MinMax(firstNode, 2, true);
      }

      public void MinMax(Node node, int depth, bool maximizingPlayer)
      {
         node.IsTerminal();
         node.GetHeuristicValue();
         foreach (Node child in node.GetChilds())
         {
            
         }
         node.GetChilds();
      }
   }
}
