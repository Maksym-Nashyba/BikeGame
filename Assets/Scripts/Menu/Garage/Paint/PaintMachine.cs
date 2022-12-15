using System;
using Garage;
using IGUIDResources;
using Menu.Garage.Paint.Containers;
using Menu.Garage.Paint.MachineButton;
using Misc;
using SaveSystem.Front;
using UnityEngine;

namespace Menu.Garage.Paint
{
    public class PaintMachine : MonoBehaviour
    {
        public event Action<Skin> BoughtSkin;
        public PaintContainer SelectedContainer => _containersHolder.SelectedContainer;
        [SerializeField] private BikeModelDisplay _modelDisplay;
        [SerializeField] private CameraCheckpoint _cameraCheckpoint;

        [SerializeField] private ButtonSide[] _buttonSides;
        [SerializeField] private MachineButtonAnimator _buttonAnimator;
        [SerializeField] private PaintContainersHolder _containersHolder;
        
        private Skin _selectedSkin;
        private Saves _saves;

        private void Awake()
        {
            _saves = FindObjectOfType<Saves>();
            SubscribeToAllEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromAllEvents();
        }
        
        private void SubscribeToAllEvents()
        {
            _cameraCheckpoint.CameraApproaching += OnCameraApproaching;
            _cameraCheckpoint.CameraDeparted += OnCameraDeparted;
            _containersHolder.ContainerSelected += OnContainerSelected;
            foreach (ButtonSide buttonSide in _buttonSides)
            {
                buttonSide.Clicked += OnButtonClicked;
            }
        }
        
        private void UnsubscribeFromAllEvents()
        {
            _cameraCheckpoint.CameraApproaching -= OnCameraApproaching;
            _cameraCheckpoint.CameraDeparted -= OnCameraDeparted;
            _containersHolder.ContainerSelected -= OnContainerSelected;
            foreach (ButtonSide buttonSide in _buttonSides)
            {
                buttonSide.Clicked -= OnButtonClicked;
            }
        }

        private async void OnCameraApproaching()
        {
            await _modelDisplay.Holder.MoveToPosition(GarageBikeModelHolder.BikePositions.Paint);
            await _containersHolder.FillContainers(_modelDisplay.CurrentBike.AllSkins);
        }
        
        private void OnCameraDeparted()
        {
            _modelDisplay.Holder.MoveToPosition(GarageBikeModelHolder.BikePositions.Preview);
            _buttonAnimator.ChangeButtonState(ButtonSides.Empty);
            _containersHolder.CleanContainers();
        }

        private void OnContainerSelected(PaintContainer container)
        {
            _selectedSkin = container.Skin;
            if (_saves.Bikes.IsSkinUnlocked(container.Skin))
            {
                _buttonAnimator.ChangeButtonState(ButtonSides.Paint);
                return;
            }
            _buttonAnimator.ChangeButtonState(ButtonSides.Purchase);
        }

        private void OnButtonClicked(ButtonSides buttonSide)
        {
            switch (buttonSide)
            {
                case ButtonSides.Empty:
                    break;
                case ButtonSides.Paint:
                    PaintBike(_selectedSkin);
                    break;
                case ButtonSides.Purchase:
                    _buttonAnimator.ChangeButtonState(ButtonSides.Confirm);
                    break;
                case ButtonSides.Confirm:
                    ConfirmSkinPurchase();
                    break;
            }
        }

        private void ConfirmSkinPurchase()
        {
            if (_saves.Currencies.GetDollans() < _selectedSkin.Price) return;
            _saves.Bikes.UnlockSkin(_modelDisplay.CurrentBike.GetGUID(), _selectedSkin.GetGUID());
            _saves.Currencies.SubtractDollans(_selectedSkin.Price);
            
            PaintBike(_selectedSkin);
            _buttonAnimator.ChangeButtonState(ButtonSides.Empty);
            BoughtSkin?.Invoke(_selectedSkin);
        }

        private void PaintBike(Skin skin)
        {
            _modelDisplay.ApplySkin(skin);
            _saves.Bikes.SelectSkinFor(_modelDisplay.CurrentBike.GetGUID(), skin.GetGUID());
            _buttonAnimator.ChangeButtonState(ButtonSides.Empty);
        }
    }
}