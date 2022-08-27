using System;
using IGUIDResources;
using UnityEngine;

namespace ProgressionStore.PaintShop
{
    public class PaintGarageShop : GarageShop
    {
        public override event Action Opened;
        public override event Action Closed;
        
        [SerializeField] private Animator _animator;
        [SerializeField] private PaintContainerArray _paintContainers;
        [SerializeField] private PaintShopUI _paintShopUI;
        
        protected override void Awake()
        {
            base.Awake();
            Garage.NewBikeSelected += OnNewBikeSelected;
            _paintShopUI.SelectionButtonPressed += OnSelectionButton;
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

        private void OnSelectionButton(int index)
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
            _paintShopUI.SelectionButtonPressed -= OnSelectionButton;
            _paintContainers.ContainerSelected -= OnPaintContainerSelected;
        }
    }
}