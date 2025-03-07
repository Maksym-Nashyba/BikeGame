﻿using System.Threading.Tasks;
using Menu;
using UnityEngine;
using UnityEngine.Audio;

namespace SetUp
{
    public class AudioSetUp : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        
        private void Awake()
        {
            SetUpOperation setUpOperation = new SetUpOperation(SetUpAudioVolume, "Audio Loaded",false);
            FindObjectOfType<GameSetUp>().RegisterSetUpTask(setUpOperation);
        }

        private Task SetUpAudioVolume()
        {
            _audioMixer.SetFloat("EffectsVolume", UserSettings.GetEffectsVolume());
            _audioMixer.SetFloat("AmbientVolume", UserSettings.GetAmbientVolume());
            return Task.CompletedTask;
        }
    }
}