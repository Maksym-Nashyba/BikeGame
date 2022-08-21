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
        [SerializeField] private GameObject _buttonPrefab;
        [SerializeField] private GameObject _layoutGroup;
        [SerializeField] private GameObject _closeButton;
        private List<GameObject> _currentButtons;

        protected override void Awake()
        {
            base.Awake();
            Garage.NewBikeSelected += InstantiateSkinButtons;
            _currentButtons = new List<GameObject>();
        }

        public override void Open()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("HiddenCamera")) return;
            _animator.Play("OpenPaintWindow");
            _closeButton.SetActive(true);
        }

        public override void Close()
        {
            _closeButton.SetActive(false);
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("ShownPaintWindow")) return;
            _animator.Play("ClosePaintWindow");
        }

        private void DestroyButtons()
        {
            foreach (GameObject button in _currentButtons)
            {
                Destroy(button);
            }
            _currentButtons.Clear();
        }

        private void InstantiateSkinButtons(BikeModel bikeModel)
        {
            DestroyButtons();
            for (int i = 0; i < bikeModel.AllSkins.Length; i++)
            { 
                int ii = i; 
                GameObject currentButtonObject = Instantiate(_buttonPrefab,_layoutGroup.transform);
                _currentButtons.Add(currentButtonObject);
                TextMeshProUGUI buttonText = currentButtonObject.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = $"{i+1}";
                _currentButtons[ii].GetComponent<Button>().onClick.AddListener((() =>
                {
                    Garage.SelectSkin(bikeModel.AllSkins[ii]);
                }));
            }
        }
    }
}