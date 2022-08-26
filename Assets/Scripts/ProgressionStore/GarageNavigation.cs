using UnityEngine;

namespace ProgressionStore
{
    public class GarageNavigation : MonoBehaviour
    {
        [SerializeField] private GarageShop[] _shops;

        public void OpenWindow(int index)
        {
            for (int  i = 0;  i < _shops.Length;  i++)
            {
                if(i != index) _shops[i].Close();
            }
            _shops[index].Open();
        }
    }
}