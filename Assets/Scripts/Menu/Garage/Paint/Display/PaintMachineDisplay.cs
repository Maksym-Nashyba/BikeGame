using Menu.Garage.Paint.Containers;
using Misc;
using SaveSystem.Front;
using UnityEngine;

namespace Menu.Garage.Paint.Display
{
    public class PaintMachineDisplay : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Color _green;
        [SerializeField] private Color _yellow;
        [SerializeField] private Color _red;
        [SerializeField] private Vector2Int _resolution;
        [Header("Patterns")]
        [SerializeField] private Texture2D _closedScreen;
        [SerializeField] private PaintDisplayPatterns _patterns;
        [Header("References")]
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private PaintContainersHolder _containerHolder;
        [SerializeField] private CameraCheckpoint _cameraCheckpoint;
        
        private TexturePainter _painter;
        private Saves _saves;
        private AsyncExecutor _asyncExecutor;
        
        private void Awake()
        {
            _asyncExecutor = new AsyncExecutor();
            _saves = FindObjectOfType<Saves>();
            _painter = new TexturePainter(_resolution, _renderer, _asyncExecutor);
            _patterns.Bake();
            _cameraCheckpoint.CameraApproaching += OnCameraApproaching;
            _cameraCheckpoint.CameraDeparted += OnCameraDeparted;
            _containerHolder.ContainerSelected += OnContainerSelected;
        }

        private void Start()
        {
            _painter.PaintFromTexture(_closedScreen);
            _painter.Apply();   
        }

        private void OnDestroy()
        {
            _asyncExecutor.Dispose();
            _cameraCheckpoint.CameraApproaching -= OnCameraApproaching;
            _cameraCheckpoint.CameraDeparted -= OnCameraDeparted;
            _containerHolder.ContainerSelected -= OnContainerSelected;
        }

        private void OnCameraApproaching()
        {
            _painter.CleanAnimated();
        }
        
        private async void OnCameraDeparted()
        {
            await _painter.CleanAnimated();
            _painter.PaintFromTexture(_closedScreen);
            _painter.Apply();
        }
        
        private void OnContainerSelected(PaintContainer container)
        {
            _painter.Clear();
            if (_saves.Bikes.IsSkinUnlocked(container.Skin)) PaintBoughtSelection(container.Cell);
            else
            {
                bool canAfford = _saves.Currencies.GetDollans() >= container.Skin.Price;
                PaintNotBoughtSelection(container.Cell, container.Skin.Price, canAfford);
            }
            _painter.Apply();
        }

        #region Selection
        private void PaintNotBoughtSelection(Vector2Int cell, uint price, bool canAfford)
        {
            _painter.PaintPatternInCell(cell, _patterns.SelectionFrame, _yellow);
            
            Vector2Int pricePatternCell = cell.x == 0 ? cell + Vector2Int.right : cell + Vector2Int.left;
            PaintPricePointer(pricePatternCell, price, canAfford?_green:_red);
            
            if (cell.y < 2) PaintBottomArrows(_green);          
        }

        private void PaintBoughtSelection(Vector2Int cell)
        {
            _painter.PaintPatternInCell(cell, _patterns.SelectionFrame, _green);
            if (cell.y < 2) PaintBottomArrows(_green);
        }

        private void PaintPricePointer(Vector2Int cell, uint price, Color color)
        {
            Direction1D direction = cell.x == 0 ? Direction1D.Right : Direction1D.Left;
            Pattern pattern = _patterns.BuildPricePattern(price, direction);
            _painter.PaintPatternInCell(cell, pattern, color);
        }
        
        private void PaintBottomArrows(Color color)
        {
            _painter.PaintPatternInCell(new Vector2Int(0,2), _patterns.ArrowDown, color);
            _painter.PaintPatternInCell(new Vector2Int(1,2), _patterns.ArrowDown, color);            
        }
        #endregion
    }
}