using System;
using IGUIDResources;
using UnityEngine;

namespace ProgressionStore.PaintShop
{
    public class PaintContainerArray : MonoBehaviour
    {
        public event Action<PaintContainer> ContainerSelected; 
        public int Length => _extends.x * _extends.y;
        public PaintContainer this[int i] => _containers[i];
        [SerializeField] private Transform _prefab;
        [SerializeField] private float _horizontalDistance;
        [SerializeField] private float _verticalDistance;

        private readonly Vector2Int _extends = new Vector2Int(3, 4);
        private PaintContainer[] _containers;
        private PaintContainer _selectedContainer;
        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _containers = new PaintContainer[Length];
            InstantiateEmptyContainers();
        }
        
        public void RebuildForSkins(Skin[] skins)
        {
            int containersToFill = Mathf.Min(skins.Length, _containers.Length);
            for (int i = 0; i < containersToFill; i++)
            {
                _containers[i].ShowWithSkin(skins[i]);
            }

            int containersToEmpty = _containers.Length - containersToFill;
            for (int i = containersToFill; i < containersToEmpty; i++)
            {
                _containers[i].ShowEmpty();
            }
        }

        private void InstantiateEmptyContainers()
        {
            for (int y = 0; y < _extends.y; y++)
            {
                for (int x = 0; x < _extends.x; x++)
                {
                    Vector3 position = _prefab.position + _prefab.forward * _horizontalDistance * x;
                    position += Vector3.down * _verticalDistance * y;
                    _containers[y * _extends.x + x] = Instantiate(_prefab, position, _prefab.rotation, _transform)
                        .GetComponent<PaintContainer>();
                    _containers[y * _extends.x + x].ShowEmpty();
                }
            }
            Destroy(_prefab.gameObject);
        }

        public void Select(int index)
        {
            if (_containers[index].IsEmpty()
                || _containers[index] == _selectedContainer) return;
            
            _selectedContainer = _containers[index];
            foreach (PaintContainer container in _containers)
            {
                if(container != _selectedContainer)container.DisplayDeselected();
            }
            _selectedContainer.DisplaySelected();
            ContainerSelected?.Invoke(_selectedContainer);
        }
    }
}