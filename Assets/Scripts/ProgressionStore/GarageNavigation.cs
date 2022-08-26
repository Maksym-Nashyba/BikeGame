using UnityEngine;

namespace ProgressionStore
{
    public class GarageNavigation : MonoBehaviour
    {
        [SerializeField] private GarageShop[] _windows;

        public void OpenWindow(int index)
        {
            for (int  i = 0;  i < _windows.Length;  i++)
            {
                if(i != index) _windows[i].Close();
            }
            _windows[index].Open();
        }
    }
}