using System;
using System.Collections.Generic;
using UnityEngine;

using Managers;
using Data;
using Enums;
using Pieces;

namespace View
{
    public class Board: MonoBehaviour
    {
        [SerializeField] private Transform _cellsRoot;
        [SerializeField] private Transform _piecesRoot;
        [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private List<GameObject> _piecesPrefabs;

        public static List<CellBehaviour> CellBehaviours;
        public static List<PieceBehaviour> PieceBehaviours;

        private void Start()
        {
            Initialize();
            // Matrix.Debug();
        }

        private void Initialize()
        {
            InitializePiecesPrefabs();
            Matrix.Init();
            CellBehaviours = CellBehaviour.InitBoard(_cellPrefab, _cellsRoot);
            PieceBehaviours = PieceBehaviour.InitBoard(_piecesRoot);
            ResetCellsTargetState();
        }

        private void InitializePiecesPrefabs()
        {
            Piece.Prefabs = new Dictionary<string, GameObject>();
        
            foreach (GameObject prefab in _piecesPrefabs)
                Piece.Prefabs.Add(prefab.name, prefab);
        }

        #region View

        public static void EnableCellsTargets(List<Cell> availableCells)
        {
            if (availableCells == null) throw new NullReferenceException("Error: availableCells list isn't initialized, is there a probleme with <GetAvailableMoves> ?");

            if (availableCells.Count == 0)
            {
                GameManager.ShowNoMovesAvailable();
                return;
            }

            foreach (Cell cell in availableCells)
            {
                GameManager.GetBehaviourCell(cell).IsTargetable(true);
                GameManager.GetBehaviourCell(cell).Highlight(HighlightType.Active);
            }
        }

        private static void ResetCellsTargetState()
        {
            Matrix.ResetCellsTargetState();
        }
    
        public static void UpdateView()
        {
            List<Cell> cells = Matrix.GetAllCells();
        
            foreach (Cell cell in cells)
            {
                if (cell.IsOccupied)
                    GameManager.GetBehaviourCell(cell).Occupant.transform.position = cell.Coordinates.World;
            }
        }

        #endregion
    }
}
