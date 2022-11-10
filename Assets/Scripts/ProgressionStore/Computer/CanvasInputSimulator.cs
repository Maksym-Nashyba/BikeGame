using System;
using Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProgressionStore.Computer
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasInputSimulator : MonoBehaviour
    {
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }
        

        public void ClickAtUV(Vector2 uv)
        {
            Rect canvasRect = _canvas.pixelRect;
            Vector2 canvasPosition = canvasRect.size * uv - canvasRect.size / 2f;
            Transform hitUIElement = Raycast(canvasPosition, transform);
            if(hitUIElement == null) return;
            
            Debug.Log($"Hit{hitUIElement.name}");
        }

        private Transform Raycast(Vector2 canvasPosition, Transform parent)
        {
            for (int i = parent.childCount-1; i >= 0; i--)
            {
                Transform found = Raycast(canvasPosition, parent.GetChild(i));
                if (found != null) return found;
            }

            if (parent.TryGetComponent(out Raycastable raycastable))
            {
                if (raycastable.RectTransform.ContainsPoint(canvasPosition)) return parent;
            }

            return null;
        }
    }
}
