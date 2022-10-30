using System;
using LevelObjectives.LevelObjects;
using Misc;
using UnityEngine;

namespace GameCycle
{
    public class PlayerSpawner : MonoBehaviour
    {
        public event Action<GameObject> Respawned;

        public bool PlayerAlive => _playerAlive;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Transform _spawnPoint;
        
        private Checkpoint _lastReachedCheckpoint;
        private bool _playerAlive;

        private void Awake()
        {
            ServiceLocator.GameLoop.Started += SpawnPlayerOnStart;
            ServiceLocator.GameLoop.Died += SpawnPlayer;//TODO add delay
            ServiceLocator.GameLoop.Died += OnDied;
            Respawned += OnSpawned;
        }

        public void UnlockCheckpoint(Checkpoint checkpoint)
        {
            _lastReachedCheckpoint = checkpoint;
        }

        private void SpawnPlayerOnStart()
        {
            GameObject player = Instantiate(_playerPrefab, _spawnPoint.position, _spawnPoint.rotation);
            Respawned?.Invoke(player);
        }
        
        private void SpawnPlayer()
        {
            if (_lastReachedCheckpoint is null) return;
            GameObject player = Instantiate(_playerPrefab);
            Transformation checkpointTransformation = _lastReachedCheckpoint.GetRespawnTransformation();
            player.GetComponent<Transform>().Apply(checkpointTransformation);
            Respawned?.Invoke(player);
        }

        private void OnSpawned(GameObject player)
        {
            _playerAlive = true;
        }

        private void OnDied()
        {
            _playerAlive = false;
        }
        
        private void OnDestroy()
        {
            ServiceLocator.GameLoop.Started -= SpawnPlayerOnStart;
            ServiceLocator.GameLoop.Died -= SpawnPlayer;
            ServiceLocator.GameLoop.Died -= OnDied;
            Respawned -= OnSpawned;
        }
    }
}