using System;
using IGUIDResources;
using SaveSystem.Front;
using UnityEngine;

namespace ProgressionStore
{
    public class Garage : MonoBehaviour
    {
        public event Action<BikeModel> NewBikeSelected;
        public event Action<Skin> NewSkinSelected;
        public Shop Shop { get; private set; }
        private BikeModel[] _bikes;
        private Saves _saves;

        private void Start()
        {
           NewBikeSelected?.Invoke(GUIDResourceLocator.Initialize().Bikes.GetDefault());
           NewSkinSelected?.Invoke(GUIDResourceLocator.Initialize().Bikes.GetDefault().AllSkins[0]);
        }
    }
}