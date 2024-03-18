using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

using Managers;
using Data;
using Enums;
using Pieces;

namespace View
{
    public class PieceBehaviour: MonoBehaviour
    {
        private MeshRenderer _mesh;

        private static readonly int FresnelColor = Shader.PropertyToID("_fresnelColor");
        private static readonly int EnableFresnel = Shader.PropertyToID("_enableFresnel");
        private static readonly int FresnelAltColor = Shader.PropertyToID("_fresnelAltColor");

        private static Color _initialColor;

        private void Awake()
        {
            _mesh = GetComponent<MeshRenderer>();
            _initialColor = _mesh.material.GetColor(FresnelColor);
        }

        public static PieceBehaviour Create(Cell cell, GameObject prefab, Transform root)
        {
            Side side = cell.Occupant.Side;
            Quaternion rotation = side == Side.Light ? Quaternion.identity : Quaternion.Euler(0, -180, 0);
            
            return Instantiate(prefab, cell.Coordinates.World, rotation, root).GetComponent<PieceBehaviour>();
        }

        public static List<PieceBehaviour> InitBoard(Transform root)
        {
            List<PieceBehaviour> pieceBehaviours = new ();
            string[] pieceOrder = { "Rook", "Knight", "Bishop", "Queen", "King", "Bishop", "Knight", "Rook" };

            for (int column = 0; column < Matrix.BoardSize; column++)
            {
                Matrix.GetCell(column, 0).Occupant = Piece.Create("Light" + pieceOrder[column], Matrix.GetCell(column, 0));
                Matrix.GetCell(column, 1).Occupant = Piece.Create("LightPawn", Matrix.GetCell(column, 1));
                Matrix.GetCell(column, 7).Occupant = Piece.Create("Dark" + pieceOrder[column], Matrix.GetCell(column, 7));
                Matrix.GetCell(column, 6).Occupant = Piece.Create("DarkPawn", Matrix.GetCell(column, 6));

                GameManager.GetBehaviourCell(Matrix.GetCell(column, 0)).Occupant = Create(Matrix.GetCell(column, 0), Piece.Prefabs["Light" + pieceOrder[column]], root);
                GameManager.GetBehaviourCell(Matrix.GetCell(column, 1)).Occupant = Create(Matrix.GetCell(column, 1), Piece.Prefabs["LightPawn"], root);
                GameManager.GetBehaviourCell(Matrix.GetCell(column, 7)).Occupant = Create(Matrix.GetCell(column, 7), Piece.Prefabs["Dark" + pieceOrder[column]], root);
                GameManager.GetBehaviourCell(Matrix.GetCell(column, 6)).Occupant = Create(Matrix.GetCell(column, 6), Piece.Prefabs["DarkPawn"], root);

                pieceBehaviours.Add(GameManager.GetBehaviourCell(Matrix.GetCell(column, 0)).Occupant);
                pieceBehaviours.Add(GameManager.GetBehaviourCell(Matrix.GetCell(column, 1)).Occupant);
                pieceBehaviours.Add(GameManager.GetBehaviourCell(Matrix.GetCell(column, 7)).Occupant);
                pieceBehaviours.Add(GameManager.GetBehaviourCell(Matrix.GetCell(column, 6)).Occupant);
            }

            return pieceBehaviours;
        }

        public void Highlight(HighlightType type)
        {
            switch (type)
            {
                case HighlightType.None:
                    _mesh.material.SetFloat(EnableFresnel, 0);
                    break;
                case HighlightType.Active:
                    _mesh.material.SetColor(FresnelColor, _initialColor);
                    _mesh.material.SetFloat(EnableFresnel, 1);
                    break;
                case HighlightType.Error:
                    Color altColor = _mesh.material.GetColor(FresnelAltColor);
                    _mesh.material.SetColor(FresnelColor, altColor);
                    _mesh.material.SetFloat(EnableFresnel, 1);
                    break;
                default:
                    throw new InvalidEnumArgumentException($"Invalid {type.ToString()} supplied for piece highlighting");
            }
        }
    }
}
