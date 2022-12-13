using System.Collections.Generic;
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

        public PaintContainer[] Spawn(Vector2Int gridSize, Vector2 padding, Transform firstLocation)
        {
            List<PaintContainer> containers = new List<PaintContainer>();
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Vector2Int cell = new Vector2Int(x, y);
                    PaintContainer instancedContainer = InstantiateContainer(cell, padding, firstLocation);
                    instancedContainer.SetUp(cell);
                    containers.Add(instancedContainer);
                }
            }
            return containers.ToArray();
        }

        private PaintContainer InstantiateContainer(Vector2Int cell, Vector2 padding, Transform firstLocation)
        {
            Transform containerTransform = Object.Instantiate(_containerPrefab, firstLocation).transform;
            containerTransform.localPosition = new Vector3(cell.x * padding.x, 0f, -cell.y * padding.y);
            containerTransform.localRotation = Quaternion.identity;
            containerTransform.gameObject.SetActive(true);
            return containerTransform.GetComponent<PaintContainer>();
        }
    }
}