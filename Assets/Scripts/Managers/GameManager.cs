using System;
using System.Collections;
using UnityEngine;

using View;
using Data;
using Enums;
using Unity.VisualScripting;
using UnityEngine.Rendering;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Settings")] public Board Board;
        //[Range(1, 8)] public int AIDepth;
        public bool EnableStepByStep;

        public static Side CurrentPlayerTurn { get; private set; }
        public static Side OpponentTurn => (CurrentPlayerTurn == Side.Light ? Side.Dark : Side.Light);

        public static Action<Side> OnMoveMade;

        public static bool Checkmate { get; set; }
        public static bool Check { get; set; }

        private static bool _updateOnce;

        private bool _confirmEscape;
        private float _escapeTimer = 3f;

        private void Awake()
        {
            CurrentPlayerTurn = Side.Light;
            Checkmate = false;
        }

        private void Start()
        {
            
            // StartCoroutine(StartGameLoop());
        }

        private void Update()
        {
            CheckEmergencyEscape(Input.GetKeyDown(KeyCode.Escape));

            if (_updateOnce)
            {
                UIManager.UpdateTurn(CurrentPlayerTurn);
                _updateOnce = false;
            }
        }

        #region Gameplay

        private IEnumerator StartGameLoop()
        {
            while (!Checkmate) // Tant que la partie est pas finie
            {
                // Ask AI to think
                // Wait until completion // yield return new WaitUntil(() => Think);
                // Perform move
                // Update Board

                if (EnableStepByStep) // Si mode pas-à-pas
                {
                    yield return new WaitUntil(() => Input.GetButtonDown("Submit") || Input.GetButtonDown("Jump"));
                }
                else
                {
                    yield return new WaitForSeconds(2);
                }

                ChangeTurn();
            }
        }

        public void PerformMovement(Coordinates origin, Coordinates destination)
        {
            Matrix.Perform(CurrentPlayerTurn, origin, destination);
            Board.UpdateView();
        }
        

        private void ChangeTurn()
        {
            CurrentPlayerTurn = OpponentTurn;
        }
        
        
   

#endregion

        #region AI

        
       /* [ContextMenu("Think")]
        public void Think()
        {
            Node node = new Node(Matrix.Grid, CurrentPlayerTurn);
            
            foreach (Piece piece in node.Grid)
            {
                if (piece == null) continue;
                if (piece.Side != CurrentPlayerTurn) continue;
                
                foreach (Piece availableMove in piece.AvailableMoves(piece.Coordinates))
                {
                    
                }
            }
        }*/
        

        #endregion
        
        #region Utils

        private void CheckEmergencyEscape(bool triggered)
        {
            if (triggered && _confirmEscape)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
                UnityEditor.EditorApplication.ExitPlaymode();
#endif
                Application.Quit();
            }
                
            if (triggered) {
                _confirmEscape = true;
            }

            if (_escapeTimer <= 0f)
                _escapeTimer = 3f;

            _escapeTimer -= Time.deltaTime;
        }

        #endregion
    }
}
