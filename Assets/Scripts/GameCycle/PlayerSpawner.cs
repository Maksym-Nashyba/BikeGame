using System;
using LevelObjectives.LevelObjects;
using Misc;
using UnityEngine;

namespace GameCycle
{
    public class PlayerSpawner : MonoBehaviour
    {
        public event Action<GameObject> Respawned;
        
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Transform _spawnPoint;
        
        private Checkpoint _lastReachedCheckpoint;

        private void Awake()
        {
            ServiceLocator.GameLoop.Started += SpawnPlayerOnStart;
            ServiceLocator.GameLoop.Died += SpawnPlayer;
        }

        public void UnlockCheckpoint(Checkpoint checkpoint)
        {
            _lastReachedCheckpoint = checkpoint;
        }

        private void SpawnPlayer()
        {
            if (_lastReachedCheckpoint is null) return;
            GameObject player = Instantiate(_playerPrefab);
            Transformation checkpointTransformation = _lastReachedCheckpoint.GetRespawnTransformation();
            player.transform.Apply(checkpointTransformation);
            Respawned?.Invoke(player);
        }
        
        private void SpawnPlayerOnStart()
        {
            GameObject player = Instantiate(_playerPrefab, _spawnPoint.position, _spawnPoint.rotation);
            Respawned?.Invoke(player);
        }

        private void OnDestroy()
        {
            ServiceLocator.GameLoop.Started -= SpawnPlayerOnStart;
            ServiceLocator.GameLoop.Died -= SpawnPlayer;
        }
    }
}