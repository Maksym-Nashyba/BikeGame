using IGUIDResources;
using UnityEngine;

namespace Effects
{
    public class BikeSkinApplier : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _parts;

        public void ApplySkin(Skin skin)
        {
            foreach (MeshRenderer part in _parts)
            {
                part.material = skin.Material;
            }
        }
    }
}