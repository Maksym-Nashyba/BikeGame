﻿using GameCycle;
using Inputs;
using LevelObjectives;
using Pausing;
using UnityEngine;

namespace Misc
{
    public class ServiceLocator : MonoBehaviour
    {
        public static ServiceLocator Singletron
        {
            get => _serviceLocator;
            private set => _serviceLocator = value;
        }
        private static ServiceLocator _serviceLocator;
        
        public static LevelStructure LevelStructure => _serviceLocator._levelStructure;
        [SerializeField] private LevelStructure _levelStructure;

        public static GameLoop GameLoop => _serviceLocator._gameLoop;
        [SerializeField] private GameLoop _gameLoop;

        public static Pause Pause => _serviceLocator._pause;
        [SerializeField] private Pause _pause;

        public static IBikeInputProvider InputProvider => _serviceLocator._inputProvider;
        [SerializeField] private GameObject _inputProviderObject;
        private IBikeInputProvider _inputProvider;
        
        private void Awake()
        {
            if (_serviceLocator is not null) Destroy(this);
            _serviceLocator = this;

            _inputProvider = _inputProviderObject.GetComponent<IBikeInputProvider>();
        }
    }
}