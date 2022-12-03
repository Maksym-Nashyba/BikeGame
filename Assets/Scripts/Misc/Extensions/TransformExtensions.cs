using UnityEngine;

namespace Misc
{
    public static class TransformExtensions
    {
        public static Transform Apply(this Transform transform, Transformation transformation)
        {
            transform.position = transformation.Position;
            transform.localScale = transformation.Scale;
            transform.rotation = transformation.Rotation;
            return transform;
        }
    }
}