using UnityEngine;

namespace ProgressionStore
{
    public class GarageUI : MonoBehaviour
    {
        [SerializeField] private GameObject[] _generalUI;

        public void SetGeneralUIActive(bool state)
        {
            foreach (GameObject uiElement in _generalUI)
            {
                uiElement.SetActive(state);
            }
        }
    }
}