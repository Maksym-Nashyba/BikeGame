using System;
using IGUIDResources;
using UnityEngine;

namespace Garage.Paint
{
    public class PaintContainersHolder : MonoBehaviour
    {
        public event Action<Skin> SkinChanged;
        private GameObject _currentPaintContainer;
        [SerializeField] private PaintContainer[] _paintContainers;

        private void Awake()
        {
            foreach (PaintContainer container in _paintContainers)
            {
                container.Clicked += OnContainerSelected;
            }
        }
        
        private void OnDestroy()
        {
            foreach (PaintContainer container in _paintContainers)
            {
                container.Clicked -= OnContainerSelected;
            }
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
            return container.GetSkin();
        }
    }
}