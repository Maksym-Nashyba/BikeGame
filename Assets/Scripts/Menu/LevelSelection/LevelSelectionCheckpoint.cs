﻿using System;
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
                _bestTimeText.SetText(Format.FormatSeconds(_saves.Career.GetBestTime(_level)));
            }
            else
            {
                _pedalDisplay.SetActive(false);
            }
        }
    }
}