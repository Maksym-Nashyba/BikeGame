using IGUIDResources;
using Menu.BikeSelectionMenu;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatsDisplay: MonoBehaviour
    {
        [SerializeField] private Slider _stat1Slider;
        [SerializeField] private Slider _stat2Slider;
        [SerializeField] private Slider _stat3Slider;
        private BikeSelection _bikeSelection;
        
        private void Awake()
        {
            _bikeSelection = FindObjectOfType<BikeSelection>();
            _bikeSelection.BikeChanged += DisplayStats;
        }

        private void DisplayStats(BikeModel bikeModel)
        {
            _stat1Slider.value = bikeModel.stat1;
            _stat2Slider.value = bikeModel.stat2;
            _stat3Slider.value = bikeModel.stat3;
        }

        private void OnDestroy()
        {
            _bikeSelection.BikeChanged -= DisplayStats;
        }
    }
}