using System;
using LevelObjectives;
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

        private void Start()
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
            GameObject playerGameObject = Instantiate(_playerPrefab);
            playerGameObject.transform.position = _lastReachedCheckpoint.GetPlayerTransform().position;
            playerGameObject.transform.rotation = _lastReachedCheckpoint.GetPlayerTransform().rotation;
        }
        
        private void SpawnPlayerOnStart()
        {
            GameObject player = Instantiate(_playerPrefab, _spawnPoint.position, _spawnPoint.rotation);
            player.SetActive(true);
            Respawned?.Invoke(player);
            
            //TODO Applying dependencies through ServiceLocator
        }

        private void OnDestroy()
        {
            ServiceLocator.GameLoop.Started -= SpawnPlayerOnStart;
            ServiceLocator.GameLoop.Died -= SpawnPlayer;
        }
    }
}