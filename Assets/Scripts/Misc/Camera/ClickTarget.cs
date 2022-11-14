using System;
using Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Misc
{
    [RequireComponent(typeof(Collider))]
    public abstract class ClickTarget<T> : MonoBehaviour
    {
        public abstract event Action<T> Clicked;
        [SerializeField] private Camera _camera;
        private InputMappings _inputMappings;

        private void Awake()
        {
            _inputMappings = new InputMappings();
            _inputMappings.General.Click.performed += OnClickAction;
        }

        private void OnEnable()
        {
            _inputMappings ??= new InputMappings();
            _inputMappings.Enable();
        }

        private void OnDisable()
        {
            _inputMappings.Disable();
        }

        protected abstract void OnClicked();
        
        private void OnClickAction(InputAction.CallbackContext obj)
        {
            if (HitWithPointerRaycast())
            {
                OnClicked();
            }
        }

        private bool HitWithPointerRaycast()
        {
            Ray ray = _camera.ScreenPointToRay(Pointer.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.transform == transform;
            }

            return false;
        }
    }
}