using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProgressionStore.PaintShop
{
    public class PaintShopUI : MonoBehaviour
    {
        public event Action<int> SelectionButtonPressed;
        
        [SerializeField] private Transform _selectionButtonPrefab;
        [SerializeField] private Transform _selectionButtonHolder;
        [SerializeField] private float _horizontalDistance;
        [SerializeField] private float _verticalDistance;
        
        private readonly Vector2Int _extends = new Vector2Int(3, 4);
        private Button[] _selectionButtons;

        private void Awake()
        {
            _selectionButtons = new Button[_extends.x * _extends.y];
            InstantiateSelectionButtons();
        }
        
        private void InstantiateSelectionButtons()
        {
            for (int y = 0; y < _extends.y; y++)
            {
                for (int x = 0; x < _extends.x; x++)
                {
                    int i = y * _extends.x + x;
                    Vector3 position = _selectionButtonPrefab.position +
                                       new Vector3(_horizontalDistance * x, -_verticalDistance * y);
                    _selectionButtons[i] = Instantiate(_selectionButtonPrefab, position,
                        _selectionButtonPrefab.rotation, _selectionButtonHolder).GetComponent<Button>();
                    _selectionButtons[i].transform.localPosition = (Vector2)_selectionButtons[i].transform.localPosition;
                    _selectionButtons[i].onClick.AddListener(() =>
                    {
                        OnSelectionButton(i);
                    });
                }
            }
            Destroy(_selectionButtonPrefab.gameObject);
        }

        private void OnSelectionButton(int index)
        {
            SelectionButtonPressed?.Invoke(index);       
        }
    }
}