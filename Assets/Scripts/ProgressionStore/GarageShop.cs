using System;
using UnityEngine;

namespace ProgressionStore
{
    public abstract class GarageShop : MonoBehaviour
    {
        public abstract event Action Opened;
        public abstract event Action Closed;
        
        protected Garage Garage;
        protected GarageUI GarageUI;
        
        protected virtual void Awake()
        {
            Garage = FindObjectOfType<Garage>();
            GarageUI = FindObjectOfType<GarageUI>();
        }

        public abstract void Open();

        public abstract void Close();
    }
}