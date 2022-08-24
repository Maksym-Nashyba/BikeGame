using System;
using System.Collections.Generic;
using IGUIDResources;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

namespace ProgressionStore
{
    public class PaintWindow : ShopWindow
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _closeButton;
        private GarageUI _garageUI;
        private List<GameObject> _currentButtons;

        protected override void Awake()
        {
            base.Awake();
            _currentButtons = new List<GameObject>();
            _garageUI = FindObjectOfType<GarageUI>();
            _garageUI.SetBikeSelectionEnabled(false);
        }

        public override void Open()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("HiddenCamera")) return;
            _animator.Play("OpenPaintWindow");
            _closeButton.SetActive(true);
            _garageUI.SetBikeSelectionEnabled(true);
        }

        public override void Close()
        {
            _closeButton.SetActive(false);
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("ShownPaintWindow")) return;
            _animator.Play("ClosePaintWindow");
            _garageUI.SetBikeSelectionEnabled(false);
        }

        private void DestroyButtons()
        {
            foreach (GameObject button in _currentButtons)
            {
                Destroy(button);
            }
            _currentButtons.Clear();
        }
    }
}