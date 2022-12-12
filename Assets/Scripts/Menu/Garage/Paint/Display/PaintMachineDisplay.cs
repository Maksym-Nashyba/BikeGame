using UnityEngine;

namespace Menu.Garage.Paint.Display
{
    public class PaintMachineDisplay : MonoBehaviour
    {
        [SerializeField] private Vector2Int _resolution;
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private PaintDisplayPatterns _patterns;
        private TexturePainter _painter;
        
        private void Awake()
        {
            _painter = new TexturePainter(_resolution, _renderer);
            _patterns.Bake();
        }

        private void Start()
        {
            SelectContainer(new Vector2Int(0,0));
        }

        public void SelectContainer(Vector2Int index)
        {
            _painter.PaintPatternInCell(index, _patterns.SelectionFrame);
            _painter.PaintPatternInCell(index+Vector2Int.right, _patterns.BuildPricePattern(37, true));
            _painter.Apply();
        }
    }
}