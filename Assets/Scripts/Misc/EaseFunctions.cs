using System;
using UnityEngine;

namespace Misc
{
    public static class EaseFunctions
    {
        public delegate float Delegate(float t);

        public static float Lerp(float t) => t;
        
        public static float InOutBack(float t)
        {
            float a = 1.70158f;
            float b = a * 1.525f;

            return t < 0.5
                ? Mathf.Pow(2 * t, 2) * ((b + 1) * 2 * t - b) / 2
                : (Mathf.Pow(2 * t - 2, 2) * ((b + 1) * (t * 2 - 2) + b) + 2) / 2;
        }

        public static float InOutQuad(float t)
        {
            return t < 0.5 ? 8 * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 4) / 2;
        }

        public static float EaseOutElastic(float t)
        {
            float a = 2f * Mathf.PI / 3f;

            return Mathf.Abs(t) < 0.001
                ? 0
                : Math.Abs(t - 1) < 0.001
                    ? 1
                    : Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10f - 0.75f) * a) + 1f;
        }
    }
}