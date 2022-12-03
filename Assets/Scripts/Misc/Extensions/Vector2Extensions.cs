using UnityEngine;

namespace Misc
{
    public static class Vector2Extensions
    {
        public static Vector2 RotatedBy(this Vector2 vector, float degrees)
        {
            degrees *= Mathf.Deg2Rad;
            float sin = Mathf.Sin(degrees);
            float cos = Mathf.Cos(degrees);
            Vector2 a = new Vector2(cos, sin);
            Vector2 b = new Vector2(-sin, cos);
            return vector.x * a + vector.y * b;
        }
    }
}