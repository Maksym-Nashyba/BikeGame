using System;
using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private TextMeshProUGUI _fpsText;
    private float _hudRefreshRate = 1f;
    private float _timer;

    private void Awake()
    {
        _fpsText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Time.unscaledTime > _timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            _fpsText.text = "FPS: " + fps;
            _timer = Time.unscaledTime + _hudRefreshRate;
        }
    }
}
