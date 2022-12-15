using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IGUIDResources;
using UnityEngine;
using UnityEngine.Serialization;

namespace Menu.Garage.Paint.Containers
{
    public class PaintContainersHolder : MonoBehaviour
    {
        public event Action<PaintContainer> ContainerSelected; 
        public PaintContainer SelectedContainer { get; private set; }
        [SerializeField] private GameObject _paintContainerPrefab;
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private Vector2  _padding;
        private PaintContainer[] _paintContainers;
        
        private void Start()
        {
            PaintContainerSpawner containerSpawner = new PaintContainerSpawner(_paintContainerPrefab);
            _paintContainers = containerSpawner.Spawn(_gridSize, _padding, transform);
            foreach (PaintContainer container in _paintContainers) 
            {
                container.Clicked += OnContainerClicked;
            }
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
            if (SelectedContainer == paintContainer) return;
            SelectedContainer = paintContainer;
            if(paintContainer.Skin!= null)ContainerSelected?.Invoke(SelectedContainer);
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
            List<Task> cleaningTasks = new List<Task>(_gridSize.x * _gridSize.y);
            foreach (PaintContainer container in _paintContainers) 
            {
                container.Clicked -= OnContainerClicked;
                cleaningTasks.Add(container.Clean());
            }
            await Task.WhenAll(cleaningTasks);
        }
    }
}