using GameCycle;
using LevelObjectives.Objectives;
using Misc;
using UnityEngine;

namespace Gameplay
{
    public class PlayerSpawner : MonoBehaviour
    {
        public Transformation CurrentSpawnPoint { get; private set; }
        [SerializeField] private GameObject _playerPrefab;

        private void Awake()
        {
            CurrentSpawnPoint = ServiceLocator.LevelStructure.ObjectiveQueue.Peek().GetSpawnPosition();
            ServiceLocator.GameLoop.EndedObjective += OnNewObjectiveStarted;
        }

        private void OnDestroy()
        {
            ServiceLocator.GameLoop.EndedObjective -= OnNewObjectiveStarted;
        }

        public PlayerClone SpawnPlayerClone()
        {
            GameObject playerClone = Instantiate(_playerPrefab);
            playerClone.GetComponent<Transform>().Apply(CurrentSpawnPoint);
            return playerClone.GetComponent<PlayerClone>();
        }

        private void OnNewObjectiveStarted(Objective objective)
        {
            CurrentSpawnPoint = objective.GetSpawnPosition();
        }
    }
}