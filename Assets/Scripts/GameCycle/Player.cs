using System;
using System.Threading.Tasks;
using Misc;
using UnityEngine;

namespace GameCycle
{
    public class Player : MonoBehaviour
    {
        public event Action Died; 
        public event Action Respawned; 
        public bool IsAlive { get; private set; }
        public PlayerClone ActivePlayerClone { get; private set; }

        public void Die()
        {
            if (!IsAlive) return;
            Died?.Invoke();
            IsAlive = false;
        }

        public async Task Respawn(int delay)
        {
            if(delay > 0) await Task.Delay(delay);
            ActivePlayerClone = ServiceLocator.PlayerSpawner.SpawnPlayerClone();
            Respawned?.Invoke();
            IsAlive = true;
        }
    }
}