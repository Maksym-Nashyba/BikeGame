using System;
using IGUIDResources;
using ProgressionStore.PaintShop;
using UnityEngine;

namespace ProgressionStore.Paint
{
    public class PaintShop : GarageShop
    {
        public override event Action Opened;
        public override event Action Closed;
        
        [SerializeField] private Animator _animator;
        [SerializeField] private PaintContainerArray _paintContainers;
        [SerializeField] private PaintShopUI _paintShopUI;
        [SerializeField] private PaintShopRotatingButton _rotatingButton;
        
        protected override void Awake()
        {
            base.Awake();
            Garage.NewBikeSelected += OnNewBikeSelected;
            _paintShopUI.SelectionButtonPressed += OnContainerSelectionButton;
            _paintContainers.ContainerSelected += OnPaintContainerSelected;
        }

        public override void Open()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("HiddenCamera")) return;
            _animator.Play("OpenPaintWindow");
            GarageUI.SetGeneralUIActive(false);
            Opened?.Invoke();
        }

        public override void Close()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("ShownPaintWindow")) return;
            _animator.Play("ClosePaintWindow");
            GarageUI.SetGeneralUIActive(true);
            Closed?.Invoke();
        }

        private void OnNewBikeSelected(BikeModel bikeModel)
        {
            _paintContainers.RebuildForSkins(bikeModel.AllSkins);            
        }

        private void OnContainerSelectionButton(int index)
        {
            _paintContainers.Select(index);
        }

        private void OnPaintContainerSelected(PaintContainer container)
        {
            Garage.SelectSkin(container.CurrentSkin);
        }

        private void OnDisable()
        {
            Garage.NewBikeSelected -= OnNewBikeSelected;
            _paintShopUI.SelectionButtonPressed -= OnContainerSelectionButton;
            _paintContainers.ContainerSelected -= OnPaintContainerSelected;
        }
    }
}