using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

using Managers;
using Data;
using Enums;

namespace View
{
    public class CellBehaviour: MonoBehaviour
    {
        public Coordinates Coordinates { get; set; }
        public PieceBehaviour Occupant { get; set; }

        public string Name => gameObject.name;

        private MeshRenderer _mesh;
        private Collider _collider;
        
        private static readonly int Enable = Shader.PropertyToID("_Enable");
        private static readonly int IntersectionColor = Shader.PropertyToID("_IntersectionColor");
        private static readonly int IntersectionColorAlt = Shader.PropertyToID("_IntersectionColorAlt");
        
        private static Color _initialColor;

        private void Awake()
        {
            Vector3 position = transform.position;
            Coordinates = new Coordinates(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
            
            _mesh = GetComponent<MeshRenderer>();
            _collider = GetComponent<Collider>();
            _initialColor = _mesh.material.GetColor(IntersectionColor);
        }

        private void OnMouseDown()
        {
            GameManager.SelectCell(Matrix.GetCell(gameObject.name));
        }
        
        public static List<CellBehaviour> InitBoard(GameObject prefab, Transform root)
        {
            List<CellBehaviour> cellBehaviours = new ();
            
            for (int row = 0; row < Matrix.BoardSize; row++)
            {
                for (int column = 0; column < Matrix.BoardSize; column++)
                {
                    GameObject cell = Instantiate(prefab, Matrix.GetCell(column, row).Coordinates.World, Quaternion.identity, root);
                    cell.name = (char)('A' + column) + (row + 1).ToString();
                    cellBehaviours.Add(cell.GetComponent<CellBehaviour>());
                }
            }

            return cellBehaviours;
        }

        public void IsTargetable(bool enable)
        {
            _collider.enabled = enable;
        }

        public void Highlight(HighlightType type)
        {
            switch (type)
            {
                case HighlightType.None:
                    _mesh.material.SetFloat(Enable, 0);
                    break;
                case HighlightType.Active:
                    _mesh.material.SetColor(IntersectionColor, _initialColor);
                    _mesh.material.SetFloat(Enable, 1);
                    break;
                case HighlightType.Error:
                    Color altColor = _mesh.material.GetColor(IntersectionColorAlt);
                    _mesh.material.SetColor(IntersectionColor, altColor);
                    _mesh.material.SetFloat(Enable, 1);
                    break;
                default:
                    throw new InvalidEnumArgumentException($"Invalid {type.ToString()} supplied for piece highlighting");
            }
        }
    }
}
