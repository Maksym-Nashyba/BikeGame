using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProgressionStore.Computer
{
    [RequireComponent(typeof(Rigidbody))]
    public class ComputerCanvasInputSimulator : MonoBehaviour
    {
        private Canvas _canvas;

        private void Update()
        {
            
        }

        public void ClickAtUV(Vector2 clickPosition)
        {
            PointerEventData pointer = new PointerEventData (EventSystem.current)
            {
                clickCount = 1,
                button = PointerEventData.InputButton.Left
            };
            ExecuteEvents.Execute(_canvas.gameObject , pointer , ExecuteEvents.pointerClickHandler);
        }
    }
}