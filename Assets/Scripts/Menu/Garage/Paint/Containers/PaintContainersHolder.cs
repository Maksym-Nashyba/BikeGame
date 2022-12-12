using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using IGUIDResources;
using Misc;
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

        private void Start()
        {
            PaintContainerSpawner containerSpawner = new PaintContainerSpawner(_paintContainerPrefab);
            _paintContainers = containerSpawner.Spawn(_gridSize, _padding, new Transformation(transform));
        }
        
        private void OnDestroy()
        {
            CleanContainers();
        }

        public Task FillContainers(Skin[] skins)
        {
            return Task.CompletedTask;
        }

        public async Task CleanContainers()
        {
            if (_paintContainers == null) throw new NullReferenceException("Containers collection was null");
            List<Task> animationTasks = new List<Task>(6);
            foreach (PaintContainer container in _paintContainers) 
            {
                container.Clicked -= OnContainerClicked;
                animationTasks.Add(container.Clean());
            }
            await Task.WhenAll(animationTasks);
            Array.Clear(_paintContainers, 0, _paintContainers.Length);
        }
        
        private void OnContainerClicked(PaintContainer paintContainer)
        {
            throw new NotImplementedException();
        }
    }
}