using UnityEngine;

namespace Misc
{
    public struct Transformation
    {
        public Vector3 Position;
        public Vector3 Scale;
        public Quaternion Rotation;

        public Transformation(Transform transform)
        {
            Position = transform.position;
            Scale = Vector3.one;
            Rotation = transform.rotation;
        }
    }
}