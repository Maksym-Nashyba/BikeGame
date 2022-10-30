using Misc;
using UnityEngine;

namespace GameCycle
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;

        public PlayerClone SpawnPlayerClone()
        {
            GameObject playerClone = Instantiate(_playerPrefab);
            Transformation checkpointTransformation = ServiceLocator.GameLoop.PreviousObjective.GetSpawnPosition();
            playerClone.GetComponent<Transform>().Apply(checkpointTransformation);
            return playerClone.GetComponent<PlayerClone>();
        }
    }
}