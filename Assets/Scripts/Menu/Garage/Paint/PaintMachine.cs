using Garage.Paint.MachineButton;
using IGUIDResources;
using Menu.Garage;
using Misc;
using SaveSystem.Front;
using UnityEngine;

namespace Garage.Paint
{
    public class PaintMachine : MonoBehaviour
    {
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
            _containersHolder.SkinChanged += OnSkinChanged;
            foreach (ButtonSide buttonSide in _buttonSides)
            {
                buttonSide.Clicked += OnButtonClicked;
            }
        }
        
        private void UnsubscribeFromAllEvents()
        {
            _cameraCheckpoint.CameraApproaching -= OnCameraApproaching;
            _cameraCheckpoint.CameraDeparted -= OnCameraDeparted;
            _containersHolder.SkinChanged -= OnSkinChanged;
            
            foreach (ButtonSide buttonSide in _buttonSides)
            {
                buttonSide.Clicked -= OnButtonClicked;
            }
        }

        private void OnCameraApproaching()
        {
            _modelDisplay.Holder.MoveToPosition(GarageBikeModelHolder.BikePositions.Paint);
            _containersHolder.ApplyPaintsToContainers(_modelDisplay.CurrentBike.AllSkins);
        }
        
        private void OnCameraDeparted()
        {
            _modelDisplay.Holder.MoveToPosition(GarageBikeModelHolder.BikePositions.Preview);
        }

        private void OnSkinChanged(Skin skin)
        {
            _selectedSkin = skin;
            if (_saves.Bikes.IsSkinUnlocked(skin))
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
                    PaintBike();
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
            _saves.Bikes.UnlockSkin(_modelDisplay.CurrentBike.GetGUID(), _selectedSkin.GetGUID());
            PaintBike();
            _buttonAnimator.ChangeButtonState(ButtonSides.Empty);
        }

        private void PaintBike()
        {
            _modelDisplay.ApplySkin(_selectedSkin);
            _buttonAnimator.ChangeButtonState(ButtonSides.Empty);
        }
    }
}