using Array2DEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Menu.Garage.Paint
{
    public class PaintMachineDisplay : MonoBehaviour
    {
        [SerializeField] private Vector2Int _resolution;
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Array2DBool _pattern;
        private Vector2Int _cellSize;
        private Texture2D _texture2D;
        
        private void Awake()
        {
            _cellSize = new Vector2Int(_resolution.x / 2, _resolution.y / 3);
            _texture2D = new Texture2D(_resolution.x, _resolution.y, GraphicsFormat.R16G16B16A16_SFloat, TextureCreationFlags.None);
            _texture2D.filterMode = FilterMode.Point;
        }

        private void Start()
        {
            SelectContainer(new Vector2Int(1,1));
            
            _renderer.material.SetTexture("_Image", _texture2D);
        }

        public void SelectContainer(Vector2Int index)
        {
            PaintPattern(_pattern, index * _cellSize);
            _texture2D.Apply();
        }

        private void PaintPattern(Array2DBool pattern, Vector2Int loverLeft)
        {
            for (int x = 0; x < pattern.GridSize.x; x++)
            {
                for (int y = 0; y < pattern.GridSize.y; y++)
                {
                    Color color = pattern.GetCell(x, y) ? Color.red : Color.clear;
                    PaintPixel(loverLeft + new Vector2Int(x, y), color);
                }
            }
        }
        
        private void PaintPixel(Vector2Int position, Color color)
        {
            _texture2D.SetPixel(position.x, position.y, color);
        }
    }
}