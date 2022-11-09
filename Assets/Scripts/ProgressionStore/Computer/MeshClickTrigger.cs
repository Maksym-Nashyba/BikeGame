using Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProgressionStore.Computer
{
    [RequireComponent(typeof(MeshRenderer))]
    public class MeshClickTrigger : MonoBehaviour
    {
        private InputMappings _inputs { get; set; }

        private void Awake()
        {
            _inputs = new InputMappings();
            _inputs.General.Click.performed += OnClicked;
        }

        private void OnClicked(InputAction.CallbackContext obj)
        {
            Debug.Log(Pointer.current.position.ReadValue());
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
            _inputs.General.Click.performed -= OnClicked;
        }
    }
}