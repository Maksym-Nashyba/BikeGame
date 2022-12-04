using Misc;
using UnityEngine;

namespace Gameplay
{
    public class PlayerClone : MonoBehaviour
    {
        private IBicycle _bicycle;

        private void Awake()
        {
            _bicycle = GetComponent<IBicycle>();
        }

        public void SetInteractable(bool enabled)
        {
            _bicycle.SetInteractable(enabled);
        }
    }
}