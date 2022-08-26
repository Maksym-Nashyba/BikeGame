using UnityEngine;

namespace ProgressionStore
{
    public abstract class GarageShop : MonoBehaviour
    {
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