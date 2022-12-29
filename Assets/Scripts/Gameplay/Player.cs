using System;
using System.Threading.Tasks;
using Misc;
using UnityEngine;

namespace Gameplay
{
    public class Player : MonoBehaviour
    {
        public event Action Died; 
        public event Action Respawned; 
        public bool IsAlive { get; private set; }
        public PlayerClone ActivePlayerClone { get; private set; }

        private void Awake()
        {
            ServiceLocator.GameLoop.Started += OnGameStarted;
            ServiceLocator.GameLoop.IntroPhase.SubscribeFireAndForget(async () =>
            {
                Respawn(0, false);
                await Task.Delay(300);
                ActivePlayerClone.SetInteractable(false);
            });
        }

        public void Die()
        {
            if (!IsAlive) return;
            Died?.Invoke();
            IsAlive = false;
        }

        public async Task Respawn(int delay, bool fireEvent = true)
        {
            if(delay > 0) await Task.Delay(delay);
            ActivePlayerClone = ServiceLocator.PlayerSpawner.SpawnPlayerClone(fireEvent);
            Respawned?.Invoke();
            IsAlive = true;
        }

        private void OnGameStarted()
        {
            ActivePlayerClone.SetInteractable(true);
            ServiceLocator.GameLoop.Started -= OnGameStarted;
            Died += async () =>
            {
                await Respawn(1000);
            };
        }
    }
}