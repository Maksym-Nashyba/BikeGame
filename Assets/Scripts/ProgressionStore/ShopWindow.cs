using UnityEngine;

namespace ProgressionStore
{
    public abstract class ShopWindow : MonoBehaviour
    {
        private Garage _garage;

        public abstract void Open();

        public abstract void Close();

    }

}