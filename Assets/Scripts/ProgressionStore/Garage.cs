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
        private BikeModel[] _bikes;
        private Saves _saves;

        private void Start()
        {
           SelectBike(GUIDResourceLocator.Initialize().Bikes.GetDefault());
        }

        public void SelectBike(BikeModel bikeModel)
        {
           NewBikeSelected?.Invoke(bikeModel); 
        }

        public void SelectSkin(Skin skin)
        {
            NewSkinSelected?.Invoke(skin);
        }
        
    }
}