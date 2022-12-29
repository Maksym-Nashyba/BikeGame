using Effects;
using LevelObjectives.Objectives;
using Misc;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private UnityEvent Respawned;
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

        public PlayerClone SpawnPlayerClone(bool fireEvent = true)
        {
            GameObject playerClone = Instantiate(ServiceLocator.LevelStructure.PlayerPrefab);
            playerClone.transform.Apply(_currentSpawnPoint);
            playerClone.GetComponent<BikeSkinApplier>().ApplySkin(ServiceLocator.LevelStructure.Skin);
            if(fireEvent)Respawned.Invoke();
            return playerClone.GetComponent<PlayerClone>();
        }

        private void OnNewObjectiveStarted(Objective objective)
        {
            _currentSpawnPoint = objective.GetSpawnPosition();
        }
    }
}