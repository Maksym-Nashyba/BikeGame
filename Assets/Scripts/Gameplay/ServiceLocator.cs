﻿using Effects.TransitionCover;
using Inputs;
using LevelObjectives;
using LevelObjectives.LevelObjects;
using Pausing;
using SaveSystem.Front;
using UI;
using UnityEngine;

namespace Gameplay
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
        
        public static Player Player => _serviceLocator._player;
        [SerializeField] private Player _player;
        
        public static PlayerSpawner PlayerSpawner => _serviceLocator._playerSpawner;
        [SerializeField] private PlayerSpawner _playerSpawner;
        
        public static Camera Camera => _serviceLocator._camera;
        [SerializeField] private Camera _camera;
        
        public static InGameUI InGameUI => _serviceLocator._inGameUI;
        [SerializeField] private InGameUI _inGameUI;
  
        public static Pause Pause => _serviceLocator._pause;
        [SerializeField] private Pause _pause;

        public static Saves Saves => _serviceLocator._saves;
        private Saves _saves;

        public static Pedal Pedal => _serviceLocator._pedal;
        [SerializeField] private Pedal _pedal;
        
        public static SceneTransitionCover SceneTransitionCover => _serviceLocator._sceneTransitionCover;
        [SerializeField] private SceneTransitionCover _sceneTransitionCover;
        
        public static IBikeInputProvider InputProvider => _serviceLocator._inputProvider;
        [SerializeField] private GameObject _inputProviderObject;
        private IBikeInputProvider _inputProvider;
        
        private void Awake()
        {
            if (_serviceLocator is not null) Destroy(this);
            _serviceLocator = this;

            _inputProvider = _inputProviderObject.GetComponent<IBikeInputProvider>();

            _saves = FindObjectOfType<Saves>();
        }
    }
}