using Misc;
using UnityEngine;

namespace Menu.Garage.Paint.Containers
{
    public class PaintContainerSpawner
    {
        private readonly GameObject _containerPrefab;

        public PaintContainerSpawner(GameObject containerPrefab)
        {
            _containerPrefab = containerPrefab;
        }

        public PaintContainer[] Spawn(Vector2Int gridSize, Vector2 padding, Transformation firstLocation)
        {
            throw new System.NotImplementedException();
        }
    }
}