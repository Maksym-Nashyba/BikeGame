using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Menu
{
    public class SettingsMenu : MonoBehaviour
    {
        public event Action BackButtonClicked;
        [SerializeField] private AudioMixer _audioMixer;
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
            SetMixerParameter("EffectsVolume", UserSettings.GetEffectsVolume());
        }
        
        public void ChangeAmbientVolume()
        {
            UserSettings.SetAmbientVolume(_ambientVolumeSlider.value);
            SetMixerParameter("AmbientVolume", UserSettings.GetAmbientVolume());
        }

        private void SetMixerParameter(string key, float value)
        {
            value = Mathf.Clamp(value, 0.00001f, 1f);
            _audioMixer.SetFloat(key, Mathf.Log10(value) * 20);
        }

        public void OnBackButton()
        {
            BackButtonClicked?.Invoke();
        }
    }
}