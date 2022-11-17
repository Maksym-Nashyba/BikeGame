using System;
using TMPro;
using UnityEngine;

namespace ProgressionStore.Computer
{
    public class Clock : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private void Awake()
        {
            InvokeRepeating(nameof(UpdateTime), 0f, 60f);
        }

        private void UpdateTime()
        {
            _text.SetText(DateTime.Now.ToString("h:mm"));    
        }
    }
}