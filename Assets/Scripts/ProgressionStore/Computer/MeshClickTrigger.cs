using System;
using Inputs;
using UnityEngine;

namespace ProgressionStore.Computer
{
    [RequireComponent(typeof(MeshRenderer))]
    public class MeshClickTrigger : MonoBehaviour
    {
        private InputMappings _inputs { get; set; }

        private void Awake()
        {
            _inputs = new InputMappings();
            _inputs.General.Click.performed += _ => OnClick();
        }

        private void OnClick()
        {
            Debug.Log("ABOBA");
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
            //_inputs.General.Click.performed -= OnClick;
        }
    }
}