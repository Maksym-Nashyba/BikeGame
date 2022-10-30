using Misc;
using UnityEngine;

namespace LevelObjectives.LevelObjects
{
    public class DeathZone : PlayerTrigger
    {
        private void Start()
        {
            Activated += OnActivated;
        }

        private void OnActivated()
        {
            ServiceLocator.GameLoop.KillPlayer();
        }

        
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.matrix = Matrix4x4.identity * Matrix4x4.Translate(transform.position) * Matrix4x4.Rotate(transform.rotation);
            Gizmos.color = Color.red;
            Gizmos.DrawCube(Vector3.zero, transform.localScale);
        }
        #endif

        private void OnDestroy()
        {
            Activated -= OnActivated;
        }
    }
}