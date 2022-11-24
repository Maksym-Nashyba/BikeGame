using System;
using System.Globalization;
using IGUIDResources;
using Misc;
using SaveSystem.Front;
using TMPro;
using UnityEngine;

namespace Menu
{
    public class LevelSelectionCheckpoint : CameraCheckpoint
    {
        public float FogHeight => _fogHeight;
        [SerializeField] private float _fogHeight;
        [SerializeField] private TextMeshPro _bestTimeText;
        [SerializeField] private GameObject _pedalDisplay;
        [SerializeField] private Level _level;
        private Saves _saves;

        private void Awake()
        {
            _saves = FindObjectOfType<Saves>();
        }

        private void Start()
        {
            if (_saves.Career.IsCompleted(_level))
            {
                _pedalDisplay.SetActive(_saves.Career.IsPedalCollected(_level));
                _bestTimeText.SetText(FormatSeconds(_saves.Career.GetBestTime(_level)));
            }
            else
            {
                _pedalDisplay.SetActive(false);
            }
        }

        private String FormatSeconds(int seconds)
        {
            int minutes = seconds / 60;
            int restSeconds = seconds - minutes * 60;
            return DoubleDigit(minutes) + ":" + DoubleDigit(restSeconds);
        }

        private String DoubleDigit(int numeric)
        {
            if (numeric < 10) return "0" + numeric;
            return numeric.ToString();
        }
    }
}