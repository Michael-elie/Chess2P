using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Data;
using Enums;
using View;
using Pieces;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static Cell _origin;
        private static Cell _destination;
        private static List<Cell> _moves;
        private static bool _updateOnce;

        private static List<Cell> _checkResponsibles = new ();
        private static Dictionary<Piece, List<Cell>> _validCheckMoves = new ();

        private bool _confirmEscape;
        private float _escapeTimer = 3f;
        public static Side CurrentPlayerTurn { get; private set; }
        public static Side OpponentTurn => (CurrentPlayerTurn == Side.Light ? Side.Dark : Side.Light);
        public static bool Checkmate { get; set; }
        public static bool Check { get; set; }

        #region Data Link

        public static Cell GetDataCell(CellBehaviour viewCell)
        {
            return Matrix.GetCell(viewCell.Coordinates.Row, viewCell.Coordinates.Column);
        }

        public static CellBehaviour GetBehaviourCell(Cell dataCell)
        {
            if (dataCell == null)
                throw new Exception($"Unexistant Cell request in Grid");
                    
            return Board.CellBehaviours.First(cell => cell.Coordinates.Equals(dataCell.Coordinates));
        }

        #endregion
        
        #region Interactions

        private void Awake()
        {
            CurrentPlayerTurn = Side.Light;
            Checkmate = false;
        }

        private void Update()
        {      
            if (!_updateOnce) {
                UIManager.UpdateTurn(CurrentPlayerTurn);
                _updateOnce = true;
            }
                
            if (Input.GetButtonDown("Fire2"))
                Unselect();

            EmergencyEscape(Input.GetKeyDown(KeyCode.Escape));
        }

        private void EmergencyEscape(bool triggered)
        {
            if (triggered && _confirmEscape)
            {
#if UNITY_EDITOR
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

        #region Game Logic

        #region Cell Selection

        public static void SelectCell(Cell cell)
        {
            if (Check)
                CheckSelection(cell);
            else
                RegularSelection(cell);
        }

        private static void CheckSelection(Cell cell)
        {
            // If no piece have been selected, checks if the current selected is part of the Pieces of the valid moves Dictionnary
            if (_origin == null && _validCheckMoves.ContainsKey(cell.Occupant))
            {
                SetOrigin(cell);
            }
            // If a piece have been selected to be played, verify if the destination matches one of the valid moves list from that Piece in the Dictionnary;
            else if (_origin != null && _validCheckMoves[cell.Occupant].Contains(cell))
            {
                SetDestination(cell);
            } 
            else
            {
                Debug.Log($"{CurrentPlayerTurn.ToString()} King is in Check. No moves from this piece can escape it.");
                Unselect();
            }
        }

        private static void RegularSelection(Cell cell)
        {
            if (_origin == null)
            {
                SetOrigin(cell);
            }
            else if (_origin != null && IsDifferentPieceSelected(cell))
            {
                Unselect();
                SelectCell(cell); // Tail Recursion
            }
            else
            {
                SetDestination(cell);
            }
        }

        private static void Unselect()
        {
            if (_origin == null) return;
                    
            GetBehaviourCell(_origin).Occupant.Highlight(HighlightType.None);
            _origin = null;

            if (_moves == null) return;

            foreach (Cell cell in _moves)
                GetBehaviourCell(cell).Highlight(HighlightType.None);
            
            _moves = null;
        }

        #endregion

        private static void ResolveMovement()
        {
            GetBehaviourCell(_origin).Occupant.Highlight(HighlightType.None);
                
            if (_destination is { IsOccupied: true } && _destination.Occupant.Side == OpponentTurn)
                Destroy(GetBehaviourCell(_destination).Occupant.gameObject);

            _destination.Occupant = _origin.Occupant;
            _destination.Occupant.Cell = _destination; // I need to update the Piece's internal reference to it's holding Cell. Since the AvailableMove works automously (without re-providing the cell as a paramater)
            GetBehaviourCell(_destination).Occupant = GetBehaviourCell(_origin).Occupant; // Manually move the Behaviour cell's Occupant since the Data/View decoupling
            _destination.Occupant.HasMoved = true;

            _origin.Occupant = null;
            GetBehaviourCell(_origin).Occupant = null; // Manually drop the _origin Behaviour cell's Occupant to null too;

            Debug.Log($"Played {_destination.GetType()} from {GetBehaviourCell(_origin).Name} to {GetBehaviourCell(_destination).Name}");
                
            Board.UpdateView();
            HandleCheck();
            ChangeTurn();
        }

        private static void HandleCheck()
        {
            if (PlayerIsInCheck(Matrix.GetCurrentGridSnapshot(), OpponentTurn))
            {
                Check = true;
                GatherValidMoves();
                
                if (_validCheckMoves.Count == 0) { // Chekmate;
                    Debug.LogWarning("No moves can be made -> Checkmate");
                    return;
                }
            }

            Check = false;
        }

        private static void ChangeTurn()
        {
            _updateOnce = false;
            _destination = null;
            _origin = null;

            foreach (Cell cell in _moves) // Reset Cells highlighting
                GetBehaviourCell(cell).Highlight(HighlightType.None);
            _moves = null;
                
            CurrentPlayerTurn = OpponentTurn;
        }

        #endregion

        #region Targets

        private static void SetOrigin(Cell cell)
        {
            if (!cell.IsOccupied) return;
            if (cell.Occupant.Side != CurrentPlayerTurn) {
                Debug.Log("You can't play an Opponent piece !");
                return;
            }
                    
            _origin = cell;
            _moves = Matrix.GetMoves(_origin);

            GetBehaviourCell(_origin).Occupant.Highlight(_moves.Count > 0 ? HighlightType.Active : HighlightType.Error);

            Board.EnableCellsTargets(_moves);
        }

        private static void SetDestination(Cell cell)
        {
            if (!_moves.Contains(cell)) return;
                    
            _destination = cell;
            ResolveMovement();
        }

        #endregion

        #region Checks handling

        /// <summary>
        /// Verify if the player from a specific side is in check from the supplied "snapshot" grid
        /// </summary>
        /// <param name="grid">The original grid or the duplicate to check</param>
        /// <param name="sideToTest">Side of the player to verify</param>
        /// <returns></returns>
        private static bool PlayerIsInCheck(Cell[,] grid, Side sideToTest)
        {
            Cell king = Matrix.GetKing(grid, sideToTest);
            List<Cell> opponentPieces = Matrix.GetPieceCells(grid, sideToTest == Side.Light ? Side.Dark : Side.Light);

            ClearChecks();

            foreach (Cell opponentCell in opponentPieces) // For each opponent pieces
            {
                List<Cell> possibleMoves = opponentCell.Occupant.AvailableMoves();

                foreach (Cell target in possibleMoves) // For each possibility
                {
                    if (target.Occupant.Equals(king.Occupant)) // If one of them threaten the King
                    {
                        Debug.Log($"{opponentCell.Occupant.Side} {opponentCell.Occupant.GetType().Name} make the {king.Occupant.Side} {king.Occupant.GetType().Name} in check, tracking down...");
                        _checkResponsibles.Add(opponentCell); // Track the piece
                        break;
                    }
                }

                if (_checkResponsibles.Count > 0) // If any opponent piece threaten the King
                {
                    Debug.Log($"{_checkResponsibles.Count} threats have been detected towards {king.Occupant.Side} {king.Occupant.GetType().Name}");
                    foreach (Cell responsible in _checkResponsibles) // For each King's threats
                    {
                        foreach (Cell cell in responsible.Occupant.AvailableMoves()) {
                            GetBehaviourCell(cell).Highlight(HighlightType.Error); // Highlight the threat line(s) 
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        private static void ClearChecks()
        {
            foreach (Cell cell in _checkResponsibles)
            {
                GetBehaviourCell(cell).Highlight(HighlightType.None);
            }
                
            _checkResponsibles.Clear();
        }

        private static void GatherValidMoves()
        {
            Cell[,] snapshot = Matrix.GetCurrentGridSnapshot();

            foreach (Cell piece in snapshot) // For each piece from current player' side
            {
                if (piece.Occupant == null || piece.Occupant.Side != CurrentPlayerTurn) continue;
                
                foreach (Cell move in piece.Occupant.AvailableMoves()) // For each moves this piece can achieve
                {
                    // Simulate a new grid (from the snapshot) by manually move the piece according to each of it's positions
                    VirtualResolve(snapshot, piece, move);
                }
                // Iterate next piece
            }
        }

        private static void VerifyifMoveIsValidForCheck( Cell[,] snapshot, Cell origin, Cell destination)
        {
            if (!PlayerIsInCheck(snapshot, OpponentTurn))
            {
                if (_validCheckMoves.ContainsKey(destination.Occupant))
                    _validCheckMoves[destination.Occupant].Add(destination);
                else
                    _validCheckMoves.Add(destination.Occupant, new List<Cell> { destination });
            }
        }
        
        public static void VirtualResolve( Cell[,] snapshot, Cell origin, Cell destination) // Resolve every possible movements from a snapshot
        {
            Cell virtualOrigin = snapshot[origin.Coordinates.Row, origin.Coordinates.Column];
            Cell virtualDestination = snapshot[destination.Coordinates.Row, destination.Coordinates.Column];
                
            virtualDestination.Occupant = virtualOrigin.Occupant;
            virtualDestination.Occupant.Cell = virtualDestination;
            virtualDestination.Occupant.HasMoved = true;
            virtualOrigin.Occupant = null;
        }
     

        #endregion

        #region Utils

        private static bool IsDifferentPieceSelected(Cell cell) {
            return cell.IsOccupied && cell.Occupant.Side == CurrentPlayerTurn;
        }

        public static void ShowNoMovesAvailable() {
            GetBehaviourCell(_origin).Occupant.Highlight(HighlightType.Error);
        }

        #endregion
    }
}
