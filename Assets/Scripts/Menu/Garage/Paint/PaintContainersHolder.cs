using System;
using System.Collections.Generic;
using IGUIDResources;
using UnityEngine;

namespace Menu.Garage.Paint
{
    public class PaintContainersHolder : MonoBehaviour
    {
        public event Action<Skin> SkinChanged;
        [SerializeField] private GameObject _paintContainerPrefab;
        private GameObject _currentPaintContainer;
        private GameObject[] _paintContainersGameObjects;
        private List<PaintContainer> _paintContainers;

        private void Start()
        {
            GeneratePaintContainers();
        }
        
        private void GeneratePaintContainers()
        {
            _paintContainersGameObjects = new GameObject[6];
            for(int i = 0; i < +_paintContainersGameObjects.Length; i++)
            {
                GameObject container = GeneratePaintContainer(_paintContainerPrefab);
                ApplyContainerPosition(container, i);
                _paintContainersGameObjects[i] = container;
            }
        }

        private GameObject GeneratePaintContainer(GameObject prefab) 
        {
            GameObject container = Instantiate(prefab, transform);
            container.SetActive(true);
            return container;
        }
        
        private void ApplyContainerPosition(GameObject container, int containerNumber)
        {
            float verticalOffset = containerNumber > 3 ? -1.4f : (containerNumber > 1 ? -0.7f : 0);
            float horizontalOffset = containerNumber %2 != 0 ? 0.7f : 0;
            container.transform.localPosition = _paintContainerPrefab.transform.localPosition + new Vector3(0, verticalOffset, horizontalOffset);
        }

        public void ApplyPaintsToContainers(Skin[] skins)
        {
            if(_paintContainers != null) ResetPaintContainers();
            _paintContainers = new List<PaintContainer>(skins.Length);

            for (int i = 0; i < skins.Length; i++)
            {
                PaintContainer paintContainer = _paintContainersGameObjects[i].GetComponentInChildren<PaintContainer>();
                ApplyPaintToContainer(paintContainer, skins[i]);
                paintContainer.PlayFillAnimation();
            }
        }
        
        private void ApplyPaintToContainer(PaintContainer paintContainer, Skin skin) 
        {
            paintContainer.Skin = skin;
            paintContainer.Clicked += OnContainerSelected;
            _paintContainers.Add(paintContainer);
        }

        public void CleanContainers()
        {
            if (_paintContainers == null) return;
            foreach (PaintContainer container in _paintContainers) 
            {
                container.PlayCleanAnimation();
            }
        }
        
        private void ResetPaintContainers() 
        {
            if (_paintContainers == null) return;
            foreach (PaintContainer container in _paintContainers) 
            {
                container.Clicked -= OnContainerSelected;
            }
            _paintContainers.Clear();
            _paintContainers = null;
        }

        private void OnDestroy()
        {
            ResetPaintContainers();
        }

        private void OnContainerSelected(GameObject paintContainer)
        {
            if(_currentPaintContainer != null) UnHollowSelectedContainer();
            _currentPaintContainer = paintContainer;
            HollowSelectedContainer();
            
            Skin skin = GetSkinFrom(_currentPaintContainer);
            SkinChanged?.Invoke(skin);
        }

        private void HollowSelectedContainer()
        {
            
        }
        
        private void UnHollowSelectedContainer()
        {
            
        }
        
        private Skin GetSkinFrom(GameObject paintContainer)
        {
            PaintContainer container = paintContainer.GetComponentInChildren<PaintContainer>();
            if (container == null) throw new NullReferenceException();
            return container.Skin;
        }
    }
}