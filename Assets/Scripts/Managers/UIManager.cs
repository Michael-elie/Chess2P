using Data;
using Enums;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public TMP_Text TurnPanel;

        public static TMP_Text Turn;

        private void Awake()
        {
            Turn = TurnPanel;
        }

        public static void UpdateTurn(Side side)
        {
            Turn.text = "Turn : " + side;
            Turn.color = side == Side.Light ? Color.white : Color.gray;
        }
    }
}
