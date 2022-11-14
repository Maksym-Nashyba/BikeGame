using System;
using Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProgressionStore.Computer
{
    [RequireComponent(typeof(MeshRenderer))]
    public class MeshClickListener : MonoBehaviour
    {
        public event Action<Vector2> ClickedUV; 
        [SerializeField] private Camera _camera;
        private InputMappings _inputs;

        private void Awake()
        {
            _inputs = new InputMappings();
            _inputs.General.Click.performed += OnClickAction;
        }

        private void OnClickAction(InputAction.CallbackContext obj)
        {
            int mask = LayerMask.GetMask("ComputerUI");
            Ray ray = _camera.ScreenPointToRay(Pointer.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, mask))
            {
                if(hit.transform != transform)return;
                ClickedUV?.Invoke(hit.textureCoord);
            }
        }

        private void OnEnable()
        {
            _inputs.Enable();
        }

        private void OnDisable()
        {
            _inputs.Disable();
        }

        private void OnDestroy()
        {
            _inputs.General.Click.performed -= OnClickAction;
        }
    }
}