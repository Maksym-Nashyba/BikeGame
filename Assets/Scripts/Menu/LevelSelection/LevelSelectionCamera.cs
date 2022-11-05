using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Menu
{
    public class LevelSelectionCamera : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private LineRenderer _path;
        
        public Task MoveToLevel(int targetLevelIndex)
        {
            throw new NotImplementedException();
        }
    }
}