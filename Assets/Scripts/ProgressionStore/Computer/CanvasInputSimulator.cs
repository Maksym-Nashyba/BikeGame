using Misc;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProgressionStore.Computer
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasInputSimulator : MonoBehaviour
    {
        [SerializeField] private Camera _canvasCamera;
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        public void ClickAtUV(Vector2 uv)
        {
            Vector2 canvasPosition = GetCanvasPosition(uv);
            Transform hitUIElement = GetGameObject(canvasPosition, transform, Vector2.zero);
            if(hitUIElement == null) return;

            SendClickEvent(hitUIElement.gameObject);
        }

        private Transform GetGameObject(Vector2 canvasPosition, Transform parent, Vector2 offset)
        {
            for (int i = parent.childCount-1; i >= 0; i--)
            {
                Transform found = GetGameObject(canvasPosition, parent.GetChild(i), offset + (Vector2)parent.GetChild(i).localPosition);
                if (found != null) return found;
            }

            if (parent.TryGetComponent(out Raycastable raycastable))
            {
                if (raycastable.RectTransform.ContainsPoint(offset, canvasPosition)
                    && raycastable.gameObject.activeInHierarchy) return parent;
            }

            return null;
        }

        private Vector2 GetCanvasPosition(Vector2 uv)
        {
            Rect canvasRect = _canvas.pixelRect;
            return canvasRect.size * uv - canvasRect.size / 2f;
        }

        private void SendClickEvent(GameObject gameObject)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                clickCount = 1,
                button = PointerEventData.InputButton.Left
            };
            ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerClickHandler);
        }
    }
}
