using System;
using UnityEngine;

namespace ProgressionStore.Computer
{
    public class Raycastable : MonoBehaviour
    {
        public RectTransform RectTransform { get; private set; }

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }
    }
}