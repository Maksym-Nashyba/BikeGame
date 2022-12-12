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
            SelectContainer(new Vector2Int(1,0));
            SelectContainer(new Vector2Int(0,1));
            SelectContainer(new Vector2Int(1,2));
        }

        public void SelectContainer(Vector2Int index)
        {
            bool leftColumn = index.x == 0;
            Vector2Int pricePatternCell = leftColumn ? index + Vector2Int.right : index + Vector2Int.left;
            _painter.PaintPatternInCell(index, _patterns.SelectionFrame);
            _painter.PaintPatternInCell(pricePatternCell, _patterns.BuildPricePattern(69, leftColumn));
            _painter.Apply();
        }
    }
}