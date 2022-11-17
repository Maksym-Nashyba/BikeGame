using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _graphicsTierDropdown;
        [SerializeField] private Slider _effectsVolumeSlider;
        [SerializeField] private Slider _ambientVolumeSlider;

        private void Start()
        {
            _graphicsTierDropdown.value = (int)UserSettings.GetGraphicsTier();
            _effectsVolumeSlider.value = UserSettings.GetEffectsVolume();
            _ambientVolumeSlider.value = UserSettings.GetAmbientVolume();
        }

        public void ChangeGraphicsTier()
        {
            UserSettings.SetGraphicsTier((UserSettings.GraphicsTier)_graphicsTierDropdown.value);
        }
        
        public void ChangeEffectsVolume()
        {
            UserSettings.SetEffectsVolume(_effectsVolumeSlider.value);
        }
        
        public void ChangeAmbientVolume()
        {
            UserSettings.SetAmbientVolume(_ambientVolumeSlider.value);
        }

        public void OnBackButton()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}