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
        private List<Button> _currentButtons;

        protected override void Awake()
        {
            base.Awake();
            Garage.NewBikeSelected += InstantiateSkinButtons;
            _currentButtons = new List<Button>();
        }

        public override void Open()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Hidden")) return;
            _animator.Play("Open");
            _closeButton.SetActive(true);
        }

        public override void Close()
        {
            _closeButton.SetActive(false);
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Shown")) return;
            _animator.Play("Close");
        }

        private void DestroyButtons()
        {
            foreach (Button button in _currentButtons)
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
                _currentButtons.Add(currentButtonObject.GetComponent<Button>()); 
                TextMeshProUGUI currentButtonChildren = currentButtonObject.GetComponentInChildren<TextMeshProUGUI>();
                currentButtonChildren.text = $"{i+1}";
                _currentButtons[ii].onClick.AddListener((() =>
                {
                    Garage.SelectSkin(bikeModel.AllSkins[ii]);
                }));
            }
        }
        
        
    }
}