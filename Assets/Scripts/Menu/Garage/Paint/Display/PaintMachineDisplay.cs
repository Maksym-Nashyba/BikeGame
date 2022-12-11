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
        }

        private void Start()
        {
            SelectContainer(new Vector2Int(0,1));
        }

        public void SelectContainer(Vector2Int index)
        {
            _painter.PaintPattern(index, _patterns.SelectionFrame);
            _painter.PaintPattern(index + Vector2Int.right, _patterns.ThreeHundrerBucks);
            _painter.Apply();
        }
    }
}