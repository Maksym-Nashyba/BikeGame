using System;
using IGUIDResources;
using TMPro;
using UnityEngine;

namespace ProgressionStore.Computer.Browser
{
    public class ScamazonTab : MonoBehaviour
    {
        [SerializeField] private BikeModel _bike;
        [SerializeField] private TextMeshProUGUI _priceText;

        private void Start()
        {
            _priceText.SetText($"{_bike.Cost}$");
        }
    }
}