using GameCycle;
using LevelObjectives.Objectives;
using Misc;
using UnityEngine;

namespace Gameplay
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        private Transformation _currentSpawnPoint;

        private void Awake()
        {
            _currentSpawnPoint = ServiceLocator.LevelStructure.ObjectiveQueue.Peek().GetSpawnPosition();
            ServiceLocator.GameLoop.EndedObjective += OnNewObjectiveStarted;
        }

        private void OnDestroy()
        {
            ServiceLocator.GameLoop.EndedObjective -= OnNewObjectiveStarted;
        }

        public PlayerClone SpawnPlayerClone()
        {
            GameObject playerClone = Instantiate(_playerPrefab);
            playerClone.GetComponent<Transform>().Apply(_currentSpawnPoint);
            return playerClone.GetComponent<PlayerClone>();
        }

        private void OnNewObjectiveStarted(Objective objective)
        {
            _currentSpawnPoint = objective.GetSpawnPosition();
        }
    }
}