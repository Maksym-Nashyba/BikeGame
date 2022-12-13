using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IGUIDResources;
using UnityEngine;

namespace Menu.Garage.Paint.Containers
{
    public class PaintContainersHolder : MonoBehaviour
    {
        public event Action<PaintContainer> ContainerSelected; 
        [SerializeField] private GameObject _paintContainerPrefab;
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private Vector2  _padding;
        private PaintContainer[] _paintContainers;
        private PaintContainer _selectedContainer;
        
        private void Start()
        {
            PaintContainerSpawner containerSpawner = new PaintContainerSpawner(_paintContainerPrefab);
            _paintContainers = containerSpawner.Spawn(_gridSize, _padding, transform);
        }
        
        private void OnDestroy()
        {
            foreach (PaintContainer container in _paintContainers) 
            {
                container.Clicked -= OnContainerClicked;
            }
        }

        private void OnContainerClicked(PaintContainer paintContainer)
        {
            if (_selectedContainer != paintContainer) return;
            _selectedContainer = paintContainer;
            ContainerSelected?.Invoke(_selectedContainer);
        }
        
        public Task FillContainers(Skin[] skins)
        {
            List<Task> fillingTasks = new List<Task>(_gridSize.x*_gridSize.y);
            for (int i = 0; i < skins.Length; i++)
            {
                _paintContainers[i].Clicked += OnContainerClicked;
                fillingTasks.Add(_paintContainers[i].Fill(skins[i]));
            }
            return Task.WhenAll(fillingTasks);
        }

        public async Task CleanContainers()
        {
            List<Task> cleaningTasks = new List<Task>(_gridSize.x*_gridSize.y);
            foreach (PaintContainer container in _paintContainers) 
            {
                container.Clicked -= OnContainerClicked;
                cleaningTasks.Add(container.Clean());
            }
            await Task.WhenAll(cleaningTasks);
        }
    }
}