using System;
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

        protected override void Awake()
        {
            base.Awake();
            Garage.NewBikeSelected += InstantiateSkinButtons;
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

        private void InstantiateSkinButtons(BikeModel bikeModel)
        {
            for (int i = 0; i < bikeModel.AllSkins.Length; i++)
            { 
                int ii = i; 
                GameObject currentButtonObject = Instantiate(_buttonPrefab,_layoutGroup.transform);
                Button currentButton = currentButtonObject.GetComponent<Button>();
                TextMeshProUGUI currentButtonChildren = currentButtonObject.GetComponentInChildren<TextMeshProUGUI>();
                currentButtonChildren.text = $"{i+1}";
                currentButton.onClick.AddListener((() =>
                {
                    Garage.SelectSkin(bikeModel.AllSkins[ii]);
                }));
            }
        }
        
        
    }
}