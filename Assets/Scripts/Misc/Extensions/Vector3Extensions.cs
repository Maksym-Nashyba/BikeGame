using UnityEngine;

namespace Misc
{
    public static class Vector3Extensions
    {
        public static Vector3 WithZeroY(this Vector3 v)
        {
            return new Vector3(v.x, 0f, v.z);
        }
    }
}