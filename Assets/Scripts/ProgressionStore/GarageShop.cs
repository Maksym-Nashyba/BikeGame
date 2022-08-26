using UnityEngine;

namespace ProgressionStore
{
    public abstract class GarageShop : MonoBehaviour
    {
        protected Garage Garage;
        
        protected virtual void Awake()
        {
            Garage = FindObjectOfType<Garage>();
        }

        public abstract void Open();

        public abstract void Close();

    }

}