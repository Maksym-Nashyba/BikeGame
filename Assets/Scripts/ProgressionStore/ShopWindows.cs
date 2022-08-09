using UnityEngine;

namespace ProgressionStore
{
    public class ShopWindows : MonoBehaviour
    {
        [SerializeField] private ShopWindow[] _windows;

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