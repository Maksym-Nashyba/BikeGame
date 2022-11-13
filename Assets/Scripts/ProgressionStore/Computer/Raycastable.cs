using UnityEngine;

namespace ProgressionStore.Computer
{
    public class Raycastable : MonoBehaviour
    {
        private RectTransform _rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
            private set => _rectTransform = value;
        }

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }
    }
}