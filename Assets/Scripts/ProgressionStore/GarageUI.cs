using UnityEngine;

namespace ProgressionStore
{
    public class GarageUI : MonoBehaviour
    {
        [SerializeField] private GameObject _bikeSelectionButton;
         
        public void SetBikeSelectionEnabled(bool state)
        {
            _bikeSelectionButton.SetActive(state);
        }
    }
}